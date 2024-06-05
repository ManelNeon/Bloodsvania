using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //music managers Instance
    public static MusicManager Instance;

    [HideInInspector] public AudioSource musicSource;

    [SerializeField] AudioClip mainMenuMusic;

    [SerializeField] AudioClip inGameMusic;

    float currentValue;

    bool isFadingOut;

    bool isFadingIn;

    bool isCombat;

    [SerializeField] float fadingDuraction;

    float m_Timer;

    // Start is called before the first frame update
    void Start()
    {
        //setting the Instance
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;
        
        musicSource = GetComponent<AudioSource>();

        musicSource.clip = mainMenuMusic;

        musicSource.Play();
    }

    private void Update()
    {
        if (isFadingOut && !isCombat)
        {
            m_Timer -= (Time.deltaTime / 2.5f);

            musicSource.volume = m_Timer / fadingDuraction;

            if (musicSource.volume == 0)
            {
                isFadingOut = false;

                musicSource.Stop();

                if (musicSource.clip == mainMenuMusic)
                {
                    musicSource.clip = inGameMusic;
                }
                else
                {
                    musicSource.clip = mainMenuMusic;
                }

                musicSource.volume = currentValue;

                musicSource.Play();
            }
        }
        
        /*if (isFadingOut & isCombat)
        {
            m_Timer -= Time.deltaTime;

            musicSource.volume = m_Timer / fadingDuraction;

            if (musicSource.volume == 0)
            {
                isFadingOut = false;

                musicSource.Stop();

                if (musicSource.clip == mainMenuMusic)
                {
                    musicSource.clip = inGameMusic;
                }
                else
                {
                    musicSource.clip = mainMenuMusic;
                }

                musicSource.volume = currentValue;

                musicSource.Play();
            }
        }

        if (isFadingIn & isCombat)
        {
            m_Timer -= (Time.deltaTime / 2.5f);

            musicSource.volume = m_Timer / fadingDuraction;

            if (musicSource.volume == 0)
            {
                isFadingOut = false;

                musicSource.Stop();

                if (musicSource.clip == mainMenuMusic)
                {
                    musicSource.clip = inGameMusic;
                }
                else
                {
                    musicSource.clip = mainMenuMusic;
                }

                musicSource.volume = currentValue;

                musicSource.Play();
            }
        }*/
    }

    public void PlayInGameMusic()
    {
        currentValue = musicSource.volume;

        m_Timer = currentValue;

        isFadingOut = true;
    }

    public void PlayMenuMusic()
    {
        currentValue = musicSource.volume;

        m_Timer = currentValue;

        isFadingOut = true;
    }

    //public void PlayCombatMusic()
    //{

    //}
}
