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
    public TextMeshProUGUI item1Text;
    public TextMeshProUGUI item2Text;

    public TMP_Dropdown buff1Dropdown;
    public TMP_Dropdown buff2Dropdown;
    public TextMeshProUGUI buff1Text;
    public TextMeshProUGUI buff2Text;

    public GameObject preparationsPanel;
    private bool showPreparations = false;

    public Button advance;

    public static Preparation _instance;

    private string tool1 = "";
    private string tool2 = "";

    private string buff1 = "";
    private string buff2 = "";

    Dictionary<string, int> playerTools = new();
    Dictionary<string, int> playerBuffs = new();

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

        buff1Dropdown.onValueChanged.AddListener(delegate
        {
            UpdateBuff1(buff1Dropdown.value);
        });

        buff2Dropdown.onValueChanged.AddListener(delegate
        {
            UpdateBuff2(buff2Dropdown.value);
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

    void PopulateBuffDropdowns()
    {
        playerBuffs = gc.GetPlayerBuffs();
        buff1Dropdown.ClearOptions();
        buff2Dropdown.ClearOptions();

        List<string> buffs = new();
        playerBuffs.Add("none", 1);
        foreach(var(key, val) in playerBuffs)
        {
            buffs.Add(key);
        }
        buff1Dropdown.AddOptions(buffs);
        buff2Dropdown.AddOptions(buffs);
        UpdateBuff1(0);
        UpdateBuff2(0);

    }
    public void ShowPreparations()
    {
        showPreparations = true;
        PopulateToolDropdowns();
        PopulateBuffDropdowns();
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

    private void UpdateBuff1(int change)
    {
        int i = 0;
        foreach (var (key, val) in playerBuffs)
        {
            if (i == change)
            {
                buff1 = key;
            }
            i++;
        }
        buff1Text.SetText($"Buff 1: {buff1}");
    }

    private void UpdateBuff2(int change)
    {
        int i = 0;
        foreach (var (key, val) in playerBuffs)
        {
            if (i == change)
            {
                buff2 = key;
            }
            i++;
        }
        buff2Text.SetText($"Buff 2: {buff2}");
    }

    private void Advance()
    {
        if (buff1 == buff2)
        {
            buff2 = "";
        }
        gc.UpdateBuffsInUse(buff1, buff2);
        gc.UpdateToolsInUse(tool1, tool2);
        preparationsPanel.SetActive(false);
        showPreparations = false;
        gc.EndDay();
    }
}
