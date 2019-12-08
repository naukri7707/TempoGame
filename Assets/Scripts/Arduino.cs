using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Arduino : MonoBehaviour
{


    public static ArduinoButton[] Buttons { get; set; } = new ArduinoButton[5];

    private void Update()
    {
        // TODO 從序列阜取得 Arduino Message
        string arduinoMessage = "2";
        //
        int arduino = int.Parse(arduinoMessage);
        // 更新按鈕狀況
        for (var i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].Press = (arduino & (i + 1)) != 0;
        }
        KeyBoard();
    }

    private void KeyBoard()
    {
        Buttons[0].Press = Input.GetKey(KeyCode.D);
        Buttons[1].Press = Input.GetKey(KeyCode.F);
        Buttons[2].Press = Input.GetKey(KeyCode.J);
        Buttons[3].Press = Input.GetKey(KeyCode.K);
    }
}

public struct ArduinoButton
{
    public bool Press { get; set; } // TODo KeyDown 避免 按住 一直消
}