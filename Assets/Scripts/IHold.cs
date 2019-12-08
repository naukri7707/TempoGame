using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IHold
{
    /// <summary>
    /// 軌道
    /// </summary>
    int Track { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    float StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    float EndTime { get; set; }
}