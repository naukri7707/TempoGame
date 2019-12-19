using UnityEngine;
using UnityEngine.UI;

public class HitEffectBehaviour : MonoBehaviour
{
    public bool Enable { get; set; } = false;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Image roundImage;

    // Update is called once per frame
    void Update()
    {
        if (Enable && (Time.frameCount & 3) == 0)
        {
            image.color = new Color(1, 1, 1, image.color.a == 1 ? 0.9F : 1);
            roundImage.color = new Color(1, 1, 1, image.color.a / 3);
        }
        else
        {
            image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, 0, Time.deltaTime * 10));
            roundImage.color = new Color(1, 1, 1, Mathf.Lerp(roundImage.color.a, 0, Time.deltaTime * 5));
        }
    }
}
