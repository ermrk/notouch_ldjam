using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour
{

    public int[] sensors;
    public float minValuesToAccept = 0.1f; //Change in script doesn't have any effect
    public float speed;
    public float minSpeed;
    public bool stop = false;

    Animator animator;
    private float realSpeed = 0.0f;
    private GameObject mainCamera;
    private ParticleSystem trail;
    private float basePitch = 0.98f;
    private AudioSource music;

    // Use this for initialization
    void Start()
    {
        foreach (Animator animatorTemp in GetComponentsInChildren<Animator>())
        {
            if (animatorTemp.gameObject.name == "Geometry")
            {
                animator = animatorTemp;
                break;
            }
        }
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        mainCamera.GetComponent<Animator>().Play("Shake", 0, 0);
        trail = GameObject.FindGameObjectWithTag("Trail").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            float speedInput = Mathf.Clamp(calculateThrust() + Input.GetAxis("Speed") + Input.GetAxis("Speed Gamepad"), 0, 1);
            if (speedInput > realSpeed)
            {
                realSpeed += 1.2f*Time.deltaTime;
            }
            if (speedInput < realSpeed)
            {
                if (Input.GetAxis("Speed") < -0.5f || Input.GetAxis("Speed Gamepad") < -0.5f)
                {
                    realSpeed -= 1.2f * Time.deltaTime;
                }
                realSpeed -= 0.8f * Time.deltaTime;
            }
            mainCamera.GetComponent<Camera>().fieldOfView = 60 + 20 * realSpeed;

            mainCamera.GetComponent<Animator>().speed = 1 - realSpeed;
            transform.position += (transform.forward * speed * realSpeed + transform.forward * minSpeed) * Time.deltaTime;
            animator.Play("Shapeshift", 0, 1 - realSpeed);
            animator.speed = 0;
            var emission = trail.emission;
            var rate = emission.rate;
            rate.constantMax = 12000 * realSpeed + 8000;
            emission.rate = rate;
            var shape = trail.shape;
            var angle = shape.angle;
            angle = 4 + 8 * (1 - realSpeed);
            shape.angle = angle;
            trail.startSpeed = 2.5f + 2.5f * realSpeed;
            GetComponent<AudioSource>().pitch = basePitch + realSpeed * 0.5f;
            GetComponent<AudioSource>().volume = 0.8f + realSpeed * 0.2f;
            basePitch += realSpeed*0.0005f*Time.deltaTime;
            music.pitch = basePitch;
            music.volume = 0.80f + realSpeed * 0.2f;
        }
        else {
            GetComponent<AudioSource>().volume -= 0.1f;
            music.volume = 0.80f;
            music.pitch = basePitch;
        }
        minSpeed += 0.04f*30f;
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
