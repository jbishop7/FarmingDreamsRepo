using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float dayLength = 6f;

    private float currentTime = 0f;
    private float gameHourInSecs = 20f;

    private float hour = 6f;    // starting time
    private float mins = 0f;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI hintText;

    public static TimeController _instance;

    public static TimeController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No time controller");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > gameHourInSecs)
        {
            hour++;
            currentTime = 0f;
        }
        mins = (currentTime / gameHourInSecs) * 6; // trust me bro

        if (hour >= 25)
        {
            hintText.color = Color.red;
            hintText.SetText("You will fall asleep at 2:00! Find your tent to prepare!");
        }
        else
        {
            hintText.color = Color.white;
        }

        if (hour > 25)
        {
            Debug.Log("You are DONE buddy...");
            GameController gc = GameController.Instance;
            gc.ForceEndDay();
            Time.timeScale = 0.0f;
        }

        clockText.SetText($"{hour%24}:{Mathf.Floor(mins)}0");
    }

    public float GetHour()
    {
        return hour;
    }

    public float GetMins()
    {
        return mins;
    }
}
