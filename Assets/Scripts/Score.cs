using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameObject vehicle;
    Text scoreText;
    Text highScoreText;
    float score;
    int highScore;
    bool stop;

    // Use this for initialization
    void Start() {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>();
        highScore = PlayerPrefs.GetInt("Highscore", 0);
        highScoreText.text = "High score: " + (int)highScore + " m";
    }

    // Update is called once per frame
    void Update() {
        if (!stop)
        {
            score = Mathf.Clamp(vehicle.transform.position.z - 3000, 0, float.MaxValue);
            scoreText.text = "Distance: " + (int)score + " m";
            if (score > highScore)
            {
                highScore = (int)score;
                highScoreText.text = "High score: " + highScore + " m";
            }
        }
    }

    public void saveHighScore() {
        stop = true;
        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();
    }
}
