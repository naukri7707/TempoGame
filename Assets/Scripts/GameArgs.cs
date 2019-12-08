using Naukri.Beatmap;
using System.IO;
using UnityEngine;

static class GameArgs
{
    public static BeatmapTileData SelectedBeatmap { get; set; }

    public static string GameDataPath { get; } = Path.Combine(Application.streamingAssetsPath, "GameData.db");

    public static string BeatmapList { get; } = "BeatmapList";
}