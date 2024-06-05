using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    
    public static SFXManager Instance;

    [HideInInspector] public AudioSource sfxAudioSoruce;

    [Header("Audio Clips")]
    [SerializeField] AudioClip punchSound;

    [SerializeField] AudioClip footstepSound;

    [SerializeField] AudioClip punchedSound;

    [SerializeField] AudioClip buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        //starting the Instance
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        sfxAudioSoruce = GetComponent<AudioSource>();
    }

    //function that plays the punch sound
    public void PlayPunch()
    {
        sfxAudioSoruce.PlayOneShot(punchSound);
    }

    //function that plays the footsteps sounds
    public void PlayFootstep()
    {
        if (!sfxAudioSoruce.isPlaying)
        {
            //sfxAudioSoruce.PlayOneShot(footstepSound);
        }
    }

    //function that plays the punched sound
    public void PlayPunched()
    {
        sfxAudioSoruce.PlayOneShot(punchedSound);
    }

    //function that plays the button clicked sound
    public void PlayButtonClicked()
    {
        sfxAudioSoruce.PlayOneShot(buttonSound);
    }
}
