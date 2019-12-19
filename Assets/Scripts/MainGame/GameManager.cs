using Naukri.Beatmap;
using Naukri.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// TODO 預設 delay 時間 (某些歌直接開始)
// TODO Hold Object
// TODO Score & Combo
// TODO 沒有HitObject 後漸按結算
// TODO UI
// TODO 校準介面

public class GameManager : Singleton<GameManager>
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

    [SerializeField]
    private Arduino arduino;

    [SerializeField]
    private GameObject game;

    [SerializeField]
    private GameObject pause;

    [SerializeField]
    private GameObject end;

    [SerializeField]
    private Image background;

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private Text textAddScore;

    [SerializeField]
    private Text textScore;

    [SerializeField]
    private Text textPercent;

    /// <summary>
    /// beatmap 最少等待時間 (毫秒)
    /// </summary>
    public const int delay = 4000;

    /// <summary>
    /// 顯示分數
    /// </summary>
    private int showScore = 0;

    /// <summary>
    /// 實際分數
    /// </summary>
    private int score = 0;

    private int maxScore = 0;

    private int perfectCount = 0;

    private int greatCount = 0;

    private int goodCount = 0;

    private int badCount = 0;

    private int missCount = 0;


    static GameManager()
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            Tracks[i] = new Queue<HitObject>();
        }
    }

    public static bool AIPlay { get; set; } = false;

    public static int Score => Instance.score;

    private float showScorePercent = 0;

    public static Beatmap Beatmap { get; set; }

    public static float Speed { get; } = 8f;

    public static Queue<HitObject>[] Tracks { get; } = new Queue<HitObject>[4];

    public static Queue<HitObject> Removeable { get; } = new Queue<HitObject>();

    private void Awake()
    {
        Application.targetFrameRate = -1;
        // GameArgs.SelectedBeatmap = ThisGame;
        HitObject.SetActive(false);
        StartCoroutine(music.SetAudioExternalAsync(GameArgs.SelectedBeatmap.MusicPath));
        Beatmap = new Beatmap(GameArgs.SelectedBeatmap.OsuPath);
        background.SetSpriteExternal(GameArgs.SelectedBeatmap.BackgroundPath);
        _ = WaitUntilReadyAndLeadIn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (HitObject.IsActive)
            {
                music.Pause();
                HitObject.SetActive(false);
                pause.SetActive(true);
            }
            else
            {
                music.UnPause();
                HitObject.SetActive(true);
                pause.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            AIPlay = !AIPlay;
        }
    }


    private void LateUpdate()
    {
        // 將過期(絕對為miss)的hitObject 轉移至 removeable 等待超出畫面時移除
        foreach (var t in Tracks)
        {
            while (t.Any() && t.Peek().IsOver())
            {
                Removeable.Enqueue(t.Dequeue());
            }
        }
        if (AIPlay)
        { 
            arduino.SetStates(StatesInfoFromAI());
        }
        // 判斷
        for (int i = 0; i < 4; i++)
        {
            if (Tracks[i].Any())
            {
                Tracks[i].Peek().OnFocus(Arduino.Buttons[i]);
            }
        }
        while (Removeable.Any() && Removeable.Peek().Top < -500) // TODO throw magic number
        {
            Destroy(Removeable.Dequeue().gameObject);
        }

        if (HitObject.Count == 0)
        {
            _ = EndGame();
        }

        showScore = Convert.ToInt32(Mathf.Lerp(showScore, score, 2 * Time.deltaTime));
        if (score - showScore < 100)
            showScore = score;
        textScore.text = showScore.ToString("00000000");

        showScorePercent = maxScore == 0 ? 100 : Mathf.Lerp(showScorePercent, (float)(score * 100) / maxScore, 2 * Time.deltaTime);
        textPercent.text = showScorePercent.ToString("00.00") + "%";
    }

    public void AddScore(int score)
    {
        this.score += score;
        maxScore += Evaluation.Perfect.Score;
        if (score == Evaluation.Perfect.Score)
        {
            perfectCount++;
            textAddScore.text = "Perfect!!";
        }
        else if (score == Evaluation.Great.Score)
        {
            greatCount++;
            textAddScore.text = "Great!";
        }
        else if (score == Evaluation.Good.Score)
        {
            goodCount++;
            textAddScore.text = "Good";
        }
        else if (score == Evaluation.Bad.Score)
        {
            badCount++;
            textAddScore.text = "Bad";
        }
        else
        {
            missCount++;
            textAddScore.text = "Miss";
        }
    }

    // TODO LeadIn 上移作時間補正，載入畫面
    private async Task WaitUntilReadyAndLeadIn()
    {
        // 載入資源
        await new WaitUntil(() => music.clip != null && spawnManager.IsReady);
        MessageBox.Show($"現在播放{Beatmap.Metadata.TrueTitle}");
        // 開始beatmap
        HitObject.SetActive(true);
        // 等候預等待時間後撥放音樂
        await Task.Delay(spawnManager.MusicDelay);
        music.Play();
    }

    private async Task EndGame()
    {
        // 設為非0值避免多次載入
        HitObject.Count = -1;
        var gameCG = game.GetComponent<CanvasGroup>();
        var endCG = end.GetComponent<CanvasGroup>();
        var endPanel = end.GetComponent<EndPanel>();
        float completion = Score * 100F / maxScore;
        void setPanel()
        {
            endPanel.Settle(
                (int)Mathf.Lerp(0, Score, endCG.alpha),
                Mathf.Lerp(0, completion, endCG.alpha),
                (int)Mathf.Lerp(0, perfectCount, endCG.alpha),
                (int)Mathf.Lerp(0, greatCount, endCG.alpha),
                (int)Mathf.Lerp(0, goodCount, endCG.alpha),
                (int)Mathf.Lerp(0, badCount, endCG.alpha),
                (int)Mathf.Lerp(0, missCount, endCG.alpha)
                );
        }
        end.SetActive(true);
        endCG.alpha = 0;
        while (gameCG.alpha > 0)
        {
            gameCG.alpha -= Time.deltaTime;
            endCG.alpha += Time.deltaTime * 0.5F;
            setPanel();
            await new WaitForUpdate();
        }
        while (endCG.alpha < 1)
        {
            endCG.alpha += Time.deltaTime * 0.5F;
            setPanel();
            await new WaitForUpdate();
        }
    }

    public int StatesInfoFromAI()
    {
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            if (!Tracks[i].Any()) continue;
            var h = Tracks[i].Peek();
            if (h.Top >= -Evaluation.Bad.Tolerance)
            {
                if (h is NoteObject)
                {
                    if (Evaluation.Perfect.IsInTolerance(h.Bottom))
                    {
                        res += 1 << i;
                    }
                }
                else if (h is HoldObject)
                {
                    if (Evaluation.Perfect.IsInTolerance(h.Bottom, h.Top))
                    {
                        res += 1 << i;
                    }
                }
            }
        }
        return res;
    }
}