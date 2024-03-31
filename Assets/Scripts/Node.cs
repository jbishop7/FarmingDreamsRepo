using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3Int worldPosition;
    public Vector3Int gridPosition;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _walkable, Vector3Int _worldPosition, Vector3Int _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
