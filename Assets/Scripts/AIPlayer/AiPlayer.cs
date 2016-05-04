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

        /*string line;
        System.IO.StreamReader file3 = new System.IO.StreamReader("BestIndividual.txt");
        while ((line = file3.ReadLine()) != null)
        {
            neuralNetwork = new NeuralNetwork(line);
        }
        file3.Close();*/
    }

    // Update is called once per frame
    void Update()
    {
            vehicle = GameObject.FindGameObjectWithTag("Vehicle");
            transform.position = vehicle.transform.position;
            float[] output = neuralNetwork.execute(getInput());
            //Debug.Log(output[0] + " " + output[1]);
        if (output[0] < 0)
        {
            thrust = -1;
        }
        else {
            thrust = 1;
        }
            //thrust = output[0];
            //stearing = output[1];
        if (output[1] < 0)
        {
            stearing = -1;
        }
        else
        {
            stearing = 1;
        }
    }

    float[] getInput()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        List<GameObject> obstaclesList = new List<GameObject>(obstacles);
        obstaclesList=obstaclesList.OrderBy(
            x => Vector3.Distance(this.transform.position, x.transform.position)
        ).ToList();
        float[] input = new float[60];
        for (int i = 0; i < 20; i++)
        {
            input[i * 3] = (int)(obstaclesList[i].transform.position.x - transform.position.x);
            input[i * 3 + 1] = (int)(obstaclesList[i].transform.position.z - transform.position.z);
            input[i * 3 + 2] = (int)obstaclesList[i].transform.lossyScale.x;
        }
        return input;
    }

    int ByDistance(GameObject a, GameObject b)
    {
        var dstToA = Vector3.Distance(transform.position, a.transform.position);
        var dstToB = Vector3.Distance(transform.position, b.transform.position);
        return dstToA.CompareTo(dstToB);
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
        float score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().getScore();
        if (sumOfScores != 0 && Mathf.Abs(((sumOfScores/currentTry)-score))>1000) {
            sumOfScores = 0;
            currentTry = 0;
            return;
        }
        currentTry++;
        sumOfScores += score;
        if (currentTry >= numberOfTries){
            evolution.setDistance(sumOfScores/numberOfTries); //calculate average of multiple tries
            neuralNetwork = evolution.getNextNeuralNetwork();
            sumOfScores = 0;
            currentTry = 0;
        }
        /*string line;
        System.IO.StreamReader file3 = new System.IO.StreamReader("BestIndividual.txt");
        while ((line = file3.ReadLine()) != null)
        {
            neuralNetwork = new NeuralNetwork(line);
        }
        file3.Close();*/
    }

    public int getGenerationNumber() {
        return evolution.getGenerationNumber();
    }

    public int getIndividualNumber() {
        return evolution.getIndex();
    }

    public void saveTheBest(Individual individual) {
        PlayerPrefs.SetString("BestCode", individual.getCode());
        PlayerPrefs.Save();
    }

    public List<float> getBestOfLast() {
        return evolution.getBestOfLast();
    }

    public float getBestOfCurrentGeneration() {
        return evolution.getBestOfCurrentGeneration();
    }

    public void writeDebug(string debug) {
        Debug.Log(debug);
    }

    public int getCurrentTry() {
        return currentTry;
    }

    public float getAverage() {
        if (currentTry == 0) {
            return 0;
        }
        return sumOfScores / currentTry;
    }
}
