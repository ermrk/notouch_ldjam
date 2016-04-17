using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameObject vehicle;
    Text scoreText;
    float score;

	// Use this for initialization
	void Start () {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        score = Mathf.Clamp(vehicle.transform.position.z-3000,0,float.MaxValue);
        scoreText.text= "Distance: "+ (int)score+" m";
    }
}
