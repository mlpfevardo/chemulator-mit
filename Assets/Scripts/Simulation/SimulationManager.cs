﻿using Assets.Scripts.Simulation.Activities.Lab1;
using Assets.Scripts.Simulation.Activities.Lab2;
using Assets.Scripts.Simulation.Activities.Lab3;
using Assets.Scripts.Simulation.Activities.Lab4;
using Assets.Scripts.Simulation.Activities.Lab5;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager instance = null;
    public PauseManager pauseMenuObject;
    public GameObject equipmentsScrollList;
    public GameObject materialsScrollList;
    private SimulationActivityBehavior activeActivity = null;

    private void Awake()
    {
        Debug.Log("SimulationManager instantiated");
        Time.timeScale = 1.0f;

        if (instance != null)
        {
            instance.Setup();

            if (instance != this)
            {
                Destroy(this.gameObject);
                Debug.Log("SimulationManager destroyed");
            }
            return;
        }

        instance = this;

        // preload stuffs
        var x = ModalPanel.Instance;
        var y = PartSelectorPanel.Instance;
        instance.Setup();
    }

    public SimulationActivityBehavior ActiveActivity
    {
        get { return activeActivity; }
    }

    private void Setup()
    {
        switch (GameManager.Instance.CurrentLabActivity)
        {
            case 1:
                activeActivity = new LabOneManager();
                break;
            case 2:
                activeActivity = new LabTwoManager();
                break;
            case 3:
                activeActivity = new LabThreeManager();
                break;
            case 4:
                activeActivity = new LabFourManager();
                break;
            case 5:
                activeActivity = new LabFiveManager();
                break;
            default:
                Debug.LogError("Unknown activity detected");
                break;
        }

        if (activeActivity != null)
        {
            PauseManager.OnPause += activeActivity.OnPause;
            activeActivity.Setup();
            RefreshList();
        }

        Debug.Log("Requested activity for " + GameManager.Instance.CurrentLabActivity);
    }

    public void AddEquipmentItem(SimulationMixableBehavior item)
    {
        equipmentsScrollList.GetComponent<SimulationScrollListHandler>().AddItem(item);
    }

    public void AddMaterial(SimulationMixableBehavior item)
    {
        materialsScrollList.GetComponent<SimulationScrollListHandler>().AddItem(item);
    }

    public void ClearList(bool materials = true, bool equipments = true)
    {
        if (materials)
        {
            materialsScrollList.GetComponent<SimulationScrollListHandler>().ResetList();
        }
        if (equipments)
        {
            equipmentsScrollList.GetComponent<SimulationScrollListHandler>().ResetList();
        }
    }
    
    public void RefreshList()
    {
        equipmentsScrollList.GetComponent<SimulationScrollListHandler>().RefreshDisplay();
        materialsScrollList.GetComponent<SimulationScrollListHandler>().RefreshDisplay();
    }

    public void DoQuit()
    {
        if (activeActivity != null)
        {
            PauseManager.OnPause -= activeActivity.OnPause;
        }
        //SceneStorageManager.Instance.LoadPreviousScene();
        SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.User, true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Input.ResetInputAxes();
            pauseMenuObject.ShowPauseMenu();
        }
    }
}
