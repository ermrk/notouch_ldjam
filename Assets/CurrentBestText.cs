using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentBestText : MonoBehaviour {

    Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        AiPlayer aiPlayer = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<AiPlayer>();
        text.text = "Current best: " + aiPlayer.getBestOfCurrentGeneration();
        if (aiPlayer.getBestOfCurrentGeneration() < aiPlayer.getBestOfLast()[5])
        {
            text.color = new Color(1, 0, 0);
        }
        else {
            text.color = new Color(0, 1, 0);
        }
    }
}
