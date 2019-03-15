using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassesPanelScript : MonoBehaviour
{
    public static ClassesPanelScript Instance { get; private set; }

    public GameObject classInfoPanel;
    public GameObject yourClassesInfoPanel;

    private void Awake()
    {
        Instance = this;
    }

    public async void LoadClassInfoPanel(LabClass labClass)
    {
        if (labClass == null)
        {
            Debug.LogError("unable to load info panel for null labClass");
        }
        else
        {
            Debug.Log("Load ClassesPanelScript, labClass=" + labClass.ID);

            yourClassesInfoPanel.SetActive(false);
            classInfoPanel.SetActive(true);

            await classInfoPanel.GetComponent<ClassInfoPanelScript>().LoadAsync(labClass);
        }
    }

    public void LoadAllClasses()
    {
        classInfoPanel.SetActive(false);
        yourClassesInfoPanel.SetActive(true);

        yourClassesInfoPanel.GetComponent<YourClassesPanelScript>().LoadClasses();
    }
}
