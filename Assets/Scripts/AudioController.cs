using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource chop;
    public AudioSource treeFalling;

    public static AudioController _instance;

    public static AudioController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void ChopWood()
    { 
        chop.Play();
        chop.volume = 0.5f;
    }

    public void TreeFalling()
    {
        treeFalling.Play();
        treeFalling.volume = 0.25f;
    }
}
