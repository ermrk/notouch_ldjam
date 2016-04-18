using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Colision : MonoBehaviour {

    Text gameOver;
    int blinking = 0;
    bool startBlinking;

	// Use this for initialization
	void Start () {
        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (startBlinking == true) {
            blinking++;
            if (blinking > 10) {
                gameOver.text="";
            }
            if (blinking > 20) {
                blinking = 0;
                gameOver.text = "Game Over";
            }
            if (Input.anyKey) {
                SceneManager.LoadScene("GameScene");
            }
        }
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Rock")
        {
            GetComponent<Engine>().stop = true;
            GetComponent<Wings>().stop = true;
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponentInChildren<ParticleSystem>().Play();
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
                renderer.enabled = false;
            }
            gameOver.text = "Game Over";
            startBlinking = true;
        }
    }
}
