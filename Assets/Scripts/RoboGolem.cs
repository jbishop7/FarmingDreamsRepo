using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class RoboGolem : MonoBehaviour
{
    public Transform playerTransform;
    public Tilemap walkableTilemap;
    public float moveSpeed = 5.0f;
    public Animator _Animator;
    public Rigidbody2D _Rigidbody;

    [SerializeField] float health, maxHealth = 5f;
    [SerializeField] FloatingHealthbar healthbar;

    void Start()
    {
        GameObject groundGameObject = GameObject.Find("Ground");
        if (groundGameObject != null)
        {
            walkableTilemap = groundGameObject.GetComponent<Tilemap>();
        }

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

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3Int playerPosition = walkableTilemap.WorldToCell(playerTransform.position);
        Vector3Int currentPosition = walkableTilemap.WorldToCell(transform.position);

        Vector3Int nextStep = FindNextStepTowardsPlayer(currentPosition, playerPosition);

        Vector3 targetPosition = walkableTilemap.CellToWorld(nextStep) + new Vector3(0.5f, 0.5f, 0);
        Vector3 movementDirection = targetPosition - transform.position;

        if (movementDirection != Vector3.zero)
        {
            _Animator.SetFloat("speed", movementDirection.sqrMagnitude);
            _Animator.SetFloat("horizontal", movementDirection.x);
            _Animator.SetFloat("vertical", movementDirection.y);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    Vector3Int FindNextStepTowardsPlayer(Vector3Int current, Vector3Int target)
    {
        Vector3Int bestStep = current;
        float bestDistance = float.MaxValue;

        foreach (Vector3Int direction in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
        {
            Vector3Int nextStep = current + direction;
            if (walkableTilemap.HasTile(nextStep))
            {
                float distance = (walkableTilemap.CellToWorld(target) - walkableTilemap.CellToWorld(nextStep)).sqrMagnitude;
                if (distance < bestDistance)
                {
                    bestStep = nextStep;
                    bestDistance = distance;
                }
            }
        }

        return bestStep;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthbar.updateHealthbar(health, maxHealth);

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
