using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinGyouProjectile : Projectile
{
    [SerializeField] float startMoveSpeed = 4f;
    [SerializeField] float endMoveSpeed = 2f;
    [SerializeField] float changeMoveSpeedTime = 2f;
    float moveSpeed = 0f;
    float deltaMoveSpeed = 0f;
    Vector2 moveDirection = Vector2.zero;

    WaitForFixedUpdate waitForFixedUpdate;

    protected override void Start()
    {
        base.Start();

        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    protected override void Move()
    {
        base.Move();

        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    public override void StartMotion(Vector2 direction)
    {
        base.StartMotion(direction);

        moveDirection = direction.normalized;
        moveSpeed = startMoveSpeed;
        deltaMoveSpeed = (endMoveSpeed - startMoveSpeed) / changeMoveSpeedTime * Time.deltaTime;

        StartCoroutine(ChangeMoveSpeed());
    }

    IEnumerator ChangeMoveSpeed()
    {
        while((moveSpeed - endMoveSpeed) > float.Epsilon)
        {
            moveSpeed += deltaMoveSpeed;

            yield return waitForFixedUpdate;
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
