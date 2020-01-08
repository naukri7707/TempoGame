using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectBaseline : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float offset;

    [SerializeField]
    private RectTransform canvasRect;

    public float Offset { get => offset; set => offset = value; }

    public RectTransform CanvasRect { get => canvasRect; set => canvasRect = value; }

    private void Awake()
    {
        GameManager.DistanceToBottom = (CanvasRect.offsetMax.y - CanvasRect.offsetMin.y) * Offset;
        var pos = new Vector2(0, GameManager.DistanceToBottom);
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
