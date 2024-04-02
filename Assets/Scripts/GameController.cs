using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    private Dictionary<string, int> playerInventory = new();
    private Dictionary<string, int> playerTools = new();
    private int playerGold = 50;

    private TextMeshProUGUI inventoryAdditions;
    private readonly float invAdditionTimer = 4f;
    private float timer = 4f;
    private bool startTimer = false;

    private GameObject journalPanel;
    private bool showingJournal = false;

    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI calendarText;
    private TextMeshProUGUI journalGoalText;
    private TextMeshProUGUI journalText;

    private GameObject craftingPanel;
    private bool showingCrafting = false;

    private GameObject inventoryPanel;
    private TextMeshProUGUI toolsText;
    private TextMeshProUGUI inventoryText;
    private bool showingInventory = false;

    private TextMeshProUGUI techPointsText;

    private string currentScene = "farm"; // otherwise could be "dungeon"
    private bool playerFellAsleep = false;

    private List<string> structures = new();
    private List<Vector3> structureLocations = new();

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

    private int dayCounter = 0;

    private string tool1 = "axe";
    private string tool2 = "axe";  // will update this as we go, default to axe.
    private string buff1 = "";
    private string buff2 = "";

    private int techPoints = 9; // use these for the tech tree.

    // a ridiculous amount of bools for the tech tree
    private bool berryEnabled = false;
    private bool berryBountiful = false;
    private bool berrySpeed = false;
    private bool berrySword = false;
    private bool berrySword2 = false;

    private bool potatoEnabled = false;
    private bool potatoBountiful = false;
    private bool potatoSpeed = false;
    private bool potatoGun = false;
    private bool potatoGun2 = false;

    private bool cornEnabled = false;
    private bool cornBountiful = false;
    private bool cornSpeed = false;
    private bool cornGun = false;
    private bool cornGun2 = false;

    private bool dungeonRewardsAvailable = false;
    private string dungeonRewards = "";

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
        {   // its the first load!
            _instance = this;
            DontDestroyOnLoad(gameObject); // not doing this yet...
            CreateInitialStructures();
            playerTools.Add("axe", 1);
            playerInventory.Add("wood", 2000);
            playerInventory.Add("bamboo", 100);
            playerInventory.Add("ingot", 1);
        }
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
            dayCounter++;
            inventoryPanel = null;
            journalPanel = null;
            craftingPanel = null;
            buff1 = "";
            buff2 = "";

            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);
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
                    case "CalendarText":
                        calendarText = ui; break;
                    case "JournalGoal":
                        journalGoalText = ui; break;
                    case "JournalText":
                        journalText = ui; break;
                    case "TechPointsText":
                        techPointsText = ui; break;

                }
                
            }

            foreach (Panel go in panels)
            {
                
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
            playerFellAsleep = false;

            buff1 = "";
            buff2 = "";

        }
        else
        {
            // Debug.Log("We are NOT on the farm.");
            // check for buffs
            Debug.Log($"{buff1}, {buff2}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(1);
            currentScene = "dungeon";
        }
        if(currentScene == "farm")
        {
            calendarText.SetText(dayCounter.ToString());    // I hate that it has come to this but something bad is happening and this is the only fix
            moneyText.SetText($"{playerGold}");
            techPointsText.SetText($"{techPoints}");

            if (dungeonRewardsAvailable)
            {
                GuiHint(dungeonRewards);
                dungeonRewardsAvailable = false;
            }

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
                ShowJournal();
            }
            else
            {
                journalPanel.SetActive(false);
                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                TechManager tm = TechManager.Instance;
                tm.ShowTechTree();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                TechMenu techMenu = TechMenu.Instance;
                techMenu.ShowCurrentTech();
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


            /*if (showingCrafting)
            {
                craftingPanel.SetActive(true);
                Crafting c = Crafting.Instance;
                c.ShowBuilding();
            }
            else
            {
                craftingPanel.SetActive(false);
                Time.timeScale = 1f;
            } */

            if (Input.GetKeyDown(KeyCode.I))
            {
                showingInventory = !showingInventory;
            }

            if (showingInventory)
            {
                ShowInventory();
            }
            else
            {
                inventoryPanel.SetActive(false);
                Time.timeScale = 1f;
            }

            if (playerFellAsleep == true)
            {
                Debug.Log("Player fell asleep?");
                Time.timeScale = 0f;
            }
            if (playerFellAsleep && Input.GetKeyDown(KeyCode.RightShift))
            {
                EndDay();
            }

            if (showingInventory || showingJournal || showingCrafting)
            {
                Time.timeScale = 0f;
            }

        }
        else
        {
            // dungeon code here
            if (Input.GetKeyDown(KeyCode.P))
            {
                DungeonSuccess();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                DungeonFail();
            }
        }


    }

    public void ShowJournal()
    {
        switch (dayCounter)
        {
            case 1:
                journalGoalText.SetText("GOAL: Living off the land");
                journalText.SetText("Chop Trees\nRepair Bamboo Plantation\nRepair Crafting Station\nPlant Bamboo\nHarvest 15 bamboo\nCreate Bamboo Sword\nSleep in Tent");
                break;
            case 2:
            case 3:
                journalGoalText.SetText("GOAL: EXPLORE");
                journalText.SetText("Meet Maurice\nHarvest Materials\nPrepare for the Night!\n");
                break;
            case 4:
            case 5:
                journalGoalText.SetText("GOAL: IMPROVE");
                journalText.SetText("Find New Crops\nHarvest Materials\nPrepare for the Night!\n");
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                journalGoalText.SetText("GOAL:UPGRADE");
                journalText.SetText("Upgrade Tools\nBuild Stronger Weapons\nUpgrade Your Farms");
                break;
            case 10:
                journalGoalText.SetText("GOAL: PREPARE");
                journalText.SetText("It's time to take back the world of Nightmares. Make sure you are prepared for the final Night.");
                break;
        }

        journalPanel.SetActive(true);

    }

    public void ShowInventory()
    {
        string tools = "";
        
        foreach(var (key, val) in playerTools)
        {
            if (val > 0)
            {
                tools += $"{key} x{val}\n";
            }
        }
        toolsText.SetText(tools);

        string items = "";

        foreach(var (key, val) in playerInventory)
        {
            if (val > 0)
            {
                items += $"{key} x{val}\n";
            }
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

    public void AddToInventory(string[] items, int[] quantity)
    {
        string invAdditions = "";
        for (int i = 0; i < items.Length; i++)
        {
            if (playerInventory.ContainsKey(items[i]))
            {
                playerInventory[items[i]] += quantity[i];
            }
            else
            {
                playerInventory.Add(items[i], quantity[i]);
            }
            invAdditions += $"{quantity[i]} {items[i]},";
        }

        invAdditions = invAdditions.Remove(invAdditions.Length - 1);


        InventoryAddition(invAdditions);
    }

    private void InventoryAddition(string addition)
    {
        startTimer = true;
        inventoryAdditions.SetText($"Added {addition} to your inventory");
    }

    public void GuiHint(string hint)
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

    public bool UseInventoryItems(string[] items, int[] quantities)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!playerInventory.ContainsKey(items[i]))
            {
                return false;
            }

            if (playerInventory[items[i]] >= quantities[i])
            {
                // then we can use these items
                playerInventory[items[i]] -= quantities[i];
            }
            else
            {
                return false;
            }
        }
        return true;
        
    }

    public bool UseInventoryToolsAndItems(string[] items, int[] quantities)
    {
        Dictionary<string, int> allPlayerInventories = new();
        foreach(var(key, val) in playerTools)
        {
            allPlayerInventories.Add(key, val);
        }
        foreach(var(key, val) in playerInventory){
            allPlayerInventories.Add(key, val);
        }
        // check if the items exist in our total inventory
        for (int i = 0; i < items.Length; i++)
        {
            if (!allPlayerInventories.ContainsKey(items[i]))
            {
                return false;
            }
            if (allPlayerInventories[items[i]] < quantities[i])
            {
                return false;
            }
        }
        // if we made it here then we can start removing items from both inventories.
        for (int i = 0; i < items.Length; i++)
        {
            if (playerTools.ContainsKey(items[i]))
            {
                playerTools[items[i]] -= quantities[i];
                if (playerTools[items[i]] < 1)
                {
                    playerTools.Remove(items[i]);
                }
            }
            if (playerInventory.ContainsKey(items[i]))
            {
                playerInventory[items[i]] -= quantities[i];
            }
        }

        return true;
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

    public void Sell5Item(string item, int price)
    {
        // so we are just going to sell 1 of this item every time we click it
        if (playerInventory.ContainsKey(item))
        {
            if (playerInventory[item] > 5)
            {
                playerInventory[item] -= 5;
                playerGold += price * 5;
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
        playerTools.Add("bamboo_sword", 1);
        GuiHint("Bamboo Sword Created.");
        Player p = Player.Instance;
        p.SetHint("Go to the tent (when ready) to equip your new weapon for the Night!");

    }

    public void CraftBambooSwordII()
    {
        playerTools.Add("bamboo_sword_II", 1);
        GuiHint("Bamboo Sword II Created.");
    }

    public void CraftThornedSword()
    {
        playerTools.Add("thorned_sword", 1);
        GuiHint("Thorned Sword Created.");
    }

    public void CraftThornedSwordII()
    {
        playerTools.Add("thorned_sword_II", 1);
        GuiHint("Thorned Sword II Created.");
    }

    public void CraftCornShooter()
    {
        playerTools.Add("corn_shooter", 1);
        GuiHint("Corn Shooter Created.");
    }

    public void CraftCornShooterII()
    {
        playerTools.Add("corn_shooter_II", 1);
        GuiHint("Corn Shooter II Created.");
    }

    public void CraftPotatoGun()
    {
        playerTools.Add("RPotatoG", 1);
        GuiHint("RPotatoG Created.");
    }

    public void CraftPotatoGunII()
    {
        playerTools.Add("RPotatoG_II", 1);
        GuiHint("RPotatoG Created.");
    }

    public void CraftBerryAid()
    {
        AddToInventory("berry_aid", 1);
        GuiHint("Berry Aid Created.");
    }

    public void CraftSpeedSlurp()
    {
        AddToInventory("speed_slurp", 1);
        GuiHint("Speed Slurp Created.");
    }

    public void CraftResist()
    {
        AddToInventory("resist", 1);
        GuiHint("Resist Buff Created.");
    }

    // END OF CRAFTING METHODS
    public void EndDay()
    {
        if (playerFellAsleep)
        {
            playerFellAsleep = false;
            Time.timeScale = 1f;
            Debug.Log("We fell asleep. Now we must fight with nothing good.");
            SaveStructures();
            SceneManager.LoadScene(1);
            return;
        }
        dungeonRewards = "";
        dungeonRewardsAvailable = false;
        SaveStructures();
        SceneManager.LoadScene(1);
         /* if (CheckToolInventory("bamboo_sword")) // TODO get rid of this shit
        {
            Debug.Log("I want to end the day.");
            SaveStructures();
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("I cannot end the day");
        } */
        playerFellAsleep = false;

    }

    public void ForceEndDay()
    {
        playerFellAsleep = true;
        inventoryAdditions.SetText("You fell asleep on the job. You will go into the Nightmare world unprepared. Good luck.\nPress Right Shift to continue.");
    }

    private void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1f;
        if (level == 1)
        {
            currentScene = "dungeon";
        }
        else
        {
            currentScene = "farm";
            SpawnStructures();
        }
        InitializeGameController();
    }

    public void EndDemo()
    {
        SceneManager.LoadScene(0);
    }

    private void SpawnStructures()
    {
        for (int i = 0;  i < structures.Count; i++)
        {
            switch(structures[i])
            {
                case "BrokenBamboo":
                    if (structureLocations[i] == new Vector3(1.38f, 4.64f, 0f))
                    {
                        RepairableStructure bamRs = Instantiate(bambooRepairPrefab, structureLocations[i], bambooRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
                        int[] quantities = { 40, 30 };
                        bamRs.itemQuantities = quantities;
                        string[] items = { "wood", "bamboo" };
                        bamRs.itemsRequired = items;
                    }
                    else
                    {
                        Instantiate(bambooRepairPrefab, structureLocations[i], bambooRepairPrefab.transform.rotation);
                    }
                    break;
                case "BambooFarm":
                    Instantiate(bambooFarmPrefab, structureLocations[i], bambooFarmPrefab.transform.rotation);
                    break;
                case "BrokenPotato":
                    if (structureLocations[i] == new Vector3(27.57f, -6.73f, 0f))
                    {
                        RepairableStructure potRs = Instantiate(potatoRepairPrefab, structureLocations[i], potatoRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
                        int[] quantities = { 40, 30 };
                        potRs.itemQuantities = quantities;
                        string[] items = { "wood", "bamboo" };
                        potRs.itemsRequired = items;
                    }
                    else
                    {
                        Instantiate(potatoRepairPrefab, structureLocations[i], potatoRepairPrefab.transform.rotation);
                    }
                    break;
                case "PotatoFarm":
                    Instantiate(potatoFarmPrefab, structureLocations[i], potatoFarmPrefab.transform.rotation);
                    break;
                case "BrokenBerry":
                    if (structureLocations[i] == new Vector3(21.45f, 10.88f, 0f))
                    {
                        RepairableStructure berryRs = Instantiate(berryRepairPrefab, structureLocations[i], berryRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
                        int[] quantities = { 40, 30 };
                        berryRs.itemQuantities = quantities;
                        string[] items = { "wood", "bamboo" };
                        berryRs.itemsRequired = items;
                    }
                    else
                    {
                        Instantiate(berryRepairPrefab, structureLocations[i], berryRepairPrefab.transform.rotation);
                    }
                    break;
                case "BerryFarm":
                    Instantiate(berryFarmPrefab, structureLocations[i], berryFarmPrefab.transform.rotation);
                    break;
                case "BrokenCorn":
                    if (structureLocations[i] == new Vector3(8.44f, -9.81f, 0f))
                    {
                        RepairableStructure cornRs = Instantiate(cornRepairPrefab, structureLocations[i], cornRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
                        int[] quantities = { 40, 30 };
                        cornRs.itemQuantities = quantities;
                        string[] items = { "wood", "bamboo" };
                        cornRs.itemsRequired = items;
                    }
                    else
                    {
                        Instantiate(cornRepairPrefab, structureLocations[i], cornRepairPrefab.transform.rotation);
                    }
                    break;
                case "CornFarm":
                    Instantiate(cornFarmPrefab, structureLocations[i], cornFarmPrefab.transform.rotation);
                    break;
                case "BrokenBench":
                    Instantiate(craftingRepairPrefab, structureLocations[i], craftingRepairPrefab.transform.rotation);
                    break;
                case "FixedBench":
                    Instantiate(craftingBenchPrefab, structureLocations[i], craftingBenchPrefab.transform.rotation);
                    break;
                case "Restricted":
                    if (structureLocations[i] == new Vector3(6.24f, -0.29f, 0f))
                    {
                        GameObject go = Instantiate(restrictedPrefab, structureLocations[i], restrictedPrefab.transform.rotation);
                        Rigidbody2D grb = go.GetComponent<Rigidbody2D>();
                        grb.rotation = -41.16f;
                    }
                    else
                    {
                        Instantiate(restrictedPrefab, structureLocations[i], restrictedPrefab.transform.rotation);
                    }
                    break;
            }
        }
        // and now we respawn the trees
        Vector3[] treePositions =
        {
            new Vector3(-3.82f, -2.76f, 0f),    // trees
            new Vector3(-5.4f, -11.65f, 0f),
            new Vector3(12.64f, -10.36f, 0f),
            new Vector3(26.53f, -4.15f, 0f),
            new Vector3(12.79f, 3.97f, 0f),
            new Vector3(0.26f, -4.31f, 0f),
            new Vector3(-5.8f, -4.24f, 0f),
            new Vector3(2.87f, -4.34f, 0f),
        };

        foreach(Vector3 position in treePositions)
        {
            Instantiate(treePrefab, position, treePrefab.transform.rotation);
        }


    }

    private void SaveStructures()
    {
        GameObject[] repairables = GameObject.FindGameObjectsWithTag("Repairable");
        GameObject[] farms = GameObject.FindGameObjectsWithTag("Farm");
        GameObject[] craftables = GameObject.FindGameObjectsWithTag("Craft");

        // now I need to track all the positions of this thing
        List<GameObject> structs = new();
        foreach (GameObject go in repairables)
        {
            structs.Add(go);
        }
        foreach (GameObject go in farms)
        {
            structs.Add(go);
        }
        foreach (GameObject go in craftables)
        {
            structs.Add(go);
        }

        List<string> structNames = new();
        List<Vector3> positions = new();
        foreach(GameObject go in structs)
        {
            if (go.name.Contains("BambooRepair"))
            {
                // we have a broken bamboo plot
                structNames.Add("BrokenBamboo");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("PotatoRepair"))
            {
                // we have a broken potato plot
                structNames.Add("BrokenPotato");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("CornRepair"))
            {
                // we have a broken corn plot
                structNames.Add("BrokenCorn");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("BerryRepair"))
            {
                // we have a broken berry plot
                structNames.Add("BrokenBerry");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("BambooPlot"))
            {
                // we have found the functional bamboo
                structNames.Add("BambooFarm");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("PotatoPlot"))
            {
                // we have found the functional potato
                structNames.Add("PotatoFarm");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("CornPlot"))
            {
                // we have found the functional corn
                structNames.Add("CornFarm");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("BerryPlot"))
            {
                // we have found the functional berry
                structNames.Add("BerryFarm");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("WorkbenchBroken"))
            {
                // we have a broken workbench
                structNames.Add("BrokenBench");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("WorkbenchFixed"))
            {
                // we have a fixed workbench
                structNames.Add("FixedBench");
                positions.Add(go.transform.position);
            }
            if (go.name.Contains("restricted"))
            {
                structNames.Add("Restricted");
                positions.Add(go.transform.position);
            }
        }

        structures = structNames;
        structureLocations = positions;
        
    }

    private void CreateInitialStructures()
    {
        if (currentScene != "farm")
        {
            return;
        }

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
            new Vector3(23.04f, -2.07f, 0f),
            new Vector3(6.24f, -0.29f, 0f),  // this one is rotated -41.16 deg on the Z
            new Vector3(1.38f, 3.02f, 0f),  // bamboo farms now
            new Vector3(1.38f, 4.64f, 0f),
            new Vector3(22.44f, -6.73f, 0f),  // potato farms
            new Vector3(27.57f, -6.73f, 0f),
            new Vector3(-0.52f, -10.76f, 0f),  // corn farms
            new Vector3(8.44f, -9.81f, 0f),
            new Vector3(21.45f, 12.45f, 0f),  // berry farms
            new Vector3(21.45f, 10.88f, 0f),
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
        RepairableStructure bamRs = Instantiate(bambooRepairPrefab, positions[15], bambooRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
        int[] quantities = { 40, 30 };
        bamRs.itemQuantities = quantities;
        string[] items = { "wood", "bamboo" };
        bamRs.itemsRequired = items;

        // then potato farms
        Instantiate(potatoRepairPrefab, positions[16], potatoRepairPrefab.transform.rotation);
        RepairableStructure potRs = Instantiate(potatoRepairPrefab, positions[17], potatoRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
        potRs.itemQuantities = quantities;
        potRs.itemsRequired = items;

        // then corn farms
        Instantiate(cornRepairPrefab, positions[18], cornRepairPrefab.transform.rotation);
        RepairableStructure cornRs = Instantiate(cornRepairPrefab, positions[19], cornRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
        cornRs.itemQuantities = quantities;
        cornRs.itemsRequired = items;
        // then berry farms
        Instantiate(berryRepairPrefab, positions[20], berryRepairPrefab.transform.rotation);
        RepairableStructure berryRs = Instantiate(berryRepairPrefab, positions[21], berryRepairPrefab.transform.rotation).GetComponent<RepairableStructure>();
        berryRs.itemQuantities = quantities;
        berryRs.itemsRequired = items;
    }

    public int GetDayCount()
    {
        return dayCounter;
    }

    public Dictionary<string, int> GetPlayerTools()
    {
        Dictionary<string, int> tools = new();

        foreach(var(key, val) in playerTools)
        {
            if (playerTools[key] > 0)
            {
                tools.Add(key, val);
            }
        }

        return tools;
    }

    public Dictionary<string, int> GetPlayerBuffs()
    {
        Dictionary<string, int> buffs = new();

        foreach(var(key, val) in playerInventory)
        {
            switch (key)
            {
                case "speed_slurp":
                    buffs.Add(key, val);
                    break;
                case "resist":
                    buffs.Add(key, val);
                    break;
            }
        }

        return buffs;
    }

    public void UpdateToolsInUse(string t1, string t2)
    {
        tool1 = t1;
        tool2 = t2;
    }

    public void UpdateBuffsInUse(string b1, string b2)
    {
        buff1 = b1;
        buff2 = b2;
    }

    public string[] GetToolsInUse()
    {
        string[] toolsInUse = { tool1, tool2 };
        return toolsInUse;
    }

    public void DungeonSuccess()
    {
        Debug.Log("Great success in the dungeon");
        UpdateToolsInUse("axe", "axe"); // they should be back on the farm, don't need a sword.
        // and then give rewards
        techPoints++;
        AddToInventory("dream_ingot", 1);
        AddToInventory("wood", 10); // for example...
        playerGold += 25;
        dungeonRewardsAvailable = true;
        dungeonRewards = "Rewarded:\n1 Dream Ingot, 10 wood, 25 gold, 1 Tech Point";
        SceneManager.LoadScene(0);
    }

    public void DungeonFail()
    {
        Debug.Log("Great failure in the dungeon");
        UpdateToolsInUse("axe", "axe");
        SceneManager.LoadScene(0);
    }

    public int GetTechPoints()
    {
        return techPoints;
    }

    public void SpendTechPoints(int cost)
    {
        techPoints -= cost;
    }

    public bool BerryEnabled()
    {
        return berryEnabled;
    }

    public void SetBerryEnabled(bool b)
    {
        if (b == true)
        {
            GuiHint("Berries are now farmable. Go find them!");
        }
        berryEnabled = b;
    }

    public bool BerrySpeed()
    {
        return berrySpeed;
    }

    public void SetBerrySpeed(bool b)
    {
        if (b == true)
        {
            GuiHint("Berry growth rate doubled.");
        }
        berrySpeed = b;
    }

    public bool BerryBountiful()
    {
        return berryBountiful;
    }

    public void SetBerryBountiful(bool b)
    {
        if (b == true)
        {
            GuiHint("Berry harvests are bountiful. Doubled your harvests.");
        }
        berryBountiful = b;
    }

    public bool BerrySword()
    {
        return berrySword;
    }

    public void SetBerrySword(bool b)
    {
        if (b == true)
        {
            GuiHint("Thorned Sword is available to craft. See your crafting bench.");
        }
        berrySword = b;
    }

    public bool BerrySword2()
    {
        return berrySword2;
    }

    public void SetBerrySword2(bool b)
    {
        if (b == true)
        {
            GuiHint("Thorned Sword II is available to craft. See your crafting bench.");
        }
        berrySword2 = b;
    }

    public bool PotatoEnabled()
    {
        return potatoEnabled;
    }

    public void SetPotatoEnabled(bool b)
    {
        if (b == true)
        {
            GuiHint("Potatoes are now farmable. Go find them!");
        }
        potatoEnabled = b;
    }

    public bool PotatoSpeed()
    {
        return potatoSpeed;
    }

    public void SetPotatoSpeed(bool b)
    {
        if (b == true)
        {
            GuiHint("Potato growth rate doubled.");
        }
        potatoSpeed = b;
    }

    public bool PotatoBountiful()
    {
        return potatoBountiful;
    }

    public void SetPotatoBountiful(bool b)
    {
        if (b == true)
        {
            GuiHint("Potato harvests are bountiful. Doubled your harvests.");
        }
        potatoBountiful = b;
    }

    public bool PotatoGun()
    {
        return potatoGun;
    }

    public void SetPotatoGun(bool b)
    {
        if (b == true)
        {
            GuiHint("RPotatoG is available to craft. See your crafting bench.");
        }
        potatoGun = b;
    }

    public bool PotatoGun2()
    {
        return potatoGun2;
    }

    public void SetPotatoGun2(bool b)
    {
        if (b == true)
        {
            GuiHint("RPotatoG II is available to craft. See your crafting bench.");
        }
        potatoGun2 = b;
    }

    public bool CornEnabled()
    {
        return cornEnabled;
    }

    public void SetCornEnabled(bool b)
    {
        if (b == true)
        {
            GuiHint("Corn is now farmable. Go find them!");
        }
        cornEnabled = b;
    }

    public bool CornSpeed()
    {
        return cornSpeed;
    }

    public void SetCornSpeed(bool b)
    {
        if (b == true)
        {
            GuiHint("Corn growth rate doubled.");
        }
        cornSpeed = b;
    }

    public bool CornBountiful()
    {
        return cornBountiful;
    }

    public void SetCornBountiful(bool b)
    {
        if (b == true)
        {
            GuiHint("Corn harvests are bountiful. Doubled your harvests.");
        }
        cornBountiful = b;
    }

    public bool CornGun()
    {
        return cornGun;
    }

    public void SetCornGun(bool b)
    {
        if (b == true)
        {
            GuiHint("Corn Shooter is available to craft. See your crafting bench.");
        }
        cornGun = b;
    }

    public bool CornGun2()
    {
        return cornGun2;
    }

    public void SetCornGun2(bool b)
    {
        if (b == true)
        {
            GuiHint("Corn Shooter II is available to craft. See your crafting bench.");
        }
        cornGun2 = b;
    }
}
