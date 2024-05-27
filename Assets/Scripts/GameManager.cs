using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isControlable;

    bool isFading;

    float m_Timer;

    [SerializeField] float fadeDuraction = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        isControlable = true;

        m_Timer = fadeDuraction;

        isFading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            GameObject blackScreen = GameObject.Find("BlackScreen");

            m_Timer -= Time.deltaTime;

            blackScreen.GetComponent<CanvasGroup>().alpha = m_Timer/fadeDuraction;

            if (blackScreen.GetComponent<CanvasGroup>().alpha == 0)
            {
                isFading = false;
            }
        }
    }
}
