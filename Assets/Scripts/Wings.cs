using UnityEngine;
using System.Collections;

public class Wings : MonoBehaviour
{

    public int[] sensors;
    public float minValuesToAccept = 0.5f;
    public float force;
    public bool stop = false;
    private float realRotation = 0.0f;
    private float rotation = 0.0f;

    private Rigidbody rigidbody;
    private Transform geometry;
    private GameObject mainCamera;

    // Use this for initialization
    void Start()
    {
        this.rigidbody = GetComponentInParent<Rigidbody>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform gameObject = transform.GetChild(i);
            if (gameObject.name == "Geometry")
            {
                geometry = gameObject;
            }
        }
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            rotation = Mathf.Clamp(calculateSteering() + Input.GetAxis("Steering Gamepad") + Input.GetAxis("Steering"), -1, 1);
            transform.position += transform.right * force * rotation * Time.deltaTime;

            if (rotation > realRotation)
            {
                realRotation += 5f*Time.deltaTime;
            }
            if (rotation < realRotation)
            {
                realRotation -= 5*Time.deltaTime;
            }
            geometry.eulerAngles = new Vector3(-30 * realRotation, 270, 0);
            mainCamera.transform.eulerAngles = new Vector3(2.2159f, 0, -1f * realRotation);
        }
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
