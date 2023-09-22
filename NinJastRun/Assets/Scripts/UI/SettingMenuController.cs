using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] Button increaseResolutionButton;
    [SerializeField] Button decreaseResolutionButton;

    [SerializeField] Toggle fullScreenToggle;

    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmText;

    [SerializeField] Slider sfxSlider;
    [SerializeField] TextMeshProUGUI sfxText;

    SettingConfig setting;

    string[] totalResolution = { "1280X760", "1920X1080" };
    int currentResolutionIndex = 0;

    void Start()
    {
        setting = SaveData.ReadSettingConfig();

        ShowSetting();
    }

    private void FixedUpdate()
    {
        bgmText.text = Mathf.RoundToInt(bgmSlider.value * 100f).ToString();

        sfxText.text = Mathf.RoundToInt(sfxSlider.value * 100f).ToString();
    }

    void ShowSetting()
    {
        resolutionText.text = setting.resolution.x + "X" + setting.resolution.y;

        currentResolutionIndex = -1;
        for(int i = 0; i < totalResolution.Length; i++)
        {
            if(totalResolution[i].Equals(resolutionText.text))
            {
                currentResolutionIndex = i;
                break;
            }
        }
        if(currentResolutionIndex <= 0)
        {
            decreaseResolutionButton.interactable = false;
        }
        else if(currentResolutionIndex >= totalResolution.Length - 1)
        {
            increaseResolutionButton.interactable = false;
        }

        fullScreenToggle.isOn = setting.isFullScreen;

        bgmSlider.value = setting.bgmVolumn;
        bgmText.text = Mathf.RoundToInt(setting.bgmVolumn * 100f).ToString();

        sfxSlider.value = setting.sfxVolumn;
        sfxText.text = Mathf.RoundToInt(setting.sfxVolumn * 100f).ToString();
    }

    public void OnIncreaseRevolution()
    {
        int newIndex = currentResolutionIndex + 1;

        if(newIndex >= totalResolution.Length)
        {
            return;
        }

        resolutionText.text = totalResolution[newIndex];
        currentResolutionIndex = newIndex;

        if (currentResolutionIndex >= totalResolution.Length - 1)
        {
            increaseResolutionButton.interactable = false;
        }
        decreaseResolutionButton.interactable = true;
    }

    public void OnDecreaseRevolution()
    {
        int newIndex = currentResolutionIndex - 1;

        if(newIndex < 0)
        {
            return;
        }

        resolutionText.text = totalResolution[newIndex];
        currentResolutionIndex = newIndex;

        if (currentResolutionIndex <= 0)
        {
            decreaseResolutionButton.interactable = false;
        }
        increaseResolutionButton.interactable = true;
    }

    public void OnApplyClick()
    {
        string[] strs = resolutionText.text.Split("X");
        Vector2Int newResolution = new Vector2Int(int.Parse(strs[0]), int.Parse(strs[1]));
        setting.resolution = newResolution;

        setting.isFullScreen = fullScreenToggle.isOn;

        setting.bgmVolumn = bgmSlider.value;

        setting.sfxVolumn = sfxSlider.value;

        SaveData.WriteSettingConfig(setting);

        SFXController.Instance.ApplySetting();
        BGMController.Instance.ApplySetting();
        ScreenController.Instance.ApplySetting();
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);
    }
}
