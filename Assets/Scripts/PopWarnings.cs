using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopWarnings : MonoBehaviour
{
    CanvasGroup canvasGroup;

    bool isFading;

    float m_Timer;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(DestroyOurselves());
    }

    void Update()
    {
        if (isFading)
        {
            m_Timer -= Time.deltaTime;

            canvasGroup.alpha = m_Timer / 1.5f;

            if (canvasGroup.alpha == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DestroyOurselves()
    {
        m_Timer = 1.5f;

        yield return new WaitForSeconds(3);

        isFading = true;

        yield break;
    }
}
