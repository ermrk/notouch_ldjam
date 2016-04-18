using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameObject vehicle;
    Text scoreText;
    Text highScoreText;
    AudioSource highScoreAudio;
    ParticleSystem particleSystem;
    float score;
    int highScore;
    bool stop;
    bool saied;
    bool highscoreNull;

    // Use this for initialization
    void Start() {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>();
        highScoreAudio = GameObject.FindGameObjectWithTag("Highscore").GetComponent<AudioSource>();
        particleSystem = GameObject.FindGameObjectWithTag("Highscore").GetComponent<ParticleSystem>();
        highScore = PlayerPrefs.GetInt("Highscore", 0);
        if (highScore == 0) {
            highscoreNull = true;
        }
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
                if (!saied && !highscoreNull){
                    highScoreAudio.Play();
                    particleSystem.Play();
                    saied = true;
                }
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
