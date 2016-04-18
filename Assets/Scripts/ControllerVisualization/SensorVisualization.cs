using UnityEngine;
using System.Collections;

public class SensorVisualization : MonoBehaviour
{

    public int sensor;

    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = Color.Lerp(new Color(1.0f, 0.0f, 0.0f), new Color(0.0f, 1.0f, 0.0f), ControllerValues.GetSensorValue(sensor)/255.0f);
    }
}
