using Naukri.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapTile : MonoBehaviour
{
    private const float alpha = 0.725F;

    private readonly Vector3[] metaTilesInitPos = {
        new Vector3(250,660),
        new Vector3(220,580),
        new Vector3(190,500),
        new Vector3(160,420),
        new Vector3(140,340),
        new Vector3(120,260),
        new Vector3(110,180),
        new Vector3(100,100),
        new Vector3(60,0),
        new Vector3(100,-100),
        new Vector3(110,-180),
        new Vector3(120,-260),
        new Vector3(140,-340),
        new Vector3(160,-420),
        new Vector3(190,-500),
        new Vector3(220,-580),
        new Vector3(250,-660)
    };

    private float speed = 2.5F;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text artist;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image selfImage;

    public Image Image => image;

    public int Offset { get; set; }

    public Vector3 TargetPosition => metaTilesInitPos[Offset + BeatmapTileList.OffsetRange];

    public RectTransform RectTransform => rectTransform;


    // Update is called once per frame
    private void Update()
    {
        RectTransform.anchoredPosition = Vector3.Lerp(RectTransform.anchoredPosition, TargetPosition, speed * Time.deltaTime);
    }

    public void SetInfo(int offset, BeatmapTileData beatmap)
    {
        Offset = offset;
        Random.InitState(beatmap.BeatmapSetID);
        selfImage.color = new Color(Random.Range(0.5F, 1), Random.Range(0.5F, 1), Random.Range(0.5F, 1), alpha);
        RectTransform.anchoredPosition = TargetPosition;
        image.SetSpriteExternal(beatmap.BackgroundPath);
        title.text = $"{(beatmap.TitleUnicode == "" ? beatmap.Title : beatmap.TitleUnicode)} - {beatmap.Version}";
        artist.text = $"{(beatmap.ArtistUnicode == "" ? beatmap.Artist : beatmap.ArtistUnicode)} // {beatmap.Creator}";
    }
}
