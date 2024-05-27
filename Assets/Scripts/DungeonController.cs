using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;


public class DungeonController : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap boundaryTilemap;
    public Tile walkableTile;
    public Tile walkableTileWithGrass;
    public Tile boundaryTile;

    [SerializeField] public TMP_Text Text;
    public GameObject player;

    public int mapWidth = 50;
    public int mapHeight = 50;
    public int minRoomSize = 2;
    public int maxRoomSize = 4;
    public int corridorWidth = 2;
    public int numberOfRooms = 10;

    private float grassChance = 0.15f;

    public GameObject _RoboGolem;
    public GameObject _Eyeball;
    public GameObject _Boss;

    public GameObject resistPanel;
    public GameObject speedPanel;

    public TextMeshProUGUI flaskCount;


    public int minSpawnDistanceFromPlayer = 10;

    private GameController gameController;

    private List<Vector3Int> roomCenters;
    private List<Vector3Int> occupiedSpawnLocations = new List<Vector3Int>();
    private List<Vector3> enemySpawnPoints = new List<Vector3>();

    public static DungeonController _instance;

    public static DungeonController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        gameController = GameController.Instance;

        GenerateDungeon();

       string[] buffs = gameController.GetBuffsInUse();


        resistPanel.SetActive(false);
        speedPanel.SetActive(false);


        for(int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i] == "speed_slurp")
            {
                speedPanel.SetActive(true);
            }

            if (buffs[i] == "resist")
            {
                resistPanel.SetActive(true);
            }
        }

        switch (gameController.GetDayCount())
        {
            case 1:
                SpawnEnemies(0, 3, 0, 1);
                break;
            case 2:
                SpawnEnemies(0, 5, 0, 1);
                break;
            case 3:
                SpawnEnemies(1, 3, 0, 2);
                break;
            case 4:
                SpawnEnemies(1, 5, 0, 1);
                break;
            case 5:
                SpawnEnemies(1, 7, 0, 1);
                break;
            case 6:
                SpawnEnemies(2, 5, 0, 1);
                break;
            case 7:
                SpawnEnemies(4, 2, 0, 2);
                break;
            case 8:
                SpawnEnemies(3, 5, 0, 2);
                break;
            case 9:
                SpawnEnemies(3, 8, 0, 2);
                break;
            case 10:
                SpawnEnemies(3, 2, 1, 3);
                break;

        }
        
    }

    private void Update()
    {
        updateEnemyCount();
        flaskCount.SetText($"{gameController.GetBerryAids()}");
    }

    public void SpawnEnemies(int eyes, int robos, int boss, int level)
    {
        
        SpawnEnemy(_Eyeball, eyes, level);
        SpawnEnemy(_RoboGolem, robos, level);
        SpawnEnemy(_Boss, boss, level);
    }

    private void updateEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Text.text = "Enemies remaining: " + enemies.Length;

        if (enemies.Length == 0)
        {
            gameController.DungeonSuccess();
        }
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
                Tile tileToUse = (Random.value < grassChance) ? walkableTileWithGrass : walkableTile;
                walkableTilemap.SetTile(tilePosition, tileToUse);
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

            for (int dx = Mathf.Min(start.x, end.x); dx <= Mathf.Max(start.x, end.x); dx++)
            {
                for (int dy = -corridorWidth / 2; dy <= corridorWidth / 2; dy++)
                {
                    Vector3Int corridorPosition = new Vector3Int(dx, start.y + dy, 0);
                    if (IsWithinBounds(corridorPosition))
                    {
                        Tile tileToUse = (Random.value < grassChance) ? walkableTileWithGrass : walkableTile;
                        walkableTilemap.SetTile(corridorPosition, tileToUse);
                        boundaryTilemap.SetTile(corridorPosition, null);
                    }
                }
            }

            for (int dy = Mathf.Min(start.y, end.y); dy <= Mathf.Max(start.y, end.y); dy++)
            {
                for (int dx = -corridorWidth / 2; dx <= corridorWidth / 2; dx++)
                {
                    Vector3Int corridorPosition = new Vector3Int(end.x + dx, dy, 0);
                    if (IsWithinBounds(corridorPosition))
                    {
                        Tile tileToUse = (Random.value < grassChance) ? walkableTileWithGrass : walkableTile;
                        walkableTilemap.SetTile(corridorPosition, tileToUse);
                        boundaryTilemap.SetTile(corridorPosition, null);
                    }
                }
            }
        }
    }


    bool IsWithinBounds(Vector3Int position)
    {
        return position.x > -mapWidth / 2 && position.x < mapWidth / 2 - 1 &&
               position.y > -mapHeight / 2 && position.y < mapHeight / 2 - 1;
    }


    void PositionPlayer(Vector3Int playerSpawn)
    {
        player.transform.position = walkableTilemap.CellToWorld(playerSpawn);
    }

    void SpawnEnemy(GameObject enemyPrefab, int numberOfEnemies, int level)
    {
        int spawnAttempts = 0;
        int spawnCount = 0;

        string enemyType = enemyPrefab.name;

        float minimumDistanceApart = 10f;

        while (spawnCount < numberOfEnemies && spawnAttempts < numberOfEnemies * 10)
        {
            Vector3Int spawnLocation = GetSpawnLocationAwayFromPlayer();
            Vector3 worldLocation = walkableTilemap.CellToWorld(spawnLocation);

            // Check distance from existing spawn points
            bool tooClose = enemySpawnPoints.Any(point => Vector3.Distance(point, worldLocation) < minimumDistanceApart);

            if (!tooClose)
            {
                GameObject g = Instantiate(enemyPrefab, worldLocation, Quaternion.identity);
                
                if (enemyType == "Boss")
                {
                    Boss b = g.GetComponent<Boss>();
                    b.SetMaxHealth(150);
                }

                if(enemyType == "RoboGolem")
                {
                    RoboGolem r = g.GetComponent<RoboGolem>();
                    r.SetMaxHealth(level * 15);
                }

                if (enemyType == "EyeballSquid")
                {
                    EyeballSquid e = g.GetComponent<EyeballSquid>();
                    e.SetMaxHealth(10 * level);
                }
                enemySpawnPoints.Add(worldLocation);
                spawnCount++;

            }

            spawnAttempts++;
        }

        if (spawnAttempts == numberOfEnemies * 10)
        {
            Debug.LogWarning("Could not find enough valid spawn points for enemies.");
        }
    }

    Vector3Int GetSpawnLocationAwayFromPlayer()
    {
        const int maxAttempts = 100;
        for (int i = 0; i < maxAttempts; i++)
        {
            int x = Random.Range(1, mapWidth - 1);
            int y = Random.Range(1, mapHeight - 1);
            Vector3Int tilePosition = new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0);

            if (walkableTilemap.HasTile(tilePosition) &&
                Vector3.Distance(walkableTilemap.CellToWorld(tilePosition), player.transform.position) > minSpawnDistanceFromPlayer &&
                !occupiedSpawnLocations.Contains(tilePosition))
            {
                return tilePosition;
            }
        }
        return new Vector3Int(0, 0, 0);
    }


}



