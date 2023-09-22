using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int initHealth = 5;
    [SerializeField] int maxHealth = 10;

    int currentHealth;

    public event Action onHealthChangeEvent;

    [SerializeField] float invincibleTime = 0.5f;
    SpriteRenderer spriteRenderer;
    bool isInvincible = false;
    WaitForFixedUpdate waitForFixedUpdate;

    void Start()
    {
        currentHealth = initHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if(amount <= 0 || isInvincible == true)
        {
            return;
        }

        currentHealth -= amount;
        onHealthChangeEvent?.Invoke();

        if(currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(InvinsibleTimer());
    }

    public void HealHealth(int amount)
    {
        if(amount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 1, maxHealth);
        onHealthChangeEvent.Invoke();
    }

    public float GetHealthPercent()
    {
        return 1.0f * currentHealth / maxHealth;
    }

    void Die()
    {
        float dieDelay = 0f;

        Animator animator = GetComponent<Animator>();
        GetComponent<NinjaGirlAnimeChange>().AnimateDead();

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
        if(other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && isInvincible == false)
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage);
            projectile.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && isInvincible == false)
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage);
            projectile.Die();
        }
    }

    IEnumerator InvinsibleTimer()
    {
        isInvincible = true;

        float ellapseTime = 0f;
        while(ellapseTime < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            ellapseTime += Time.deltaTime;

            yield return waitForFixedUpdate;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
