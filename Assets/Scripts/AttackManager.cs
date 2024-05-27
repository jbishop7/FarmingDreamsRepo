using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private const string PlayerDamageKey = "PlayerDamage";
    private Player player;
    private float defaultDamage;
    [SerializeField] Tool tool;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            tool = player.CurrentTool();

            EyeballSquid eyeballSquid = collision.gameObject.GetComponent<EyeballSquid>();
            RoboGolem roboGolem =  collision.gameObject.GetComponent<RoboGolem>();
            Boss boss = collision.gameObject.GetComponent<Boss>();

            if (roboGolem != null )
            {
                //roboGolem.TakeDamage(1, 1, transform.position);
                roboGolem.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
            if(eyeballSquid != null )
            {
                //eyeballSquid.TakeDamage(1, 1, transform.position);
                eyeballSquid.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
            if (boss != null)
            {
                //eyeballSquid.TakeDamage(1, 1, transform.position);
                boss.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
        }
    }
}
