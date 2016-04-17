using UnityEngine;
using System.Collections;

public class Delete : MonoBehaviour {

    GameObject vehicle;

    // Use this for initialization
    void Start () {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
    }
	
	// Update is called once per frame
	void Update () {
        if ((transform.position.z+1000) < vehicle.transform.position.z) {
            Destroy(gameObject);
        }
	}
}
