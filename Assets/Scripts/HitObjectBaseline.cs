using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectBaseline : MonoBehaviour
{
    [SerializeField][Range(0,1)]
    private float offset;

    [SerializeField]
    private RectTransform canvasRect;

    public float Offset { get => offset; set => offset = value; }

    public RectTransform CanvasRect { get => canvasRect; set => canvasRect = value; }

    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (CanvasRect.offsetMax.y - CanvasRect.offsetMin.y) * Offset);
    }
}
