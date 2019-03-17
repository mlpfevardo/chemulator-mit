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
    public static GradesPanelScript Instance { get; private set; }
    public GameObject gradeListPanel;
    public GameObject gradeInfoPanel;

    private void Awake()
    {
        Instance = this;
    }

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start GradesPanelScript, lab=" + lab.ID);
        gradeInfoPanel.SetActive(false);
        gradeListPanel.SetActive(true);

        if (FirebaseAuthManager.instance.IsInstructor())
        {

            // load students of class
            await gradeListPanel.GetComponent<GradeListPanelScript>().LoadAsync(lab);
        }
        else
        {
            // load grades of current student
            await LoadGradeInfo(FirebaseAuthManager.instance.GetStudentInfo(), lab);
        }
    }

    public async Task LoadGradeInfo(Student student, LabClass labClass)
    {
        gradeListPanel.SetActive(false);
        gradeInfoPanel.SetActive(true);

        await gradeInfoPanel.GetComponent<GradeInfoPanelScript>().LoadAsync(student, labClass);
    }
}
