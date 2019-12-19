using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class HoldObject : HitObject
{
    [SerializeField]
    private Image[] images = new Image[3];

    public float HoldTime { get; private set; } = 0F;

    public float FullTime => EndTime - StartTime;

    public float CurrentPercent => (Time.time - HoldTime) / FullTime;

    /// <summary>
    /// 按住狀態 0 = 沒按過, 1 = 按下, 2 = 按下後放開
    /// </summary>
    private int HoldState { get; set; } = 0;

    public override bool IsOver()
    {
        if (Top < -Evaluation.Bad.Tolerance)
        {
            //  如果沒有放開到離開Miss範圍，判定為Miss
            if (HoldState != 2)
            {
                GameArgs.HitEffect[Track].Enable = false;
                Score = Evaluation.Miss.Score;
            }
            Settle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnFocus(ButtonState state)
    {
        // 如果在判定範圍且尚未被放開過
        if (Evaluation.Great.IsInTolerance(Bottom, Top) && HoldState != 2)
        {
            switch (state)
            {
                case ButtonState.Down:
                    HoldState = 1;
                    GameArgs.HitEffect[Track].Enable = true;
                    HoldTime = Time.time;
                    break;
                case ButtonState.Hold:
                    break;
                case ButtonState.Up:
                    if (HoldState == 1)
                    {
                        HoldState = 2;
                        GameArgs.HitEffect[Track].Enable = false;
                        foreach (var i in images)
                        {
                            i.color = new Color(i.color.r, i.color.g, i.color.b, 0.5F);
                        }
                        float percent = CurrentPercent;
                        if (percent >= 0.9)
                        {
                            Score = Evaluation.Perfect.Score;
                        }
                        else if (percent >= 0.8)
                        {
                            Score = Evaluation.Great.Score;
                        }
                        else if (percent >= 0.6)
                        {
                            Score = Evaluation.Good.Score;
                        }
                        else
                        {
                            Score = Evaluation.Bad.Score;
                        }
                    }
                    break;
            }
        }
    }
}