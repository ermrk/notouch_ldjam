using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AverageText : MonoBehaviour {

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
        text.text = "Average: " + aiPlayer.getAverage();
    }
}
