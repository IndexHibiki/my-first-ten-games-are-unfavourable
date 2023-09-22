using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : PersistentSingleton<PointSystem>
{
    [SerializeField] float timePointRatio = 1.5f;

    [HideInInspector] public List<LevelData> levelDatas;

    [HideInInspector] public int currentLevelIndex = 0;

    [HideInInspector] public int currentSceneIndex = 5;

    int currentPoint = 0;

    void Start()
    {
        levelDatas = SaveData.ReadLevelData();
    }

    public void StartNewGamePoint(int gamingLevelIndex)
    {
        currentLevelIndex = gamingLevelIndex;
        currentSceneIndex = gamingLevelIndex + 5;
    }

    public void ResetGamingPoint()
    {
        currentPoint = 0;
    }

    public void AddGamingPoint(int point)
    {
        currentPoint += point;
    }

    public void AddTimePoint()
    {
        currentPoint += Mathf.CeilToInt(TimerUI.Instance.GetCurrentTime() * timePointRatio);
    }

    public void UploadPoint()
    {
        levelDatas[currentLevelIndex].AddLevelInfo(new LevelInfo(currentPoint, System.DateTime.Now.ToString()));
        Debug.Log(levelDatas[currentLevelIndex].levelInfos[0].clearDate);

        SaveData.WriteLevelData(levelDatas);
    }
}
