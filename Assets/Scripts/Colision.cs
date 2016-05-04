using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Colision : MonoBehaviour {

    Text gameOver;
    float blinking = 0;
    bool startBlinking;
    AudioSource explosion;
    Score score;
    AiPlayer aiPlayer;

	// Use this for initialization
	void Start () {
        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Text>();
        explosion = GameObject.FindGameObjectWithTag("Explosion").GetComponent<AudioSource>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        aiPlayer = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<AiPlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (startBlinking == true) {
            blinking+=15*Time.deltaTime;
            if (blinking > 10) {
                gameOver.text="";
            }
            if (blinking > 20) {
                blinking = 0;
                gameOver.text = "Game Over";
            }
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("GameScene");
            }
            aiPlayer.newAI();
            SceneManager.LoadScene("GameScene");
        }
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Rock")
        {
            GetComponent<Engine>().stop = true;
            GetComponent<Wings>().stop = true;
            GetComponent<Rigidbody>().freezeRotation = true;
            foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>()) {
                if (particleSystem.name == "Particle System") {
                    particleSystem.Play();
                }
            }
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
                if (renderer.gameObject.name == "Particle System") {
                    continue;
                }
                renderer.enabled = false;
            }
            gameOver.text = "Game Over";
            startBlinking = true;
            explosion.Play();
            score.saveHighScore();
        }
    }
}
