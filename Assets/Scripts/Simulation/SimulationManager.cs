using Assets.Scripts.Simulation.Activities.Lab1;
using Assets.Scripts.Simulation.Activities.Lab2;
using Assets.Scripts.Simulation.Activities.Lab3;
using Assets.Scripts.Simulation.Activities.Lab4;
using Assets.Scripts.Simulation.Activities.Lab5;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager instance = null;
    public PauseManager pauseMenuObject;
    public GameObject equipmentsScrollList;
    public GameObject materialsScrollList;
    public List<GameObject> allowedDropRegions;
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

    public bool IsAllowedToDrop(GameObject obj)
    {
        return allowedDropRegions.Contains(obj);
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
            
            if (GameStateManagerScript.Instance.HasSavedState())
            {
                LoadActivityFromState();
            }
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

    private async void LoadActivityFromState()
    {
        Debug.Log("Start LoadActivityFromState");
        var data = await GameStateManagerScript.Instance.LoadState();
        if (data != null)
        {
            if (data.activityId == GameManager.Instance.CurrentLabActivity)
            {
                GameTimerScript.Instance.SetTime(data.timer);
                SimulationMixtureManager.instance.SetMixturePool(data.mixturePool);
                SimulationMixtureManager.instance.SetSavedMixtures(data.savedMixtures);

                foreach (GameStateTableObject elem in data.elements)
                {
                    elem.mixtureItem.icon = Resources.Load<Sprite>(elem.iconPath);
                    TableDropZone.Instance.AddObject(elem.mixtureItem, new Vector3(elem.x, elem.y, 0), Quaternion.identity);
                }
            }
        }

        Canvas.ForceUpdateCanvases();
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
