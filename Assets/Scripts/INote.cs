using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INote
{
    int Track { get; set; }

    int TimeCode { get; set; }
}
