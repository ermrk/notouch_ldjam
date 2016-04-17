using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    public float elementHeight = 5f;
    public float elementWidth = 5f;

    public Transform objectToGenerate;
    private Vector3 beginningMiddle;

    private enum State
    {
        NOT_FREE = 0,
        FREE = 1
    }

    private enum Direction
    {
        STRAIGHT = 0,
        LEFT = 1,
        RIGHT = 2,
        BOTH = 3
    }

    // Use this for initialization
    void Start()
    {
        beginningMiddle = transform.position;
        int[] entrances = new int[50];
        for (int i = 0; i < 49; i++) {
            entrances[i] = i * 2;
        }
        build(generate(entrances));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int[,] generate(int[] entrances)
    {
        int[,] generation = new int[100, 100];
        foreach (int entrancePosition in entrances)
        {
            generatePath(generation, entrancePosition);
        }
        return generation;
    }

    private void generatePath(int[,] generation, int entrancePosition)
    {
        Queue<int[]> pathStarts = new Queue<int[]>();
        int[] entranceCoordinates = {entrancePosition, 0};
        pathStarts.Enqueue(entranceCoordinates);
        while (pathStarts.Count > 0)
        {
            int[] coordinates = pathStarts.Dequeue();
            int x = coordinates[0];
            int y = coordinates[1];

            while (y != generation.GetLength(0) && generation[x, y] != (int)State.FREE)
            {
                generation[x, y] = (int)State.FREE;
                if (x > 0 && x < (generation.GetLength(1)-1))
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            break;
                        case 1:
                            x--;
                            generation[x, y] = (int)State.FREE;
                            break;
                        case 2:
                            x++;
                            generation[x, y] = (int)State.FREE;
                            break;
                        case 3:
                            x--;
                            generation[x, y] = (int)State.FREE;
                            generation[x+2, y] = (int)State.FREE;
                            int[] segmentStart = { x+2 , y+1 };
                            pathStarts.Enqueue(segmentStart);
                            break;
                    }
                }
                else if (x == (generation.GetLength(1)-1))
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            break;
                        case 1:
                            x--;
                            generation[x, y] = (int)State.FREE;
                            break;
                    }
                }
                else if (x == 0)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            break;
                        case 1:
                            x++;
                            generation[x, y] = (int)State.FREE;
                            break;
                    }
                }
                y++;
            }
        }
    }

    private void build(int[,] generation)
    {
        for (int i = 0; i < generation.GetLength(0); i++)
        {
            for (int j = 0; j < generation.GetLength(1); j++)
            {
                if (generation[i, j] == (int)State.NOT_FREE)
                {
                    Transform block = GameObject.Instantiate(objectToGenerate, new Vector3(i * elementHeight - 0.5f*elementHeight* generation.GetLength(0), 0+beginningMiddle.y, j * elementWidth + beginningMiddle.z), Quaternion.identity) as Transform;
                    block.transform.localScale=new Vector3(elementHeight, 100, elementWidth);
                    Color color= new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    block.GetComponent<Renderer>().material.color = color;
                }
            }
        }
    }
}
