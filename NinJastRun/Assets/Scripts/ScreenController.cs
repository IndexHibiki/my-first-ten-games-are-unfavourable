using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : PersistentSingleton<ScreenController>
{
    void Start()
    {
        ApplySetting();
    }

    public void ApplySetting()
    {
        SettingConfig setting = SaveData.ReadSettingConfig();

        Screen.SetResolution(setting.resolution.x, setting.resolution.y, setting.isFullScreen);
    }
}
