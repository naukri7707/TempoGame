using UnityEngine;

/// <summary>
/// 繼承此類別以創建單例類別
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 互斥鎖
    /// </summary>
    private static readonly object mLock = new object();

    /// <summary>
    /// 單例實例
    /// </summary>
    private static T mInstance;

    /// <summary>
    /// 通過這個屬性來存取單例物件
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (mLock)
            {
                if (mInstance == null)
                {
                    // 檢查 Hierarchy 是否有包含有該類的物件
                    mInstance = FindObjectOfType<T>();

                    // 如果 Hierarchy 沒有包含有該類的物件
                    if (mInstance == null)
                    {
                        // 新增一個包含該類的物件
                        var singletonObject = new GameObject();
                        mInstance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).Name} (Singleton)";

                        // 使該類永久化
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return mInstance;
            }
        }
    }
}