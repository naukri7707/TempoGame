using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IHold
{
    int Track { get; set; }

    int StartTime { get; set; }

    int EndTime { get; set; }
}