using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTransform;
    public Tilemap walkableTilemap;
    public float moveSpeed = 5.0f; // Increased speed

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
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3Int playerPosition = walkableTilemap.WorldToCell(playerTransform.position);
        Vector3Int currentPosition = walkableTilemap.WorldToCell(transform.position);
        Vector3Int currentTarget = GetNextTargetTowardsPlayer(currentPosition, playerPosition);

        // Move towards the current target with increased speed and more frequent updates
        transform.position = Vector3.MoveTowards(transform.position, walkableTilemap.CellToWorld(currentTarget), moveSpeed * Time.deltaTime);
    }

    Vector3Int GetNextTargetTowardsPlayer(Vector3Int current, Vector3Int target)
    {
        Vector3Int direction = target - current;
        direction.x = (int)Mathf.Clamp(direction.x, -1, 1);
        direction.y = (int)Mathf.Clamp(direction.y, -1, 1);
        Vector3Int nextStep = current + direction;

        if (walkableTilemap.HasTile(nextStep))
        {
            return nextStep;
        }
        else
        {
            return current;
        }
    }
}
