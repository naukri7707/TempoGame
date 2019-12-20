using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    private int prevSelection = 0;

    private static int currentSelection = 0;

    [SerializeField]
    private Text[] Selections = new Text[3];

    public static int CurrentSelection { get => currentSelection; set => currentSelection = value > 2 ? 0 : value < 0 ? 2 : value; }

    private void Start()
    {
        CurrentSelection = 0;
        prevSelection = 0;
        Selections[0].color = Color.yellow;
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
