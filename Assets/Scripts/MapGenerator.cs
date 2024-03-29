using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap boundaryTilemap;
    public Tile walkableTile;
    public Tile boundaryTile;
    public GameObject player;

    public int mapWidth = 50; 
    public int mapHeight = 50;
    public int minRoomSize = 3; 
    public int maxRoomSize = 7;
    public int corridorWidth = 1;
    public int numberOfRooms = 20;

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
            int roomX = Random.Range(0, mapWidth - roomWidth);
            int roomY = Random.Range(0, mapHeight - roomHeight);

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
        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            Vector3Int start = roomCenters[i];
            Vector3Int end = roomCenters[i + 1];

            while (start.x != end.x)
            {
                walkableTilemap.SetTile(start, walkableTile);
                boundaryTilemap.SetTile(start, null);
                start.x += start.x < end.x ? 1 : -1;
            }

            while (start.y != end.y)
            {
                walkableTilemap.SetTile(start, walkableTile);
                boundaryTilemap.SetTile(start, null);
                start.y += start.y < end.y ? 1 : -1;
            }
        }
    }

    void PositionPlayer(Vector3Int playerSpawn)
    {
        player.transform.position = walkableTilemap.CellToWorld(playerSpawn);
    }
}



