using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : PersistentSingleton<BGMController>
{
    [SerializeField] AudioClip mainMenuAudio;
    [SerializeField] AudioClip gamingAudio;
    [SerializeField] float bgmDelay = 2f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ApplySetting();

        if(SceneManager.GetActiveScene().buildIndex < 5)
        {
            audioSource.clip = mainMenuAudio;
        }
        else
        {
            audioSource.clip = gamingAudio;
        }
        audioSource.PlayDelayed(bgmDelay);
    }

    public void ApplySetting()
    {
        audioSource.volume = SaveData.ReadSettingConfig().bgmVolumn;
    }

    public void PlayMenuAudio()
    {
        audioSource.Stop();

        audioSource.clip = mainMenuAudio;

        audioSource.PlayDelayed(bgmDelay);
    }

    public void PlayGamingAudio()
    {
        audioSource.Stop();

        audioSource.clip = gamingAudio;

        audioSource.PlayDelayed(bgmDelay);
    }
}
