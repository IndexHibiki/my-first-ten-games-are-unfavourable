using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] float moveSpeed = 30f;

    [Header("Map Status")]
    [SerializeField] List<Tilemap> tilemaps;
    int currentTilemapIndex = 0;

    [SerializeField] RuleTile groundTile;

    [SerializeField] int oneMapWidth = 100;

    int mapMinY = 0;
    int mapMaxY = 0;

    [SerializeField] int minFloorLength = 15;
    [SerializeField] int maxFloorLength = 30;
    int floorYCount = 0;
    int currentFloorY = 0;

    [SerializeField] int minEmptyOccurCount = 1;
    [SerializeField] int maxEmptyOccurCount = 6;
    int emptyOccurCount = 0;
    [SerializeField] int minEmptyLength = 5;
    [SerializeField] int maxEmptyLength = 10;

    [SerializeField] [Range(1, 15)] int minFloorYPrec = 5;
    int minFloorY = 0;
    [SerializeField] [Range(15, 25)] int maxFloorYPrec = 20;
    int maxFloorY = 0;

    [SerializeField] [Range(0, 50)] int airBlockPerc= 2;

    [Header("Obstacle Status")]
    [SerializeField] RuleTile obstacleTile;
    [SerializeField] [Range(1, 10)] int obstacleGeneratePerc = 2;

    [Header("Plat Status")]
    [SerializeField] RuleTile platTile;
    [SerializeField] [Range(1, 20)] int platGeneratePerc = 5;
    [SerializeField] [Range(30, 60)] int minPlatYPrec = 40;
    int minPlatY = 0;
    [SerializeField] [Range(60, 90)] int maxPlatYPrec = 80;
    int maxPlatY = 0;

    void Start()
    {
        InitData();
        InitMap();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);

        int nextTilemapIndex = (currentTilemapIndex + 1) % tilemaps.Count;
        if (tilemaps[nextTilemapIndex].transform.position.x < Camera.main.transform.position.x - CameraController.Instance.cameraOffsetX)
        {
            tilemaps[currentTilemapIndex].transform.position += new Vector3(oneMapWidth * tilemaps.Count, 0, 0);

            StartCoroutine(ChangeOneMap(tilemaps[currentTilemapIndex], currentTilemapIndex));

            currentTilemapIndex = nextTilemapIndex;
        }
    }

    void InitData()
    {
        mapMinY = 0;
        mapMaxY = CameraController.Instance.cameraOffsetY * 2;

        minFloorY = Mathf.FloorToInt(mapMaxY / 100.0f * minFloorYPrec);
        maxFloorY = Mathf.FloorToInt(mapMaxY / 100.0f * maxFloorYPrec);

        emptyOccurCount = maxEmptyOccurCount;

        minPlatY = Mathf.FloorToInt(mapMaxY / 100.0f * minPlatYPrec);
        maxPlatY = Mathf.FloorToInt(mapMaxY / 100.0f * maxPlatYPrec);
    }

    void InitMap()
    {
        for(int i = 0; i < tilemaps.Count; i++)
        {
            for(int j = 0; j < oneMapWidth; j++)
            {
                GenerateOneVolumn(tilemaps[i], i, j, 0);
            }
            tilemaps[i].transform.position += new Vector3(i * oneMapWidth, 0, 0);
        }
    }

    IEnumerator ChangeOneMap(Tilemap tilemap, int mapIndex)
    {
        for (int i = 0; i < oneMapWidth; i++)
        {
            DeleteOneVolumn(tilemap, mapIndex, i);
            GenerateOneVolumn(tilemap, mapIndex, i, oneMapWidth * (tilemaps.Count - 1));

            yield return new WaitForEndOfFrame();
        }
    }

    void GenerateOneVolumn(Tilemap tilemap, int mapIndex, int i, int obstacleOffsetX)
    {
        if (floorYCount <= 0)
        {
            if(emptyOccurCount <= 0)
            {
                currentFloorY = -1;
                floorYCount = Random.Range(minEmptyLength, maxEmptyLength);

                emptyOccurCount = Random.Range(minEmptyOccurCount, maxEmptyOccurCount); ;
            }
            else
            {
                currentFloorY = Random.Range(minFloorY, maxFloorY);

                floorYCount = Random.Range(minFloorLength, maxFloorLength);

                emptyOccurCount--;
            }
        }

        floorYCount--;

        if (currentFloorY < 0)
        {
            return;
        }

        int j = mapMinY;
        for (; j <= currentFloorY; j++)
        {
            if (Random.Range(0, 100) < airBlockPerc && j < currentFloorY)
            {
                continue;
            }

            tilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
        }

        if (Random.Range(0, 100) < obstacleGeneratePerc)
        {
            tilemap.SetTile(new Vector3Int(i, j, 0), obstacleTile);
        }

        if (Random.Range(0, 100) < platGeneratePerc)
        {
            int platY = Random.Range(minPlatY, maxPlatY);

            tilemap.SetTile(new Vector3Int(i, platY, 0), platTile);
        }
    }

    void DeleteOneVolumn(Tilemap tilemap, int mapIndex, int i)
    {
        for (int j = mapMinY; j <= mapMaxY; j++)
        {
            tilemap.SetTile(new Vector3Int(i, j, 0), null);
        }
    }
}
