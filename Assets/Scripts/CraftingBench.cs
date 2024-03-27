using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{

    public GameObject craftingPanel;

    private bool isCrafting = false;


    void Start()
    {
        craftingPanel.SetActive(false);
    }

    
    void Update()
    {
        if (isCrafting && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E)))
        {
            isCrafting = false;
            Time.timeScale = 1f;
        }
    }

    public void ShowCrafting()
    {
        craftingPanel.SetActive(true);
        isCrafting = true;
        Time.timeScale = 0f;
    }

    
}
