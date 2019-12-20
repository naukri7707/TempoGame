using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField]
    private Text score;

    [SerializeField]
    private Text perfect;

    [SerializeField]
    private Text great;

    [SerializeField]
    private Text good;

    [SerializeField]
    private Text bad;

    [SerializeField]
    private Text miss;

    [SerializeField]
    private Text rank;

    private int prevSelection = 0;

    private static int currentSelection = 0;

    [SerializeField]
    private Text[] Selections = new Text[2];

    public static int CurrentSelection { get => currentSelection; set => currentSelection = value > 1 ? 0 : value < 0 ? 1 : value; }

    private void Start()
    {
        CurrentSelection = 0;
        prevSelection = 0;
        Selections[0].color = Color.yellow;
    }

    private void Update()
    {
        if (prevSelection != CurrentSelection)
        {
            foreach (var s in Selections)
            {
                s.color = Color.white;
            }
            Selections[CurrentSelection].color = Color.yellow;
            prevSelection = CurrentSelection;
        }
    }

    public void Settle(int score, float completion, int perfect, int great, int good, int bad, int miss)
    {
        this.score.text = $"{score.ToString("00000000")} ({completion.ToString("00.00")})";
        this.perfect.text = $"prefect : {perfect}x";
        this.great.text = $"great : {great}x";
        this.good.text = $"good : {good}x";
        this.bad.text = $"bad : {bad}x";
        this.miss.text = $"miss : {miss}x";
        if (completion >= 99.99999F)
        {
            SS();
        }
        if (completion > 95)
        {
            rank.text = "S";
        }
        else if (completion > 90)
        {
            rank.text = "A";
        }
        else if (completion > 80)
        {
            rank.text = "B";
        }
        else if (completion > 70)
        {
            rank.text = "C";
        }
        else if (completion > 60)
        {
            rank.text = "D";
        }
        else
        {
            rank.text = "F";
        }
    }

    private async void SS()
    {
        rank.text = "S";
        var rank2 = Instantiate(rank, rank.transform.parent);
        var rect = rank.GetComponent<RectTransform>();
        var rect2 = rank2.GetComponent<RectTransform>();
        for (int i = 0; i < 60; i++)
        {
            rect.anchoredPosition -= new Vector2(0.4F, 0);
            rect2.anchoredPosition += new Vector2(0.4F, 0);
            rank2.color -= new Color(0, 0, 0, 0.007F);
            await new WaitForUpdate();
        }
    }
}
