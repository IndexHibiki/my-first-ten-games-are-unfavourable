using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWood : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float maxShakeAngle = 20f;
    [SerializeField] float deltaShakeAngle = 1f;
    float currentShakeAngle = 0f;

    bool isOnGround = false;

    void Start()
    {
        currentShakeAngle = maxShakeAngle;
    }

    private void FixedUpdate()
    {
        if(isOnGround == false)
        {
            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
        }
        else if(currentShakeAngle > float.Epsilon)
        {
            float angleZ = Random.Range(0f, currentShakeAngle);
            transform.rotation = Quaternion.Euler(0, 0, angleZ);

            currentShakeAngle -= deltaShakeAngle * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isOnGround = true;
        }
    }
}
