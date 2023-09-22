using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    public Vector3 position;

    public bool isWalkable;

    public MapNode parent;

    public int gCost;

    public int hCost;

    public int fCost { get { return gCost + hCost; } }

    public MapNode(Vector3 pos, bool walkable)
    {
        position = pos;
        isWalkable = walkable;
    }
}
