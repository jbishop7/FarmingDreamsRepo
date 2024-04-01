using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class FloatingHealthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        GameObject cameraGameObject = GameObject.Find("Main Camera");
        if (cameraGameObject != null)
        {
            camera = cameraGameObject.GetComponent<Camera>();
        }
    }
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }

    public void updateHealthbar(float current, float max)
    {
        healthBar.value = current / max;
    }
}
