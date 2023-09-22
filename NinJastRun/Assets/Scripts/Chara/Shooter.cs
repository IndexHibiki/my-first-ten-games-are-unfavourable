using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float shootCD = 0.2f;
    [HideInInspector] public bool isShooting = false;
    [SerializeField] Vector2 shooterOffset = Vector2.zero;
    bool canShoot = true;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    public void Shoot()
    {
        if(canShoot && isShooting)
        {
            Cursor cursor = FindObjectOfType<Cursor>();
            Vector2 direction = cursor.GetCursorPosition2D() - (Vector2)transform.position;

            GameObject projectile = Instantiate(projectilePrefab, transform.position + (Vector3)shooterOffset, Quaternion.identity);
            projectile.GetComponent<Projectile>().StartMotion(direction);

            StartCoroutine(ShootCDTimer());
        }
    }

    IEnumerator ShootCDTimer()
    {
        canShoot = false;

        yield return new WaitForSeconds(shootCD);

        canShoot = true;
    }
}
