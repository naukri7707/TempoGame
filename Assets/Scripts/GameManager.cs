using Mono.Data.Sqlite;
using Naukri.OsuAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;

    public static float Speed { get; } = 8f;
    SqliteManager db;
    public Song analysis;
    private void Awake()
    {
        analysis = new Song($@"{Application.streamingAssetsPath}/Songs/Shoukaihan/Orangestar feat.IA - Asu no Yozora Shoukaihan (dakemoto) [Normal].osu");
        var a = analysis.TimingPoints[0];
    }

    private void Start()
    {

    }

    void Update()
    {

    }
}
