using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using System;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas Groups")] //canvas groups to fade the specific parts
    [SerializeField] CanvasGroup mainMenu;

    [SerializeField] CanvasGroup saveFileSelectMenu;

    [SerializeField] CanvasGroup options;

    [SerializeField] CanvasGroup background;

    [SerializeField] CanvasGroup logoCanvas;

    [Header("Animators")]
    [SerializeField] Animator logoAnimator;

    [SerializeField] Animator creditsAnimator;

    [Header("Options Menus")]
    [SerializeField] GameObject gameplaySettings;

    [SerializeField] GameObject keyBindsMenu;

    [SerializeField] GameObject videoSettings;

    [SerializeField] GameObject soundSettings;

    [SerializeField] GameObject accessibilitySettings;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;

    [SerializeField] Slider musicSlider;

    [SerializeField] Slider sfxSlider;

    [SerializeField] AudioMixer audioMixer;

    [Header("Key Mapping Buttons")]
    [SerializeField] TextMeshProUGUI jumpKey;

    [SerializeField] TextMeshProUGUI dashKey;

    [SerializeField] TextMeshProUGUI interactKey;

    [SerializeField] TextMeshProUGUI attackKey;

    [SerializeField] TextMeshProUGUI counterKey;

    [SerializeField] TextMeshProUGUI rageKey;

    [SerializeField] TextMeshProUGUI healKey;

    [SerializeField] TextMeshProUGUI finisherKey;

    bool isChangingKey;

    [Header("Fading Bools")]
    bool isFading; //bool to see if it's fading in

    bool isChangingColor; //bool to change the color of the background, when going from the white screen to the black screen

    bool isFadingBack; //bool to see if it's fading fading out

    bool isOptions; //bool to see if it's affecting the options

    bool isSaveFile;

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
        yield return new WaitForSeconds(1);

        logoCanvas.gameObject.SetActive(true);

        logoAnimator.Play("LogoAnimation");

        yield return new WaitForSeconds(3f);

        isChangingColor = true;

        yield return new WaitForSeconds(fadeDuraction);

        isChangingColor = false;

        isFading = true;

        yield break;
    }

    private void Update()
    {
        Fades();

        if (isChangingKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    if (jumpKey.text == "Awaiting Input")
                    {
                        jumpKey.text = keyCode.ToString();

                        KeyManager.Instance.jumpKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (dashKey.text == "Awaiting Input")
                    {
                        dashKey.text = keyCode.ToString();

                        KeyManager.Instance.dashKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (interactKey.text == "Awaiting Input")
                    {
                        interactKey.text = keyCode.ToString();

                        KeyManager.Instance.interactKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (attackKey.text == "Awaiting Input")
                    {
                        attackKey.text = keyCode.ToString();

                        KeyManager.Instance.attackKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (counterKey.text == "Awaiting Input")
                    {
                        counterKey.text = keyCode.ToString();

                        KeyManager.Instance.counterKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (rageKey.text == "Awaiting Input")
                    {
                        rageKey.text = keyCode.ToString();

                        KeyManager.Instance.rageModeKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (healKey.text == "Awaiting Input")
                    {
                        healKey.text = keyCode.ToString();

                        KeyManager.Instance.healKey = keyCode;

                        isChangingKey = false;

                        return;
                    }

                    if (finisherKey.text == "Awaiting Input")
                    {
                        finisherKey.text = keyCode.ToString();

                        KeyManager.Instance.finisherKey = keyCode;

                        isChangingKey = false;

                        return;
                    }
                }
            }
        }
    }

    //All the fades
    void Fades()
    {
        //this makes the logo disappear
        if (isChangingColor)
        {
            m_Timer -= Time.deltaTime;

            logoCanvas.alpha = m_Timer / fadeDuraction;

            if (logoCanvas.alpha == 0)
            {
                m_Timer = 0;
            }
        }

        //this makes the main menu appear
        if (isFading && !isOptions && !isSaveFile)
        {
            m_Timer += Time.deltaTime;

            mainMenu.alpha = m_Timer / fadeDuraction;

            if (mainMenu.alpha == 1)
            {
                isFading = false;

                m_Timer = fadeDuraction;
            }
        }

        //this makes the main menu disappear
        if (isFadingBack && !isOptions && !isSaveFile)
        {
            m_Timer -= Time.deltaTime;

            mainMenu.alpha = m_Timer / fadeDuraction;

            if (mainMenu.alpha == 0)
            {
                isFadingBack = false;

                m_Timer = 0;
            }
        }

        //this makes the options appear
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

        //this makes the options appear
        if (isFadingBack && isOptions)
        {
            m_Timer -= Time.deltaTime;

            options.alpha = m_Timer / fadeDuraction;

            if (options.alpha == 0)
            {
                isFadingBack = false;

                options.blocksRaycasts = false;

                m_Timer = 0;
            }
        }

        //this makes the savefile menu appear
        if (isFading && isSaveFile)
        {
            m_Timer += Time.deltaTime;

            saveFileSelectMenu.alpha = m_Timer / fadeDuraction;

            if (saveFileSelectMenu.alpha == 1)
            {
                isFading = false;

                saveFileSelectMenu.blocksRaycasts = true;

                m_Timer = fadeDuraction;
            }
        }

        //this makes the savefile menu disappear
        if (isExiting)
        {
            m_Timer -= Time.deltaTime;

            saveFileSelectMenu.alpha = m_Timer / fadeDuraction;
            
            if (saveFileSelectMenu.alpha == 0)
            {
                isExiting = false;

                m_Timer = 0;
            }
        }
    }

    //when clicking on the play button we play this
    IEnumerator Playing()
    {
        m_Timer = fadeDuraction;

        isExiting = true;

        yield return new WaitForSeconds(fadeDuraction + .5f);

        SceneManager.LoadScene("Blocking");

        yield break;
    }

    //function when clicking the play demo button
    public void ButtonPlayDemo()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
            StartCoroutine(WaitSaveSelect());
        }
    }

    public void ButtonPlayGame()
    {
        if (!isFading && !isFadingBack && saveFileSelectMenu.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameMusic, true);
            StartCoroutine(Playing());
        }
    }

    //function when clicking the options demo
    public void ButtonOptions()
    {
        if (!isFading && !isFadingBack && mainMenu.alpha == 1)
        {
            keyBindsMenu.SetActive(false);

            videoSettings.SetActive(false);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(true);

            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            StartCoroutine(WaitOptions());
        }
    }

    public void ButtonGameplaySettings()
    {
        if (!isFading && !isFadingBack && options.alpha == 1 && !gameplaySettings.activeInHierarchy && !isChangingKey)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            keyBindsMenu.SetActive(false);

            videoSettings.SetActive(false);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(true);
        }
    }

    public void ButtonKeyBinds()
    {
        if (!isFading && !isFadingBack && options.alpha == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            jumpKey.text = KeyManager.Instance.jumpKey.ToString();

            dashKey.text = KeyManager.Instance.dashKey.ToString();

            interactKey.text = KeyManager.Instance.interactKey.ToString();

            attackKey.text = KeyManager.Instance.attackKey.ToString();

            counterKey.text = KeyManager.Instance.counterKey.ToString();

            rageKey.text = KeyManager.Instance.rageModeKey.ToString();

            healKey.text = KeyManager.Instance.healKey.ToString();

            finisherKey.text = KeyManager.Instance.finisherKey.ToString();

            videoSettings.SetActive(false);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(false);

            keyBindsMenu.SetActive(true);
        }
    }

    public void ButtonVideoSettings()
    {
        if (!isFading && !isFadingBack && options.alpha == 1 && !videoSettings.activeInHierarchy && !isChangingKey)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(false);

            keyBindsMenu.SetActive(false);

            videoSettings.SetActive(true);
        }
    }

    public void ButtonSoundSettings()
    {
        if (!isFading && !isFadingBack && options.alpha == 1 && !soundSettings.activeInHierarchy && !isChangingKey)
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

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(false);

            keyBindsMenu.SetActive(false);

            videoSettings.SetActive(false);

            soundSettings.SetActive(true);
        }
    }

    public void ButtonAccessibilitySettings()
    {
        if (!isFading && !isFadingBack && options.alpha == 1 && !accessibilitySettings.activeInHierarchy && !isChangingKey)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            gameplaySettings.SetActive(false);

            keyBindsMenu.SetActive(false);

            videoSettings.SetActive(false);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(true);
        }
    }

    //function when playing the back button on the options page
    public void ButtonBackOptions()
    {
        if (!isFading && !isFadingBack && options.alpha == 1 && !isChangingKey)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            StartCoroutine(WaitOptions());
        }
    }

    IEnumerator WaitOptions()
    {
        isFadingBack = true;

        yield return new WaitForSeconds(fadeDuraction + .2f);

        isOptions = !isOptions;

        isFading = true;

        yield break;
    }

    IEnumerator WaitSaveSelect()
    {
        isFadingBack = true;

        yield return new WaitForSeconds(fadeDuraction + .2f);

        isSaveFile = !isSaveFile;

        isFading = true;

        yield break;
    }

    public void ButtonCredits()
    {
        isFadingBack = true;

        StartCoroutine(WaitCredits());
    }

    IEnumerator WaitCredits()
    {
        yield return new WaitForSeconds(fadeDuraction + .4f);

        creditsAnimator.Play("Credits");

        yield return new WaitForSeconds(20);

        isFading = true;
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

    public void KeyChangingJump()
    {
        isChangingKey = true;

        jumpKey.text = "Awaiting Input";
    }

    public void KeyChangingDash()
    {
        isChangingKey = true;

        dashKey.text = "Awaiting Input";
    }

    public void KeyChangingInteract()
    {
        isChangingKey = true;

        interactKey.text = "Awaiting Input";
    }

    public void KeyChangingAttack()
    {
        isChangingKey = true;

        attackKey.text = "Awaiting Input";
    }

    public void KeyChangingCounter()
    {
        isChangingKey = true;

        counterKey.text = "Awaiting Input";
    }

    public void KeyChangingRage()
    {
        isChangingKey = true;

        rageKey.text = "Awaiting Input";
    }

    public void KeyChangingHeal()
    {
        isChangingKey = true;

        healKey.text = "Awaiting Input";
    }

    public void KeyChangingFinisher()
    {
        isChangingKey = true;

        finisherKey.text = "Awaiting Input";
    }
}
