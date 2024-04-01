using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhasing : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5.0f; 
    private bool shouldPhase = false;

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void Update()
    {
        if (shouldPhase)
        {
            Phase();
        }
    }

    private void Phase()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer.Normalize();

            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided");
            shouldPhase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("un collided");
            shouldPhase = false;
        }
    }
}
