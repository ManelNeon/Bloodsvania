using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    //if it's fading, we do the fade
    bool isFading;

    //if it's black, we do the fade in, if its not, we do the fade out
    bool isBlack;

    float m_Timer;

    [SerializeField] float fadeDuraction;

    CanvasGroup blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;

        blackScreen = GetComponent<CanvasGroup>();

        isBlack = true;

        m_Timer = fadeDuraction;
        
        isFading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading && isBlack)
        {
            m_Timer -= Time.deltaTime;

            blackScreen.alpha = m_Timer / fadeDuraction;

            if (blackScreen.alpha == 0)
            {
                isFading = false;

                isBlack = false;

                m_Timer = 0;
            }
        }

        if (isFading && !isBlack)
        {
            m_Timer += Time.deltaTime;

            blackScreen.alpha = m_Timer / fadeDuraction;

            if (blackScreen.alpha == 1)
            {
                isFading = false;

                isBlack = true;

                m_Timer = fadeDuraction;
            }
        }
    }

    public void StartFadeOutAndIn()
    {
        StartCoroutine(FadeOutAndInCoroutine());

        isFading = true;
    }

    public void StartFadeOut()
    {
        isFading = true;
    }

    IEnumerator FadeOutAndInCoroutine()
    {
        yield return new WaitForSeconds(fadeDuraction + .8f);

        isFading = true;

        yield break;
    }
}
