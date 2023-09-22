using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : PersistentSingleton<SFXController>
{
    [SerializeField] AudioClip mouseHover;
    [SerializeField] AudioClip mouseClick;
    [SerializeField] AudioClip ninjaWhistle;
    [SerializeField] AudioClip lastBossDie;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void ApplySetting()
    {
        audioSource.volume = SaveData.ReadSettingConfig().sfxVolumn;
    }

    public void PlayMouseHoverSFX()
    {
        audioSource.clip = mouseHover;
        audioSource.Play();
    }

    public void PlayMouseClickSFX()
    {
        audioSource.clip = mouseClick;
        audioSource.Play();
    }

    public void PlayNinjaWhistle()
    {
        audioSource.clip = ninjaWhistle;
        audioSource.Play();
    }

    public void PlayLastBossDie()
    {
        audioSource.clip = lastBossDie;
        audioSource.Play();
    }
}
