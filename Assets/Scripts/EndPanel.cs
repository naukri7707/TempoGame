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

    public void Settle(int score, float completion, int perfect, int great, int good, int bad, int miss)
    {
        this.score.text = $"{score.ToString("00000000")} ({completion.ToString("00.00")})";
        this.perfect.text = $"prefect : {perfect}x";
        this.great.text = $"great : {great}x";
        this.good.text = $"good : {good}x";
        this.bad.text = $"bad : {bad}x";
        this.miss.text = $"miss : {miss}x";
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
}
