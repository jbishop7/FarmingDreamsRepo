using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private const string PlayerDamageKey = "PlayerDamage";
    private float defaultDamage;
    [SerializeField] Tool tool;



    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit an enemy");
            EyeballSquid eyeballSquid = collision.gameObject.GetComponent<EyeballSquid>();
            RoboGolem roboGolem =  collision.gameObject.GetComponent<RoboGolem>();

            if(roboGolem != null )
            {
                roboGolem.TakeDamage(1, 1, transform.position);
                //roboGolem.TakeDamage(tool.GetDamage());
            }
            if(eyeballSquid != null )
            {
                eyeballSquid.TakeDamage(1, 1, transform.position);
                //eyeballSquid.TakeDamage(tool.GetDamage());
            }
        }
    }
}
