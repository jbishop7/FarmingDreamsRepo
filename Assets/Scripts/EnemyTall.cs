using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTall : MonoBehaviour
{
    public float speed = 2f;

    public Rigidbody2D rb;
    public Animator anim;
    private Vector2 movement;

    public float maxHealth = 15f;
    private float health;

    public float attackDamage = 1f;

    public float range = 1.75f;

    private Player player;

    private bool attacking = false;
    void Start()
    {
        player = Player.Instance;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.R))    // some other conditional for attacking
        {
            StartAttack();
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        if (!attacking)
        {
            anim.SetFloat("speed", movement.sqrMagnitude);
            anim.SetFloat("horizontal", movement.x);
            anim.SetFloat("vertical", movement.y);
        }


    }

    private void StartAttack()
    {
        attacking = true;
        anim.SetTrigger("attack");
    }

    public void ShootAttack()
    {
        attacking = false;
        Debug.Log("I would shoot rn");
    }
}
