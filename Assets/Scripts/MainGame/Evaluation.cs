using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Evaluation
{
    public Evaluation(int score, float tolerance)
    {
        Score = score;
        Tolerance = tolerance;
    }

    public int Score { get; }

    public float Tolerance { get; }

    public bool IsInTolerance(float positton) => 0F.InRange(positton - Tolerance, positton + Tolerance);

    public bool IsInTolerance(float bottom, float top) => 0F.InRange(bottom - Tolerance, top + Tolerance);

    public static Evaluation Perfect { get; } = new Evaluation(3000, 30);

    public static Evaluation Great { get; } = new Evaluation(2000, 50);

    public static Evaluation Good { get; } = new Evaluation(1000, 70);

    public static Evaluation Bad { get; } = new Evaluation(500, 80);

    public static Evaluation Miss { get; } = new Evaluation(0, -1);

    /// <summary>
    /// 取得得分
    /// </summary>
    /// <param name="hit">目標物件</param>
    /// <returns></returns>
    public static int GetScore(float positton)
    {
        if (Perfect.IsInTolerance(positton))
        {
            return Perfect.Score;
        }
        else if (Great.IsInTolerance(positton))
        {
            return Great.Score;
        }
        else if (Good.IsInTolerance(positton))
        {
            return Good.Score;
        }
        else
        {
            return Bad.Score;
        }
    }

}
