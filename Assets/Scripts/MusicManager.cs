using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //music managers Instance
    public static MusicManager Instance;

    [HideInInspector] public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        //setting the Instance
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
