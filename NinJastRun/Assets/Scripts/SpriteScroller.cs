using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollSpeed;

    Material material;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;

        transform.position += new Vector3(CameraController.Instance.cameraOffsetX, CameraController.Instance.cameraOffsetY, 0);
    }

    private void FixedUpdate()
    {
        ScrollSprite();
    }

    void ScrollSprite()
    {
        material.mainTextureOffset += scrollSpeed * Time.deltaTime;
    }
}
