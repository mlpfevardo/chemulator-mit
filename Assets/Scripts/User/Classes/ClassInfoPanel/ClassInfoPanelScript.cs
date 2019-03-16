using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ClassInfoPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public GameObject rosterPanel;
    public GameObject exercisePanel;
    public GameObject gradesPanel;
    public GameObject viewClassInfoRoot;

    private LabClass labClass;

    public async Task LoadAsync(LabClass lab)
    {
        labClass = lab;

        rosterPanel.SetActive(false);
        exercisePanel.SetActive(false);
        gradesPanel.SetActive(false);
        viewClassInfoRoot.SetActive(true);

        await viewClassInfoRoot.GetComponent<ViewClassInfoRootScript>().LoadAsync(lab);
    }

    public async void OnClickViewRoster()
    {
        viewClassInfoRoot.SetActive(false);
        exercisePanel.SetActive(false);
        gradesPanel.SetActive(false);
        rosterPanel.SetActive(true);

        await rosterPanel.GetComponent<RosterPanelScript>().LoadAsync(labClass);
    }

    public async void OnClickExersises()
    {
        rosterPanel.SetActive(false);
        viewClassInfoRoot.SetActive(false);
        gradesPanel.SetActive(false);
        exercisePanel.SetActive(true);

        await exercisePanel.GetComponent<ExercisesPanelScript>().LoadAsync(labClass);
    }

    public async void OnClickGrades()
    {
        rosterPanel.SetActive(false);
        viewClassInfoRoot.SetActive(false);
        exercisePanel.SetActive(false);
        gradesPanel.SetActive(true);

        await gradesPanel.GetComponent<GradesPanelScript>().LoadAsync(labClass);
    }
}
