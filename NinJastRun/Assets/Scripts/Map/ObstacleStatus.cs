using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleStatus
{
    [SerializeField] GameObject obstaclePrefab;

    [SerializeField] Vector2Int size;

    [SerializeField] int weight;

    [SerializeField] bool needRandomY;

    public GameObject GetObstaclePrefab()
    {
        return obstaclePrefab;
    }

    public Vector2Int GetSize()
    {
        return size;
    }

    public int GetWeight()
    {
        return weight;
    }

    public bool isNeedRandomY()
    {
        return needRandomY;
    }
}
