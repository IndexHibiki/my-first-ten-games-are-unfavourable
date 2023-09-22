using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PathFinder : MonoBehaviour
{
    List<MapNode> openSet;
    List<MapNode> closedSet;

    GroundTileGenerator gtg;
    Vector2Int mapSize;
    LoopMap loopMap;

    private void Start()
    {
        gtg = FindObjectOfType<GroundTileGenerator>();
        mapSize = gtg.GetMapSize();
        loopMap = gtg.loopMap;
    }

    public List<Vector2Int> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // 寻路算法的具体实现
        MapNode startNode = NodeFromWorldPoint(startPos);
        MapNode targetNode = NodeFromWorldPoint(targetPos);
        Debug.Log(targetNode.position);
        Debug.Log(gtg.GetMovedOffsetX());
        Debug.Log(gtg.loopMap.currentMapIndex);
        startNode.gCost = 0;
        startNode.hCost = 0;

/*        if (!startNode.isWalkable || !targetNode.isWalkable)
        {
            return null;
        }*/
        if (!targetNode.isWalkable)
        {
            return null;
        }

        openSet = new List<MapNode> { startNode };
        closedSet = new List<MapNode>();

        while (openSet.Count > 0)
        {
            MapNode currentNode = GetLowestFCostNode(openSet);

            if (currentNode == targetNode)
            {
                Debug.Log("3333");
                return RetracePath(startNode, targetNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            AddToOpenSet(openSet, currentNode, startNode, targetNode);
        }
        Debug.Log("4444");
        return null;
    }

    void AddToOpenSet(List<MapNode> openSet, MapNode currentNode, MapNode startNode, MapNode targetNode)
    {
        foreach (MapNode neighbor in GetNeighbors(currentNode, startNode))
        {
            if (!neighbor.isWalkable || closedSet.Contains(neighbor))
            {
                continue;
            }

            int tentativeGCost = currentNode.gCost + GetDistance(currentNode, neighbor);

            if (!openSet.Contains(neighbor) || tentativeGCost < neighbor.gCost)
            {
                neighbor.gCost = tentativeGCost;
                neighbor.hCost = GetDistance(neighbor, targetNode);
                neighbor.parent = currentNode;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }
    }

    public List<Vector2Int> FindPath(Vector3 startPos, float targetX)
    {
        if(targetX < 0)
        {
            return null;
        }

        for (int i = 0; i < mapSize.y; i++)
        {
            if(NodeFromWorldPoint(new Vector3(targetX, i - Mathf.Abs(gtg.GetMapMinY()), 0)).isWalkable == true)
            {
                return FindPath(startPos, new Vector3(targetX, i - Mathf.Abs(gtg.GetMapMinY()) + 2, 0));
            }
        }

        return null;
    }

    public async Task<List<Vector2Int>> FindPathAsync(Vector3 startPos, float targetX)
    {
        return await Task.Run(() => FindPath(startPos, targetX));
    }

    private MapNode NodeFromWorldPoint(Vector3 worldPosition)
    {
/*        int x = Mathf.RoundToInt(worldPosition.x) - gtg.GetMovedOffsetX() + gtg.GetMapLength();
        int y = Mathf.RoundToInt(worldPosition.y) + Mathf.Abs(gtg.GetMapMinY());

        x = Mathf.Clamp(x, 0, mapSize.x - 1);
        y = Mathf.Clamp(y, 0, mapSize.y - 1);
        Debug.Log(worldPosition + ": " + x + "," + y);
        return map[x, y];*/

        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);

        return gtg.WorldPosToMapNode(x, y);
    }

    private List<MapNode> GetNeighbors(MapNode node, MapNode startNode)
    {
        List<MapNode> neighbors = new List<MapNode>();
        int offset = Mathf.RoundToInt(startNode.position.x) / mapSize.x * mapSize.x;
        int x = Mathf.RoundToInt(node.position.x) - offset;
        int y = Mathf.RoundToInt(node.position.y) + Mathf.Abs(gtg.GetMapMinY());

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                int checkX = x + dx;
                int checkY = y + dy;

/*                if (checkX >= 0 && checkX < mapSize.x && checkY >= 0 && checkY < mapSize.y)
                {
                    neighbors.Add(map[checkX, checkY]);
                }*/

                if(checkX >= 0 && checkY >= 0 && checkY < mapSize.y)
                {
                    Debug.Log(checkX);
                    if(checkX < mapSize.x)
                    {
                        neighbors.Add(loopMap.GetCurrentMap()[checkX, checkY]);
                    }
                    else
                    {
                        neighbors.Add(loopMap.GetNextMap()[checkX % loopMap.mapSize.x, checkY]);
                    }
                }
            }
        }

        return neighbors;
    }

    private MapNode GetLowestFCostNode(List<MapNode> nodeList)
    {
        MapNode lowestFCostNode = nodeList[0];

        foreach (MapNode node in nodeList)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    private List<Vector2Int> RetracePath(MapNode startNode, MapNode endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        MapNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(new Vector2Int(Mathf.RoundToInt(currentNode.position.x), Mathf.RoundToInt(currentNode.position.y)));
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(MapNode nodeA, MapNode nodeB)
    {
        int distX = Mathf.Abs(Mathf.RoundToInt(nodeA.position.x) - Mathf.RoundToInt(nodeB.position.x));
        int distY = Mathf.Abs(Mathf.RoundToInt(nodeA.position.y) - Mathf.RoundToInt(nodeB.position.y));

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}
