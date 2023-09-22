using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GroundTileGenerator : MonoBehaviour
{
    [Header("Ground Status")]

    [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
    int currentTileMapIndex = 0;

    [SerializeField] RuleTile groundTile;

    [HideInInspector] public LoopMap loopMap;
    [SerializeField] int mapWidth = 100;
    int mapMinY = -49;
    int mapMaxY = 50;
    int mapHeight = 100;
    int cameraOffsetX = 20;

    int movedOffsetX = 0;

    [SerializeField] int minFloorY = -13;
    [SerializeField] int maxFloorY = -7;
    [SerializeField] int minFloorLength = 10;
    [SerializeField] int maxFloorLength = 20;

    [SerializeField][Range(0, 100)] int airBlockPercentage = 2;

    int currentFloorY = 0;
    int floorYCount = 0;
    int generateVolumnIndex = -37;

    [Header("Obstacle Status")]

    [SerializeField] [Range(1, 50)] int obstacleGenerateRate = 1;

    [SerializeField] List<ObstacleStatus> obstacles;
    [SerializeField] int maxObstacleY = 0;
    [SerializeField] float obstacleExistTime;
    int totalWeight = 0;
    List<int> weightMap;

    void Start()
    {
        InitWeight();

        initLoopMap();

        initGroundTile();

        CheckNullMapNode();
    }

    private void FixedUpdate()
    {
        int nextTileMapIndex = (currentTileMapIndex + 1) % tilemaps.Count;
        if (tilemaps[nextTileMapIndex].transform.position.x < Camera.main.transform.position.x - cameraOffsetX)
        {
            tilemaps[currentTileMapIndex].transform.position += new Vector3(mapWidth * tilemaps.Count, 0, 0);

            StartCoroutine(ChangeGroundTile(tilemaps[currentTileMapIndex], loopMap.currentMapIndex));

            currentTileMapIndex = nextTileMapIndex;
            loopMap.currentMapIndex = nextTileMapIndex;
        }
    }

    void initLoopMap()
    {
        Vector2 pointLeftBottom = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 pointTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        cameraOffsetX = Mathf.FloorToInt(pointTopRight.x);
        Camera.main.transform.position += new Vector3(cameraOffsetX, 0, 0);

        mapMinY = Mathf.FloorToInt(pointLeftBottom.y) - 1;
        mapMaxY = Mathf.FloorToInt(pointTopRight.y) + 1;
        mapHeight = mapMaxY - mapMinY + 1;

        loopMap = new LoopMap(tilemaps.Count, mapWidth, mapHeight);

        loopMap.currentMapIndex = 0;
    }

    void initGroundTile()
    {
        for(int tileMapIndex = 0; tileMapIndex < tilemaps.Count; tileMapIndex++)
        {
            for (generateVolumnIndex = 0; generateVolumnIndex < mapWidth; generateVolumnIndex++)
            {
                GenerateOneVolumn(tilemaps[tileMapIndex], tileMapIndex, generateVolumnIndex);
            }

            movedOffsetX += mapWidth;
            tilemaps[tileMapIndex].transform.position = new Vector3(mapWidth, 0, 0) * tileMapIndex;
        }
    }

    IEnumerator ChangeGroundTile(Tilemap tilemap, int mapIndex)
    {
        for (generateVolumnIndex = 0; generateVolumnIndex < mapWidth; generateVolumnIndex++)
        {
            DeleteOneVolumn(tilemap, mapIndex, generateVolumnIndex);
        }

        for (generateVolumnIndex = 0; generateVolumnIndex < mapWidth; generateVolumnIndex++)
        {
            GenerateOneVolumn(tilemap, mapIndex, generateVolumnIndex);

            yield return new WaitForEndOfFrame();
        }

        movedOffsetX += mapWidth;
    }

    void GenerateOneVolumn(Tilemap tilemap, int mapIndex, int i)
    {
        if (floorYCount <= 0)
        {
            currentFloorY = (int)Mathf.Floor(Random.Range(minFloorY, maxFloorY));

            floorYCount = (int)Mathf.Floor(Random.Range(minFloorLength, maxFloorLength));
        }

        int j = mapMinY;
        for (; j <= currentFloorY; j++)
        {
            Vector3 nodePosition = new Vector3(i + movedOffsetX, j, 0);
            bool isWalkable = false;
            //nodeMap[i, j + Mathf.Abs(mapMinY)] = new MapNode(nodePosition, isWalkable);
            loopMap.SetOneNode(mapIndex, RelativePosToMapPos(i, j), new MapNode(nodePosition, isWalkable));

            if (Random.Range(0, 100) < airBlockPercentage && j < currentFloorY)
            {
                continue;
            }

            tilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
        }

        for(; j <= mapMaxY; j++)
        {
/*            if(nodeMap[i, j + Mathf.Abs(mapMinY)] != null)
            {
                continue;
            }*/
            if(loopMap.GetOneNode(mapIndex, i, j + Mathf.Abs(mapMinY)) != null)
            {
                continue;
            }

            Vector3 nodePosition = new Vector3(i + movedOffsetX, j, 0);
            bool isWalkable = true;
            //nodeMap[i, j + Mathf.Abs(mapMinY)] = new MapNode(nodePosition, isWalkable);
            loopMap.SetOneNode(mapIndex, RelativePosToMapPos(i, j), new MapNode(nodePosition, isWalkable));
        }

        if (Random.Range(0, 100) < obstacleGenerateRate)
        {
            int obstacleIndex = GetRandomObstacleIndex();
            GameObject obstacle = Instantiate(obstacles[obstacleIndex].GetObstaclePrefab(), transform);

            int obstacleY = currentFloorY;
            if (obstacles[obstacleIndex].isNeedRandomY() == true)
            {
                obstacleY = Random.Range(currentFloorY, maxObstacleY);
                
            }

            Vector2 obstacleSize = obstacles[obstacleIndex].GetSize();
            if (i + obstacleSize.x >= mapWidth || obstacleY - mapMinY + obstacleSize.y + 1 >= mapHeight)
            {

            }
            else
            {
                obstacle.transform.position = new Vector2(i + movedOffsetX, obstacleY);

                for (int a = 0; a < obstacleSize.x; a++)
                {
                    for (int b = 1; b <= obstacleSize.y; b++)
                    {
                        Vector3 nodePosition = new Vector3(i + movedOffsetX + a, j + b, 0);
                        bool isWalkable = false;
                        //nodeMap[i + a, obstacleY + Mathf.Abs(mapMinY) + b] = new MapNode(nodePosition, isWalkable);
                        loopMap.SetOneNode(mapIndex, RelativePosToMapPos(i + a, obstacleY + b), new MapNode(nodePosition, isWalkable));
                    }
                }
            }

            Destroy(obstacle, obstacleExistTime);
        }

        floorYCount--;
    }

    void DeleteOneVolumn(Tilemap tilemap, int mapIndex, int i)
    {
        for (int j = mapMinY; j <= mapMaxY; j++)
        {
            tilemap.SetTile(new Vector3Int(i, j, 0), null);
            //nodeMap[i, j + Mathf.Abs(mapMinY)] = null;
            loopMap.SetOneNode(mapIndex, RelativePosToMapPos(i, j), null);
        }
    }

    void InitWeight()
    {
        weightMap = new List<int>();

        foreach(ObstacleStatus obstacleStatus in obstacles)
        {
            totalWeight += obstacleStatus.GetWeight();

            weightMap.Add(totalWeight);
        }
    }

    int GetRandomObstacleIndex()
    {
        int weight = Random.Range(0, totalWeight);

        for(int i = 0; i < weightMap.Count; i++)
        {
            if(weight < weightMap[i])
            {
                return i;
            }
        }

        return weightMap.Count - 1;
    }

    void CheckNullMapNode()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (loopMap.GetOneNode(0, i, j) == null)
                {
                    Debug.Log("Map0 Null: " + i + ", " + j);
                }
            }
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (loopMap.GetOneNode(1, i, j) == null)
                {
                    Debug.Log("Map1 Null: " + i + ", " + j);
                }
            }
        }
    }

    public Vector2Int RelativePosToMapPos(Vector2Int worldPos)
    {
        return new Vector2Int(worldPos.x, worldPos.y + Mathf.Abs(mapMinY));
    }

    public Vector2Int RelativePosToMapPos(int worldX, int worldY)
    {
        return new Vector2Int(worldX, worldY + Mathf.Abs(mapMinY));
    }

    public MapNode WorldPosToMapNode(Vector2Int worldPos)
    {
        return WorldPosToMapNode(worldPos.x, worldPos.y);
    }

    public MapNode WorldPosToMapNode(int worldX, int worldY)
    {
        int mapIndex = worldX / mapWidth % loopMap.mapLength;
        int x = worldX % mapWidth;
        int y = worldY + Mathf.Abs(mapMinY);

        return loopMap.GetOneNode(mapIndex, x, y);
    }

    public Vector2Int MapPosToRelativePos(Vector2Int mapPos)
    {
        return new Vector2Int(mapPos.x, mapPos.y - Mathf.Abs(mapMinY));
    }

    public Vector2Int MapPosToRelativePos(int mapX, int mapY)
    {
        return new Vector2Int(mapX, mapY - Mathf.Abs(mapMinY));
    }

    public int GetMovedOffsetX()
    {
        return movedOffsetX;
    }

    public Vector2Int GetMapSize()
    {
        return new Vector2Int(mapWidth, mapHeight);
    }

    public int GetMapWidth()
    {
        return mapWidth;
    }

    public int GetMapMinY()
    {
        return mapMinY;
    }
}