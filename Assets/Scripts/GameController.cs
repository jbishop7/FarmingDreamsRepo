using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    private Dictionary<string, int> playerInventory = new();
    private Dictionary<string, int> playerTools = new();
    private int playerGold = 50;

    public TextMeshProUGUI inventoryAdditions;
    private readonly float invAdditionTimer = 3f;
    private float timer = 3f;
    private bool startTimer = false;

    public GameObject journalPanel;
    private bool showingJournal = false;

    private TextMeshProUGUI moneyText;

    public GameObject craftingPanel;
    private bool showingCrafting = false;

    public GameObject inventoryPanel;
    public TextMeshProUGUI toolsText;
    public TextMeshProUGUI inventoryText;
    private bool showingInventory = false;

    private string currentScene = "farm"; // otherwise could be "dungeon"
    private bool playerFellAsleep = false;

    private List<GameObject> structures = new();
    public GameObject treePrefab;

    public GameObject bambooRepairPrefab;
    public GameObject bambooFarmPrefab;

    public GameObject potatoRepairPrefab;
    public GameObject potatoFarmPrefab;

    public GameObject cornRepairPrefab;
    public GameObject cornFarmPrefab;

    public GameObject berryRepairPrefab;
    public GameObject berryFarmPrefab;

    public GameObject craftingRepairPrefab;
    public GameObject craftingBenchPrefab;

    public GameObject restrictedPrefab;
    
    

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No game controller");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // not doing this yet...
        }

        playerTools.Add("axe", 1);
        CreateInitialStructures();

    }
    void Start()
    {
        InitializeGameController();

        inventoryAdditions.SetText("");
        inventoryPanel.SetActive(false);
        craftingPanel.SetActive(false);
        journalPanel.SetActive(false);
    }

    public void InitializeGameController()
    {
        if (currentScene == "farm")
        {
            inventoryPanel = null;
            journalPanel = null;
            craftingPanel = null;

            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
            Panel[] panels = FindObjectsOfType<Panel>(true);


            foreach (TextMeshProUGUI ui in texts)
            {
                switch (ui.name)
                {
                    case "ToolsText":
                        toolsText = ui; break;
                    case "InventoryText":
                        inventoryText = ui; break;
                    case "InventoryAdditions":
                        inventoryAdditions = ui; break;
                    case "MoneyText":
                        moneyText = ui; break;

                }
                Debug.Log(ui.name);
            }

            foreach (Panel go in panels)
            {
                Debug.Log(go.gameObject.name);
                switch (go.gameObject.name)
                {
                    case "Journal":
                        journalPanel = go.gameObject; break;
                    case "InventoryPanel":
                        inventoryPanel = go.gameObject; break;
                    case "CraftingPanel":
                        craftingPanel = go.gameObject; break;
                }
            }

            inventoryAdditions.SetText("");
            inventoryPanel.SetActive(false);
            craftingPanel.SetActive(false);
            journalPanel.SetActive(false);
            moneyText.SetText($"{playerGold}");
        }
        else
        {
            Debug.Log("We are NOT on the farm.");
        }
        
        // might do something different tbh, make each component talk to the GC and add itself...
    }

    // Update is called once per frame
    void Update()
    {
        if(currentScene == "farm")
        {
            if (startTimer)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    inventoryAdditions.SetText("");
                    startTimer = false;
                    timer = invAdditionTimer;
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                showingJournal = !showingJournal;
            }

            if (showingJournal)
            {
                journalPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                journalPanel.SetActive(false);
                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (showingCrafting || showingInventory)
                {
                    showingInventory = false;
                    showingCrafting = false;
                    showingJournal = false;
                }
            }

            if (showingCrafting)
            {
                Time.timeScale = 0f;
                craftingPanel.SetActive(true);
            }
            else
            {
                craftingPanel.SetActive(false);
                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                showingInventory = !showingInventory;
            }

            if (showingInventory)
            {
                Time.timeScale = 0f;
                ShowInventory();
            }
            else
            {
                inventoryPanel.SetActive(false);
                Time.timeScale = 1f;
            }

            if (playerFellAsleep)
            {
                Time.timeScale = 0f;
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    EndDay();
                }

            }
        }
        else
        {

        }


    }

    public void ShowInventory()
    {
        string tools = "";
        
        foreach(var (key, val) in playerTools)
        {
            tools += $"{key} x{val}\n";
        }
        toolsText.SetText(tools);

        string items = "";

        foreach(var (key, val) in playerInventory)
        {
            items += $"{key} x{val}\n";
        }
        inventoryText.SetText(items);

        inventoryPanel.SetActive(true);
    }

    public void AddToInventory(string item, int quantity)
    {
        if (playerInventory.ContainsKey(item))
        {
            playerInventory[item] += quantity;
        }
        else
        {
            playerInventory.Add(item, quantity);
        }

        InventoryAddition($"{quantity} {item}");
    }

    private void InventoryAddition(string addition)
    {
        startTimer = true;
        inventoryAdditions.SetText($"Added {addition} to your inventory");
    }

    private void GuiHint(string hint)
    {
        startTimer = true;
        inventoryAdditions.SetText($"{hint}");
    }

    public bool UseInventoryItems(string item, int quantity)
    {
        if (!playerInventory.ContainsKey(item))
        {
            return false;
        }

        if (playerInventory[item] >= quantity)
        {
            // then we can use these items
            playerInventory[item] -= quantity;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetInventoryItemCount(string item)
    {
        if (playerInventory.ContainsKey(item))
        {
            return playerInventory[item];
        }
        else
        {
            return -1;
        }
    }

    public void SellItem(string item, int price)
    {
        // so we are just going to sell 1 of this item every time we click it
        if (playerInventory.ContainsKey(item))
        {
            if (playerInventory[item] > 0)
            {
                playerInventory[item]--;
                playerGold += price;
            }
        }
        moneyText.SetText($"{playerGold}");
    }

    public int BuyItem(string item, int price)
    {
        if (playerGold >= price)
        {
            AddToInventory(item, 1);
            playerGold -= price;
        }
        moneyText.SetText($"{playerGold}");
        return playerGold;
        
    }
    public bool CheckToolInventory(string tool)
    {
        if (playerTools.ContainsKey(tool))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // CRAFTING METHODS
    public void CraftBambooSword()
    {
        if (UseInventoryItems("bamboo", 15))
        {
            playerTools.Add("bamboo_sword", 1);
            Debug.Log("Created bamboo sword");
            GuiHint("Bamboo Sword Created.");
            Player p = Player.Instance;
            p.SetHint("Press 1 and 2 to swap between your tools!");
        }
        else
        {
            GuiHint("Insufficient Materials!");
        }
        
    }

    public void ShowCrafting()
    {
        showingCrafting = true;
        Time.timeScale = 0f;
    }

    public void EndDay()
    {
        if (playerFellAsleep)
        {
            Time.timeScale = 1f;
            playerFellAsleep = false;
            Debug.Log("We fell asleep. Now we must fight with nothing good.");
            SceneManager.LoadScene(1);
            return;
        }

        if (CheckToolInventory("bamboo_sword"))
        {
            Debug.Log("I want to end the day. And my life.");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("I cannot end the day");
        }
        // now we would travel to the nightmare world
        // Add check that they have a sworda
       
    }

    public void ForceEndDay()
    {
        playerFellAsleep = true;
        inventoryAdditions.SetText("You fell asleep on the job. You will go into the Nightmare world unprepared. Good luck.\nPress Right Shift to continue.");
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            currentScene = "dungeon";
        }
        else
        {
            currentScene = "farm";
        }
        InitializeGameController();
    }

    public void EndDemo()
    {
        SceneManager.LoadScene(0);
    }

    private void SpawnStructures()
    {

    }

    private void SaveStructures()
    {

    }

    private void CreateInitialStructures()
    {
        Vector3[] positions =
        {
            new Vector3(-3.82f, -2.76f, 0f),    // trees
            new Vector3(-5.4f, -11.65f, 0f),
            new Vector3(12.64f, -10.36f, 0f),
            new Vector3(26.53f, -4.15f, 0f),
            new Vector3(12.79f, 3.97f, 0f),
            new Vector3(0.26f, -4.31f, 0f),
            new Vector3(-5.8f, -4.24f, 0f),
            new Vector3(2.87f, -4.34f, 0f),
            new Vector3(-2.11f, 4.72f, 0f), // workbench
            new Vector3(-3.91f, -7.05f, 0f),   // restricteds
            new Vector3(13.1f, -5.76f, 0f),
            new Vector3(15.02f, 9.3f, 0f),
            new Vector3(23.23f, -2.07f, 0f),
            new Vector3(6.24f, -0.29f, 0f),  // this one is rotated -41.16 deg on the Z
            new Vector3(1.38f, 3.02f, 0f),  // bamboo farms now
            new Vector3(1.38f, 4.64f, 0f),
            new Vector3(22.44f, -6.73f, 0f),  // potato farms
            new Vector3(27.57f, -6.73f, 0f),
            new Vector3(-0.52f, -10.76f, 0f),  // corn farms
            new Vector3(8.44f, -9.81f, 0f),
            new Vector3(21.45f, 12.45f, 0f),  // berry farms
            new Vector3(21.45f, 10.88f, 0f),
            new Vector3(0, 0, 0f),

        };

        // do the trees first
        for (int i = 0; i < 8; i++)
        {
            Instantiate(treePrefab, positions[i], treePrefab.transform.rotation);
        }
        // workbench
        Instantiate(craftingRepairPrefab, positions[8], craftingRepairPrefab.transform.rotation);
        // then restricteds
        for (int i = 9; i < 13; i++)
        {
            Instantiate(restrictedPrefab, positions[i], restrictedPrefab.transform.rotation);
        }
        GameObject rR = Instantiate(restrictedPrefab, positions[13], restrictedPrefab.transform.rotation);
        Rigidbody2D rRB = rR.GetComponent<Rigidbody2D>();
        rRB.rotation = -41.16f;
        // then bamboo farms
        Instantiate(bambooRepairPrefab, positions[14], bambooRepairPrefab.transform.rotation);
        GameObject t2BbFarm = Instantiate(bambooRepairPrefab, positions[15], bambooRepairPrefab.transform.rotation);
        RepairableStructure rsBbFarm = t2BbFarm.GetComponent<RepairableStructure>();
        rsBbFarm.itemQuantity = 40;

        // then potato farms
        Instantiate(potatoRepairPrefab, positions[16], potatoRepairPrefab.transform.rotation);
        GameObject t2PtFarm = Instantiate(potatoRepairPrefab, positions[17], potatoRepairPrefab.transform.rotation);
        RepairableStructure rsPtFarm = t2PtFarm.GetComponent<RepairableStructure>();
        rsPtFarm.itemQuantity = 40;

        // then corn farms
        Instantiate(cornRepairPrefab, positions[18], cornRepairPrefab.transform.rotation);
        GameObject t2CnFarm = Instantiate(cornRepairPrefab, positions[19], cornRepairPrefab.transform.rotation);
        RepairableStructure rsCnFarm = t2CnFarm.GetComponent<RepairableStructure>();
        rsCnFarm.itemQuantity = 40;
        // then berry farms
        Instantiate(berryRepairPrefab, positions[20], berryRepairPrefab.transform.rotation);
        GameObject t2Bfarm = Instantiate(berryRepairPrefab, positions[21], berryRepairPrefab.transform.rotation);
        RepairableStructure rsBFarm = t2Bfarm.GetComponent<RepairableStructure>();
        rsBFarm.itemQuantity = 40;
    }



}
