using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScroller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        MoveTile();
    }

    void MoveTile()
    {
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }
}
