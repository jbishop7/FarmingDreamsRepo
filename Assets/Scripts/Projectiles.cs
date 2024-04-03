using System.Collections;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public string projectileType; // "corn" or "potato"
    [SerializeField] Tool tool;
    private Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        tool = player.CurrentTool();
        if (tool == null)
        {
            Debug.LogError("Tool is not assigned to the ProjectileBehavior.");
            Destroy(gameObject); // Destroy bullet if no tool is associated
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            tool = player.CurrentTool();
            Debug.Log(tool.GetDamage());

            EyeballSquid eyeballSquid = collision.gameObject.GetComponent<EyeballSquid>();
            RoboGolem roboGolem = collision.gameObject.GetComponent<RoboGolem>();
            Boss boss = collision.gameObject.GetComponent<Boss>();

            if (roboGolem != null)
            {
                //roboGolem.TakeDamage(1, 1, transform.position);
                roboGolem.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
            if (eyeballSquid != null)
            {
                //eyeballSquid.TakeDamage(1, 1, transform.position);
                eyeballSquid.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
            if (boss != null)
            {
                //eyeballSquid.TakeDamage(1, 1, transform.position);
                boss.TakeDamage(tool.GetDamage(), 1, transform.position, tool.type);
            }
            Destroy(gameObject);
        }
    }
}



