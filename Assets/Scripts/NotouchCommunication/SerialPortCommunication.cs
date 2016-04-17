using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.IO;
using System.Collections.Generic;

public class SerialPortCommunication
{

    SerialPort stream;

    public delegate void CallbackFunction<T>(IEnumerable<string> param1, ref T param2);

    public bool OpenSerialPort(string port, int baudRate)
    {
        if (stream != null && stream.IsOpen)
        {
            CloseSerialPort();
            return OpenSerialPort(port, baudRate);
        }
        stream = new SerialPort(port, baudRate);
        stream.ReadTimeout = 1;
        try { stream.Open(); } catch (IOException) { };
        return stream.IsOpen;
    }

    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        float initialTime = Time.realtimeSinceStartup;

        string dataString = null;
        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield break;
            }
            else
                yield return new WaitForSeconds(0.5f);
        } while (Time.realtimeSinceStartup - initialTime < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    void CloseSerialPort()
    {
        stream.Close();
    }

}
