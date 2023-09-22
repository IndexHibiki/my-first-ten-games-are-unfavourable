using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] int collisionDamage = 2;

    Vector2 moveDirection;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(collisionDamage);
        }
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }
}
