using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PointListController : MonoBehaviour
{
    [SerializeField] Scrollbar pointsScroller;
    [SerializeField] List<TextMeshProUGUI> pointTexts;
    [SerializeField] TextMeshProUGUI levelNameText;

    int levelIndex = 0;

    void Start()
    {
        ShowPoints();
    }

    void ShowPoints()
    {
        pointsScroller.value = 1f;

        LevelData levelData = PointSystem.Instance.levelDatas[levelIndex];
        levelNameText.text = levelData.levelName;

        for (int i = 0; i < levelData.levelInfos.Count && i < pointTexts.Count; i++)
        {
            LevelInfo levelInfo = levelData.levelInfos[i];

            string text = string.Format("{0:00}. {1:00000000}   {2:G}", i + 1, levelInfo.point, levelInfo.clearDate);

            pointTexts[i].text = text;
        }
    }

    public void OnLeftClick()
    {
        levelIndex = (levelIndex + 1) % PointSystem.Instance.levelDatas.Count;

        ShowPoints();
    }

    public void OnRightClick()
    {
        levelIndex -= 1;
        if(levelIndex < 0)
        {
            levelIndex = PointSystem.Instance.levelDatas.Count - 1;
        }

        ShowPoints();
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);
    }
}
