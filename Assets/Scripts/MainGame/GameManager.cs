using Naukri.Beatmap;
using Naukri.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private enum GameScene
    {
        Gaming,
        Pause,
        End
    }

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
    private Text textEvluation;

    [SerializeField]
    private Text textScore;

    [SerializeField]
    private Text textPercent;

    [SerializeField]
    private Text Combo;

    [SerializeField]
    private Text ComboEffect;

    private GameScene gameScene;

    /// <summary>
    /// beatmap 最少等待時間 (毫秒)
    /// </summary>
    public const int delay = 4000;

    /// <summary>
    /// 顯示分數
    /// </summary>
    private long showScore = 0;

    private int comboTarget = 25;

    const long MAXSCORE = 100000000;

    int bonus = 0;

    int maxBonus = 0;

    /// <summary>
    /// 實際分數
    /// </summary>
    private long score = 0;

    private long maxScore = 0;

    private int perfectCount = 0;

    private int greatCount = 0;

    private int goodCount = 0;

    private int badCount = 0;

    private int missCount = 0;

    /// <summary>
    /// 玩家最大 Combo
    /// </summary>
    public int playerMaxCombo { get; set; } = 0;

    /// <summary>
    /// 玩家當前 Combo
    /// </summary>
    public int combo { get; set; } = 0;

    public int hitCount { get; set; } = 0;

    public int TotalNote { get; set; } = 0;

    static GameManager()
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            Tracks[i] = new Queue<HitObject>();
        }
    }

    private static Color textEvaluationSourceColor = new Color();

    public static bool AIPlay { get; set; } = false;

    public static long Score => Instance.score;

    private float showScorePercent = 0;

    public static Beatmap Beatmap { get; set; }

    public static float Speed { get; } = 8f;

    public static Queue<HitObject>[] Tracks { get; } = new Queue<HitObject>[4];

    public static Queue<HitObject> Removeable { get; } = new Queue<HitObject>();

    private void Awake()
    {
        if (GameArgs.SelectedBeatmap.OsuFile == null)
        {
            SceneManager.LoadScene(0);
            return;
        }
        Application.targetFrameRate = -1;
        textEvaluationSourceColor = textEvluation.color;
        HitObject.SetActive(false);
        StartCoroutine(music.SetAudioExternalAsync(GameArgs.SelectedBeatmap.MusicPath));
        Beatmap = new Beatmap(GameArgs.SelectedBeatmap.OsuPath);
        background.SetSpriteExternal(GameArgs.SelectedBeatmap.BackgroundPath);
        _ = WaitUntilReadyAndLeadIn();
    }

    private void LateUpdate()
    {
        switch (gameScene)
        {
            case GameScene.Gaming:
                // 將過期(絕對為miss)的hitObject 轉移至 removeable 等待超出畫面時移除
                foreach (var t in Tracks)
                {
                    while (t.Any() && t.Peek().IsOver())
                    {
                        Removeable.Enqueue(t.Dequeue());
                    }
                }
                //按鍵事件
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    music.Pause();
                    HitObject.SetActive(false);
                    pause.SetActive(true);
                    gameScene = GameScene.Pause;
                }
                KeyInfo.UpdateStates();
                // 判斷
                for (int i = 0; i < 4; i++)
                {
                    // 設定特效
                    GameArgs.PressEffect[i].enabled = KeyInfo.Buttons[i].State == KeyState.Down || KeyInfo.Buttons[i].State == KeyState.Hold;
                    // 判斷 HitObject
                    if (Tracks[i].Any())
                    {
                        Tracks[i].Peek().OnFocus(KeyInfo.Buttons[i]);
                    }
                }
                while (Removeable.Any() && Removeable.Peek().Top < -10) // 當Top在比畫面在下面一點時刪除以保證完全脫離畫面
                {
                    Destroy(Removeable.Dequeue().gameObject);
                }

                if (HitObject.Count == 0 && !Removeable.Any())
                {
                    _ = EndGame();
                }

                showScore = (int)Mathf.Lerp(showScore, score, 20 * Time.deltaTime);
                if (score - showScore < 100)
                    showScore = score;
                textScore.text = showScore.ToString("00000000");

                showScorePercent = maxScore == 0 ? 100 : Mathf.Lerp(showScorePercent, (float)(score * 100) / maxScore, 20 * Time.deltaTime);
                textPercent.text = showScorePercent.ToString("00.00") + "%";
                Combo.text = combo.ToString();
                if (combo >= comboTarget)
                {
                    ShowComboEffect();
                }
                if (textEvluation.transform.localScale.x > 0.9)
                {
                    textEvluation.transform.localScale -= new Vector3(0.5F, 0.5F, 0) * Time.deltaTime;
                }
                else
                {
                    textEvluation.color -= new Color(0, 0, 0, 0.5F * Time.deltaTime);
                }
                break;
            case GameScene.Pause:
                void SelectPause()
                {
                    switch (PausePanel.CurrentSelection)
                    {
                        case 0:
                            music.UnPause();
                            HitObject.SetActive(true);
                            pause.SetActive(false);
                            gameScene = GameScene.Gaming;
                            break;
                        case 1:
                            SceneManager.LoadScene(1);
                            break;
                        case 2:
                            SceneManager.LoadScene(0);
                            break;
                    }
                }
                if (GameArgs.OperatingMode == OperatingMode.Arduino)
                {
                    KeyInfo.UpdateStates();
                    if (KeyInfo.Buttons[0] == KeyState.Down || KeyInfo.Buttons[1] == KeyState.Down)
                    {
                        SelectPause();
                    }
                    if (KeyInfo.Buttons[2] == KeyState.Down)
                    {
                        PausePanel.CurrentSelection++;
                    }
                    else if (KeyInfo.Buttons[3] == KeyState.Down)
                    {
                        PausePanel.CurrentSelection--;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        music.UnPause();
                        HitObject.SetActive(true);
                        pause.SetActive(false);
                        gameScene = GameScene.Gaming;
                    }
                    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                    {
                        SelectPause();
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PausePanel.CurrentSelection++;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PausePanel.CurrentSelection--;
                    }
                }
                break;
            case GameScene.End:
                void SelectEnd()
                {
                    switch (EndPanel.CurrentSelection)
                    {
                        case 0:
                            SceneManager.LoadScene(1);
                            break;
                        case 1:
                            SceneManager.LoadScene(0);
                            break;
                    }
                }
                if (GameArgs.OperatingMode == OperatingMode.Arduino)
                {
                    KeyInfo.UpdateStates();
                    if (KeyInfo.Buttons[0] == KeyState.Down || KeyInfo.Buttons[1] == KeyState.Down)
                    {
                        SelectEnd();
                    }
                    if (KeyInfo.Buttons[2] == KeyState.Down)
                    {
                        EndPanel.CurrentSelection++;
                    }
                    else if (KeyInfo.Buttons[3] == KeyState.Down)
                    {
                        EndPanel.CurrentSelection--;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                    {
                        SelectEnd();
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        EndPanel.CurrentSelection++;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        EndPanel.CurrentSelection--;
                    }
                }
                break;
        }
    }

    private async void ShowComboEffect()
    {
        ComboEffect.gameObject.SetActive(true);
        ComboEffect.text = comboTarget.ToString();
        comboTarget += 25;
        ComboEffect.color = Combo.color + new Color(0, 0, 0, Combo.color.a);
        ComboEffect.fontSize = (int)(Combo.fontSize * 0.9);
        while (ComboEffect.color.a > 0)
        {
            ComboEffect.fontSize += 6;
            ComboEffect.color -= new Color(0, 0, 0, 0.5F * Time.deltaTime);
            await new WaitForUpdate();
        }
        ComboEffect.gameObject.SetActive(false);
    }

    public void AddScore(Evaluation evl)
    {
        // int baseScore = (MAXSCORE / 2) / TotalNote * (evl.HitValue / Evaluation.Perfect.HitValue);
        // int bonusScore = (MAXSCORE / 2) / TotalNote * (evl.HitBonusValue * (int)Mathf.Sqrt(bonus) / Evaluation.Perfect.HitValue);
        // bonus = Mathf.Clamp(bonus + evl.HitBonus, 0, 100);
        score += MAXSCORE * (long)(evl.HitValue + evl.HitBonusValue * Mathf.Sqrt(bonus)) / (Evaluation.Perfect.HitValue * TotalNote * 2);
        maxScore += MAXSCORE * (long)(Evaluation.Perfect.HitValue + Evaluation.Perfect.HitBonusValue * Mathf.Sqrt(maxBonus)) / (Evaluation.Perfect.HitValue * TotalNote * 2);
        bonus = Mathf.Clamp(bonus + evl.HitBonus, 0, 100);
        maxBonus = Mathf.Clamp(maxBonus + Evaluation.Perfect.HitBonus, 0, 100);
        if (evl.HitValue < Evaluation.Good.HitValue)
        {
            if (playerMaxCombo < combo)
            {
                playerMaxCombo = combo;
            }
            combo = 0;
            comboTarget = 25;
        }
        else
        {
            combo++;
        }
        if (evl.HitValue == Evaluation.Perfect.HitValue)
        {
            perfectCount++;
            textEvluation.text = "Perfect!!";
        }
        else if (evl.HitValue == Evaluation.Great.HitValue)
        {
            greatCount++;
            textEvluation.text = "Great!";
        }
        else if (evl.HitValue == Evaluation.Good.HitValue)
        {
            goodCount++;
            textEvluation.text = "Good";
        }
        else if (evl.HitValue == Evaluation.Bad.HitValue)
        {
            badCount++;
            textEvluation.text = "Bad";
        }
        else
        {
            missCount++;
            textEvluation.text = "Miss";
        }
        textEvluation.transform.localScale = new Vector3(1, 1, 1);
        textEvluation.color = textEvaluationSourceColor;
    }

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
        if (playerMaxCombo < combo)
        {
            playerMaxCombo = combo;
        }
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
                (int)Mathf.Lerp(0, missCount, endCG.alpha),
                (int)Mathf.Lerp(0, playerMaxCombo, endCG.alpha)
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
        gameScene = GameScene.End;
        while (endCG.alpha < 1)
        {
            endCG.alpha += Time.deltaTime * 0.5F;
            setPanel();
            await new WaitForUpdate();
        }
    }
}