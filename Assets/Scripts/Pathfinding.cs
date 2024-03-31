
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    Tilemap tilemap;
    public Transform target;
    List<Node> openSet;
    HashSet<Node> closedSet;
    public Vector2Int gridSize;
        
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        if (mapGenerator != null)
        {
            gridSize = new Vector2Int(mapGenerator.mapWidth, mapGenerator.mapHeight);
        }
        else
        {
            Debug.LogError("MapGenerator not found. Make sure it exists in the scene.");
        }
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        Node startNode = new Node(true, startPos, startPos);
        Node targetNode = new Node(true, targetPos, targetPos);

        openSet = new List<Node>();
        closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.worldPosition == targetNode.worldPosition)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Vector3Int>();
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector3Int checkPosition = node.worldPosition + new Vector3Int(x, y, 0);
                if (checkPosition.x >= 0 && checkPosition.x < gridSize.x && checkPosition.y >= 0 && checkPosition.y < gridSize.y)
                {
                    neighbours.Add(new Node(tilemap.HasTile(checkPosition), checkPosition, checkPosition));
                }
            }
        }

        return neighbours;
    }

    List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.worldPosition.x - nodeB.worldPosition.x);
        int distY = Mathf.Abs(nodeA.worldPosition.y - nodeB.worldPosition.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}






