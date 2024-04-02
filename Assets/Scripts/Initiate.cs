using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiate : MonoBehaviour
{
    [SerializeField] EyeballSquid EyeballSquid;

    private void Start()
    {
        EyeballSquid = GetComponentInParent<EyeballSquid>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("phasing...");
            EyeballSquid.SetPhase(true);
        }
    }
}
