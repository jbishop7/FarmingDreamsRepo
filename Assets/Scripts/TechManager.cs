using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechManager : MonoBehaviour
{
    public GameObject techPanel;
    // berry buttons
    public Button berryUnlock;
    public Button berryBountiful;
    public Button berrySpeed;
    public Button berrySword;
    public Button berrySword2;

    // potato buttons
    public Button potatoUnlock;
    public Button potatoBountiful;
    public Button potatoSpeed;
    public Button potatoGun;
    public Button potatoGun2;

    // corn buttons
    public Button cornUnlock;
    public Button cornBountiful;
    public Button cornSpeed;
    public Button cornGun;
    public Button cornGun2;

    private bool showTech = false;

    GameController gc;

    public static TechManager _instance;

    public static TechManager Instance
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
        techPanel.SetActive(false);
        gc = GameController.Instance;

        berryUnlock.onClick.AddListener(delegate
        {
            UnlockBerries();
        });

        berrySpeed.onClick.AddListener(delegate
        {
            UnlockBerrySpeed();
        });

        berryBountiful.onClick.AddListener(delegate
        {
            UnlockBerryBountiful();
        });

        berrySword.onClick.AddListener(delegate
        {
            UnlockBerrySword();
        });

        berrySword2.onClick.AddListener(delegate
        {
            UnlockBerrySword2();
        });

        SetUpBerries();

        potatoUnlock.onClick.AddListener(delegate
        {
            UnlockPotatoes();
        });

        potatoBountiful.onClick.AddListener(delegate
        {
            UnlockPotatoBountiful();
        });

        potatoSpeed.onClick.AddListener(delegate
        {
            UnlockPotatoSpeed();
        });

        potatoGun.onClick.AddListener(delegate
        {
            UnlockPotatoGun();
        });

        potatoGun2.onClick.AddListener(delegate
        {
            UnlockPotatoGun2();
        });

        SetUpPotatoes();

        cornUnlock.onClick.AddListener(delegate
        {
            UnlockCorn();
        });

        cornSpeed.onClick.AddListener(delegate
        {
            UnlockCornSpeed();
        });

        cornBountiful.onClick.AddListener(delegate
        {
            UnlockCornBountiful();
        });

        cornGun.onClick.AddListener(delegate
        {
            UnlockCornGun();
        });

        cornGun2.onClick.AddListener(delegate
        {
            UnlockCornGun2();
        });

        SetUpCorn();
    }

    void Update()
    {
        if (showTech == true && Input.GetKeyDown(KeyCode.Escape))
        {
            showTech = false;
            techPanel.SetActive(false);
        }
    }

    public void ShowTechTree()
    {
        techPanel.SetActive(true);
        showTech = true;
    }

    void SetUpBerries()
    {
        berrySpeed.enabled = false;
        berryBountiful.enabled = false;
        berrySword.enabled = false;
        berrySword2.enabled = false;
        berryUnlock.enabled = false;

        if (gc.BerryEnabled() == false) // has unlocked nothing
        {
            berryUnlock.enabled = true;

            berrySpeed.enabled = false;
            berryBountiful.enabled = false;
            berrySword.enabled = false;
            berrySword2.enabled = false;
        }
        else
        {
            // has unlocked berries
            berrySpeed.enabled = true;
            berryBountiful.enabled = true;

            berryUnlock.enabled = false;
            berrySword.enabled = false;
            berrySword2.enabled = false;
        }
        
        if (gc.BerryEnabled() == true && gc.BerrySpeed() == true)
        {
            berryUnlock.enabled = false;
            berrySpeed.enabled = false;
            berrySword2.enabled = false;

            berryBountiful.enabled = true;
            berrySword.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerryBountiful() == true)
        {
            berryUnlock.enabled = false;
            berryBountiful.enabled = false;
            berrySword2.enabled = false;

            berrySpeed.enabled = true;
            berrySword.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerrySpeed() == true && gc.BerrySword() == true)
        {
            berryUnlock.enabled = false;
            berrySpeed.enabled = false;
            berrySword.enabled = false;

            berryBountiful.enabled = true;
            berrySword2.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerryBountiful() == true && gc.BerrySword() == true)
        {
            berryUnlock.enabled = false;
            berryBountiful.enabled = false;
            berrySword.enabled = false;

            berrySpeed.enabled = true;
            berrySword2.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerrySpeed() == true && gc.BerryBountiful() == true && gc.BerrySword() == true)
        {
            berryUnlock.enabled = false;
            berryBountiful.enabled = false;
            berrySword.enabled = false;
            berrySpeed.enabled = false;

            berrySword2.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerrySpeed() == true && gc.BerrySword() == true && gc.BerrySword2() == true)
        {
            berryUnlock.enabled = false;
            berrySpeed.enabled = false;
            berrySword.enabled = false;
            berrySword2.enabled = false;

            berryBountiful.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerryBountiful() == true && gc.BerrySword() == true && gc.BerrySword2() == true)
        {
            berryUnlock.enabled = false;
            berryBountiful.enabled = false;
            berrySword.enabled = false;
            berrySword2.enabled = false;

            berrySpeed.enabled = true;
        }

        if (gc.BerryEnabled() == true && gc.BerrySpeed() == true&& gc.BerryBountiful() == true && gc.BerrySword() == true && gc.BerrySword2() == true)
        {
            berryUnlock.enabled = false;
            berryBountiful.enabled = false;
            berrySword.enabled = false;
            berrySword2.enabled = false;
            berrySpeed.enabled = false;
        }

    }

    void UnlockBerries()
    {
        if (gc.BerryEnabled() == false) // if they haven't unlocked it yet
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetBerryEnabled(true);   // enable them to farm the berries
                gc.SpendTechPoints(1);
                SetUpBerries();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockBerrySpeed()
    {
        if (gc.BerryEnabled() == true && gc.BerrySpeed() == false)  // if they have access to berries and haven't unlocked it yet
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetBerrySpeed(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpBerries();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockBerryBountiful()
    {
        if (gc.BerryEnabled() == true && gc.BerryBountiful() == false)  // if they have access to berries and haven't unlocked it yet
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetBerryBountiful(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpBerries();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockBerrySword()
    {
        if (gc.BerryEnabled() == true && (gc.BerryBountiful() || gc.BerrySpeed()) && gc.BerrySword() == false)  // if they have access to berries and have either speed or bountiful and don't have the sword
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetBerrySword(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpBerries();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockBerrySword2()
    {
        if (gc.BerryEnabled() == true && (gc.BerryBountiful() || gc.BerrySpeed()) && gc.BerrySword() && gc.BerrySword2() == false)  // if they have access to berries and have either speed or bountiful, sword1, and no sword2
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetBerrySword2(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpBerries();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void SetUpPotatoes()
    {
        potatoSpeed.enabled = false;
        potatoBountiful.enabled = false;
        potatoGun.enabled = false;
        potatoGun2.enabled = false;
        potatoUnlock.enabled = false;

        if (gc.PotatoEnabled() == false) // has unlocked nothing
        {
            potatoUnlock.enabled = true;

            potatoSpeed.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun.enabled = false;
            potatoGun2.enabled = false;
        }
        else
        {
            // has unlocked berries
            potatoSpeed.enabled = true;
            potatoBountiful.enabled = true;

            potatoUnlock.enabled = false;
            potatoGun.enabled = false;
            potatoGun2.enabled = false;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoSpeed() == true)
        {
            potatoUnlock.enabled = false;
            potatoSpeed.enabled = false;
            potatoGun2.enabled = false;

            potatoBountiful.enabled = true;
            potatoGun.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoBountiful() == true)
        {
            potatoUnlock.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun2.enabled = false;

            potatoSpeed.enabled = true;
            potatoGun.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoSpeed() == true && gc.PotatoGun() == true)
        {
            potatoUnlock.enabled = false;
            potatoSpeed.enabled = false;
            potatoGun.enabled = false;

            potatoBountiful.enabled = true;
            potatoGun2.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoBountiful() == true && gc.PotatoGun() == true)
        {
            potatoUnlock.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun.enabled = false;

            potatoSpeed.enabled = true;
            potatoGun2.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoSpeed() == true && gc.PotatoBountiful() == true && gc.PotatoGun() == true)
        {
            potatoUnlock.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun.enabled = false;
            potatoSpeed.enabled = false;

            potatoGun2.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoSpeed() == true && gc.PotatoGun() == true && gc.PotatoGun2() == true)
        {
            potatoUnlock.enabled = false;
            potatoSpeed.enabled = false;
            potatoGun.enabled = false;
            potatoGun2.enabled = false;

            potatoBountiful.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoBountiful() == true && gc.PotatoGun() == true && gc.PotatoGun2() == true)
        {
            potatoUnlock.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun.enabled = false;
            potatoGun2.enabled = false;

            potatoSpeed.enabled = true;
        }

        if (gc.PotatoEnabled() == true && gc.PotatoSpeed() == true && gc.PotatoBountiful() == true && gc.PotatoGun() == true && gc.PotatoGun2() == true)
        {
            potatoUnlock.enabled = false;
            potatoBountiful.enabled = false;
            potatoGun.enabled = false;
            potatoGun2.enabled = false;
            potatoSpeed.enabled = false;
        }
    }

    void UnlockPotatoes()
    { 
        if (gc.PotatoEnabled() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetPotatoEnabled(true);
                gc.SpendTechPoints(1);
                SetUpPotatoes();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockPotatoSpeed()
    {
        if(gc.PotatoEnabled() == true && gc.PotatoSpeed() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetPotatoSpeed(true);
                gc.SpendTechPoints(1);
                SetUpPotatoes();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockPotatoBountiful()
    {
        if (gc.PotatoEnabled() == true && gc.PotatoBountiful() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetPotatoBountiful(true);
                gc.SpendTechPoints(1);
                SetUpPotatoes();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockPotatoGun()
    {
        if (gc.PotatoEnabled() == true && (gc.PotatoBountiful() || gc.PotatoSpeed()) && gc.PotatoGun() == false)  // if they have access to berries and have either speed or bountiful and don't have the sword
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetPotatoGun(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpPotatoes();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockPotatoGun2()
    {
        if (gc.PotatoEnabled() == true && (gc.PotatoBountiful() || gc.PotatoSpeed()) && gc.PotatoGun() && gc.PotatoGun2() == false)  // if they have access to berries and have either speed or bountiful, sword1, and no sword2
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetPotatoGun2(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpPotatoes();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void SetUpCorn()
    {
       cornSpeed.enabled = false;
       cornBountiful.enabled = false;
       cornGun.enabled = false;
       cornGun2.enabled = false;
        cornUnlock.enabled = false;

        if (gc.CornEnabled() == false) // has unlocked nothing
        {
            cornUnlock.enabled = true;

            cornSpeed.enabled = false;
            cornBountiful.enabled = false;
            cornGun.enabled = false;
            cornGun2.enabled = false;
        }
        else
        {
            // has unlocked berries
            cornSpeed.enabled = true;
            cornBountiful.enabled = true;

            cornUnlock.enabled = false;
            cornGun.enabled = false;
            cornGun2.enabled = false;
        }

        if (gc.CornEnabled() == true && gc.CornSpeed() == true)
        {
            cornUnlock.enabled = false;
            cornSpeed.enabled = false;
            cornGun2.enabled = false;

            cornBountiful.enabled = true;
            cornGun.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornBountiful() == true)
        {
            cornUnlock.enabled = false;
            cornBountiful.enabled = false;
            cornGun2.enabled = false;

            cornSpeed.enabled = true;
            cornGun.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornSpeed() == true && gc.CornGun() == true)
        {
            cornUnlock.enabled = false;
            cornSpeed.enabled = false;
            cornGun.enabled = false;

            cornBountiful.enabled = true;
            cornGun2.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornBountiful() == true && gc.CornGun() == true)
        {
            cornUnlock.enabled = false;
            cornBountiful.enabled = false;
            cornGun.enabled = false;

            cornSpeed.enabled = true;
            cornGun2.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornSpeed() == true && gc.CornBountiful() == true && gc.CornGun() == true)
        {
            cornUnlock.enabled = false;
            cornBountiful.enabled = false;
            cornGun.enabled = false;
            cornSpeed.enabled = false;

            cornGun2.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornSpeed() == true && gc.CornGun() == true && gc.CornGun2() == true)
        {
            cornUnlock.enabled = false;
            cornSpeed.enabled = false;
            cornGun.enabled = false;
            cornGun2.enabled = false;

            cornBountiful.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornBountiful() == true && gc.CornGun() == true && gc.CornGun2() == true)
        {
            cornUnlock.enabled = false;
            cornBountiful.enabled = false;
            cornGun.enabled = false;
            cornGun2.enabled = false;

            cornSpeed.enabled = true;
        }

        if (gc.CornEnabled() == true && gc.CornSpeed() == true && gc.CornBountiful() == true && gc.CornGun() == true && gc.CornGun2() == true)
        {
            cornUnlock.enabled = false;
            cornBountiful.enabled = false;
            cornGun.enabled = false;
            cornGun2.enabled = false;
            cornSpeed.enabled = false;
        }
    }

    void UnlockCorn()
    {
        if (gc.CornEnabled() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetCornEnabled(true);
                gc.SpendTechPoints(1);
                SetUpCorn();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockCornSpeed()
    {
        if (gc.CornEnabled() == true && gc.CornSpeed() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetCornSpeed(true);
                gc.SpendTechPoints(1);
                SetUpCorn();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockCornBountiful()
    {
        if (gc.CornEnabled() == true && gc.CornBountiful() == false)
        {
            if (gc.GetTechPoints() > 0)
            {
                gc.SetCornBountiful(true);
                gc.SpendTechPoints(1);
                SetUpCorn();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockCornGun()
    {
        if (gc.CornEnabled() == true && (gc.CornBountiful() || gc.CornSpeed()) && gc.CornGun() == false)  // if they have access to berries and have either speed or bountiful and don't have the sword
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetCornGun(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpCorn();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }

    void UnlockCornGun2()
    {
        if (gc.CornEnabled() == true && (gc.CornBountiful() || gc.CornSpeed()) && gc.CornGun() && gc.CornGun2() == false)  // if they have access to berries and have either speed or bountiful, sword1, and no sword2
        {
            if (gc.GetTechPoints() > 0) // and they have enough tech points
            {
                gc.SetCornGun2(true); // give them double time
                gc.SpendTechPoints(1);
                SetUpCorn();
            }
            else
            {
                gc.GuiHint("Not enough Tech Points.");
            }
        }
    }
}
