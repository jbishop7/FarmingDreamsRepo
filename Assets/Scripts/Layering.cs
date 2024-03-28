using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layering : MonoBehaviour
{
    private Player player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = Player.Instance;
    }

    void Update()
    {
        if (player.transform.position.y > transform.position.y)
        {
            spriteRenderer.sortingOrder = 3;
        }
        else
        {
            spriteRenderer.sortingOrder = 1;
        }
    }
}
