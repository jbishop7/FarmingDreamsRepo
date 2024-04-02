using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement;
    private float speed = 5f;
    [SerializeField] float health, maxHealth = 10f;
    [SerializeField] FloatingHealthbar healthbar;

    private SpriteRenderer playerSprite;

    private Vector2 mousePosition = new Vector3(0, 0);
    private Vector2 toolToMouseVector = new Vector3(0, 0, 0);

    public GameObject tool; // this is the tool parent, holds all tools. 
    private GameObject AttackArea;
    private Animator toolAnimator;

    private Tool tool1 = null;
    private Tool tool2 = null;
    private Tool currentTool = null;

    // private float toolUseTimer = 0.5f;
    private bool usingTool = false;
    private bool attacking = false;
    private bool isInvulnerable = false;

    private CraftingBench craftingBench = null;

    private Tent tent = null;

    private FarmingPlot currentFarmingPlot = null;
    private string farmingState = "planting"; // or can be "harvesting"

    private RepairableStructure currentRepair = null;

    private Merchant merchant = null;

    private GameController gameController = null;

    public TextMeshProUGUI playerHints;

    private static Player _instance;

    public static Player Instance
    {
        get
        {
            if ( _instance == null)
            {
                Debug.LogError("No player instance.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        playerHints.SetText("");

        // Get the tools in use here...
        GameController gc = GameController.Instance;
        string[] toolsInUse = gc.GetToolsInUse();
        // there is just going to be 2 tools in use. 
        // so we need to grab them, run them through a switch, and identify which tools to use. 
        string firstTool = toolsInUse[0];
        string secondTool = toolsInUse[1];
        
        Tool[] tools = tool.GetComponentsInChildren<Tool>();

        foreach (var item in tools) // first disable ALL tools.
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in tools) // set the first tool
        {
            if (item.gameObject.name == firstTool)
            {
                tool1 = item;
                toolAnimator = tool1.gameObject.GetComponent<Animator>();
                currentTool = item;
            }

            if (item.gameObject.name == secondTool){
                tool2 = item;
            }
        }
        tool2.gameObject.SetActive(false);

        playerSprite = GetComponent<SpriteRenderer>();
        tool1.gameObject.SetActive(true);
    }

    void Start()
    {
        AttackArea = GameObject.Find("AttackArea");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        AttackArea.SetActive(attacking);

        health = maxHealth;
        healthbar.updateHealthbar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        RotateAttackArea();

        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (currentFarmingPlot != null && Input.GetKeyDown(KeyCode.E))
        {
            if (farmingState == "planting")
            {
                currentFarmingPlot.PlantCrop();
            }
            else if (farmingState == "harvesting")
            {
                currentFarmingPlot.HarvestCrop();
            }
            currentFarmingPlot = null;
            SetHint("");
        }

        if (currentRepair != null && Input.GetKeyDown(KeyCode.E))
        {
            currentRepair.Repair();
            currentRepair = null;
            SetHint("");
        }

        if (craftingBench != null && Input.GetKeyDown(KeyCode.E))
        {
            Crafting c = Crafting.Instance;
            c.ShowCrafting();
            SetHint("");
        }

        if (tent != null && Input.GetKeyDown(KeyCode.E))
        {
            SetHint("");
            Preparation p = Preparation.Instance;
            p.ShowPreparations();
            /*GameController gc = GameController.Instance;
            gc.EndDay();*/
        }

        if (merchant != null && Input.GetKeyDown(KeyCode.E))
        {
            merchant.ShowTrades();
            SetHint("");
        }

        RotateTool();

        if (Input.GetMouseButtonDown(0))
        {
            UseTool();
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolAnimator = tool1.gameObject.GetComponent<Animator>();
            tool2.gameObject.SetActive(false);
            tool1.gameObject.SetActive(true);
            currentTool = tool1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolAnimator = tool2.gameObject.GetComponent<Animator>();
            tool1.gameObject.SetActive(false);
            tool2.gameObject.SetActive(true);
            currentTool = tool2;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Farm"))
        {
            FarmingPlot farm = collision.gameObject.GetComponent<FarmingPlot>();
            if (farm.GetPlantedState() == false)
            {
                currentFarmingPlot = farm;
                farmingState = "planting";
                SetHint("Press E to plant seeds");
            }
            else if (farm.CanHarvest() == true)
            {
                currentFarmingPlot = farm;
                farmingState = "harvesting";
                SetHint("Press E to harvest crop");
            }
            else
            {
                currentFarmingPlot = null;
            }
        }

        if (collision.gameObject.CompareTag("Repairable"))
        {
            RepairableStructure structure = collision.gameObject.GetComponent<RepairableStructure>();
            currentRepair = structure;
            SetHint($"Press E to use {structure.GetRequiredMaterials()} to repair");
        }

        if (collision.gameObject.CompareTag("Craft"))
        {
            SetHint("Press E to open Crafting");
            CraftingBench bench = collision.gameObject.GetComponent<CraftingBench>();
            craftingBench = bench;
        }

        if (collision.gameObject.CompareTag("Tent"))
        {
            Tent t = collision.gameObject.GetComponent<Tent>();
            tent = t;
            // This will need to change
            GameController gc = GameController.Instance;
            if (gc.CheckToolInventory("bamboo_sword"))
            {
                SetHint("Press E to sleep and travel to the World of Nightmares");
            }
            else
            {
                SetHint("You need a weapon before you can go to the World of Nightmares!");
            }
        }

        if(collision.gameObject.name == "Merchant")
        {
            SetHint("Press E to trade with Merchant Maurice");
            merchant = collision.gameObject.GetComponent<Merchant>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Farm"))
        {
            currentFarmingPlot = null;
            SetHint("");
        }

        if (collision.gameObject.CompareTag("Repairable"))
        {
            currentRepair = null;
            SetHint("");
        }

        if (collision.gameObject.CompareTag("Craft"))
        {
            craftingBench = null;
            SetHint("");
        }

        if (collision.gameObject.CompareTag("Tent"))
        {
            tent = null;
            SetHint("");
        }

        if (collision.gameObject.name == "Merchant")
        {
            SetHint("");
            merchant = null;
        }
    }

    public void SetHint(string hint)
    {
        playerHints.SetText(hint);
    }

    private void RotateTool()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rigidbody2D trb = tool.GetComponent<Rigidbody2D>();
        trb.position = new Vector2(transform.position.x, transform.position.y + 0.38f);
        

        toolToMouseVector = mousePosition - trb.position;

        float angle = Mathf.Atan2(toolToMouseVector.y, toolToMouseVector.x) * Mathf.Rad2Deg;

        if (angle < 90 && angle > -90)
        {
            tool.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            tool.transform.localScale = new Vector3(1, -1, 1);
        }

        trb.rotation = angle;

    }

    private void Attack()
    {
        if (!attacking)
        {
            StartCoroutine(PerformAttack()); 
        }
    }

    private IEnumerator PerformAttack()
    {
        attacking = true;
        AttackArea.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        attacking = false;
        AttackArea.SetActive(false);
    }

    private void RotateAttackArea()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackAreaToMouseVector = mousePosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(attackAreaToMouseVector.y, attackAreaToMouseVector.x) * Mathf.Rad2Deg;
        AttackArea.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("I took damage: " + damage);
        healthbar.updateHealthbar(health, maxHealth);

        if(!isInvulnerable)
        {
            if (health <= 0)
            {
                gameController.DungeonFail();
                //Load farm scene?
            }
            else
            {
                StartCoroutine(BecomeInvulnerable());
            }
        }
        
    }

    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        float invulnerabilityDuration = 0.5f;
        for (float i = 0; i < invulnerabilityDuration; i += 0.1f)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        playerSprite.enabled = true;
        isInvulnerable = false;
    }

    private void UseTool()
    {
        toolAnimator.SetTrigger("use");
        usingTool = true;
    }

    public void SetUsingTool(bool t)
    {
        usingTool = t;
    }

    public bool IsUsingTool()
    {
        return usingTool;
    }

    public Vector2 GetPosition()
    {
        return rb.position;
    }

    public Tool CurrentTool()
    {
        return currentTool;
    }
}
