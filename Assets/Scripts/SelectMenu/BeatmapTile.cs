using Naukri.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapTile : MonoBehaviour
{
    private readonly Color songTileColor = new Color(0.511F, 0.556F, 1F, 0.725F);

    private readonly Color songFocusTileColor = new Color(0.656F, 1F, 0.511F, 0.725F);

    private readonly Vector3[] metaTilesInitPos = {
        new Vector3(180,660),
        new Vector3(170,580),
        new Vector3(160,500),
        new Vector3(150,420),
        new Vector3(140,340),
        new Vector3(130,260),
        new Vector3(120,180),
        new Vector3(110,100),
        new Vector3(80,0),
        new Vector3(110,-100),
        new Vector3(120,-180),
        new Vector3(130,-260),
        new Vector3(140,-340),
        new Vector3(150,-420),
        new Vector3(160,-500),
        new Vector3(170,-580),
        new Vector3(180,-660)
    };

    private float speed = 2.5F;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text artist;

    private RectTransform rectTransform;

    private Image selfImage;

    public int Offset { get; set; }

    public Image Image { get => image; set => image = value; }

    public Text Title { get => title; set => title = value; }

    public Text Artist { get => artist; set => artist = value; }

    public Vector3 TargetPosition { get => metaTilesInitPos[Offset + BeatmapTileList.OffsetRange]; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        selfImage = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, TargetPosition, speed * Time.deltaTime);
    }

    public void SetInfo(int offset, BeatmapTileData beatmap)
    {
        Offset = offset;
        SetColor(offset == 0);
        rectTransform.anchoredPosition = TargetPosition;
        Image.GetExternalSprite(beatmap.BackgroundPath);
        Title.text = $"{(beatmap.TitleUnicode == "" ? beatmap.Title : beatmap.TitleUnicode)} - {beatmap.Version}";
        Artist.text = $"{(beatmap.ArtistUnicode == "" ? beatmap.Artist : beatmap.ArtistUnicode)} // {beatmap.Creator}";
    }

    /// <summary>
    /// 設定音樂磚背景色
    /// </summary>
    /// <param name="isFocus">是否為聚焦項</param>
    public void SetColor(bool isFocus)
    {
        selfImage.color = isFocus ? songFocusTileColor : songTileColor;
    }
}
