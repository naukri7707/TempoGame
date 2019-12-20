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
                Evaluation = Evaluation.Miss;
                Settle();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnFocus(KeyState state)
    {
        // 如果按鈕按下且進入判定範圍
        if (state == KeyState.Down && Evaluation.Bad.IsInTolerance(Bottom))
        {
            GameArgs.HitEffect[Track].Enable = true;
            _ = disableEffect();
            Evaluation = Evaluation.GetScore(Top);
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