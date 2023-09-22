using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int totalHealth = 10;
    [SerializeField] int killedPoint = 5;

    int currentHealth;

    void Start()
    {
        currentHealth = totalHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            PointSystem.Instance.AddGamingPoint(killedPoint);
            Die();
        }
    }

    public void HealHealth(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 1, totalHealth);
    }

    void Die()
    {
        float dieDelay = 0f;

        Animator animator = GetComponent<Animator>();
        animator.SetBool("isDead", true);

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        dieDelay = info.length / (info.speed * info.speed) + 1f;

        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, dieDelay);
    }

    private void OnDestroy()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage);
            projectile.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage);
            projectile.Die();
        }
    }
}
