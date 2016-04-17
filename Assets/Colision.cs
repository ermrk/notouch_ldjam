using UnityEngine;
using System.Collections;

public class Colision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        Debug.Log("Collision");
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Rock")
        {
            GetComponent<Engine>().stop = true;
            GetComponent<Wings>().stop = true;
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
