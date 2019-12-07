using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehaviour : MonoBehaviour, INote
{
    private static readonly KeyCode[] buttons = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };

    private static readonly float[] initPosX = { -2.7f, -0.9f, 0.9f, 2.7f };

    private static readonly Vector2[] end = {
        new Vector3(0, -2.7f),
        new Vector3(0, -0.9f),
        new Vector3(0, 0.9f),
        new Vector3(0, 2.7f)
    };

    [SerializeField] private int _track;

    [SerializeField] private int _timeCode;

    /// <summary>
    /// 通道
    /// </summary>
    public int Track { get => _track; set => _track = value; }

    /// <summary>
    /// 時間碼
    /// </summary>
    public int StartTime { get => _timeCode; set => _timeCode = value; }

    /// <summary>
    /// 對應按鈕
    /// </summary>
    public KeyCode CorrespondButton { get; set; }

    private void Start()
    {
        CorrespondButton = buttons[Track];
        transform.position = new Vector3(initPosX[Track], _timeCode * GameManager.Speed, 0);
    }

    private void Update()
    {
        transform.localScale = new Vector3(1 - transform.position.y * 0.05f, 1 - transform.position.y * 0.05f, 1);
        transform.Translate(0, -GameManager.Speed * Time.deltaTime, 0);
        transform.position = Vector3.Lerp(new Vector3(0, transform.position.y, 0), new Vector3(initPosX[Track], transform.position.y, 0), 1 - transform.position.y / 30);
    }
}