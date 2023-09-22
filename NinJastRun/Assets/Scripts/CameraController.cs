using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [HideInInspector] public int cameraOffsetX;
    [HideInInspector] public int cameraOffsetY;
    [HideInInspector] public Vector2 topRight;
    [HideInInspector] public Vector2 bottomLeft;

    protected override void Awake()
    {
        base.Awake();

        Vector2 pointTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        cameraOffsetX = Mathf.CeilToInt(pointTopRight.x);
        cameraOffsetY = Mathf.CeilToInt(pointTopRight.y);
        Camera.main.transform.position += new Vector3(cameraOffsetX, cameraOffsetY, 0);

        topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
