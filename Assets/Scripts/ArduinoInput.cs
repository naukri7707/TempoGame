// for unity c# scripts
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoInput :MonoBehaviour
{
    public static string COMPort = "COM4";
    public bool portOpen = false;
    public SerialPort serialPort = new SerialPort(COMPort, 9600);
    public bool errorHandling = false;

    public string incomingData = "";

    public int updateInterval = 4;
    public int updateSlower = 0;

    public bool useData = false;


    // Use this for initialization
    void Start()
    {
        OpenConnection();
    }

    // Update is called once per frame
    void Update()
    {

        updateSlower++;
        if (portOpen && serialPort.IsOpen && updateSlower >= updateInterval)
        {
            RecieveInput();
            updateSlower = 0;
        }
    }

    public void OpenConnection()
    {
        if (serialPort != null)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();

                Debug.Log("Closing port as it was already open");
            }
            else
            {
                serialPort.Open();
                serialPort.ReadTimeout = 500;
                portOpen = true;

                if (errorHandling)
                {
                    Debug.Log("Port open = " + serialPort.IsOpen);
                }
            }
        }
        else
        {
            if (serialPort.IsOpen)
            {
                Debug.Log("Port is already open");
            }
            else
            {
                Debug.Log("Port == Null");
            }
        }
    }

    void RecieveInput()
    {
        try
        {
            incomingData = serialPort.ReadLine();

            //Debug.Log(serialPort.ReadLine());
            if (errorHandling)
            {
                Debug.Log("data = " + incomingData);
            }
            useData = true;
        }
        catch (Exception errorpiece)
        {
            if (useData)
            {
                Debug.Log("Error 1: " + errorpiece);
                useData = false;
            }
        }
    }

}

