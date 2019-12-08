using Naukri.Beatmap;
using Naukri.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO 預設 delay 時間 (某些歌直接開始)
// TODO Hold Object
// TODO Score & Combo
// TODO 沒有HitObject 後漸按結算
// TODO UI
// TODO 校準介面

public class GameManager : MonoBehaviour
{
    private static readonly BeatmapTileData ThisGame = new BeatmapTileData(
            1604005,
            749557,
            "This game",
            "This game",
            "Roselia",
            "Roselia",
            "-Aqua",
            "Rave's Normal",
            "749557 Roselia - This game",
            "Sayo best girl fight me.jpg",
            "This game.wav",
            "Roselia - This game (-Aqua) [Rave's Normal].osu"
            );

    public struct Evaluation
    {
        public Evaluation(string name, int score, float tolerance)
        {
            Name = name;
            Score = score;
            Tolerance = tolerance;
        }

        public string Name { get; }

        public int Score { get; }

        public float Tolerance { get; }
    }

    private static readonly Evaluation[] evaluations = {
        new Evaluation("Perfect", 300, 10),
        new Evaluation("Great", 200, 15),
        new Evaluation("Good", 100, 20),
        new Evaluation("Bad", 50, 25),
        new Evaluation("Miss", 0, 30),
        };

    [SerializeField]
    private Image background;

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private SpawnManager spawnManager;

    static GameManager()
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            Tracks[i] = new Queue<HitObject>();
        }
    }

    public static Beatmap Beatmap { get; set; }

    public static float Speed { get; } = 8f;

    public static Queue<HitObject>[] Tracks { get; } = new Queue<HitObject>[4];

    public static Queue<HitObject> Removeable { get; } = new Queue<HitObject>();

    private void Awake()
    {
        // GameArgs.SelectedBeatmap = ThisGame;
        HitObject.SetActive(false);
        StartCoroutine(music.SetAudioExternalAsync(GameArgs.SelectedBeatmap.MusicPath));
        Beatmap = new Beatmap(GameArgs.SelectedBeatmap.OsuPath);
        background.SetSpriteExternal(GameArgs.SelectedBeatmap.BackgroundPath);
        StartCoroutine(WaitUntilReadyAndLeadIn());
    }

    private void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (HitObject.IsActive)
            {
                music.Pause();
                HitObject.SetActive(false);
            }
            else
            {
                music.UnPause();
                HitObject.SetActive(true);
            }
        }
    }


    private void LateUpdate()
    {
        // 將過期(絕對為miss)的hitObject 轉移至 removeable 移除
        foreach (var t in Tracks)
        {
            while (t.Any() && t.Peek().Top < -evaluations.Last().Tolerance)
            {
                Removeable.Enqueue(t.Dequeue());
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (Arduino.Buttons[i].Press)
            {
                var e = GetEvaluation(Tracks[i].Peek());
                if (e.Score >= 0)
                {
                    Destroy(Tracks[i].Dequeue().gameObject);
                }
            }
        }
        while (Removeable.Any() && Removeable.Peek().Top < -100)
        {
            Destroy(Removeable.Dequeue().gameObject);
        }
    }

    private IEnumerator WaitUntilReadyAndLeadIn()
    {
        yield return new WaitUntil(() => ((music.clip != null && spawnManager.IsReady)));
        yield return new WaitForSeconds((float)Beatmap.General.AudioLeadIn / 1000);
        MessageBox.Show($"現在播放{Beatmap.Metadata.TrueTitle}");
        music.Play();
        HitObject.SetActive(true);
    }

    private Evaluation GetEvaluation(HitObject hitObject)
    {
        foreach (var e in evaluations)
        {
            if (hitObject.InTolerance(e.Tolerance))
            {
                return e;
            }
        }
        return new Evaluation("", -1, 0);
    }


}