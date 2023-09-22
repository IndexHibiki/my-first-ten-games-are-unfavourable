using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : Singleton<TimerUI>
{
    int currentTime = 0;
    TextMeshProUGUI tmpTimer;

    void Start()
    {
        tmpTimer = GetComponent<TextMeshProUGUI>();

        InvokeRepeating("UpdateTMPText", 1f, 1f);
    }

    void UpdateTMPText()
    {
        currentTime++;

        string minute = string.Format("{0:00}", currentTime / 60);
        string second = string.Format("{0:00}", currentTime % 60);

        tmpTimer.text = minute + ":" + second;
    }

    public int GetCurrentTime()
    {
        return currentTime;
    }
}
