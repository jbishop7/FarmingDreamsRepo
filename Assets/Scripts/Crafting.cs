using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    private GameController gc;
    public TMP_Dropdown toolsDropdown;
    public TMP_Dropdown itemsDropdown;

    public Button buildToolButton;
    public Button craftItemButton;

    public GameObject craftingPanel;
    private bool showCrafting = false;

    public TextMeshProUGUI toolRequirementsText;
    public TextMeshProUGUI itemRequirementsText;

    public static Crafting _instance;

    // all craftable tools
    private List<string> craftableTools = new();
    private Dictionary<string, int> bambooSwordCosts = new();
    private Dictionary<string, int> bambooSwordIICosts = new();
    private Dictionary<string, int> thornedSwordCosts = new();
    private Dictionary<string, int> thornedSwordIICosts = new() ;
    private Dictionary<string, int> cornShooterCosts = new()    ;
    private Dictionary<string, int> cornShooterIICosts = new();
    private Dictionary<string, int> RPotatoGCosts = new();
    private Dictionary<string, int> RPotatoGIICosts = new();

    // all craftable items
    private List<string> craftableItems = new();
    private Dictionary<string, int> berryAidCosts = new();
    private Dictionary<string, int> speedSlurpCosts = new();
    private Dictionary<string, int> resistCosts = new();

    private string toolToBuild = "";
    private Dictionary<string, int> toolToBuildCosts = new();

    private string itemToCraft = "";
    private Dictionary<string, int> itemToCraftCosts = new();

    public static Crafting Instance
    {
        get
        {
            if ( _instance == null)
            {
                Debug.LogError("No Crafting controller");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        toolsDropdown.onValueChanged.AddListener(delegate
        {
            UpdateToolOffer(toolsDropdown.value);
        });

        buildToolButton.onClick.AddListener(delegate
        {
            BuildTool();
        });

        itemsDropdown.onValueChanged.AddListener(delegate
        {
            UpdateItemOffer(itemsDropdown.value);
        });

        craftItemButton.onClick.AddListener(delegate
        {
            CraftItem();
        });

        craftingPanel.SetActive(false);
        gc = GameController.Instance;
        
    }

    public void CreateTools()
    {
        craftableTools = new();
        bambooSwordCosts = new();
        bambooSwordIICosts = new();
        thornedSwordCosts = new();
        thornedSwordIICosts = new();
        cornShooterCosts = new();
        cornShooterIICosts = new();
        RPotatoGCosts = new();
        RPotatoGIICosts = new();
        // bamboo sword cost
        bambooSwordCosts.Add("bamboo", 15);
        craftableTools.Add("bamboo_sword");
        // bamboo sword II 
        bambooSwordIICosts.Add("bamboo", 30);
        bambooSwordIICosts.Add("ingot", 1);
        bambooSwordIICosts.Add("bamboo_sword", 1);
        craftableTools.Add("bamboo_sword_II");
        
        if (gc.BerrySword() == true)
        {
            // thorned sword
            thornedSwordCosts.Add("wood", 10);
            thornedSwordCosts.Add("blackberry", 10);
            thornedSwordCosts.Add("thorn", 5);
            thornedSwordCosts.Add("ingot", 1);
            craftableTools.Add("thorned_sword");
        }
       
        if (gc.BerrySword2() == true)
        {
            // thorned sword II
            thornedSwordIICosts.Add("wood", 5);
            thornedSwordIICosts.Add("blackberry", 10);
            thornedSwordIICosts.Add("thorn", 20);
            thornedSwordIICosts.Add("dream_ingot", 4);
            thornedSwordIICosts.Add("thorned_sword", 1);
            craftableTools.Add("thorned_sword_II");
        }
       
       if (gc.CornGun() == true)
        {
            // corn shooter
            cornShooterCosts.Add("wood", 5);
            cornShooterCosts.Add("corn", 30);
            cornShooterCosts.Add("corn_husk", 5);
            cornShooterCosts.Add("ingot", 1);
            craftableTools.Add("corn_shooter");
        }

       if (gc.CornGun2() == true)
        {
            // corn shooter II
            cornShooterIICosts.Add("wood", 5);
            cornShooterIICosts.Add("corn", 50);
            cornShooterIICosts.Add("corn_husk", 20);
            cornShooterIICosts.Add("dream_ingot", 4);
            cornShooterIICosts.Add("corn_shooter", 1);
            craftableTools.Add("corn_shooter_II");
        }
        
       if (gc.PotatoGun() == true)
        {
            // RPotatoG
            RPotatoGCosts.Add("wood", 5);
            RPotatoGCosts.Add("potato_flower", 10);
            RPotatoGCosts.Add("potato", 15);
            RPotatoGCosts.Add("ingot", 1);
            craftableTools.Add("RPotatoG");
        }
        
        if (gc.PotatoGun2() == true)
        {
            //RPotatoG_II
            RPotatoGIICosts.Add("wood", 5);
            RPotatoGIICosts.Add("potato_flower", 20);
            RPotatoGIICosts.Add("potato", 25);
            RPotatoGIICosts.Add("dream_ingot", 4);
            RPotatoGIICosts.Add("RPotatoG", 1);
            craftableTools.Add("RPotatoG_II");
        }
        // ALL WEAPONS ARE DONE NOW.


    }

    public void CreateItems()
    {
        craftableItems = new();
        berryAidCosts = new();
        speedSlurpCosts = new();
        resistCosts = new();

        craftableItems.Add("berry_aid");
        berryAidCosts.Add("blackberry", 10);
        berryAidCosts.Add("bamboo", 1);

        craftableItems.Add("speed_slurp");
        speedSlurpCosts.Add("corn_husk", 7);
        speedSlurpCosts.Add("bamboo", 1);

        craftableItems.Add("resist");
        resistCosts.Add("potato_flower", 10);
        resistCosts.Add("bamboo", 1);

    }

    public void ShowCrafting()
    {
        craftingPanel.SetActive(true);
        showCrafting = true;
        CreateTools();
        CreateItems();
        itemsDropdown.ClearOptions();
        toolsDropdown.ClearOptions();

        toolToBuild = "";
        toolToBuildCosts = new();

        itemToCraft = "";
        itemToCraftCosts = new();

        
        toolsDropdown.AddOptions(craftableTools);
        UpdateToolOffer(0);
        itemsDropdown.AddOptions(craftableItems);
        UpdateItemOffer(0);
    }

    private void UpdateToolOffer(int change)
    {
        Dictionary<string, int> required = new();
        int i = 0;
        foreach(string key in craftableTools)
        {
            if (i == change)
            {
                switch (key)
                {
                    case "bamboo_sword":
                        required = bambooSwordCosts;
                        break;
                    case "bamboo_sword_II":
                        required = bambooSwordIICosts;
                        break;
                    case "thorned_sword":
                        required = thornedSwordCosts;
                        break;
                    case "thorned_sword_II":
                        required = thornedSwordIICosts;
                        break;
                    case "corn_shooter":
                        required = cornShooterCosts;
                        break;
                    case "corn_shooter_II":
                        required = cornShooterIICosts;
                        break;
                    case "RPotatoG":
                        required = RPotatoGCosts;
                        break;
                    case "RPotatoG_II":
                        required = RPotatoGIICosts;
                        break;
                }
                toolToBuild = key;
                toolToBuildCosts = required;
            }
            i++;
        }
        string reqs = "Requires:\n";
        

        foreach(var(key, val) in required)
        {
            reqs += $"{val} {key}\n";
        }
      
        toolRequirementsText.SetText(reqs);

    }

    private void BuildTool()
    {

        // check if we already have this tool
        Dictionary<string, int> playerTools = gc.GetPlayerTools();

        bool hasTool = false;
        foreach(var(key, val) in playerTools)
        {
            if (toolToBuild == key)
            { 
                hasTool = true;
            }
        }

        if (hasTool)
        {
            gc.GuiHint("You already have this tool.");
            return;
        }

        // gotta check that we have all the materials. 
        List<string> items = new();
        List<int> quantities = new();
        foreach(var (key, val) in toolToBuildCosts)
        {
            items.Add(key);
            quantities.Add(val);
        }

        if (gc.UseInventoryToolsAndItems(items.ToArray(), quantities.ToArray()))
        {
            switch(toolToBuild)
            {
                case "bamboo_sword":
                    gc.CraftBambooSword();
                    break;
                case "bamboo_sword_II":
                    gc.CraftBambooSwordII();
                    break;
                case "thorned_sword":
                    gc.CraftThornedSword();
                    break;
                case "thorned_sword_II":
                    gc.CraftThornedSwordII();
                    break;
                case "corn_shooter":
                    gc.CraftCornShooter();
                    break;
                case "corn_shooter_II":
                    gc.CraftCornShooterII();
                    break;
                case "RPotatoG":
                    gc.CraftPotatoGun();
                    break;
                case "RPotatoG_II":
                    gc.CraftPotatoGunII();
                    break;
            }
        }
        else
        {
            gc.GuiHint("Not enough materials.");
        }
    }
    
    private void CraftItem()
    {
        // gotta check that we have all the materials. 
        List<string> items = new();
        List<int> quantities = new();
        foreach (var (key, val) in itemToCraftCosts)
        {
            items.Add(key);
            quantities.Add(val);
        }

        if (gc.UseInventoryToolsAndItems(items.ToArray(), quantities.ToArray()))
        {
            switch (itemToCraft)
            {
                case "berry_aid":
                    gc.CraftBerryAid();
                    break;
                case "speed_slurp":
                    gc.CraftSpeedSlurp();
                    break;
                case "resist":
                    gc.CraftResist();
                    break;
            }
        }
        else
        {
            gc.GuiHint("Not enough materials.");
        }
    }
    private void UpdateItemOffer(int change)
    {
        Dictionary<string, int> required = new();
        int i = 0;
        foreach (string key in craftableItems)
        {
            if (i == change)
            {
                switch (key)
                {
                    case "berry_aid":
                        required = berryAidCosts;
                        break;
                    case "speed_slurp":
                        required = speedSlurpCosts;
                        break;
                    case "resist":
                        required = resistCosts;
                        break;
                }
                itemToCraft = key;
                itemToCraftCosts = required;
            }
            i++;
        }
        string reqs = "Requires:\n";


        foreach (var (key, val) in required)
        {
            reqs += $"{val} {key}\n";
        }

        itemRequirementsText.SetText(reqs);
    }
    void Update()
    {
        if (showCrafting && Input.GetKeyDown(KeyCode.Escape))
        {
            showCrafting = false;
            craftingPanel.SetActive(false);
        }
    }

    public void CraftBambooSword()
    {
        gc.CraftBambooSword();
    }
}
