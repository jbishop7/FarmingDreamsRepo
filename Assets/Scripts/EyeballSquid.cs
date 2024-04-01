using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballSquid : MonoBehaviour
{
    public Transform playerTransform;
    public Animator _Animator;
    public Rigidbody2D _Rigidbody;
    public float moveSpeed = 5.0f;
    private bool shouldPhase = false;
    private bool attackPlayer = false;
    public float attackRange = 2.5f;

    [SerializeField] float health, maxHealth = 5f;
    [SerializeField] FloatingHealthbar healthbar;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        healthbar = GetComponentInChildren<FloatingHealthbar>();

        health = maxHealth;
        healthbar.updateHealthbar(health, maxHealth);
    }

    private void Update()
    {
        if (shouldPhase)
        {
            Phase();
        }

        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            if (!attackPlayer)
            {
                Debug.Log("Player entered attack range");
                attackPlayer = true;
                _Animator.SetTrigger("attack");
            }
        }
        else
        {
            if (attackPlayer)
            {
                Debug.Log("Player exited attack range");
                attackPlayer = false;
                _Animator.ResetTrigger("attack");
            }
        }
    }

    private void Phase()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        _Animator.SetFloat("speed", moveSpeed);
        _Animator.SetFloat("horizontal", directionToPlayer.x);
        _Animator.SetFloat("vertical", directionToPlayer.y);
        _Rigidbody.MovePosition(_Rigidbody.position + new Vector2(directionToPlayer.x, directionToPlayer.y) * (moveSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected for phasing/movement");
            shouldPhase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player out of phasing/movement range");
            shouldPhase = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthbar.updateHealthbar(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

