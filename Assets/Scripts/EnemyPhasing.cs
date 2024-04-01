using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhasing : MonoBehaviour
{
    public Transform playerTransform;
    public Animator _Animator;
    public Rigidbody2D _Rigidbody;
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

        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody2D>();
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

            _Animator.SetFloat("speed", directionToPlayer.sqrMagnitude);
            _Animator.SetFloat("horizontal", directionToPlayer.x);
            _Animator.SetFloat("vertical", directionToPlayer.y);

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
