using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitButton;


    public static PauseController _instance;

    public static PauseController Instance
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
    void Start()
    {
        resumeButton.onClick.AddListener(delegate
        {
            Resume();
        });

        quitButton.onClick.AddListener(delegate
        {
            Quit();
        });

        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Resume()
    {
        pausePanel.SetActive(false);
        GameController gc = GameController.Instance;
        gc.SetPaused(false);
    }

    void Quit()
    {
        Debug.Log("I would load up a new scene but I won't for now.");
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
    }
}
