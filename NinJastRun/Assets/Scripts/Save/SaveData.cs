using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveData
{
    static string SAVEDATA_URL = Application.persistentDataPath + "/SaveData.txt";
    static string[] levelNames = { "level0" };
    static SaveDataStruct saveDataStruct;

    static void InitSaveData()
    {
        List<LevelData> levelDatas = new List<LevelData>();

        foreach(string levelName in levelNames)
        {
            LevelData levelData = new LevelData(levelName);
            levelData.levelInfos = new List<LevelInfo>();

            levelDatas.Add(levelData);
        }

        saveDataStruct = new SaveDataStruct(levelDatas, new SettingConfig());

        WriteData();
    }

    static void ReadData()
    {
        if(!File.Exists(SAVEDATA_URL))
        {
            InitSaveData();
        }

        byte[] bytes = File.ReadAllBytes(SAVEDATA_URL);

        string json = Encoding.UTF8.GetString(bytes);

        saveDataStruct = JsonUtil.fromJson<SaveDataStruct>(json);

    }

    static void WriteData()
    {
        string json = JsonUtil.toJson(saveDataStruct);
        Debug.Log(json);

        File.WriteAllBytes(SAVEDATA_URL, Encoding.UTF8.GetBytes(json));
    }

    public static List<LevelData> ReadLevelData()
    {
        BeforeAllOperation();

        return saveDataStruct.levelDatas;
    }

    public static void WriteLevelData(List<LevelData> newLevelDatas)
    {
        BeforeAllOperation();

        saveDataStruct.levelDatas = newLevelDatas;

        AfterWriteOperation();
    }

    public static SettingConfig ReadSettingConfig()
    {
        BeforeAllOperation();

        return saveDataStruct.setting;
    }

    public static void WriteSettingConfig(SettingConfig newSetting)
    {
        BeforeAllOperation();

        saveDataStruct.setting = newSetting;

        AfterWriteOperation();
    }

    static void BeforeAllOperation()
    {
        if(saveDataStruct == null)
        {
            ReadData();
        }
    }

    static void AfterWriteOperation()
    {
        WriteData();
    }
}

[System.Serializable]
public class SaveDataStruct
{
    public List<LevelData> levelDatas;

    public SettingConfig setting;

    public SaveDataStruct(List<LevelData> levelDatas, SettingConfig setting)
    {
        this.levelDatas = levelDatas;
        this.setting = setting;
    }
}

[System.Serializable]
public class LevelData
{
    public string levelName;

    public List<LevelInfo> levelInfos;

    public static int maxSize = 15;

    public LevelData(string levelName)
    {
        this.levelName = levelName;
        levelInfos = new List<LevelInfo>();
    }

    public void AddLevelInfo(LevelInfo newInfo)
    {
        levelInfos.Add(newInfo);

        levelInfos.Sort((a, b) => -(a.point - b.point));

        if (levelInfos.Count > maxSize)
        {
            levelInfos.RemoveAt(levelInfos.Count - 1);
        }
    }
}

[System.Serializable]
public class LevelInfo
{
    public int point;

    public string clearDate;

    public LevelInfo(int point, string clearDate)
    {
        this.point = point;
        this.clearDate = clearDate;
    }
}