using UnityEngine;
using System.Collections;

public class Wings : MonoBehaviour
{

    public int[] sensors;
    public float minValuesToAccept = 0.5f;
    public float force;

    private Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {
        this.rigidbody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * force * calculateSteering();
    }



    float calculateSteering()
    {
        float nonCenteredValue = 0.0f;
        float normalizeFactor = getNormalizeFactor();
        for (int i = 0; i < sensors.Length; i++)
        {
            nonCenteredValue += (ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept) / normalizeFactor) * (i + 1) * 100;
        }
        if (nonCenteredValue < 100)
        {
            return 0;
        }

        float maxNonCenteredValue = 100 * (sensors.Length - 1);
        float fraction = (nonCenteredValue - 100) / maxNonCenteredValue;
        float newPosition;
        newPosition = 2 * fraction - 1;
        return newPosition;
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
}
