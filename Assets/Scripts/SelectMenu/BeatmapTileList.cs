using Mono.Data.Sqlite;
using Naukri.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BeatmapTileList : MonoBehaviour
{
    public const int OffsetRange = 8;

    [SerializeField]
    private AudioSource musicPreview;

    [SerializeField]
    private Image background;

    [SerializeField]
    private GameObject beatmapTilePrefab;

    private int currentBeatmapIndex;

    /// <summary>
    /// 音樂磚串列
    /// </summary>
    public static LinkedList<BeatmapTile> BeatmapTiles { get; private set; }

    /// <summary>
    /// 歌曲資料表
    /// </summary>
    public static List<BeatmapTileData> BeatmapTileDatas { get; private set; }

    /// <summary>
    /// 當前歌曲包 (中間)
    /// </summary>
    public int CurrentBeatmapIndex
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
        var theard = new Thread(ExtractOsz);
        theard.Start();
        // 監聽解壓執行緒
        StartCoroutine(ShowLazyMessage(theard));
        // 產生圖譜表
        CreateBeatmapTileList();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShiftUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShiftDown();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SelectSong();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    private void CreateBeatmapTileList()
    {
        if (BeatmapTileDatas.Count == 0 || IsBeatmapTileListCreated == true)
        {
            return;
        }
        CurrentBeatmapIndex = 0;
        PlayCurrentMusic();
        for (int i = -OffsetRange; i <= OffsetRange; i++)
        {
            var newBeatmapTile = InstantiateBeatmapTile(i);
            BeatmapTiles.AddLast(newBeatmapTile);
            if (i == 0)
            {
                background.sprite = newBeatmapTile.Image.sprite;
            }
            newBeatmapTile.GetComponent<RectTransform>().Translate(newBeatmapTile.transform.position.x * (Mathf.Abs(i) + 1.5F), 0, 0);
        }
        IsBeatmapTileListCreated = true;
    }

    private void ShiftUp()
    {
        int prevBeatmapSet = BeatmapTileDatas[CurrentBeatmapIndex].BeatmapSetID;
        CurrentBeatmapIndex--;
        if (BeatmapTileDatas[CurrentBeatmapIndex].BeatmapSetID != prevBeatmapSet)
        {
            PlayCurrentMusic();
        }
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
        int prevBeatmapSet = BeatmapTileDatas[CurrentBeatmapIndex].BeatmapSetID;
        CurrentBeatmapIndex++;
        if (BeatmapTileDatas[CurrentBeatmapIndex].BeatmapSetID != prevBeatmapSet)
        {
            PlayCurrentMusic();
        }
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

    private void SelectSong()
    {
        GameArgs.SelectedBeatmap = BeatmapTileDatas[CurrentBeatmapIndex];
        LoadingScene.LoadScene(3);
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
    /// 播放當前 beatmap 音樂
    /// </summary>
    private void PlayCurrentMusic()
    {
        StartCoroutine(musicPreview.SetAudioExternalAsync(BeatmapTileDatas[CurrentBeatmapIndex].MusicPath, true));
    }

    /// <summary>
    /// 解壓縮 Osz
    /// </summary>
    private void ExtractOsz()
    {
        var newBeatmap = BeatmapManager.ExtractOszFromPackageFolder();
        if (newBeatmap > 0)
        {
            MessageBox.ShowLazy($"解析完成，共新增了{newBeatmap}張圖譜！");
        }
    }

    private IEnumerator ShowLazyMessage(Thread listen)
    {
        // 當執行緒還在執行時，每秒監聽一次
        while (listen.IsAlive)
        {
            yield return new WaitForFixedUpdate();
            if (BeatmapTileDatas.Count > 0 && IsBeatmapTileListCreated == false)
            {
                MessageBox.Show($"發現資源正在重新載入...");
                CreateBeatmapTileList();
            }
            MessageBox.PushLazy();
        }
    }
}
