using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas Groups")] //canvas groups to fade the specific parts
    [SerializeField] CanvasGroup mainMenu;

    [SerializeField] CanvasGroup options;

    [SerializeField] CanvasGroup background;

    [SerializeField] CanvasGroup firstFade;

    [Header("Animators")]
    [SerializeField] Animator logoAnimator;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;

    [SerializeField] Slider musicSlider;

    [SerializeField] Slider sfxSlider;

    [SerializeField] AudioMixer audioMixer;

    [Header("Fading Bools")]
    bool isFading; //bool to see if it's fading in

    bool isFirstFade; //bool to see if it's the first fade, going from the black screen to the white screen

    bool isChangingColor; //bool to change the color of the background, when going from the white screen to the black screen

    bool isFadingBack; //bool to see if it's fading fading out

    bool isOptions; //bool to see if it's affecting the options

    bool isExiting; //bool to see if we're exiting the main menu to the game

    [Header("Timers Fade")]
    float m_Timer; 

    [SerializeField] float fadeDuraction = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        //setting the timer to the fade duraction, so that we fade out
        m_Timer = fadeDuraction;

        //starting the coroutine to start the screen
        StartCoroutine(StartScreen());
    }

    //Coroutine to start the screen
    IEnumerator StartScreen()
    {
        isFirstFade = true;

        yield return new WaitForSeconds(1f);

        logoAnimator.Play("LogoAnimation");

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

    //All the fades
    void Fades()
    {
        if (isFirstFade)
        {
            m_Timer -= Time.deltaTime;

            firstFade.alpha = m_Timer / fadeDuraction;

            if (firstFade.alpha == 0)
            {
                isFirstFade = false;

                m_Timer = 0;
            }
        }

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

                m_Timer = fadeDuraction;
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

                options.blocksRaycasts = true;

                m_Timer = fadeDuraction;
            }
        }

        if (isFadingBack && isOptions)
        {
            m_Timer -= Time.deltaTime;

            options.alpha = m_Timer / fadeDuraction;

            if (options.alpha == 0)
            {
                isFadingBack = false;

                options.blocksRaycasts = false;

                StartCoroutine(Wait());

                m_Timer = 0;
            }
        }

        if (isExiting)
        {
            m_Timer -= Time.deltaTime;

            mainMenu.alpha = m_Timer / fadeDuraction;

            if (mainMenu.alpha == 0)
            {
                isExiting = false;

                m_Timer = 0;
            }
        }
    }

    //when clicking on the play button we play this
    IEnumerator Playing(bool isDemo)
    {
        m_Timer = fadeDuraction;

        isExiting = true;

        yield return new WaitForSeconds(fadeDuraction);

        if (isDemo)
        {
            SceneManager.LoadScene("Blocking");
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }

        yield break;
    }

    //function when clicking the play demo button
    public void ButtonPlayDemo()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameMusic);
            StartCoroutine(Playing(true));
        }
    }

    //function when clicking the combat demo button
    public void ButtonPlayTechDemo()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameMusic);
            StartCoroutine(Playing(false));
        }
    }

    //function when clicking the options demo
    public void ButtonOptions()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            float temporary = 0;

            audioMixer.GetFloat("Master", out temporary);

            temporary /= 20;

            temporary = Mathf.Pow(10, temporary);

            masterSlider.value = temporary;

            audioMixer.GetFloat("Music", out temporary);

            temporary /= 20;

            temporary = Mathf.Pow(10, temporary);

            musicSlider.value = temporary;

            audioMixer.GetFloat("SFX", out temporary);

            temporary /= 20;

            temporary = Mathf.Pow(10, temporary);

            sfxSlider.value = temporary;

            isFadingBack = true;
        }
    }

    //functionw hen playing the exit game button
    public void ButtonExitGame()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            //UnityEditor.EditorApplication.isPlaying = false;

            Application.Quit();
        }
    }

    //function when playing the back button on the options page
    public void ButtonBackOptions()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            isFadingBack = true;
        }
    }

    public void SliderMaster()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        }
    }

    //function for the music slider
    public void SliderMusic()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);

        }
    }

    //function for the sfx slider
    public void SliderSFX()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
        }
    }
}
