using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AiPlayer : MonoBehaviour
{

    GameObject vehicle;

    NeuralNetwork neuralNetwork;
    Evolution evolution;

    float thrust = 0;
    float stearing = 0;

    float sumOfScores = 0;
    int numberOfTries = 3;
    int currentTry = 0;

    public Transform detectorPrefab;
    List<Transform> detectors;

    private static AiPlayer instance = null;
    public static AiPlayer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.Save();
        Debug.Log("Creating");
        evolution = new Evolution(this);
        neuralNetwork = evolution.getNextNeuralNetwork();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Transform detector = GameObject.Instantiate(detectorPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
                detector.transform.localScale = new Vector3(200, 1, 400);
                detector.parent = this.transform;
                detector.transform.localPosition = new Vector3(i * 200 - ((10 / 2) * 200), 0, j * 400);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        transform.position = vehicle.transform.position;
        float[] output = neuralNetwork.execute(getInput());
        thrust = output[0];
        stearing = output[1];
    }

    float[] getInput()
    {
        Detector[] detectors = transform.GetComponentsInChildren<Detector>();

        float[] input = new float[100];
        for (int i = 0; i < 100; i++)
        {
            input[i] = detectors[i].isColliding();
        }
        return input;
    }

    public float getThrust()
    {
        return thrust;
    }

    public float getStearing()
    {
        return stearing;
    }

    public void newAI()
    {
        Detector[] detectors = transform.GetComponentsInChildren<Detector>();
        for (int i = 0; i < 100; i++)
        {
            detectors[i].reset();
        }

        float score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().getScore();
        if (sumOfScores != 0 && Mathf.Abs(((sumOfScores / currentTry) - score)) > 1000)
        {
            sumOfScores = 0;
            currentTry = 0;
            return;
        }
        currentTry++;
        sumOfScores += score;
        if (currentTry >= numberOfTries)
        {
            evolution.setDistance(sumOfScores / numberOfTries); //calculate average of multiple tries
            neuralNetwork = evolution.getNextNeuralNetwork();
            sumOfScores = 0;
            currentTry = 0;
        }
    }

    public int getGenerationNumber()
    {
        return evolution.getGenerationNumber();
    }

    public int getIndividualNumber()
    {
        return evolution.getIndex();
    }

    public void saveTheBest(Individual individual)
    {
        PlayerPrefs.SetString("BestCode", individual.getCode());
        PlayerPrefs.Save();
    }

    public List<float> getBestOfLast()
    {
        return evolution.getBestOfLast();
    }

    public float getBestOfCurrentGeneration()
    {
        return evolution.getBestOfCurrentGeneration();
    }

    public void writeDebug(string debug)
    {
        Debug.Log(debug);
    }

    public int getCurrentTry()
    {
        return currentTry;
    }

    public float getAverage()
    {
        if (currentTry == 0)
        {
            return 0;
        }
        return sumOfScores / currentTry;
    }
}
