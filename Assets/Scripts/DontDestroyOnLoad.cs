using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{

    static bool AudioBegin = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static DontDestroyOnLoad instance = null;
    public static DontDestroyOnLoad Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (!AudioBegin)
        {
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }
}
