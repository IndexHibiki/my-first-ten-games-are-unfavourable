using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingNinjaSpawner : MonoBehaviour
{
    [SerializeField] GameObject dashingNinjaPrefab;
    [SerializeField] GameObject alertPrefab;
    [SerializeField] LineRenderer alertLinePrefab;
    [SerializeField] float alertTime = 1f;
    [SerializeField] float alertLineExistTime = 1.5f;

    Player player;
    WaitForFixedUpdate waitForFixedUpdate;

    void Start()
    {
        player = FindObjectOfType<Player>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void Update()
    {
        
    }

    public IEnumerator SpawnTopDashingNinja()
    {
        SFXController.Instance.PlayNinjaWhistle();

        yield return waitForFixedUpdate;

        float deltaY = 0.1f * CameraController.Instance.cameraOffsetY;

        float x = Random.Range(CameraController.Instance.bottomLeft.x, CameraController.Instance.topRight.x);
        float y = CameraController.Instance.topRight.y + deltaY;
        Vector3 ninjaPos = new Vector3(x, y, 0);

        Vector3 playerPos = player.transform.position;

        yield return waitForFixedUpdate;

        StartCoroutine(DashingNinja(playerPos, ninjaPos));
    }

    public IEnumerator SpawnRightDashingNinja()
    {
        SFXController.Instance.PlayNinjaWhistle();

        yield return waitForFixedUpdate;

        float deltaX = 0.1f * CameraController.Instance.cameraOffsetX;

        float x = CameraController.Instance.topRight.x + deltaX;
        float y = Random.Range(CameraController.Instance.bottomLeft.y, CameraController.Instance.topRight.y);
        Vector3 ninjaPos = new Vector3(x, y, 0);

        Vector3 playerPos = player.transform.position;

        yield return waitForFixedUpdate;

        StartCoroutine(DashingNinja(playerPos, ninjaPos));
    }

    public IEnumerator SpawnRightHorizontalDashingNinja()
    {
        SFXController.Instance.PlayNinjaWhistle();

        yield return waitForFixedUpdate;

        float deltaX = 0.1f * CameraController.Instance.cameraOffsetX;

        float x = CameraController.Instance.topRight.x + deltaX;
        float y = Random.Range(CameraController.Instance.bottomLeft.y, CameraController.Instance.topRight.y);
        Vector3 ninjaPos = new Vector3(x, y, 0);

        Vector3 playerPos = new Vector3(CameraController.Instance.bottomLeft.x - deltaX, y, 0);

        yield return waitForFixedUpdate;

        StartCoroutine(DashingNinja(playerPos, ninjaPos));
    }

    public IEnumerator SpawnLeftDashingNinja()
    {
        SFXController.Instance.PlayNinjaWhistle();

        yield return waitForFixedUpdate;

        float deltaX = 0.1f * CameraController.Instance.cameraOffsetX;

        float x = CameraController.Instance.bottomLeft.x - deltaX;
        float y = Random.Range(CameraController.Instance.bottomLeft.y, CameraController.Instance.topRight.y);
        Vector3 ninjaPos = new Vector3(x, y, 0);

        Vector3 playerPos = player.transform.position;

        yield return waitForFixedUpdate;

        StartCoroutine(DashingNinja(playerPos, ninjaPos));
    }

    public IEnumerator SpawnLeftHorizontalDashingNinja()
    {
        SFXController.Instance.PlayNinjaWhistle();

        yield return waitForFixedUpdate;

        float deltaX = 0.1f * CameraController.Instance.cameraOffsetX;

        float x = CameraController.Instance.bottomLeft.x - deltaX;
        float y = Random.Range(CameraController.Instance.bottomLeft.y, CameraController.Instance.topRight.y);
        Vector3 ninjaPos = new Vector3(x, y, 0);

        Vector3 playerPos = new Vector3(CameraController.Instance.topRight.x + deltaX, y, 0);

        yield return waitForFixedUpdate;

        StartCoroutine(DashingNinja(playerPos, ninjaPos));
    }

    private IEnumerator DashingNinja(Vector3 playerPos, Vector3 ninjaPos)
    {
        Vector3 direction = (playerPos - ninjaPos).normalized;

        GameObject dashingNinja = Instantiate(dashingNinjaPrefab, ninjaPos, Quaternion.identity, transform);
        dashingNinja.GetComponent<SpriteRenderer>().flipX = direction.x > 0 ? false : true;

        float alertNinjaDistance = 0.2f * CameraController.Instance.cameraOffsetX;
        Vector3 alertPos = direction * alertNinjaDistance + ninjaPos;
        GameObject alert = Instantiate(alertPrefab, alertPos, Quaternion.identity, transform);

        LineRenderer alertLine = Instantiate(alertLinePrefab, transform.position, Quaternion.identity, transform);
        alertLine.positionCount = 4;
        alertLine.startWidth = dashingNinja.GetComponent<SpriteRenderer>().bounds.size.x;
        alertLine.endWidth = dashingNinja.GetComponent<SpriteRenderer>().bounds.size.x;

        float startEndDistance = 3f * CameraController.Instance.cameraOffsetX;
        Vector3 endPos = direction * startEndDistance + ninjaPos;
        alertLine.SetPositions(new Vector3[] { ninjaPos, alertPos, playerPos, endPos});

        yield return waitForFixedUpdate;
        SpriteRenderer alertSpriteRenderer = alert.gameObject.GetComponent<SpriteRenderer>();
        float ellapseTime = 0f;
        while (ellapseTime < alertTime)
        {
            alertSpriteRenderer.enabled = !alertSpriteRenderer.enabled;
            ellapseTime += Time.deltaTime;

            yield return waitForFixedUpdate;
        }

        alertSpriteRenderer.enabled = false;

        dashingNinja.GetComponent<DashingEnemy>().SetMoveDirection(direction.normalized);

        Destroy(alert);
        Destroy(alertLine, alertLineExistTime);
    }
}

public enum DashingNinjaMotionType
{
    Left,
    Right,
    Top,
    LeftHorizontal,
    RightHorizontal
}
