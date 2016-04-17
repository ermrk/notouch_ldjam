using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlotRenderer : MonoBehaviour {

    public int numberOfValues;
    public int maximalValue;
    public Vector2 origin;
    public float width;
    public float height;
    public Material lineMat;
    public Color color;

    int[] positions;

    // Use this for initialization
    void Start() {
        initialize();
    }

    void initialize() {
        positions = new int[numberOfValues];
        for (int i = 0; i < numberOfValues; i++) {
            positions[i] = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        addValue(Random.Range(0,254));
    }

    void OnPostRender()
    {
        for (int i = 0; i < positions.Length - 1; i++)
        {
            drawLine(i);
        }
    }

    void OnDrawGizmos()
    {
        if (positions != null)
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                drawLine(i);
            }
        }
    }

void addValue(int value) {
        movePositions();
        positions[0] = value;
    }

    void movePositions() {
        for (int i = numberOfValues - 1; i >= 1; i--)
        {
            positions[i] = positions[i - 1];
        }
    }

    Vector3 createPositionVector(int i) {
        return new Vector3(origin.x + i * width / numberOfValues, origin.y + (height * positions[i] / maximalValue));
    }

    void drawLine(int startingIndex) {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(color);
        GL.Vertex(createPositionVector(startingIndex));
        GL.Vertex(createPositionVector(startingIndex+1));
        GL.End();
    }
}
