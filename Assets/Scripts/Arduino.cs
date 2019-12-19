using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Arduino : MonoBehaviour
{
    public static ArduinoButton[] Buttons { get; set; } = new ArduinoButton[5];

    private void Update()
    {
        if (!GameManager.AIPlay)
        {
            SetStates(StatesInfoFromKeyBoard());
            // SetStates(int.Parse(ArduinoInput.IncomingData));
        }
    }

    // 將輸入資訊轉譯成按鈕資訊
    public void SetStates(int stateInfo)
    {
        for (int i = 0; i < 4; i++)
        {
            Buttons[i].SetState((stateInfo & (1 << i)) != 0);
            GameArgs.PressEffect[i].enabled = Buttons[i].State == ButtonState.Down || Buttons[i].State == ButtonState.Hold;
        }
    }

    // 鍵盤模擬輸入
    private int StatesInfoFromKeyBoard()
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
}

public struct ArduinoButton
{
    public ButtonState State { get; set; }

    public void SetState(bool isKeyDown)
    {
        if (isKeyDown)
        {
            switch (State)
            {
                case ButtonState.Down:
                    State = ButtonState.Hold;
                    break;
                case ButtonState.Up:
                case ButtonState.None:
                    State = ButtonState.Down;
                    break;
            }
        }
        else
        {
            switch (State)
            {
                case ButtonState.Down:
                case ButtonState.Hold:
                    State = ButtonState.Up;
                    break;
                case ButtonState.Up:
                    State = ButtonState.None;
                    break;
            }
        }
    }

    public static implicit operator ButtonState(ArduinoButton state)
    {
        return state.State;
    }
}

public enum ButtonState
{
    None,
    Down,
    Hold,
    Up,
}