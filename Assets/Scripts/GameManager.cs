using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        isControlable = true;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        m_Timer = fadeDuraction;

        yield return new WaitForSeconds(.3f);

        isFading = true;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Wait());
            SceneManager.LoadScene(1);
        }

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
