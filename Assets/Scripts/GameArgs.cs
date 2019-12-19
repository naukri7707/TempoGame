using System.IO;
using UnityEngine;
using UnityEngine.UI;

static class GameArgs
{
    public static BeatmapTileData SelectedBeatmap { get; set; }

    public static string GameDataPath { get; } = Path.Combine(Application.streamingAssetsPath, "GameData.db");

    public static string BeatmapList { get; } = "BeatmapList";

    /// <summary>
    /// 軌道X軸
    /// </summary>
    public static float[] TrackPosX { get; } = {
        -108F,
        -36F,
        36F,
        108F,
        0 // for separate
    };

    /// <summary>
    /// 網格X軸
    /// </summary>
    public static float[] MeshPosX { get; } = {
        -144,
        -72,
        0,
        72,
        144
    };

    public static Image[] PressEffect { get; set; } = new Image[4];

    public static HitEffectBehaviour[] HitEffect { get; set; } = new HitEffectBehaviour[4];

}