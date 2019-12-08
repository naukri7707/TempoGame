using UnityEngine;

public class HoldObject : MonoBehaviour, IHitObject, IHold
{
    private const float movePerFrame = -200F;

    [SerializeField]
    private RectTransform rectTransform;

    private readonly static float[] hitObjectsPosX = {
        -90F,
        -30F,
        30F,
        90F
    };

    /// <summary>
    /// 軌道
    /// </summary>
    public int Track { get; set; }

    /// <summary>
    /// 開始時間 (秒)
    /// </summary>
    public float StartTime { get; set; }

    /// <summary>
    /// 結束時間 (秒)
    /// </summary>
    public float EndTime { get; set; }

    private void Start()
    {
        rectTransform.anchoredPosition = new Vector2(hitObjectsPosX[Track], -StartTime * movePerFrame);
        rectTransform.SetHeight((StartTime - EndTime) * movePerFrame);
    }

    private void Update()
    {
        rectTransform.Translate(0, movePerFrame * Time.deltaTime, 0);
    }

}

public static partial class ExtensionMethods
{
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