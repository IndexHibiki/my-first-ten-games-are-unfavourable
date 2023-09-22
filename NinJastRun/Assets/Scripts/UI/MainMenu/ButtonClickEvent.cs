using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickEvent : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnStartGameClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

        PointSystem.Instance.StartNewGamePoint(0);

        BGMController.Instance.PlayGamingAudio();

        SceneManager.LoadScene(5);
    }

    public void OnPointListClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

        SceneManager.LoadScene(2);
    }

    public void OnSettingClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

        SceneManager.LoadScene(3);
    }

    public void OnQuitGameClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
