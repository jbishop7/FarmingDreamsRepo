using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairableStructure : MonoBehaviour
{
    public GameObject structure;
    public string itemRequired;
    public int itemQuantity;
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

        if (!gc.UseInventoryItems(itemRequired, itemQuantity))
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
        return $"{itemQuantity} {itemRequired}";
    }
}
