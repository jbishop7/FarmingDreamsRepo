using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{

    private static Merchant _instance;

    private Dictionary<string, int> buying = new();
    private Dictionary<string, int> selling = new();

    public GameObject tradePanel;
    private bool showTradePanel = false;

    public TMP_Dropdown sellDropdown;
    public TMP_Dropdown buyDropdown;
    public TextMeshProUGUI sellText;
    public TextMeshProUGUI buyText;

    public Button sellButton;
    public Button buyButton;

    private string itemToSell = "";
    private int sellPrice = 0;
    private string itemToBuy = "";
    private int buyPrice = 0;

    public static Merchant Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No merchant! Nooooo");
            }
            return _instance;
        }
    }

    void Start()
    {
        tradePanel.SetActive(false);
        buying.Add("wood", 4);
        buying.Add("bamboo", 2);
        buying.Add("corn", 1);
        buying.Add("potato", 5);
        buying.Add("blackberry", 6);

        selling.Add("wood", 8);
        selling.Add("axe_upgrade", 200);
        selling.Add("berry_aid", 80);
        selling.Add("ingot", 120);

        sellDropdown.onValueChanged.AddListener(delegate
        {
            UpdateSellOffer(sellDropdown.value);
        });

        sellButton.onClick.AddListener(delegate
        {
            SellItem();
        });

        buyButton.onClick.AddListener(delegate
        {
            BuyItem();
        });

        buyDropdown.onValueChanged.AddListener(delegate
        {
            UpdateBuyOffer(buyDropdown.value);
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (showTradePanel && Input.GetKeyDown(KeyCode.Escape)){
            showTradePanel = false;
            sellText.SetText("");
            sellPrice = 0;
            buyText.SetText("");
            buyPrice = 0;
            tradePanel.SetActive(false);
        }
    }

    public void ShowTrades()
    {
        Debug.Log("Okay");
        tradePanel.SetActive(true);
        sellDropdown.ClearOptions();
        buyDropdown.ClearOptions();
        List<string> buyList = new();
        foreach(var (key, val) in buying)
        {
            buyList.Add(key);
        }
        sellDropdown.AddOptions(buyList);

        List<string> sellList = new();
        foreach(var (key, val) in selling)
        {
            sellList.Add(key);
        }
        buyDropdown.AddOptions(sellList);

        itemToSell = "";
        sellPrice = 0;
        itemToBuy = "";
        buyPrice = 0;

        UpdateSellOffer(0);
        UpdateBuyOffer(0);

        showTradePanel = true;
        Debug.Log("Should be showing trades now...");
    }

    private void UpdateSellOffer(int change)
    {
        int i = 0;
        foreach(var (key, val) in buying)
        {
            if (change == i)
            {
                UpdateSellText(key, val);
                itemToSell = key;
                sellPrice = val;
            }
            i++;
        }
    }

    private void UpdateSellText(string item, int money)
    {
        GameController gc = GameController.Instance;
        int itemsAvailable = gc.GetInventoryItemCount(item);
        string itemsLeft = $"You have {item} x{itemsAvailable}";

        if (itemsAvailable <= 0)
        {
            itemsLeft = $"You don't have any {item}";
        }
        
        sellText.SetText($"1 {item} sells for {money} gold.\n{itemsLeft}.");
    }

    private void SellItem()
    {
        GameController gc = GameController.Instance;
        gc.SellItem(itemToSell, sellPrice);
        UpdateSellText(itemToSell, sellPrice);
        UpdateBuyText(itemToBuy, buyPrice);
    }

    private void UpdateBuyOffer(int change)
    {
        int i = 0;
        foreach (var (key, val) in selling)
        {
            if (change == i)
            {
                UpdateBuyText(key, val);
                itemToBuy = key;
                buyPrice = val;
            }
            i++;
        }
    }

    private void UpdateBuyText(string item, int money)
    {
        GameController gc = GameController.Instance;
        int itemsAvailable = gc.GetInventoryItemCount(item);
        string itemsLeft = $"You have {item} x{itemsAvailable}";

        if (itemsAvailable <= 0)
        {
            itemsLeft = $"You don't have any {item}";
        }

        buyText.SetText($"1 {item} is bought for {money} gold.\n{itemsLeft}.");
    }

    private void BuyItem()
    {
        GameController gc = GameController.Instance;
        gc.BuyItem(itemToBuy, buyPrice);
        UpdateSellText(itemToSell, sellPrice);
        UpdateBuyText(itemToBuy, buyPrice);
    }

    public void AddToSellCatalogue(string item, int payment)
    {

    }

    public void AddToBuyCatalogue(string item, int cost)
    {

    }
}
