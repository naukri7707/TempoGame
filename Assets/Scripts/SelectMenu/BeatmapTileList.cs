using Mono.Data.Sqlite;
using Naukri.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeatmapTileList : MonoBehaviour
{
    public const int OffsetRange = 8;

    [SerializeField]
    private GameObject exitPanel;

    [SerializeField]
    private AudioSource musicPreview;

    [SerializeField]
    private Image background;

    [SerializeField]
    private GameObject beatmapTilePrefab;

    private static int currentBeatmapIndex = -1;

    private bool ShiftDowning = false;

    private bool ShiftDownEnd = true;

    private bool ShiftUping = false;

    private bool ShiftUpEnd = true;

    private int fisrtShiftTime = 400;

    private int loopShiftTime = 150;

    private bool extracting = false;

    /// <summary>
    /// 音樂磚串列
    /// </summary>
    public static LinkedList<BeatmapTile> BeatmapTiles { get; private set; }

    /// <summary>
    /// 歌曲資料表
    /// </summary>
    public static List<BeatmapTileData> BeatmapTileDatas { get; private set; }

    public static LinkedListNode<BeatmapTile> CurrentBeatmapTileNode { get; private set; }

    /// <summary>
    /// 當前歌曲包 (中間)
    /// </summary>
    public static int CurrentBeatmapIndex
    {
        get => currentBeatmapIndex;
        set
        {
            if (value < 0)
            {
                value = -value % BeatmapTileDatas.Count;
                currentBeatmapIndex = value == 0 ? 0 : BeatmapTileDatas.Count - value;
            }
            else if (value >= BeatmapTileDatas.Count)
            {
                currentBeatmapIndex = value % BeatmapTileDatas.Count;
            }
            else
            {
                currentBeatmapIndex = value;
            }
        }
    }

    public bool IsBeatmapTileListCreated { get; private set; } = false;

    private void Awake()
    {
        BeatmapTiles = new LinkedList<BeatmapTile>();
        BeatmapTileDatas = new List<BeatmapTileData>();
        // data
        using (var gameData = new SqliteManager(GameArgs.GameDataPath) { Table = GameArgs.BeatmapList })
        {
            gameData.CreateTable(
                "BeatmapList",
                new SQLiteField("BeatmapID", SQLiteDataType.INTEGER, SqlKeyWord.PRIMARY_KEY),
                new SQLiteField("BeatmapSetID", SQLiteDataType.INTEGER, SqlKeyWord.NOT_NULL),
                new SQLiteField("Title", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("TitleUnicode", SQLiteDataType.TEXT),
                new SQLiteField("Artist", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("ArtistUnicode", SQLiteDataType.TEXT),
                new SQLiteField("Creator", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("Version", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("PackageName", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("BackgroundFile", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("MusicFile", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL),
                new SQLiteField("OsuFile", SQLiteDataType.TEXT, SqlKeyWord.NOT_NULL)
                );
            using (var dr = gameData.SelectAllOrderBy(
                new KeyValuePair<string, bool>("BeatmapSetID", true),
                new KeyValuePair<string, bool>("BeatmapID", true)
                ))
            {
                int beatmapCount = 0;
                while (dr.Read())
                {
                    beatmapCount++;
                    BeatmapTileDatas.Add(new BeatmapTileData(
                        dr.GetInt32(0),
                        dr.GetInt32(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        dr.GetString(4),
                        dr.GetString(5),
                        dr.GetString(6),
                        dr.GetString(7),
                        dr.GetString(8),
                        dr.GetString(9),
                        dr.GetString(10),
                        dr.GetString(11)
                        ));
                }
                dr.Close();
                if (beatmapCount == 0)
                {
                    background.sprite = Resources.Load<Sprite>(@"Sprites/NoBeatmap");
                    background.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    MessageBox.Show($"共載入{beatmapCount}張圖譜");
                }
            }
        }
        // 開一個解壓縮用的執行緒
        ExtractOsz();
        // 產生圖譜表
        CreateBeatmapTileList();
    }

    private void Update()
    {
        if (exitPanel.activeSelf == false)
        {
            if (GameArgs.OperatingMode == OperatingMode.Arduino)
            {
                KeyInfo.UpdateStates();
                if (KeyInfo.Buttons[0] == KeyState.Down || KeyInfo.Buttons[1] == KeyState.Down)
                {
                    SelectSong();
                }
                if (KeyInfo.Buttons[2] == KeyState.Down)
                {
                    ShiftDowning = true;
                    AutoShiftDown();
                }
                else if (KeyInfo.Buttons[2] == KeyState.Up)
                {
                    ShiftDowning = false;
                }
                else if (KeyInfo.Buttons[3] == KeyState.Down)
                {
                    ShiftUping = true;
                    AutoShiftUp();
                }
                else if (KeyInfo.Buttons[3] == KeyState.Up)
                {
                    ShiftUping = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                {
                    SelectSong();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ShiftDowning = true;
                    AutoShiftDown();
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    ShiftDowning = false;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ShiftUping = true;
                    AutoShiftUp();
                }
                else if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    ShiftUping = false;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exitPanel.SetActive(true);
                }
            }
        }
        else
        {
            if (GameArgs.OperatingMode == OperatingMode.Arduino)
            {
                KeyInfo.UpdateStates();
                if (KeyInfo.Buttons[0] == KeyState.Down || KeyInfo.Buttons[1] == KeyState.Down)
                {
                    if (ExitPanel.CurrentSelection == 0)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        exitPanel.SetActive(false);
                    }
                }
                if (KeyInfo.Buttons[2] == KeyState.Down)
                {
                    ExitPanel.CurrentSelection++;
                }
                else if (KeyInfo.Buttons[3] == KeyState.Down)
                {
                    ExitPanel.CurrentSelection--;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                {
                    if (ExitPanel.CurrentSelection == 0)
                    {
                        Application.Quit(); 
                    }
                    else
                    {
                        exitPanel.SetActive(false);
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ExitPanel.CurrentSelection++;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ExitPanel.CurrentSelection--;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exitPanel.SetActive(false);
                }
            }
        }
    }

    private void CreateBeatmapTileList()
    {
        if (BeatmapTileDatas.Count == 0 || IsBeatmapTileListCreated == true)
        {
            return;
        }
        if (CurrentBeatmapIndex == -1)
        {
            CurrentBeatmapIndex = Random.Range(0, BeatmapTileDatas.Count);
        }
        PlayMusic(BeatmapTileDatas[CurrentBeatmapIndex].MusicPath);
        for (int i = -OffsetRange; i <= OffsetRange; i++)
        {
            var newBeatmapTile = InstantiateBeatmapTile(i);
            BeatmapTiles.AddLast(newBeatmapTile);
            if (i == 0)
            {
                CurrentBeatmapTileNode = BeatmapTiles.Last;
                background.sprite = newBeatmapTile.Image.sprite;
            }
            newBeatmapTile.GetComponent<RectTransform>().Translate(newBeatmapTile.transform.position.x * (Mathf.Abs(i) + 1.5F), 0, 0);
        }
        IsBeatmapTileListCreated = true;
    }

    private void ShiftUp()
    {
        if (CurrentBeatmapTileNode.Value.BeatmapSetID != CurrentBeatmapTileNode.Previous.Value.BeatmapSetID)
        {
            PlayMusic(CurrentBeatmapTileNode.Previous.Value.MusicPath);
        }
        CurrentBeatmapTileNode = CurrentBeatmapTileNode.Previous;
        CurrentBeatmapIndex--;
        Destroy(BeatmapTiles.Last.Value.gameObject);
        BeatmapTiles.RemoveLast();
        foreach (var s in BeatmapTiles)
        {
            s.Offset++;
            if (s.Offset == 0)
            {
                background.sprite = s.Image.sprite;
            }
        }
        BeatmapTiles.AddFirst(InstantiateBeatmapTile(-OffsetRange));
    }

    private void ShiftDown()
    {
        if (CurrentBeatmapTileNode.Value.BeatmapSetID != CurrentBeatmapTileNode.Next.Value.BeatmapSetID)
        {
            PlayMusic(CurrentBeatmapTileNode.Next.Value.MusicPath);
        }
        CurrentBeatmapTileNode = CurrentBeatmapTileNode.Next;
        CurrentBeatmapIndex++;
        Destroy(BeatmapTiles.First.Value.gameObject);
        BeatmapTiles.RemoveFirst();
        foreach (var s in BeatmapTiles)
        {
            s.Offset--;
            if (s.Offset == 0)
            {
                background.sprite = s.Image.sprite;
            }
        }
        BeatmapTiles.AddLast(InstantiateBeatmapTile(OffsetRange));
    }

    /// <summary>
    /// 自動向下
    /// </summary>
    private async void AutoShiftDown()
    {
        ShiftDown();
        // 避免同時載入多個
        if (!ShiftDownEnd) return;
        ShiftDownEnd = false;
        await Task.Delay(fisrtShiftTime);
        while (ShiftDowning)
        {
            ShiftDown();
            await Task.Delay(loopShiftTime);
        }
        ShiftDownEnd = true;
    }

    /// <summary>
    /// 自動向上
    /// </summary>
    private async void AutoShiftUp()
    {
        ShiftUp();
        // 避免同時載入多個
        if (!ShiftUpEnd) return;
        ShiftUpEnd = false;
        await Task.Delay(fisrtShiftTime);
        while (ShiftUping)
        {
            ShiftUp();
            await Task.Delay(loopShiftTime);
        }
        ShiftUpEnd = true;
    }

    private void SelectSong()
    {
        GameArgs.SelectedBeatmap = BeatmapTileDatas[CurrentBeatmapIndex];
        SceneManager.LoadScene(1);
    }

    private BeatmapTile InstantiateBeatmapTile(int offset)
    {
        var obj = Instantiate(beatmapTilePrefab, transform);
        var st = obj.GetComponent<BeatmapTile>();
        st.SetInfo(offset, BeatmapTileDatas[OffsetIndex(offset)]);
        return st;
    }

    /// <summary>
    /// 取得偏移後的正確索引值
    /// </summary>
    /// <param name="offset">偏移量</param>
    /// <returns></returns>
    public int OffsetIndex(int offset)
    {
        var res = CurrentBeatmapIndex + offset;
        if (res < 0)
        {
            res = -res % BeatmapTileDatas.Count;
            return res == 0 ? 0 : BeatmapTileDatas.Count - res;
        }
        else if (res >= BeatmapTileDatas.Count)
        {
            return res % BeatmapTileDatas.Count;
        }
        else
        {
            return res;
        }
    }

    /// <summary>
    /// 播放 beatmap 音樂
    /// </summary>
    /// <param name="musicPath">音樂路徑</param>
    private void PlayMusic(string musicPath)
    {
        StartCoroutine(musicPreview.SetAudioExternalAsync(musicPath, true));
    }

    /// <summary>
    /// 解壓縮 Osz
    /// </summary>
    private async void ExtractOsz()
    {
        extracting = true;
        ExtractListener();
        int newBeatmap = 0;
        await Task.Run(() =>
        {
            newBeatmap = BeatmapManager.ExtractOszFromPackageFolder();
        });
        if (newBeatmap > 0)
        {
            MessageBox.Show($"解析完成，共新增了{newBeatmap}張圖譜！");
        }
        extracting = false;
    }

    /// <summary>
    /// 解壓縮監測器
    /// </summary>
    private async void ExtractListener()
    {
        // 等待載入首張 beatmap
        if (BeatmapTileDatas.Count == 0)
        {
            while (BeatmapTileDatas.Count == 0)
            {
                await Task.Delay(10);
            }
            MessageBox.Show("發現圖譜，正在重新載入");
            CreateBeatmapTileList();
            MessageBox.PushLazy();
        }
        int count = BeatmapTileDatas.Count;
        while (extracting)
        {
            MessageBox.PushLazy();
            count = BeatmapTileDatas.Count;
            await new WaitUntil(() => count < BeatmapTileDatas.Count);
        }
    }
}
