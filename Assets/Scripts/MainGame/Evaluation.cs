using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Evaluation
{
    public Evaluation(int hitValue, int hitBonusValue, int hitBonus, float tolerance)
    {
        HitValue = hitValue;
        HitBonusValue = hitBonusValue;
        HitBonus = hitBonus;
        Tolerance = tolerance;
    }

    public int HitValue { get; }

    public int HitBonusValue { get; }

    public int HitBonus { get; }

    public float Tolerance { get; }

    public bool IsInTolerance(float positton) => 0F.InRange(positton - Tolerance, positton + Tolerance);

    public bool IsInTolerance(float bottom, float top) => 0F.InRange(bottom - Tolerance, top + Tolerance);

    public static Evaluation Perfect { get; } = new Evaluation(3000, 32, 2, 50);

    public static Evaluation Great { get; } = new Evaluation(2000, 16, 1, 90);

    public static Evaluation Good { get; } = new Evaluation(1000, 8, -1, 120);

    public static Evaluation Bad { get; } = new Evaluation(500, 4, -2, 150);

    public static Evaluation Miss { get; } = new Evaluation(0, 0, -4, -1);

    /// <summary>
    /// 取得得分
    /// </summary>
    /// <param name="hit">目標物件</param>
    /// <returns></returns>
    public static Evaluation GetScore(float positton)
    {
        if (Perfect.IsInTolerance(positton))
        {
            return Perfect;
        }
        else if (Great.IsInTolerance(positton))
        {
            return Great;
        }
        else if (Good.IsInTolerance(positton))
        {
            return Good;
        }
        else
        {
            return Bad;
        }
    }

}
