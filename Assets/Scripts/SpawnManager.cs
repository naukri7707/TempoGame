using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SpawnManager
{
    [SerializeField] private GameObject _note;

    public GameObject Note { get => _note; set => _note = value; }

    private void Start()
    {
        StreamReader sr = new StreamReader("Assets/IOFile/txtIO.txt");
        string s = sr.ReadLine();
        // TODO Analysis
    }
    private void Analysis(string sheet)
    {

    }
}
