using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float Speed { get; } = 8f;

    [SerializeField] private static GameObject _trigger;

    public static GameObject Trigger { get; set; }

    private void Awake()
    {
        Trigger = _trigger;
    }

    void Update()
    {

    }
}
