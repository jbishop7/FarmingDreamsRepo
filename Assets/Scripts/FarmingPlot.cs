using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingPlot : MonoBehaviour
{

    public Animator animator;
    private float growTimer;
    public float growDuration;
    private bool complete = false;
    private bool planted = false;
    public string[] crops;
    public int[] quantities;

    private bool stage1 = false;
    private bool stage2 = false;
    private bool stage3 = false;

    private void Awake()
    {
        growTimer = growDuration;
    }
    void Start()
    {
        GameController gc = GameController.Instance;
        TimeController tc = TimeController.Instance;
        if (gc.GetDayCount() > 1 && tc.GetHour() < 7 && tc.GetMins() < 1)
        {
            planted = true;
            growTimer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
       

        if (planted)
        {
            growTimer -= Time.deltaTime;
            // control the growth, need all these conditions so it doesn't always update the animator. Only when needed.
            if (growTimer < 2 * growDuration / 3 && growTimer > growDuration / 3 && !stage1)
            {
                stage1 = true;
                animator.SetFloat("GrowStage", 1);
            }
            if (growTimer < growDuration / 3 && growTimer > 0 && !stage2)
            {
                stage1 = false;
                stage2 = true;
                animator.SetFloat("GrowStage", 2);
            }
            if (growTimer < 0 && !stage3)
            {
                stage1 = false;
                stage2 = false;
                stage3 = true;
                complete = true;
                animator.SetFloat("GrowStage", 3);
            }
        }
         
        
    }

    public void PlantCrop()
    {
        planted = true;
        animator.SetFloat("GrowStage", 1);
    }

    public void HarvestCrop()
    {
        planted = false;
        complete = false;
        growTimer = growDuration;
        stage3 = false;
        GameController gc = GameController.Instance;
        
        gc.AddToInventory(crops, quantities);
        animator.SetFloat("GrowStage", 0);
    }

    public bool GetPlantedState()
    {
        return planted;
    }

    public bool CanHarvest()
    {
        return complete;
    }
}
