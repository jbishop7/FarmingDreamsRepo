using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairableStructure : MonoBehaviour
{
    public GameObject structure;
    public string[] itemsRequired;
    public int[] itemQuantities;
    public bool doesSpawn = true;

    private float textDuration = 3f;
    private float timer = 3f;
    private bool textDisplayed = false;

    private TextMeshProUGUI text;
    void Start()
    {
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();

        foreach(TextMeshProUGUI ui in texts)
        {
            switch (ui.name)
            {
                case "InventoryAdditions":
                    text = ui;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplayed)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                text.SetText("");
                textDisplayed = false;
                timer = textDuration;
            }
        }
    }

    public void Repair()
    {
        GameController gc = GameController.Instance;
        if (gameObject.name.Contains("restricted") && gc.GetDayCount() < 2)
        {
            textDisplayed = true;
            text.SetText("It's too early to repair this.");
            return;
        }

        if (!gc.UseInventoryItems(itemsRequired, itemQuantities))
        {
            textDisplayed = true;
            text.SetText("Not enough resources for repair.");
            return;
        }

        Vector2 newPos = transform.position;
        if (gameObject.name.Contains("Farm"))
        {
            newPos = new Vector2(transform.position.x + 0.25f, transform.position.y - 0.2f);
        }
        if (doesSpawn)
        {
            Instantiate(structure, newPos, this.transform.rotation);
        }
        
        Destroy(gameObject);
    }

    public string GetRequiredMaterials()
    {
        string materials = "";
        for (int i = 0; i < itemsRequired.Length; i++)
        {
            materials += $"{itemQuantities[i]} {itemsRequired[i]},";
        }
        materials = materials.Remove(materials.Length - 1);
        return materials;
    }
}
