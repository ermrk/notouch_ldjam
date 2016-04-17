using UnityEngine;
using System.Collections;

public static class ControllerValues
{

    private static int[] sensorValues = new int[Constants.NUMBER_OF_SENSORS];
    private static NotouchCommunicationProtocol.State controllerState = NotouchCommunicationProtocol.State.UNCONNECTED;

    public static void _SetSensorValue(int sensorNumber, int value) {
        sensorValues[sensorNumber] = value;
    }

    public static void _SetSensorValuesToZero() {
        for (int i = 0; i < Constants.NUMBER_OF_SENSORS; i++) {
            sensorValues[i] = 0;
        }
    }

    public static void _SetControllerState(NotouchCommunicationProtocol.State controllerState)
    {
        ControllerValues.controllerState = controllerState;
    }

    public static int GetSensorValue(int sensorNumber)
    {
        return sensorValues[sensorNumber];
    }

    public static float GetSensorValueNormalized(int sensorNumber) {
        return sensorValues[sensorNumber] / 255.0f;
    }

    public static float GetSensorValueNormalized(int sensorNumber, float minTreshold)
    {
        float sensorValue= sensorValues[sensorNumber] / 255.0f;
        if (sensorValue < minTreshold) {
            return 0;
        }
        return sensorValue;
    }

    public static NotouchCommunicationProtocol.State GetControllerState() {
        return controllerState;
    }
}
