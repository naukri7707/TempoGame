using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading.Tasks;
using System.Threading;

public class ArduinoInput : Singleton<ArduinoInput>
{
    private const string COMPort = "COM4";

    private readonly SerialPort serialPort = new SerialPort(COMPort, 9600);

    [SerializeField]
    private string incomingData = "";

    public static string Data => Instance.incomingData;

    private void Start()
    {
        var theard = new Thread(new ThreadStart(GetValue))
        {
            IsBackground = true
        };
        theard.Start();
    }

    public void GetValue()
    {
        serialPort.Open();
        while (true)
        {
            incomingData = serialPort.ReadLine();
            Debug.Log(incomingData);
        }
    }
}