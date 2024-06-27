using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    CanvasGroup pauseCanvas;

    [SerializeField] CanvasGroup optionsCanvas;

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

    private void Start()
    {
        pauseCanvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyManager.Instance.pauseKey) && pauseCanvas.alpha == 0 && !isChangingKey)
        {
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

            Time.timeScale = 0;

            pauseCanvas.blocksRaycasts = true;

            pauseCanvas.alpha = 1;
        }
        else if (Input.GetKeyDown(KeyManager.Instance.pauseKey) && pauseCanvas.alpha == 1 && !isChangingKey)
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;

            optionsCanvas.blocksRaycasts = false;

            pauseCanvas.blocksRaycasts = false;

            optionsCanvas.alpha = 0;

            pauseCanvas.alpha = 0;
        }

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

    public void ResumeGame()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        pauseCanvas.alpha = 0;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
    }

    public void ButtonOptions()
    {
        optionsCanvas.blocksRaycasts = true;

        optionsCanvas.alpha = 1;

        keyBindsMenu.SetActive(false);

        videoSettings.SetActive(false);

        soundSettings.SetActive(false);

        accessibilitySettings.SetActive(false);

        gameplaySettings.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);
    }

    public void ButtonGameplaySettings()
    {
        if (!isChangingKey)
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
        if (!isChangingKey)
        {
            jumpKey.text = KeyManager.Instance.jumpKey.ToString();

            dashKey.text = KeyManager.Instance.dashKey.ToString();

            interactKey.text = KeyManager.Instance.interactKey.ToString();

            attackKey.text = KeyManager.Instance.attackKey.ToString();

            counterKey.text = KeyManager.Instance.counterKey.ToString();

            rageKey.text = KeyManager.Instance.rageModeKey.ToString();

            healKey.text = KeyManager.Instance.healKey.ToString();

            finisherKey.text = KeyManager.Instance.finisherKey.ToString();

            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            videoSettings.SetActive(false);

            soundSettings.SetActive(false);

            accessibilitySettings.SetActive(false);

            gameplaySettings.SetActive(false);

            keyBindsMenu.SetActive(true);
        }
    }

    public void ButtonVideoSettings()
    {
        if (!isChangingKey)
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
        if (!isChangingKey)
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
        if (!isChangingKey)
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
        if (!isChangingKey)
        {
            optionsCanvas.blocksRaycasts = false;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

            optionsCanvas.alpha = 0;
        }        
    }

    public void SliderMaster()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
    }

    //function for the music slider
    public void SliderMusic()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
    }

    //function for the sfx slider
    public void SliderSFX()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
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

    public void ExitGame()
    {
        pauseCanvas.alpha = 0;

        GameManager.Instance.MainMenuSequenceFunction();
    }
}
