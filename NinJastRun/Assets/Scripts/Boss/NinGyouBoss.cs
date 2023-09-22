using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinGyouBoss : BossBasic
{
    [SerializeField] float appearMoveSpeed = 8f;

    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] float maxRotateAngle = 5f;
    float currentRotationZ = 0f;
    float rotationOffset = 0f;

    [SerializeField] ParticleSystem bossDieExplosion;

    Vector2 padding = Vector2.zero;

    [SerializeField] int collisionDamage = 3;

    [Header("Idle")]
    [SerializeField] float downSpeed = 0.2f;
    [SerializeField] float idleTime = 3f;

    [Header("Move")]
    [SerializeField] float moveSpeed = 1f;
    Vector2 cameraTopRightPoint;
    Vector2 cameraBottonLeftPoint;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] GameObject alertObject;
    [SerializeField] float alertExistTime = 1f;
    [SerializeField] float alertFalshTime = 0.1f;

    [Header("Shoot")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shooterTransform;
    [SerializeField] int shootNum = 3;
    [SerializeField] float shootInterval = 0.5f;

    [Header("Skill")]
    [SerializeField] Transform headShooter;

    Transform playerTransform;
    Rigidbody2D rigid;

    WaitForFixedUpdate waitForFixedUpdate;
    WaitForSeconds waitForAlertFlash;
    WaitForSeconds waitForShootInterval;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        currentCorourine = null;
        rigid = GetComponent<Rigidbody2D>();

        cameraBottonLeftPoint = CameraController.Instance.bottomLeft;
        cameraTopRightPoint = CameraController.Instance.topRight;

        padding = GetComponent<CapsuleCollider2D>().size;

        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForAlertFlash = new WaitForSeconds(alertFalshTime);
        waitForShootInterval = new WaitForSeconds(shootInterval);

        StartCoroutine(Appear());
    }

    private void FixedUpdate()
    {
        currentRotationZ += rotateSpeed * Time.deltaTime;

        if(Mathf.Abs(currentRotationZ) > maxRotateAngle)
        {
            rotateSpeed = -rotateSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ + rotationOffset);
    }

    protected override void SwitchToNextState()
    {
        int rand = Random.Range(0, 50);
        switch (currentStateType)
        {
            case EnemyStateType.Idle:

                if (rand < 25)
                {
                    ChangeState(EnemyStateType.Move);
                }
                else if (rand < 45)
                {
                    ChangeState(EnemyStateType.Attack);
                }
                else
                {
                    ChangeState(EnemyStateType.Skill);
                }
                break;

            case EnemyStateType.Move:

                if (rand < 40)
                {
                    ChangeState(EnemyStateType.Attack);
                }
                else
                {
                    ChangeState(EnemyStateType.Idle);
                }
                break;

            case EnemyStateType.Attack:

                if (rand < 35)
                {
                    ChangeState(EnemyStateType.Move);
                }
                else
                {
                    ChangeState(EnemyStateType.Idle);
                }
                break;

            case EnemyStateType.Skill:

                ChangeState(EnemyStateType.Idle);
                break;
        }

        HandleCurrentState();
    }

    #region Appear
    protected override IEnumerator Appear()
    {
        Vector3 appearStartPos = new Vector3(
            Random.Range(cameraBottonLeftPoint.x + 1.5f * CameraController.Instance.cameraOffsetX, cameraTopRightPoint.x - padding.x),
            cameraBottonLeftPoint.y - 2 * padding.y,
            0);

        Vector3 appearEndPos = new Vector3(
            appearStartPos.x,
            Random.Range(cameraBottonLeftPoint.y + padding.y, cameraTopRightPoint.y - padding.y),
            0);

        transform.position = appearStartPos;

        while(Vector3.Distance(transform.position, appearEndPos) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, appearEndPos, appearMoveSpeed * Time.deltaTime);

            yield return waitForFixedUpdate;
        }

        SwitchToNextState();
    }
    #endregion

    #region Leave
    public override IEnumerator Leave()
    {
        StopCoroutine(currentCorourine);

        Vector3 leaveEndPos = new Vector3(
            transform.position.x,
            cameraBottonLeftPoint.y - 2 * padding.y,
            0);

        while (Vector3.Distance(transform.position, leaveEndPos) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, leaveEndPos, appearMoveSpeed * Time.deltaTime);

            yield return waitForFixedUpdate;
        }

        Destroy(gameObject);
    }
    #endregion

    #region Idle
    protected override IEnumerator Idle()
    {
        yield return null;

        float ellapseTime = 0f;
        while(ellapseTime < idleTime)
        {
            transform.position += Vector3.down * downSpeed * Time.deltaTime;
            ellapseTime += Time.deltaTime;

            yield return waitForFixedUpdate;
        }

        SwitchToNextState();
    }
    #endregion

    #region Move
    protected override IEnumerator Move()
    {
        yield return null;

        Vector2 bottonLeftPoint = new Vector2(
            Mathf.Clamp(cameraBottonLeftPoint.x + padding.x, 1.5f * CameraController.Instance.cameraOffsetX, cameraBottonLeftPoint.x),
            cameraBottonLeftPoint.y + padding.y
            );
        Vector2 topRightPoint = new Vector2(
            cameraTopRightPoint.x - padding.x, 
            cameraTopRightPoint.y - padding.y
            );

        Vector3 destination = new Vector3(
            Random.Range(bottonLeftPoint.x, topRightPoint.x),
            Random.Range(bottonLeftPoint.y, topRightPoint.y),
            0
            );
        Debug.Log("Move to " + destination);

        Vector3 direction = (destination - transform.position).normalized;

        yield return waitForFixedUpdate;
        while (Vector3.Distance(destination, transform.position) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            yield return waitForFixedUpdate;
        }

        SwitchToNextState();
    }
    #endregion

    #region Attack
    protected override IEnumerator Attack()
    {
        if (playerTransform.position.y < CameraController.Instance.cameraOffsetY)
        {
            yield return StartCoroutine(Dash());
        }
        else
        {
            yield return currentCorourine = StartCoroutine(Shoot());
        }
    }

    IEnumerator Dash()
    {
        yield return null;

        float ellapseTime = 0f;
        while(ellapseTime < alertExistTime)
        {
            alertObject.SetActive(!alertObject.activeSelf);
            ellapseTime += alertFalshTime;

            yield return waitForAlertFlash;
        }
        alertObject.SetActive(false);

        Vector3 destination = playerTransform.position;

        Vector3 direction = (destination - transform.position).normalized;

        while (Vector3.Distance(destination, transform.position) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, dashSpeed * Time.deltaTime);

            yield return waitForFixedUpdate;
        }

        SwitchToNextState();
    }

    IEnumerator Shoot()
    {
        yield return null;

        for(int i = 0; i < shootNum; i++)
        {
            Vector3 direction = playerTransform.position - shooterTransform.position;

            GameObject projectile = Instantiate(projectilePrefab, shooterTransform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            yield return waitForShootInterval;
        }

        SwitchToNextState();
    }
    #endregion

    #region Skill
    protected override IEnumerator Skill()
    {
        if (GetComponent<BossHealth>().GetCurrentHealthPerc() > 0.5f)
        {
            yield return StartCoroutine(EdgeShoot());
        }
        else
        {
            yield return StartCoroutine(CrazyEdgeShoot(6));
        }
    }

    /// <summary>
    /// Shoot at the Edge of Screen
    /// </summary>
    /// <returns></returns>
    IEnumerator EdgeShoot()
    {
        yield return StartCoroutine(CrazyEdgeShoot(1));
    }

    IEnumerator CrazyEdgeShoot(int edgeShootNum)
    {
        yield return null;

        Vector3 previousPos = transform.position;
        Vector3 disappearPos = new Vector3(
            cameraTopRightPoint.x + 2 * padding.x,
            transform.position.y,
            0
            );

        yield return StartCoroutine(MoveToPos(disappearPos));

        for(int i = 0; i < edgeShootNum; i++)
        {
            switch (Random.Range(0, 4))
            {
                case 0:

                    yield return StartCoroutine(LeftEdgeShoot());
                    break;
                case 1:

                    yield return StartCoroutine(RightEdgeShoot());
                    break;
                case 2:

                    yield return StartCoroutine(BottomEdgeShoot());
                    break;
                case 3:

                    yield return StartCoroutine(TopEdgeShoot());
                    break;
            }
        }

        transform.position = disappearPos;
        yield return StartCoroutine(MoveToPos(previousPos));

        SwitchToNextState();
    }

    IEnumerator MoveToPos(Vector3 destination)
    {
        yield return null;

        while (Vector3.Distance(transform.position, destination) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator LeftEdgeShoot()
    {
        yield return null;

        Vector3 startPos = new Vector3(
            cameraBottonLeftPoint.x - 2 * padding.y,
            Random.Range(cameraBottonLeftPoint.y + padding.y, cameraTopRightPoint.y - padding.y),
            0
            );

        Vector3 endPos = new Vector3(
            cameraBottonLeftPoint.x,
            startPos.y,
            0
            );

        transform.position = startPos;
        rotationOffset = -90;
        yield return StartCoroutine(MoveToPos(endPos));

        for (int i = 0; i < shootNum; i++)
        {
            Vector3 direction = playerTransform.position - shooterTransform.position;

            GameObject projectile = Instantiate(projectilePrefab, headShooter.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            yield return waitForShootInterval;
        }

        yield return StartCoroutine(MoveToPos(startPos));
        rotationOffset = 0;
    }

    IEnumerator RightEdgeShoot()
    {
        yield return null;

        Vector3 startPos = new Vector3(
            cameraTopRightPoint.x + 2 * padding.y,
            Random.Range(cameraBottonLeftPoint.y + padding.y, cameraTopRightPoint.y - padding.y),
            0
            );

        Vector3 endPos = new Vector3(
            cameraTopRightPoint.x,
            startPos.y,
            0
            );

        transform.position = startPos;
        rotationOffset = 90;
        yield return StartCoroutine(MoveToPos(endPos));

        for (int i = 0; i < shootNum; i++)
        {
            Vector3 direction = playerTransform.position - shooterTransform.position;

            GameObject projectile = Instantiate(projectilePrefab, headShooter.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            yield return waitForShootInterval;
        }

        yield return StartCoroutine(MoveToPos(startPos));
        rotationOffset = 0;
    }

    IEnumerator BottomEdgeShoot()
    {
        yield return null;

        Vector3 startPos = new Vector3(
            Random.Range(cameraBottonLeftPoint.x + padding.x, cameraTopRightPoint.x + padding.x),
            cameraBottonLeftPoint.y - padding.y,
            0
            );

        Vector3 endPos = new Vector3(
            startPos.x,
            cameraBottonLeftPoint.y,
            0
            );

        transform.position = startPos;
        rotationOffset = 180;
        yield return StartCoroutine(MoveToPos(endPos));

        for (int i = 0; i < shootNum; i++)
        {
            Vector3 direction = playerTransform.position - shooterTransform.position;

            GameObject projectile = Instantiate(projectilePrefab, headShooter.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            yield return waitForShootInterval;
        }

        yield return StartCoroutine(MoveToPos(startPos));
        rotationOffset = 0;
    }

    IEnumerator TopEdgeShoot()
    {
        yield return null;

        Vector3 startPos = new Vector3(
            Random.Range(cameraBottonLeftPoint.x + padding.x, cameraTopRightPoint.x + padding.x),
            cameraTopRightPoint.y + padding.y,
            0
            );

        Vector3 endPos = new Vector3(
            startPos.x,
            cameraTopRightPoint.y,
            0
            );

        transform.position = startPos;
        rotationOffset = 180;
        yield return StartCoroutine(MoveToPos(endPos));

        for (int i = 0; i < shootNum; i++)
        {
            Vector3 direction = playerTransform.position - shooterTransform.position;

            GameObject projectile = Instantiate(projectilePrefab, headShooter.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            yield return waitForShootInterval;
        }

        yield return StartCoroutine(MoveToPos(startPos));
        rotationOffset = 0;
    }
    #endregion

    public override IEnumerator Die()
    {
        yield return null;

        StopCoroutine(currentCorourine);

        ParticleSystem particleSystem = Instantiate(bossDieExplosion, transform.position, Quaternion.identity);
        particleSystem.Play();
        SFXController.Instance.PlayLastBossDie();

        Destroy(particleSystem, 2f);
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(collisionDamage);
        }
    }
}
