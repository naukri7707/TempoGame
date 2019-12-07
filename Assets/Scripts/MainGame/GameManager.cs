using Mono.Data.Sqlite;
using Naukri.Beatmap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private Image background;

    private Beatmap Beatmap { get; set; }

    public static float Speed { get; } = 8f;

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    void Update()
    {

    }
}
