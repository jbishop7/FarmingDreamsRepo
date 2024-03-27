using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    public float damage;
    public string type;
    Player player;
    

    void Start()
    {
        player = Player.Instance;
    }


    void Update()
    {

    }

    public float GetDamage()
    {
        return damage;
    }

    public string GetDamageType()
    {
        return type;
    }

    public void PlayerDoneUse()
    {
        player.SetUsingTool(false);
    }
}
