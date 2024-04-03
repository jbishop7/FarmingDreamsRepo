using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    public float damage;
    public string type;
    Player player;

    private static Tool _instance;

    public static Tool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No game controller");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        player = Player.Instance;
    }


    public float GetDamage()
    {
        return damage;
    }


    public void PlayerDoneUse()
    {
        player.SetUsingTool(false);
    }
}