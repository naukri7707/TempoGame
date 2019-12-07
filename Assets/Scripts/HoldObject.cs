using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HoldObject : IHitObject, IHold
{
    /// <summary>
    /// 初始化物件
    /// </summary>
    /// <param name="track"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    public HoldObject(int track, int startTime, int endTime)
    {
        Track = track;
        StartTime = startTime;
        EndTime = endTime;
    }

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
