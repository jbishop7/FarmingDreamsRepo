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
    [SerializeField] private bool player;


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
        if (!player)
        {
            transform.rotation = camera.transform.rotation;
            transform.position = target.position + offset;
        }
        
    }

    public void updateHealthbar(float current, float max)
    {
        healthBar.value = current / max;

        Image fillImage = healthBar.fillRect.GetComponentInChildren<Image>();
        if (fillImage != null)
        {
            float healthPercentage = current / max;
            if (healthPercentage >= 0.7f)
            {
                fillImage.color = Color.green;
            }
            else if (healthPercentage >= 0.4f)
            {
                fillImage.color = new Color(1f, 0.64f, 0f);
            }
            else
            {
                fillImage.color = Color.red;
            }
        }
        else
        {
            Debug.LogError("The health bar's fill image is not set.");
        }
    }
}
