using System.Collections;
using UnityEngine;

public class SquidInk : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.ToString());
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Has been shot");
            player.TakeDamage(3);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}