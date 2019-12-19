using System;
using UnityEngine;

// TODO 按到時特效

public abstract class HitObject : MonoBehaviour
{
    /// <summary>
    /// 預設速度
    /// </summary>
    private const float speed = -700F;

    /// <summary>
    /// 欄位 X 軸
    /// </summary>
    protected static float[] PosX => GameArgs.TrackPosX;

    [SerializeField]
    protected RectTransform rectTransform;

    /// <summary>
    /// 速度
    /// </summary>
    protected static float CurrentSpeed { get; set; } = speed;

    /// <summary>
    /// 活性
    /// </summary>
    public static bool IsActive => CurrentSpeed == speed;

    /// <summary>
    /// 數量
    /// </summary>
    public static int Count { get; set; } = 0;

    /// <summary>
    /// 開始時間 (秒)
    /// </summary>
    public float StartTime { get; set; }

    /// <summary>
    /// 結束時間 (秒)
    /// </summary>
    public float EndTime { get; set; }

    /// <summary>
    /// 軌道
    /// </summary>
    public int Track { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    protected int Score { get; set; } = 0;

    /// <summary>
    /// 已經結算
    /// </summary>
    protected bool IsSettle { get; set; } = false;

    /// <summary>
    /// 頂部
    /// </summary>
    public float Top => rectTransform.offsetMax.y;

    /// <summary>
    /// 底部
    /// </summary>
    public float Bottom => rectTransform.offsetMin.y;

    protected virtual void Awake()
    {
        Count++;
    }

    private void Start()
    {
        rectTransform.anchoredPosition = new Vector2(PosX[Track], -StartTime * speed);
        rectTransform.SetHeight((StartTime - EndTime) * speed);
    }

    private void Update()
    {
        rectTransform.Translate(0, CurrentSpeed * Time.deltaTime, 0);
    }

    // 判定是否必為 miss
    public abstract bool IsOver();

    // 當這個按鈕處於當前Track可被按下的HitObject中最下面時且按鈕按下時
    public abstract void OnFocus(ButtonState state);

    // 結算
    protected void Settle()
    {
        Count--;
        GameManager.Instance.AddScore(Score);
        IsSettle = true;
    }

    // 離開螢幕
    public bool IsOutOfScreen()
    {
        return false; // return Top + 100 getCanvas ? maybe on gameManager?
    }

    public bool InTolerance(float tolerance) => 0F.InRange(rectTransform.offsetMin.y - tolerance, rectTransform.offsetMax.y + tolerance);

    public static void SetActive(bool active) => CurrentSpeed = active ? speed : 0;
}

public static partial class ExtensionMethods
{
    /// <summary>
    /// 如果該數介於 min 和 max 之間 (包含)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">本身</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns>是否在範圍內</returns>
    public static bool InRange<T>(this T self, T min, T max) where T : IComparable
    {
        return self.CompareTo(min) >= 1 && self.CompareTo(max) <= -1;
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetWidth(this RectTransform rt, float width)
    {
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
    }

    public static void SetHeight(this RectTransform rt, float height)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }
}