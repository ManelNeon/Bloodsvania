using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicAudioSource;

    public AudioSource sfxAudioSource;

    public AudioSource voiceAudioSource;

    [Header("Audio Clips SFX")]
    public AudioClip punchSound;

    public AudioClip footstepSound;

    public AudioClip punchedSound;

    public AudioClip buttonSound;

    [Header("Audio Clips Music")]
    public AudioClip mainMenuMusic;

    public AudioClip inGameMusic;

    public AudioClip bossMusic;

    [Header("Fading Options")]
    [SerializeField] float fadeDuraction;

    bool isFading;

    float m_Timer;


    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        musicAudioSource.clip = mainMenuMusic;

        musicAudioSource.Play();
    }

    private void Update()
    {
        if (isFading)
        {
            m_Timer -= Time.deltaTime;

            musicAudioSource.volume = m_Timer / fadeDuraction;

            if (musicAudioSource.volume == 0)
            {
                isFading = false;
            }
        }
    }



    public void PlayMusic(AudioClip musicClip, bool willFade)
    {
        if (willFade)
        {
            StartCoroutine(FadingMusic(musicClip));
        }
        else
        {
            if (musicClip == null)
            {
                musicAudioSource.clip = null;
                return;
            }

            musicAudioSource.clip = musicClip;

            musicAudioSource.Play();
        }
    }

    IEnumerator FadingMusic(AudioClip musicClip)
    {
        m_Timer = fadeDuraction;

        isFading = true;

        yield return new WaitForSeconds(fadeDuraction);

        musicAudioSource.volume = 1;

        musicAudioSource.clip = musicClip;

        if (musicClip == null)
        {
            yield break;
        }

        musicAudioSource.Play();

        yield break;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxAudioSource.PlayOneShot(sfxClip);
    }

    public void PlayVoice(AudioClip voiceClip)
    {
        voiceAudioSource.PlayOneShot(voiceClip);
    }
}
