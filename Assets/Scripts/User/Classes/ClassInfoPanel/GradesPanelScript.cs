using Assets.Scripts.Firebase.Database;
using EZObjectPools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GradesPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public GameObject gradeListPanel;

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start GradesPanelScript, lab=" + lab.ID);
        gradeListPanel.SetActive(true);

        await gradeListPanel.GetComponent<GradeListPanelScript>().LoadAsync(lab);
    }
}
