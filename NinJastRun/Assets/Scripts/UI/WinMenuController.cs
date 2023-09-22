using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinMenuController : MonoBehaviour
{
    [SerializeField] Scrollbar pointsScroller;
    [SerializeField] List<TextMeshProUGUI> pointTexts;

    int levelIndex = 0;

    void Start()
    {
        ShowPoints();
    }

    void ShowPoints()
    {
        pointsScroller.value = 1f;

        LevelData levelData = PointSystem.Instance.levelDatas[PointSystem.Instance.currentLevelIndex];
        for(int i = 0; i < levelData.levelInfos.Count && i < pointTexts.Count; i++)
        {
            LevelInfo levelInfo = levelData.levelInfos[i];

            string text = string.Format("{0:00}. {1:00000000}   {2:G}", i + 1, levelInfo.point, levelInfo.clearDate);

            pointTexts[i].text = text;
        }
    }

    public void OnReplayClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

        BGMController.Instance.PlayGamingAudio();

        SceneManager.LoadScene(PointSystem.Instance.currentSceneIndex);
    }

    public void OnMainMenuClick()
    {
        SFXController.Instance.PlayMouseClickSFX();

        SceneManager.LoadScene(0);
    }
}
