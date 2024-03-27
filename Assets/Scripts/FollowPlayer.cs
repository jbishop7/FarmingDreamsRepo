using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 position = new Vector3(0, 0, -10);

    // Update is called once per frame
    void Update()
    {
        position.x = player.transform.position.x;
        position.y = player.transform.position.y;   

    }

    private void FixedUpdate()
    {
        transform.position = position;
    }
}
