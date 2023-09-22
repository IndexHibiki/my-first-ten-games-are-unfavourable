using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float existTime = 10f;
    [SerializeField] public int damage = 2;

    protected Rigidbody2D rigid;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        StartCoroutine(ExistTimer());
    }

    protected virtual void FixedUpdate()
    {
        Move();
        Rotate();
    }

    protected virtual void Move() { }

    protected virtual void Rotate() { }

    public virtual void StartMotion(Vector2 direction) { }

    IEnumerator ExistTimer()
    {
        yield return new WaitForSeconds(existTime);

        Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
