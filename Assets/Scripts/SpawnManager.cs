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

    }
    List<IHitObject> hitObjects = new List<IHitObject>();
    /// <summary>
    /// 取得Beatmap
    /// </summary>
    /// <param name="beatmapID">Beatmap ID</param>
    private void GetBeatmap(int beatmapID)
    {
        var beatmap = new Beatmap(GameArgs.SelectedBeatmapTileData.OsuFile);
        foreach (var hit in beatmap.HitObjects.Collection)
        {
            // hitObjects.Add(new HoldObject(hit.))
        }
    }
}
