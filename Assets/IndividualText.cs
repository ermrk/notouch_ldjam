using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IndividualText : MonoBehaviour {
    Text text;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        AiPlayer aiplayer = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<AiPlayer>();
        text.text = "Individual: " + (aiplayer.getIndividualNumber()-1)+" / "+ aiplayer.getCurrentTry();
    }
}
