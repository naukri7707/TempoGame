using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    private int prevSelection = 1;

    private static int currentSelection = 1;

    [SerializeField]
    private Text[] Selections = new Text[2];

    public static int CurrentSelection { get => currentSelection; set => currentSelection = value > 1 ? 0 : value < 0 ? 1 : value; }

    private void Start()
    {
        CurrentSelection = 1;
        prevSelection = 1;
        Selections[1].color = Color.yellow;
    }

    private void Update()
    {
        if (prevSelection != CurrentSelection)
        {
            foreach (var s in Selections)
            {
                s.color = Color.white;
            }
            Selections[CurrentSelection].color = Color.yellow;
            prevSelection = CurrentSelection;
        }
    }
}
