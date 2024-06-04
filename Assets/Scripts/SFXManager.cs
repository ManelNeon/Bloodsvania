using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [HideInInspector] public AudioSource sfxAudioSoruce;

    [SerializeField] AudioClip punchSound;

    [SerializeField] AudioClip footstepSound;

    [SerializeField] AudioClip punchedSound;

    [SerializeField] AudioClip buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        sfxAudioSoruce = GetComponent<AudioSource>();
    }

    public void PlayPunch()
    {
        sfxAudioSoruce.PlayOneShot(punchSound);
    }

    public void PlayFootstep()
    {
        if (!sfxAudioSoruce.isPlaying)
        {
            sfxAudioSoruce.PlayOneShot(footstepSound);
        }
    }

    public void PlayPunched()
    {
        sfxAudioSoruce.PlayOneShot(punchedSound);
    }

    public void PlayButtonClicked()
    {
        sfxAudioSoruce.PlayOneShot(buttonSound);
    }
}
