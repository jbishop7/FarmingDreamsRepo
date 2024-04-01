using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Preparation : MonoBehaviour
{
    private GameController gc;
    public TMP_Dropdown tool1Dropdown;
    public TMP_Dropdown tool2Dropdown;

    public GameObject preparationsPanel;
    private bool showPreparations = false;

    public Button advance;

    public TextMeshProUGUI item1Text;
    public TextMeshProUGUI item2Text;

    public static Preparation _instance;

    private string tool1 = "";
    private string tool2 = "";

    Dictionary<string, int> playerTools = new();

    public static Preparation Instance
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
        tool1Dropdown.onValueChanged.AddListener(delegate
        {
            UpdateTool1(tool1Dropdown.value);
        });

        tool2Dropdown.onValueChanged.AddListener(delegate
        {
            UpdateTool2(tool2Dropdown.value);
        });

        advance.onClick.AddListener(delegate
        {
            Advance();
        });

        gc = GameController.Instance;
        preparationsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showPreparations && Input.GetKeyDown(KeyCode.Escape))
        {
            showPreparations = false;
            preparationsPanel.SetActive(false);
        }
    }

    void PopulateToolDropdowns()
    {
        playerTools = gc.GetPlayerTools();
        tool1Dropdown.ClearOptions();
        tool2Dropdown.ClearOptions();

        List<string> tools = new();
        foreach(var (key, val) in playerTools)
        {
            tools.Add(key);
        }

        tool1Dropdown.AddOptions(tools);
        tool2Dropdown.AddOptions(tools);
        UpdateTool1(0);
        UpdateTool2(0);
    }

    public void ShowPreparations()
    {
        showPreparations = true;
        PopulateToolDropdowns();
        preparationsPanel.SetActive(true);
    }

    private void UpdateTool1(int change)
    {
        int i = 0;
        foreach(var (key, val) in playerTools)
        {
            if (i == change)
            {
                tool1 = key;
            }
            i++;
        }
        item1Text.SetText($"Tool 1: {tool1}");
    }

    private void UpdateTool2(int change)
    {
        int i = 0;
        foreach (var (key, val) in playerTools)
        {
            if (i == change)
            {
                tool2 = key;
            }
            i++;
        }

        item2Text.SetText($"Tool 2: {tool2}");
    }

    private void Advance()
    {
        gc.UpdateToolsInUse(tool1, tool2);
        preparationsPanel.SetActive(false);
        showPreparations = false;
        gc.EndDay();
    }
}
