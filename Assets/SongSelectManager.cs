using Naukri.ExtensionMethods;
using Naukri.OsuAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectManager : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public Image Image => image;

    public Song currentSong;

    // Start is called before the first frame update
    void Start()
    {
        currentSong = new Song($@"{Application.streamingAssetsPath}/Songs/Shoukaihan/Orangestar feat.IA - Asu no Yozora Shoukaihan (dakemoto) [Normal].osu");
        image.SetSprite($@"D:\Users\Naukri\Desktop\301331 Orangestar feat.IA - Asu no Yozora Shoukaihan\Honeyview_background.png");
    }

    // Update is called once per frame
    void Update()
    {

    }

}


