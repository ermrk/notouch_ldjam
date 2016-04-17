using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour
{

    public int[] sensors;
    public float minValuesToAccept = 0.1f; //Change in script doesn't have any effect
    public float speed;
    public float minSpeed;
    public bool stop = false;

    private Rigidbody rigidbody;
    Animator animator;
    private float realSpeed = 0.0f;
    private GameObject mainCamera;
    private Vector3 cameraPosition;

    // Use this for initialization
    void Start()
    {
        this.rigidbody = GetComponentInParent<Rigidbody>();
        foreach (Animator animatorTemp in GetComponentsInChildren<Animator>())
        {
            if (animatorTemp.gameObject.name == "Geometry")
            {
                animator = animatorTemp;
                break;
            }
        }
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<Animator>().Play("Shake", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            float speedInput = Mathf.Clamp(calculateThrust() + Input.GetAxis("Speed") + Input.GetAxis("Speed Gamepad"), 0, 1);
            if (speedInput > realSpeed)
            {
                realSpeed += 0.02f;
            }
            if (speedInput < realSpeed)
            {
                if (Input.GetAxis("Speed") < -0.5f || Input.GetAxis("Speed Gamepad") < -0.5f)
                {
                    realSpeed -= 0.02f;
                }
                realSpeed -= 0.01f;
            }
            mainCamera.GetComponent<Camera>().fieldOfView = 60 + 20 * realSpeed;

            mainCamera.GetComponent<Animator>().speed = 1 - realSpeed;
            transform.position += transform.forward * speed * realSpeed + transform.forward * minSpeed;
            animator.Play("Shapeshift", 0, 1 - realSpeed);
            animator.speed = 0;
        }
        minSpeed += 0.01f;
    }

    private float getNormalizeFactor()
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

    private float calculateThrust()
    {
        float nonCenteredValue = 0.0f;
        float normalizeFactor = getNormalizeFactor();
        for (int i = 0; i < sensors.Length; i++)
        {
            nonCenteredValue += (ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept) / normalizeFactor) * (i + 1) * 100;
        }
        float maxNonCenteredValue = 100 * sensors.Length;
        float thrust = nonCenteredValue / maxNonCenteredValue;
        return thrust;
    }


}
