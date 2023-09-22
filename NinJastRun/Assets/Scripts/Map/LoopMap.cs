using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMap
{
    public MapNode[][,] map;

    public int mapLength;

    public Vector2Int mapSize;

    public int currentMapIndex;

    public LoopMap(int length, Vector2Int size)
    {
        mapLength = length;
        mapSize = size;
        map = new MapNode[length][,];

        for(int i = 0; i < length; i++)
        {
            map[i] = new MapNode[size.x, size.y];
        }
    }

    public LoopMap(int length, int x, int y)
    {
        mapLength = length;
        mapSize = new Vector2Int(x, y);
        map = new MapNode[length][,];

        for (int i = 0; i < length; i++)
        {
            map[i] = new MapNode[x, y];
        }
    }

    public MapNode[,] GetOneMap(int mapIndex)
    {
        return map[mapIndex];
    }

    public MapNode[,] GetCurrentMap()
    {
        return map[currentMapIndex];
    }

    public MapNode[,] GetNextMap()
    {
        return map[(currentMapIndex + 1) % mapLength];
    }

    public MapNode GetOneNode(int mapIndex, int x, int y)
    {
        return map[mapIndex][x, y];
    }

    public MapNode GetOneNode(int mapIndex, Vector2Int pos)
    {
        return map[mapIndex][pos.x, pos.y];
    }

    public void SetOneNode(int mapIndex, int x, int y, MapNode node)
    {
        map[mapIndex][x, y] = node;
    }

    public void SetOneNode(int mapIndex, Vector2Int pos, MapNode node)
    {
        map[mapIndex][pos.x, pos.y] = node;
    }
}
