using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerState : MonoBehaviour
{
    const string UNCONNECTED = "UNCONNECTED";
    const string CALIBRATING = "CALIBRATING";
    const string CONNECTED = "CONNECTED";

    Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ControllerValues.GetControllerState()) {
            case NotouchCommunicationProtocol.State.UNCONNECTED:
                text.text = UNCONNECTED;
                break;
            case NotouchCommunicationProtocol.State.CALIBRATION:
                text.text = CALIBRATING;
                break;
            case NotouchCommunicationProtocol.State.CONNECTED:
                text.text = CONNECTED;
                break;
        }
    }
}
