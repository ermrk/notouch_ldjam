using UnityEngine;
using System.Collections;

public class LineVisualization : MonoBehaviour
{

    public Vector3 startPosition;
    public float maxBias;
    public int[] sensors;
    public float minValuesToAccept = 0.5f;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float nonCenteredValue = 0.0f;
        float normalizeFactor = getNormalizeFactor();
        for (int i = 0; i < sensors.Length; i++)
        {
            nonCenteredValue += (ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept) / normalizeFactor) * (i + 1) * 100;
        }
        Debug.Log(nonCenteredValue);
        transform.position = new Vector3(getXPosition(nonCenteredValue), startPosition.y, startPosition.z);
    }

    float getNormalizeFactor()
    {
        float totalSensorValue = 0.0f;
        for (int i = 0; i < sensors.Length; i++)
        {
            totalSensorValue += ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept);
        }
        if (totalSensorValue == 0)
        {
            return 1;
        }
        return totalSensorValue;
    }

    float getXPosition(float nonCenteredValue)
    {
        if (nonCenteredValue < 100)
        {
            return startPosition.x;
        }
        float maxNonCenteredValue = 100 * (sensors.Length - 1);
        float fraction = (nonCenteredValue - 100) / maxNonCenteredValue;
        float newPosition = 2 * maxBias * fraction - maxBias;
        return startPosition.x + newPosition;
    }
}
