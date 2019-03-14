using System.Collections;
using System.Collections.Generic;
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

    public void LoadClassInfoPanel(LabClass labClass)
    {
        if (labClass == null)
        {
            Debug.LogError("unable to load info panel for null labClass");
        }
        else
        {
            yourClassesInfoPanel.SetActive(false);
            classInfoPanel.SetActive(true);

            classInfoPanel.GetComponent<ClassInfoPanelScript>().LoadRoot(labClass);
        }
    }

    public void LoadAllClasses()
    {
        classInfoPanel.SetActive(false);
        yourClassesInfoPanel.SetActive(true);

        yourClassesInfoPanel.GetComponent<YourClassesPanelScript>().LoadClasses();
    }
}
