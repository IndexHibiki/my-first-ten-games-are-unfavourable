using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingConfig
{
    public Vector2Int resolution;

    public bool isFullScreen;

    public float bgmVolumn;

    public float sfxVolumn;

    public SettingConfig()
    {
        resolution = new Vector2Int(1280, 760);

        isFullScreen = false;

        bgmVolumn = 0.8f;

        sfxVolumn = 0.8f;
    }
}
