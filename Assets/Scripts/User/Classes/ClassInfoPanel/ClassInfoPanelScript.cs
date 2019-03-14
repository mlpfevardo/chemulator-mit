using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassInfoPanelScript : MonoBehaviour
{
    public GameObject rosterPanel;
    public GameObject viewClassInfoRoot;

    private LabClass labClass;

    public void LoadRoot(LabClass lab)
    {
        labClass = lab;

        rosterPanel.SetActive(false);
        viewClassInfoRoot.SetActive(true);
    }

    public void OnClickViewRoster()
    {
        viewClassInfoRoot.SetActive(false);
        rosterPanel.SetActive(true);
        rosterPanel.GetComponent<RosterPanelScript>().LoadRosterAsync(labClass);
    }
}
