using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System;
using System.Collections.Generic;

[AddComponentMenu("Notouch controller/Sensor values getter")]
public class NotouchCommunicationProtocol : MonoBehaviour
{
    public enum State
    {
        UNCONNECTED,
        CONNECTED,
        CALIBRATION
    };

    enum CommunicationState
    {
        FREE,
        SEND,
        READING
    };

    State actualState = State.UNCONNECTED;
    CommunicationState actualCommunicationState = CommunicationState.FREE;

    SerialPortCommunication serialPortCommunication;

    string port;
    int actualPortIndex = 0;
    string[] serialPorts;


    int actualSensor = 0;

    void Start()
    {
        initializeComunication();
        calibrate();
    }

    void Update()
    {
        switch (actualState)
        {
            case State.UNCONNECTED:
                initializeComunication();
                break;
            case State.CONNECTED:
                readSensors();
                break;
            case State.CALIBRATION:
                _calibrate();
                break;
        }
    }

    public void calibrate()
    {
        if (actualState.Equals(State.CONNECTED))
        {
            setState(State.CALIBRATION);
            ControllerValues._SetSensorValuesToZero();
        }
        else
        {
            Debug.Log("Dont worry, I am just unconnected");
        }
    }

    void initializeComunication()
    {
        if (serialPortCommunication == null)
        {
            serialPortCommunication = new SerialPortCommunication();
        }
        findPortAndConnect();
    }

    void findPortAndConnect()
    {
        if (actualPortIndex == 0)
        {
            serialPorts = SerialPort.GetPortNames();
        }
        for (; actualPortIndex < serialPorts.Length;)
        {
            if (!actualCommunicationState.Equals(CommunicationState.FREE))
            { break; }
            this.port = serialPorts[actualPortIndex];
            serialPortCommunication.OpenSerialPort("\\\\.\\" + port, 9600);
            handshake();
            actualPortIndex++;
        }
        if (serialPorts.Length <= 0)
        {
            actualPortIndex = 0;
        }
        else
        {
            actualPortIndex = actualPortIndex % serialPorts.Length;
        }
    }

    void handshake()
    {
        sendCommand("NotouchHandshake");
        read(handshakeCallback, 0.5f);
    }

    void handshakeCallback(string readedValue)
    {
        actualCommunicationState = CommunicationState.FREE;
        if (readedValue.Equals("NotouchListen"))
        {
            Debug.Log("SUCESS Connected to port " + port);
            setState(State.CONNECTED);
            calibrate();
        }
    }

    void readSensors()
    {
        for (; actualSensor < Constants.NUMBER_OF_SENSORS;)
        {
            if (actualState.Equals(State.UNCONNECTED) || !actualCommunicationState.Equals(CommunicationState.FREE))
            { break; }
            readSensor();
            actualSensor++;
        }
        actualSensor = actualSensor % Constants.NUMBER_OF_SENSORS;
    }

    void readSensor()
    {
        sendCommand("Sensor " + actualSensor);
        if (actualState.Equals(State.UNCONNECTED))
        {
            return;
        }
        read(readSensorCallback);
    }

    void readSensorCallback(string readedValue)
    {
        int value = 0;
        int sensorNumber = 0;
        try
        {
            String[] splited = readedValue.Split('_');
            sensorNumber = int.Parse(splited[0]);
            value = int.Parse(splited[1]);
            ControllerValues._SetSensorValue(sensorNumber, value);
        }
        catch (FormatException) { }
        actualCommunicationState = CommunicationState.FREE;
    }

    void _calibrate()
    {
        sendCommand("Calibrate");
        if (actualState.Equals(State.UNCONNECTED))
        {
            return;
        }
        read(_calibrateCallback, 10.0f);
    }

    void _calibrateCallback(string readedValue)
    {
        if (readedValue.Equals("Calibrated"))
        {
            actualCommunicationState = CommunicationState.FREE;
            Debug.Log("CALIBRATED");
            setState(State.CONNECTED);
            return;
        }
    }

    void sendCommand(string command)
    {
        try
        {
            if (actualCommunicationState.Equals(CommunicationState.FREE))
            {
                serialPortCommunication.WriteToArduino(command);
                actualCommunicationState = CommunicationState.SEND;
            }
        }
        catch (SystemException)
        {
            if (actualState != State.UNCONNECTED)
            {
                ControllerValues._SetSensorValuesToZero();
                Debug.LogWarning("Notouch unconnected");
            }
            setState(State.UNCONNECTED);
        }
    }

    string read(Action<string> callback, float timeout = 0.5f)
    {
        string read = "";
        if (actualCommunicationState.Equals(CommunicationState.SEND))
        {
            actualCommunicationState = CommunicationState.READING;
            StartCoroutine(serialPortCommunication.AsynchronousReadFromArduino
                ((string s) => callback(s),                           // Callback
                () => errorCallback(),                            // Error callback
                timeout                                           // Timeout (seconds))
                ));
        }

        return read;
    }

    void errorCallback()
    {
        actualCommunicationState = CommunicationState.FREE;
    }

    void setState(State state)
    {
        this.actualState = state;
        ControllerValues._SetControllerState(state);
    }
}
