using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : Projectile
{
    [SerializeField] float shootPower = 5f;

    bool isCollide = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Rotate()
    {
        base.Rotate();

        if(isCollide == false)
        {
            float angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x);

            transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
    }

    public override void StartMotion(Vector2 direction)
    {
        base.StartMotion(direction);

        rigid.velocity = direction.normalized * shootPower;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isCollide = true;
            rigid.velocity = Vector2.zero;
        }
    }
}
