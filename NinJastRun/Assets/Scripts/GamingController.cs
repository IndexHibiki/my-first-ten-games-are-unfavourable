using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamingController : Singleton<GamingController>
{
    [SerializeField] Canvas pauseUI;
    [SerializeField] Canvas loseUI;

    [SerializeField] float oneMiddleTime = 60f;

    [SerializeField] BossBasic boss;
    [SerializeField] float bossTime = 60f;

    WaitForFixedUpdate waitForFixedUpdate;

    void Start()
    {
        StartCoroutine(GamingTimer());
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public void OnPause()
    {
        SetGamingSpeed(0f);
        pauseUI.enabled = true;
    }

    public void OnContinueClick()
    {
        pauseUI.enabled = false;
        SetGamingSpeed(1f);
    }

    public void OnRestartClick()
    {
        SetGamingSpeed(1f);

        BGMController.Instance.PlayGamingAudio();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitClick()
    {
        SetGamingSpeed(1f);

        BGMController.Instance.PlayMenuAudio();

        SceneManager.LoadScene(0);
    }

    public void OnWin()
    {
        PointSystem.Instance.AddTimePoint();

        PointSystem.Instance.UploadPoint();

        BGMController.Instance.PlayMenuAudio();

        SceneManager.LoadScene(4);
    }

    public void OnLose()
    {
        SetGamingSpeed(0f);
        loseUI.enabled = true;
    }

    void SetGamingSpeed(float gamingSpeed)
    {
        Time.timeScale = gamingSpeed;
    }

    IEnumerator GamingTimer()
    {
        yield return null;

        yield return StartCoroutine(Middle(oneMiddleTime));

        StartCoroutine(LastBoss());
    }

    IEnumerator Middle(float middleTime)
    {
        float startTime = middleTime * 0.05f;
        float endTime = middleTime * 0.02f;

        int spawnWaveCount = Random.Range(3, 6);
        float spawnWaveInterval = (middleTime - startTime - endTime) / spawnWaveCount;

        yield return new WaitForSeconds(startTime);

        WaitForSeconds waitForSpawnWaveInterval = new WaitForSeconds(spawnWaveInterval);
        for(int i = 0; i < spawnWaveCount; i++)
        {
            int spawnNum = Random.Range(3, 6);
            float spawnInterval = 2.5f;
            DashingNinjaMotionType motionType = (DashingNinjaMotionType)Random.Range(0, 5);
            //DashingNinjaMotionType motionType = DashingNinjaMotionType.Right;

            StartCoroutine(EnemySpawner.Instance.SpawnDashingNinjaEnemy(spawnNum, spawnInterval, motionType));

            yield return waitForSpawnWaveInterval;
        }

        yield return new WaitForSeconds(endTime);
    }

    IEnumerator MiddleBoss()
    {
        yield return null;
    }

    IEnumerator LastBoss()
    {
        yield return null;

        BossBasic lastBoss = Instantiate(boss, transform.position, Quaternion.identity);

        while(lastBoss != null)
        {
            yield return waitForFixedUpdate;
        }

        yield return new WaitForSeconds(2);

        OnWin();
    }
}
