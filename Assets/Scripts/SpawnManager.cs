using Mono.Data.Sqlite;
using Naukri.Beatmap;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SpawnManager
{
    private void Start()
    {
        StreamReader sr = new StreamReader("Assets/IOFile/txtIO.txt");
        string s = sr.ReadLine();
        // TODO Analysis
    }

    /// <summary>
    /// 取得Beatmap
    /// </summary>
    /// <param name="beatmapID">Beatmap ID</param>
    private void GetBeatmap(int beatmapID)
    {
        var beatmap = new Beatmap(GameArgs.SelectedBeatmapTileData.OsuFile);
        foreach (var hit in beatmap.HitObjects.Collection)
        {
            //
        }
    }
}
