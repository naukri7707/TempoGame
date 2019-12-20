using UnityEngine;
using UnityEngine.UI;

// TODO Lucky Star Bug Fix (檔案太大?檔名? => 格式化錯誤

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform hitObjectContainer;

    [SerializeField]
    private Image meshLinePrefab;

    [SerializeField]
    private Image pressEffectPrefab;

    [SerializeField]
    private GameObject hitEffectPrefab;

    [SerializeField]
    private GameObject separatePrefab;

    [SerializeField]
    private GameObject holdObjectPrefab;

    [SerializeField]
    private GameObject noteObjectPrefab;

    public bool IsReady { get; set; }

    /// <summary>
    /// 音樂等待時間 (毫秒)
    /// </summary>
    public int MusicDelay { get; set; }

    private void Start()
    {
        InstantisteHitObjects();
        SetEffect();
    }

    /// <summary>
    /// 取得Beatmap
    /// </summary>
    /// <param name="beatmapID">Beatmap ID</param>
    private void InstantisteHitObjects()
    {
        int trueLeadIn = GameManager.Beatmap.General.AudioLeadIn + GameManager.Beatmap.HitObjects[0].Time; // 第一個 hitObject 的實際時間
        MusicDelay = Mathf.Max(GameManager.delay - trueLeadIn, 0);
        int leadIn = GameManager.Beatmap.General.AudioLeadIn + MusicDelay;
        InstantisteSeparate(Mathf.Max(trueLeadIn, MusicDelay));
        HitObject.Count = 0;
        GameManager.Instance.TotalNote = 0;
        foreach (var t in GameManager.Tracks)
        {
            t.Clear();
        }
        IsReady = false;
        foreach (var hit in GameManager.Beatmap.HitObjects.Collection)
        {
            HitObject newHit;
            if (hit.Type == 1)
            {
                newHit = Instantiate(noteObjectPrefab, hitObjectContainer).GetComponent<NoteObject>();
            }
            else if (hit.Type == 128)
            {
                newHit = Instantiate(holdObjectPrefab, hitObjectContainer).GetComponent<HoldObject>();
            }
            else
            {
                continue;
            }
            newHit.Track = Mathf.FloorToInt(hit.X * 4 / 512);
            newHit.StartTime = (float)(leadIn + hit.Time) / 1000;
            newHit.EndTime = (float)(leadIn + hit.EndTime) / 1000;
            GameManager.Tracks[newHit.Track].Enqueue(newHit);
            GameManager.Instance.TotalNote++;
        }
        IsReady = true;
    }

    /// <summary>
    /// 產生分隔符號
    /// </summary>
    /// <param name="delay">延遲 (毫秒)</param>
    public void InstantisteSeparate(int delay)
    {
        delay /= 1000;
        for (int d = 1; d <= delay; d++)
        {
            SeparateObject sep = Instantiate(separatePrefab, hitObjectContainer).GetComponent<SeparateObject>();
            sep.Track = 4;
            sep.StartTime = d;
            sep.EndTime = d;
        }
    }

    private void SetEffect()
    {
        // Set Mesh
        foreach (var x in GameArgs.MeshPosX)
        {
            var mesh = Instantiate(meshLinePrefab, transform.parent);
            var rect = mesh.GetComponent<RectTransform>();
            rect.anchoredPosition += new Vector2(x, 0);
        }
        // Set Effect
        for (int i = 0; i < 4; i++)
        {
            var press = Instantiate(pressEffectPrefab, transform);
            var pressRect = press.GetComponent<RectTransform>();
            pressRect.anchoredPosition += new Vector2(GameArgs.TrackPosX[i], 0);
            press.enabled = false;
            GameArgs.PressEffect[i] = press;
            var hit = Instantiate(hitEffectPrefab, transform);
            var hitRect = hit.GetComponent<RectTransform>();
            hitRect.anchoredPosition += new Vector2(GameArgs.TrackPosX[i], 0);
            GameArgs.HitEffect[i] = hit.GetComponent<HitEffectBehaviour>();
        }
    }
}
