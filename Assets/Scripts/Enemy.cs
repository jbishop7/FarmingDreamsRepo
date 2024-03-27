using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    public Rigidbody2D rb;

    private Vector2 movement;

    private float wanderDecisionTimer = 3.0f;
    private float wanderTime = 1.5f;
    private string currentState = "wander"; // or attack or seek or run or distance 
    private Vector2[] wanderMovements = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
    private Vector2 wanderSelectedMovement = new Vector2(0, 0);

    Player player;

    public float maxHealth = 15f;
    private float health;

    public float attackDamage = 1f;

    public float range = 1.75f;

    private void Awake()
    {
        

    }
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        


        if (currentState == "wander" && wanderTime > 0)
        {
            wanderTime -= Time.deltaTime;
            if (wanderSelectedMovement != new Vector2(0, 0))
            {
                movement.x = wanderSelectedMovement.x;
                movement.y = wanderSelectedMovement.y;

            }
            Wander();
        }
        else
        {
            movement = new Vector2(0, 0);
            wanderSelectedMovement = movement;
            wanderDecisionTimer -= Time.deltaTime;
        }

        if (wanderDecisionTimer < 0)
        {
            wanderTime = Random.Range(1f, 5f);
            wanderDecisionTimer = Random.Range(1f, 3f);
        }

        if (((player.GetPosition() - rb.position).magnitude) < 10)
        {
            Attack();
            Vector2 lineOfSight = (player.GetPosition() - rb.position);
            
        }
        else
        {
            Wander();
        }



       

        

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Wander()
    {
        if (wanderSelectedMovement == new Vector2(0, 0))
        {
            currentState = "wander";
            // then we can set it to be a new movement vector
            Vector2 newMovement = wanderMovements[Mathf.RoundToInt(Random.Range(0, wanderMovements.Length))];
            wanderSelectedMovement = newMovement;

        }
    }

    void Attack()
    {
        movement = (player.GetPosition() - rb.position).normalized; // move toward the player
        currentState = "attack";
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameController gc = GameController.Instance;
        gc.EndDemo();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name.Contains("axe") || collision.gameObject.name.Contains("sword"))
        {
            Tool t = player.CurrentTool();
            TakeDamage(t.damage);
            Debug.Log($"{ t.damage } { t.name}");
        }
    }

}
