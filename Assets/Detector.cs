using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {
    int collisions = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (collisions == 0)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0);
        }
        if (collisions > 0) {
            GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle" || other.tag == "Rock") {
            collisions++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle" || other.tag == "Rock")
        {
            collisions--;
        }
    }

    public int isColliding() {
        if (collisions == 0) {
            return 0;
        }
        return 1;
    }

    public void reset() {
        collisions = 0;
    }
}
