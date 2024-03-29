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

    public TextMeshProUGUI inventoryAdditions;
    private readonly float invAdditionTimer = 3f;
    private float timer = 3f;
    private bool startTimer = false;

    public GameObject journalPanel;
    private bool showingJournal = false;

    public GameObject craftingPanel;
    private bool showingCrafting = false;

    public GameObject inventoryPanel;
    public TextMeshProUGUI toolsText;
    public TextMeshProUGUI inventoryText;
    private bool showingInventory = false;

    private string currentScene = "farm"; // otherwise could be "dungeon"
    private bool playerFellAsleep = false;

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
        inventoryPanel = null;
        journalPanel = null;
        craftingPanel = null;
        
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        Panel[] panels = FindObjectsOfType<Panel>();


        foreach(TextMeshProUGUI ui in texts)
        {
            switch (ui.name)
            {
                case "ToolsText":
                    toolsText = ui; break;
                case "InventoryText":
                    inventoryText = ui; break;
                case "InventoryAdditions":
                    inventoryAdditions = ui;  break;
                
            }
            Debug.Log(ui.name);
        }

        foreach(Panel go in panels)
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
        // might do something different tbh, make each component talk to the GC and add itself...
    }

    // Update is called once per frame
    void Update()
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
                Debug.Log("HELLO from sleeping");
                EndDay();
            }

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
        if (level > 1)
        {
            return;
        }
        InitializeGameController();
    }

    public void EndDemo()
    {
        SceneManager.LoadScene(2);
    }

    



}
