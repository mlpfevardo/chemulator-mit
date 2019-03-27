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
    public GameObject editGradePanel;

    private void Awake()
    {
        Instance = this;
    }

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start GradesPanelScript, lab=" + lab.ID);
        gradeInfoPanel.SetActive(false);
        editGradePanel.SetActive(false);
        gradeListPanel.SetActive(true);

        if (FirebaseAuthManager.instance.IsInstructor())
        {

            // load students of class
            await gradeListPanel.GetComponent<GradeListPanelScript>().LoadAsync(lab);
        }
        else
        {
            // load grades of current student
            await LoadGradeInfo(FirebaseAuthManager.instance.ActiveUserInfo, lab);
        }
    }

    public async Task LoadGradeInfo(UserInfo user, LabClass labClass)
    {
        gradeListPanel.SetActive(false);
        editGradePanel.SetActive(false);
        gradeInfoPanel.SetActive(true);

        await gradeInfoPanel.GetComponent<GradeInfoPanelScript>().LoadAsync(user, labClass);
    }

    public async Task LoadEditGrade(UserInfo user, LabClass lab, Exercise exercise, StudentGrade studentGrade)
    {
        gradeListPanel.SetActive(false);
        editGradePanel.SetActive(true);
        gradeInfoPanel.SetActive(false);

        await editGradePanel.GetComponent<EditGradePanelScript>().LoadAsync(user, lab, exercise, studentGrade);
    }
}
