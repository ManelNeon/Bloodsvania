using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenu;

    [SerializeField] CanvasGroup options;

    [SerializeField] CanvasGroup background;

    //[SerializeField] Slider musicSlider;

    //[SerializeField] Slider sfxSlider;

    bool isFading;

    bool isChangingColor;

    bool isFadingBack;

    bool isOptions;

    float m_Timer;

    [SerializeField] float fadeDuraction = 1f;

    private void Start()
    {
        StartCoroutine(StartScreen());
    }

    IEnumerator StartScreen()
    {
        yield return new WaitForSeconds(4.2f);

        isChangingColor = true;

        yield return new WaitForSeconds(fadeDuraction);

        isChangingColor = false;

        isFading = true;

        yield break;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.2f);

        isFading = true;

        isOptions = !isOptions;

        yield break;
    }

    private void Update()
    {
        Fades();
    }

    void Fades()
    {
        if (isChangingColor)
        {
            m_Timer += Time.deltaTime;

            background.alpha = m_Timer / fadeDuraction;

            if (background.alpha == 1)
            {
                m_Timer = 0;
            }
        }

        if (isFading && !isOptions)
        {
            m_Timer += Time.deltaTime;

            mainMenu.alpha = m_Timer / fadeDuraction;

            if (mainMenu.alpha == 1)
            {
                isFading = false;

                m_Timer = 1;
            }
        }

        if (isFadingBack && !isOptions)
        {
            m_Timer -= Time.deltaTime;

            mainMenu.alpha = m_Timer / fadeDuraction;

            if (mainMenu.alpha == 0)
            {
                isFadingBack = false;

                StartCoroutine(Wait());

                m_Timer = 0;
            }
        }

        if (isFading && isOptions)
        {
            m_Timer += Time.deltaTime;

            options.alpha = m_Timer / fadeDuraction;

            if (options.alpha == 1)
            {
                isFading = false;

                m_Timer = 1;
            }
        }

        if (isFadingBack && isOptions)
        {
            m_Timer -= Time.deltaTime;

            options.alpha = m_Timer / fadeDuraction;

            if (options.alpha == 0)
            {
                isFadingBack = false;

                StartCoroutine(Wait());

                m_Timer = 0;
            }
        }
    }

    public void ButtonPlayDemo()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void ButtonPlayTechDemo()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void ButtonOptions()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            //musicSlider.value = MusicManager.Instance.musicSource.volume;

            //sfxSlider.value = SoundManager.Instance.soundSource.volume;

            isFadingBack = true;
        }
    }

    public void ButtonExitGame()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            UnityEditor.EditorApplication.isPlaying = false;

            Application.Quit();
        }
    }

    public void ButtonBackOptions()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            isFadingBack = true;
        }
    }

    public void SliderMusic()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            //MusicManager.Instance.musicSource.volume = musicSlider.value;
        }
    }

    public void SliderSFX()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            //SoundManager.Instance.soundSource.volume = sfxSlider.value;
        }
    }

}
