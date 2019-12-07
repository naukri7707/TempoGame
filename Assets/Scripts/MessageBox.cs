using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    /// <summary>
    /// 懶訊息結構
    /// </summary>
    private struct LazyDate
    {
        public LazyDate(string message, LogType logType)
        {
            Message = message;
            LogType = logType;
        }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 型態
        /// </summary>
        public LogType LogType { get; set; }
    }

    private static GameObject messageBoxPrefab;

    private static GameObject canvas;

    private static string message;

    /// <summary>
    /// 用幾幀的時間抵達終點 (滑動速度)
    /// </summary>
    public const int frameToEnd = 50;

    /// <summary>
    /// 每幀的淡出量
    /// </summary>
    public const float fadeoutPreFrame = 0.02F;

    /// <summary>
    /// 訊息停留時間 (保證不被新訊息往上推的時間)
    /// </summary>
    public const float messageStopTime = 1F;

    /// <summary>
    /// 訊息存活時間
    /// </summary>
    public const float messageAliveTime = 10F;
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text text;

    /// <summary>
    /// MessageBox 搜集器
    /// </summary>
    public static List<MessageBox> Collection { get; set; } = new List<MessageBox>();

    /// <summary>
    /// 訊息佇列
    /// </summary>
    public static Queue<MessageBox> MessageBoxQueue { get; set; } = new Queue<MessageBox>();

    /// <summary>
    /// 懶訊息佇列
    /// </summary>
    private static List<LazyDate> LazyMessageQueue { get; set; } = new List<LazyDate>();

    /// <summary>
    /// 上移物件數量
    /// </summary>
    public static int MovingUp { get; private set; } = 0;

    /// <summary>
    /// 訊息
    /// </summary>
    public string Message { get => text.text; set => text.text = value; }

    public LogType LogType { get; set; } = LogType.Log;

    private void Awake()
    {
        // 如果正在 MessageBoxes 正在上移或佇列尚有訊息就加入佇列，否則直接加入Collection
        if (MovingUp == 0 && MessageBoxQueue.Count == 0)
        {
            Collection.Add(this);
            InitMessage();
        }
        else
        {
            MessageBoxQueue.Enqueue(this);
            gameObject.SetActive(false);
        }
    }

    private void InitMessage()
    {
        MoveUpMessages();
        StartCoroutine(DeleteWhenTimeUp());
    }

    /// <summary>
    /// 實例化訊息
    /// </summary>
    /// <param name="message">訊息</param>
    /// <param name="logType">型態</param>
    private static void InstantiateMessageBox(string message, LogType logType = LogType.Log)
    {
        var msg = (GameObject.Instantiate(messageBoxPrefab, canvas.transform) as GameObject).GetComponent<MessageBox>();
        msg.Message = message;
        switch (logType)
        {
            case LogType.Warning:
                msg.text.color = Color.yellow;
                break;
            case LogType.Error:
                msg.text.color = Color.red;
                break;
            default:
                msg.text.color = Color.white;
                break;
        }
    }

    /// <summary>
    /// 顯示訊息
    /// </summary>
    /// <param name="message">訊息</param>
    /// <param name="logType">訊息樣式</param>
    public static void Show(string message, LogType logType = LogType.Log)
    {
        if (messageBoxPrefab == null)
        {
            messageBoxPrefab = Resources.Load<GameObject>(@"Prefabs/MessageBox");
            canvas = GameObject.Find("Canvas");
        }
        // 實例化自己
        InstantiateMessageBox(message, logType);
    }

    /// <summary>
    /// 顯示懶訊息 (供副執行緒使用)
    /// </summary>
    public static void ShowLazy(string message, LogType logType = LogType.Log)
    {
        LazyMessageQueue.Add(new LazyDate(message, logType));
    }

    /// <summary>
    /// 將懶訊息推送至訊息佇列，在主執行緒配合 IEnumerator 使用
    /// </summary>
    public static void PushLazy()
    {
        // 減少反覆運算消耗
        if (LazyMessageQueue.Count > 0)
        {
            if (messageBoxPrefab == null)
            {
                messageBoxPrefab = Resources.Load<GameObject>(@"Prefabs/MessageBox");
                canvas = GameObject.Find("Canvas");
            }
            foreach (var message in LazyMessageQueue)
            {
                InstantiateMessageBox(message.Message, message.LogType);
            }
            LazyMessageQueue.Clear();
        }
    }

    /// <summary>
    /// 讓訊息框向上移動
    /// </summary>
    private IEnumerator MoveUpAsync()
    {
        MovingUp++;
        var start = rectTransform.anchoredPosition;
        var end = start + new Vector2(0, rectTransform.rect.height);
        for (int i = 1; i <= frameToEnd; i++)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(start, end, (float)i / frameToEnd);
            yield return new WaitForEndOfFrame();
        }
        MovingUp--;
        if (MovingUp == 0)
        {
            if (MessageBoxQueue.Count > 0)
            {
                yield return new WaitForSeconds(messageStopTime);
                var nextMessage = MessageBoxQueue.Dequeue();
                Collection.Add(nextMessage);
                nextMessage.gameObject.SetActive(true);
                nextMessage.InitMessage();
            }
        }
    }

    /// <summary>
    /// 讓全部訊息框向上移動
    /// </summary>
    private void MoveUpMessages()
    {
        Collection.ForEach(it =>
        {
            StartCoroutine(it.MoveUpAsync());
        });
    }

    /// <summary>
    /// 時間到時刪除自己
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteWhenTimeUp()
    {
        yield return new WaitForSeconds(messageAliveTime);
        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, fadeoutPreFrame);
            text.color -= new Color(0, 0, 0, fadeoutPreFrame);
            yield return new WaitForEndOfFrame();
        }
        Collection.Remove(this);
        while (MovingUp != 0)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
