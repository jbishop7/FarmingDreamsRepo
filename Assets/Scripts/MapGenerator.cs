using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap boundaryTilemap;
    public Tile walkableTile;
    public Tile boundaryTile;
    public GameObject player;

    public int mapWidth = 50; 
    public int mapHeight = 50;
    public int minRoomSize = 2; 
    public int maxRoomSize = 4;
    public int corridorWidth = 2;
    public int numberOfRooms = 10;

    private List<Vector3Int> roomCenters;

    void Start()
    {
        GenerateDungeon();
    }


    void GenerateDungeon()
    {
        walkableTilemap.ClearAllTiles();
        boundaryTilemap.ClearAllTiles();

        FillMapWithBoundaryTiles();

        roomCenters = new List<Vector3Int>();

        for (int i = 0; i < numberOfRooms; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int roomX = Random.Range(1, mapWidth - roomWidth - 1);
            int roomY = Random.Range(1, mapHeight - roomHeight - 1);

            CreateRoom(roomX, roomY, roomWidth, roomHeight);
        }

        GenerateCorridors();

        if (roomCenters.Count > 0)
        {
            PositionPlayer(roomCenters[0]);
        }
    }

    void FillMapWithBoundaryTiles()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                boundaryTilemap.SetTile(new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0), boundaryTile);
            }
        }
    }

    void CreateRoom(int x, int y, int width, int height)
    {
        Vector3Int roomCenter = new Vector3Int(x + width / 2, y + height / 2, 0);
        roomCenters.Add(roomCenter - new Vector3Int(mapWidth / 2, mapHeight / 2, 0));

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i - mapWidth / 2, j - mapHeight / 2, 0);
                walkableTilemap.SetTile(tilePosition, walkableTile);
                boundaryTilemap.SetTile(tilePosition, null);
            }
        }
    }

    void GenerateCorridors()
    {
        corridorWidth = 2;

        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            Vector3Int start = roomCenters[i];
            Vector3Int end = roomCenters[i + 1];

            // Generate corridors horizontally
            for (int dx = Mathf.Min(start.x, end.x); dx <= Mathf.Max(start.x, end.x); dx++)
            {
                for (int dy = -corridorWidth / 2; dy <= corridorWidth / 2; dy++)
                {
                    Vector3Int corridorPosition = new Vector3Int(dx, start.y + dy, 0);
                    if (IsWithinBounds(corridorPosition))
                    {
                        walkableTilemap.SetTile(corridorPosition, walkableTile);
                        boundaryTilemap.SetTile(corridorPosition, null);
                    }
                }
            }

            // Update the start position to the end of the horizontal corridor
            start = new Vector3Int(end.x, start.y, 0);

            // Generate corridors vertically
            for (int dy = Mathf.Min(start.y, end.y); dy <= Mathf.Max(start.y, end.y); dy++)
            {
                for (int dx = -corridorWidth / 2; dx <= corridorWidth / 2; dx++)
                {
                    Vector3Int corridorPosition = new Vector3Int(start.x + dx, dy, 0);
                    if (IsWithinBounds(corridorPosition))
                    {
                        walkableTilemap.SetTile(corridorPosition, walkableTile);
                        boundaryTilemap.SetTile(corridorPosition, null);
                    }
                }
            }
        }
    }

    // Helper method to check if a position is within the map bounds
    bool IsWithinBounds(Vector3Int position)
    {
        return position.x > -mapWidth / 2 && position.x < mapWidth / 2 - 1 &&
               position.y > -mapHeight / 2 && position.y < mapHeight / 2 - 1;
    }


    void PositionPlayer(Vector3Int playerSpawn)
    {
        player.transform.position = walkableTilemap.CellToWorld(playerSpawn);
    }
}



