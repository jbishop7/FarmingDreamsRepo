using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TechMenu : MonoBehaviour
{
    public TextMeshProUGUI techMenuText;
    public GameObject techMenuPanel;

    private bool showingMenu = false;

    GameController gc;

    public static TechMenu _instance;

    public static TechMenu Instance
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
    // Start is called before the first frame update
    void Start()
    {
        gc = GameController.Instance;
        techMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showingMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            showingMenu = false;
            techMenuPanel.SetActive(false);
        }
    }

    public void ShowCurrentTech()
    {
        showingMenu = true;
        techMenuPanel.SetActive(true);
        string tech = "";

        if (gc.BerryEnabled())
        {
            tech += "Berry Farming\n";
        }

        if (gc.BerrySpeed())
        {
            tech += "Berry Double Time\n";
        }

        if (gc.BerryBountiful())
        {
            tech += "Berry Bountiful Harvest\n";
        }

        if (gc.BerrySword())
        {
            tech += "Berry Thorned Sword\n";
        }

        if (gc.BerrySword2())
        {
            tech += "Berry Thorned Sword II\n";
        }

        if (gc.PotatoEnabled())
        {
            tech += "Potato Farming\n";
        }

        if (gc.PotatoSpeed())
        {
            tech += "Potato Double Time\n";
        }

        if (gc.PotatoBountiful())
        {
            tech += "Potato Bountiful Harvest\n";
        }

        if (gc.PotatoGun())
        {
            tech += "Potato RPotatoG\n";
        }

        if (gc.PotatoGun2())
        {
            tech += "Potato RPotatoG II\n";
        }

        if (gc.CornEnabled())
        {
            tech += "Corn Farming\n";
        }

        if (gc.CornSpeed())
        {
            tech += "Corn Double Time\n";
        }

        if (gc.CornBountiful())
        {
            tech += "Corn Bountiful Harvest\n";
        }

        if (gc.CornGun())
        {
            tech += "Corn Shooter\n";
        }

        if (gc.CornGun2())
        {
            tech += "Corn Shooter II\n";
        }

        if (tech == "")
        {
            tech = "No Upgrades.\nPress U to start using Tech Points.";
        }


        techMenuText.SetText(tech);
    }
}
