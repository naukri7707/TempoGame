using Naukri.Beatmap;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject holdObjectPrefab;

    private void Awake()
    {
        InstantisteBeatmap();
    }

    /// <summary>
    /// 取得Beatmap
    /// </summary>
    /// <param name="beatmapID">Beatmap ID</param>
    private void InstantisteBeatmap()
    {
        var beatmap = new Beatmap(Path.Combine(BeatmapManager.SongFolderPath, GameArgs.SelectedBeatmapTileData.PackageName, GameArgs.SelectedBeatmapTileData.OsuFile));
        foreach (var hit in beatmap.HitObjects.Collection)
        {
            var newObject = Instantiate(holdObjectPrefab, transform).GetComponent<HoldObject>();
            newObject.Track = Mathf.FloorToInt(hit.X * 4 / 512);
            newObject.StartTime = (float)hit.Time / 1000;
            newObject.EndTime = (float)hit.EndTime / 1000;

        }
    }
}
