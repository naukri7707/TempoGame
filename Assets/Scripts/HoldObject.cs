using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HoldObject : IHitObject, IHold
{
    /// <summary>
    /// 軌道
    /// </summary>
    public int Track { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public int StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public int EndTime { get; set; }

}
