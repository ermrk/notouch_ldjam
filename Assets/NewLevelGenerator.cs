using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewLevelGenerator : MonoBehaviour
{

    enum Directions
    {
        STRAIGHT,
        LEFT,
        RIGHT
    }

    enum Obstacles
    {
        MIDLE,
        LEFT,
        RIGHT
    }

    public int numberOfSegments;
    public Transform objectToGenerate;
    public float segmentWidth;
    public float segmentLength;
    public int pathWidth;
    int segment = 0;
    int bias = 0;
    int lastSegmentOfObstacle = 0;
    float colorTransition = 2;
    int colorSegment = -1;

    Queue<Directions> segments = new Queue<Directions>();
    GameObject vehicle;
    GameObject mainCamera;

    public Color[] colors;
    Color from;
    Color to;
    public float height;

    // Use this for initialization
    void Start()
    {
        generate();
        build();
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicle.transform.position.z > (segment - numberOfSegments / 2) * segmentLength)
        {
            Debug.Log("GENERATING");
            generate();
            build();
        }
        if (vehicle.transform.position.z > (segmentLength * numberOfSegments *  (colorSegment+1)) - 1000)
        {
            if (colorSegment == -1)
            {
                colorSegment++;
                mainCamera.GetComponent<Camera>().backgroundColor = colors[((colorSegment * 3) + 2) % colors.Length];
                RenderSettings.fogColor = colors[((colorSegment * 3) + 2) % colors.Length];
            }
            else
            {
                from = colors[((colorSegment * 3) + 2) % colors.Length];
                to = colors[(((colorSegment + 1) * 3) + 2) % colors.Length];
                colorSegment++;
                colorTransition = 0;
            }
        }
        if (colorTransition < 1f)
        {
            mainCamera.GetComponent<Camera>().backgroundColor = Color.Lerp(from, to, colorTransition);
            RenderSettings.fogColor = Color.Lerp(from, to, colorTransition);
            colorTransition += 0.01f;
        }
    }

    void generate()
    {
        for (int i = 0; i < numberOfSegments; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    segments.Enqueue(Directions.STRAIGHT);
                    break;
                case 1:
                    segments.Enqueue(Directions.LEFT);
                    break;
                case 2:
                    segments.Enqueue(Directions.RIGHT);
                    break;
            }
        }
    }

    void build()
    {
        bool generated;
        while (segments.Count > 0)
        {
            Transform blockLeft;
            Transform blockRight;
            switch (segments.Dequeue())
            {
                case Directions.STRAIGHT:
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 1) * segmentWidth - pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 1) * segmentWidth + pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockLeft.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    blockRight.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    generated = generateObstacles(bias, segment, lastSegmentOfObstacle, Directions.STRAIGHT);
                    if (generated)
                    {
                        lastSegmentOfObstacle = 0;
                    }
                    else
                    {
                        lastSegmentOfObstacle++;
                    }
                    break;
                case Directions.LEFT:
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 2) * segmentWidth - pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 1) * segmentWidth + pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockLeft.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    blockRight.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    bias--;
                    generated = generateObstacles(bias, segment, lastSegmentOfObstacle, Directions.LEFT);
                    if (generated)
                    {
                        lastSegmentOfObstacle = 0;
                    }
                    else
                    {
                        lastSegmentOfObstacle++;
                    }
                    break;
                case Directions.RIGHT:
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 1) * segmentWidth - pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 2) * segmentWidth + pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, height, segmentLength);
                    blockLeft.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    blockRight.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3) % colors.Length];
                    bias++;
                    generated = generateObstacles(bias, segment, lastSegmentOfObstacle, Directions.RIGHT);
                    if (generated)
                    {
                        lastSegmentOfObstacle = 0;
                    }
                    else
                    {
                        lastSegmentOfObstacle++;
                    }
                    break;
            }
            segment++;
        }
    }

    bool generateObstacles(int bias, int segment, int lastSegmentOfObstacles, Directions direction)
    {
        float probability = 1.0f - (1.0f / lastSegmentOfObstacles);
        if (Random.Range(0.0f, 1.0f) < probability)
        {
            buildObstacle(bias, segment, direction);
            return true;
        }
        return false;
    }

    void buildObstacle(int bias, int segment, Directions direction)
    {
        Transform obstacle;
        float obstacleWidth = Random.Range(2, 4);
        int offset = 0;
        if (direction == Directions.LEFT)
        {
            offset = 1;
        }
        else if (direction == Directions.RIGHT)
        {
            offset = -1;
        }
        switch (Random.Range(0, 3))
        {
            case 0:
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias - (obstacleWidth / 2) + offset) * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, height, segmentLength);
                obstacle.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3 + 1) % colors.Length];
                break;
            case 1:
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias + (obstacleWidth / 2) - 0.5f + offset) * segmentWidth - pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, height, segmentLength);
                obstacle.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3 + 1) % colors.Length];
                break;
            case 2:
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias - (obstacleWidth / 2) + 0.5f + offset) * segmentWidth + pathWidth * segmentWidth, transform.position.y, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, height, segmentLength);
                obstacle.GetComponent<Renderer>().material.color = colors[((segment / numberOfSegments) * 3 + 1) % colors.Length];
                break;
        }
    }
}
