using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Top == Bottom
class NoteObject : HitObject
{
    public override bool IsOver()
    {
        if (Top < -Evaluation.Bad.Tolerance)
        {
            if (!IsSettle)
            {
                Score = Evaluation.Miss.Score;
                Settle();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnFocus(ButtonState state)
    {
        // 如果按鈕按下且進入判定範圍
        if (state == ButtonState.Down && Evaluation.Bad.IsInTolerance(Bottom))
        {
            GameArgs.HitEffect[Track].Enable = true;
            _ = disableEffect();
            Score = Evaluation.GetScore(Top);
            Settle();
            GameManager.Tracks[Track].Dequeue();
            Destroy(gameObject);
        }
    }

    private async Task disableEffect()
    {
        await Task.Delay(100);
        GameArgs.HitEffect[Track].Enable = false;
    }
}