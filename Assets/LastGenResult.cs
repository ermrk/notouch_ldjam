using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LastGenResult : MonoBehaviour
{

    Text text;
    public int generation = 0;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        AiPlayer aiPlayer = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<AiPlayer>();
        text.text = "n-" + (generation + 1) + ": " + aiPlayer.getBestOfLast()[5 - generation];
        if (aiPlayer.getBestOfLast()[5 - generation] < aiPlayer.getBestOfLast()[5 - (generation + 1)])
        {
            text.color = new Color(1, 0, 0);
        }
        else
        {
            text.color = new Color(0, 1, 0);
        }
    }
}
