using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    DashingNinjaSpawner dashingNinjaSpawner;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        dashingNinjaSpawner = GetComponent<DashingNinjaSpawner>();
    }


    private void FixedUpdate()
    {
        
    }

    public IEnumerator SpawnDashingNinjaEnemy(int spawnNum, float spawnInterval, DashingNinjaMotionType motionType)
    {
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(spawnInterval);

        for (int i = 0; i < spawnNum; i++)
        {
            if (FindObjectOfType<Player>())
            {
                switch(motionType)
                {
                    case DashingNinjaMotionType.Left:

                        StartCoroutine(dashingNinjaSpawner.SpawnLeftDashingNinja());
                        break;
                    case DashingNinjaMotionType.Right:

                        StartCoroutine(dashingNinjaSpawner.SpawnRightDashingNinja());
                        break;
                    case DashingNinjaMotionType.Top:

                        StartCoroutine(dashingNinjaSpawner.SpawnTopDashingNinja());
                        break;
                    case DashingNinjaMotionType.LeftHorizontal:

                        StartCoroutine(dashingNinjaSpawner.SpawnLeftHorizontalDashingNinja());
                        break;
                    case DashingNinjaMotionType.RightHorizontal:

                        StartCoroutine(dashingNinjaSpawner.SpawnRightHorizontalDashingNinja());
                        break;
                }
            }

            yield return waitForSpawnInterval;
        }
    }
}
