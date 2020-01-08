using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class KeyInfo
{
    public static KeyButton[] Buttons { get; set; } = new KeyButton[5];

    public static void UpdateStates()
    {
        if (GameArgs.OperatingMode == OperatingMode.KeyBoard)
        {
            SetStates(StatesInfoFromKeyBoard());
        }
        else if (GameArgs.OperatingMode == OperatingMode.Arduino)
        {
            SetStates(StatesInfoFromArduino());
        }
        else
        {
            SetStates(StatesInfoFromAI());
        }
    }

    // 將輸入資訊轉譯成按鈕資訊
    private static void SetStates(int stateInfo)
    {
        for (int i = 0; i < 5; i++)
        {
            Buttons[i].SetState((stateInfo & (1 << i)) != 0);
        }
    }

    // 鍵盤輸入
    private static int StatesInfoFromKeyBoard()
    {
        int res = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
            res |= 1;
        if (Input.GetKey(KeyCode.F) || Input.GetKeyDown(KeyCode.F))
            res |= 2;
        if (Input.GetKey(KeyCode.J) || Input.GetKeyDown(KeyCode.J))
            res |= 4;
        if (Input.GetKey(KeyCode.K) || Input.GetKeyDown(KeyCode.K))
            res |= 8;
        return res;
    }

    // Arduino輸入
    private static int StatesInfoFromArduino()
    {
        if (int.TryParse(ArduinoInput.Data, out int res))
        {
            return res;
        }
        else
        {
            return 0;
        }
    }

    // AI 輸入
    private static int StatesInfoFromAI()
    {
        var tracks = GameManager.Tracks;
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            if (!tracks[i].Any()) continue;
            var h = tracks[i].Peek();
            if (h.Top >= -Evaluation.Bad.Tolerance)
            {
                if (h is NoteObject)
                {
                    if (Evaluation.Perfect.IsInTolerance(h.Bottom))
                    {
                        res += 1 << i;
                    }
                }
                else if (h is HoldObject)
                {
                    if (Evaluation.Good.IsInTolerance(h.Bottom, h.Top) && h.Top > 0)
                    {
                        res += 1 << i;
                    }
                }
            }
        }
        return res;
    }
}

public struct KeyButton
{
    public KeyState State { get; set; }

    public void SetState(bool isKeyDown)
    {
        if (isKeyDown)
        {
            switch (State)
            {
                case KeyState.Down:
                    State = KeyState.Hold;
                    break;
                case KeyState.Up:
                case KeyState.None:
                    State = KeyState.Down;
                    break;
            }
        }
        else
        {
            switch (State)
            {
                case KeyState.Down:
                case KeyState.Hold:
                    State = KeyState.Up;
                    break;
                case KeyState.Up:
                    State = KeyState.None;
                    break;
            }
        }
    }

    public static implicit operator KeyState(KeyButton state)
    {
        return state.State;
    }
}

public enum KeyState
{
    None,
    Down,
    Hold,
    Up,
}