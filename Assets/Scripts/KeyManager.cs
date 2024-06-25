using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [Header("Keys")]
    public KeyCode jumpKey;

    public KeyCode dashKey;

    public KeyCode attackKey;

    public KeyCode counterKey;

    public KeyCode rageModeKey;

    public KeyCode finisherKey;

    public KeyCode healKey;

    public KeyCode interactKey;

    public KeyCode pauseKey;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
