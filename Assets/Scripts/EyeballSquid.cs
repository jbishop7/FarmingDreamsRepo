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
    public float attackRange = 2.5f;
    public float bulletSpeed = 10f;

    public Player player;
    [SerializeField] public GameObject bulletPrefab;

    [SerializeField] float health, maxHealth = 5f;
    [SerializeField] FloatingHealthbar healthbar;

    private Coroutine bulletLaunchCoroutine;
    private Coroutine DamageCoroutine;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        healthbar = GetComponentInChildren<FloatingHealthbar>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        health = maxHealth;
        healthbar.updateHealthbar(health, maxHealth);
    }

    private void Update()
    {
        if (shouldPhase)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            _Animator.SetFloat("speed", moveSpeed);
            _Animator.SetFloat("horizontal", directionToPlayer.x);
            _Animator.SetFloat("vertical", directionToPlayer.y);
            _Rigidbody.MovePosition(_Rigidbody.position + new Vector2(directionToPlayer.x, directionToPlayer.y) * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    public void SetPhase(bool phase)
    {
        shouldPhase = phase;
        if (shouldPhase && bulletLaunchCoroutine == null)
        {
            bulletLaunchCoroutine = StartCoroutine(LaunchBulletsWithCooldown(2f));
        }
        else if (!shouldPhase && bulletLaunchCoroutine != null)
        {
            StopCoroutine(bulletLaunchCoroutine);
            bulletLaunchCoroutine = null;
        }
    }

    private IEnumerator LaunchBulletsWithCooldown(float cooldown)
    {
        while (shouldPhase)
        {
            LaunchBullet();
            yield return new WaitForSeconds(cooldown);
        }
    }

    void LaunchBullet()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab is not assigned in the EyeballSquid.");
        }
    }

    public void TakeDamage(float damage, float knockbackDistance, Vector3 AttackPos)
    {
        health -= damage;
        healthbar.updateHealthbar(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Vector3 knockbackDir = transform.position - AttackPos;
            knockbackDir = knockbackDir.normalized * knockbackDistance;
            transform.position += knockbackDir;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator DamagePlayerOverTime(int damage, float interval)
    {
        while (true)
        {
            player.TakeDamage(damage);
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DamageCoroutine != null)
            {
                StopCoroutine(DamageCoroutine);
                DamageCoroutine = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DamageCoroutine == null)
            {
                DamageCoroutine = StartCoroutine(DamagePlayerOverTime(1, 1f));
            }
        }
    }
}

