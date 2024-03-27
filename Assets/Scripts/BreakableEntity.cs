using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableEntity : MonoBehaviour
{
    public float hitPoints;
    private bool destroyed = false;
    public float exitTime = 0.5f;
    public Animator animator;
    public string drop;

    private bool alreadyHit = false;

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            hitPoints--;
        }

        if (hitPoints < 0 && !destroyed)
        {
            destroyed = true;
            animator.SetBool("falling", true);
            Debug.Log("Falling");
        }
        if (destroyed)
        {
            exitTime -= Time.deltaTime;
        }
        if (exitTime <= 0)
        {
            animator.SetBool("falling", false);
            animator.SetBool("destroyed", true);
            DestroyEntity();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = Player.Instance;
        
        if (collision.gameObject.name.Contains("axe") && !alreadyHit && p.IsUsingTool())
        {
            alreadyHit = true; // this kinda makes it less wonk
            AudioController ac = AudioController.Instance;
            ac.ChopWood();
            // Debug.Log("Hit with an axe");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.name.Contains("axe") && alreadyHit)
        {
            Tool tool = collision.gameObject.GetComponent<Tool>();
            alreadyHit = false;
            hitPoints -= tool.GetDamage();
        }
    }

    public void DestroyEntity()
    {
        AudioController ac = AudioController.Instance;
        ac.TreeFalling();
        GameController gc = GameController.Instance;
        gc.AddToInventory(drop, 5);
        Destroy(gameObject);
    }
}
