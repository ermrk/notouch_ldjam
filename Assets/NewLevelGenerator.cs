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

    Vector3 startPosition;

    Queue<Directions> segments = new Queue<Directions>();

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        generate();
        build();
    }

    // Update is called once per frame
    void Update()
    {

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
        int segment = 0;
        int bias = 0;
        int lastSegmentOfObstacle = 0;
        bool generated;
        while (segments.Count > 0)
        {
            Transform blockLeft;
            Transform blockRight;
            switch (segments.Dequeue())
            {
                case Directions.STRAIGHT:
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 1) * segmentWidth - pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 1) * segmentWidth + pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
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
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 2) * segmentWidth - pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 1) * segmentWidth + pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
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
                    blockLeft = GameObject.Instantiate(objectToGenerate, new Vector3((bias - 1) * segmentWidth - pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockRight = GameObject.Instantiate(objectToGenerate, new Vector3((bias + 2) * segmentWidth + pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                    blockLeft.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
                    blockRight.transform.localScale = new Vector3(segmentWidth, 100, segmentLength);
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
        float obstacleWidth = Random.RandomRange(2, 4);
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
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias - (obstacleWidth / 2) + offset) * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, 100, segmentLength);
                break;
            case 1:
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias + (obstacleWidth / 2) - 0.5f + offset) * segmentWidth - pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, 100, segmentLength);
                break;
            case 2:
                obstacle = GameObject.Instantiate(objectToGenerate, new Vector3((bias - (obstacleWidth / 2) + 0.5f + offset) * segmentWidth + pathWidth * segmentWidth, 0, segment * segmentLength), Quaternion.identity) as Transform;
                obstacle.transform.localScale = new Vector3(segmentWidth * obstacleWidth, 100, segmentLength);
                break;
        }
    }
}
