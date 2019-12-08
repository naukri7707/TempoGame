using Naukri.Beatmap;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject holdObjectPrefab;


    public bool IsReady { get; set; }

    private void Start()
    {
        InstantisteHitObjects();
    }

    /// <summary>
    /// 取得Beatmap
    /// </summary>
    /// <param name="beatmapID">Beatmap ID</param>
    private void InstantisteHitObjects()
    {
        foreach (var t in GameManager.Tracks)
        {
            t.Clear();
        }
        IsReady = false;
        foreach (var hit in GameManager.Beatmap.HitObjects.Collection)
        {
            var newObject = Instantiate(holdObjectPrefab, transform).GetComponent<HitObject>();
            newObject.Track = Mathf.FloorToInt(hit.X * 4 / 512);
            newObject.StartTime = (float)hit.Time / 1000;
            newObject.EndTime = (float)hit.EndTime / 1000;
            GameManager.Tracks[newObject.Track].Enqueue(newObject);
        }
        IsReady = true;
    }
}
