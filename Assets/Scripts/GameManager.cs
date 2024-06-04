using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //this bool controls either if the player can or cannot control
    public bool isControlable;

    // Start is called before the first frame update
    void Start()
    {
        //setting the Instnace
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        isControlable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging only
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(1);
        }
    }
}
