using Assets.Scripts.Firebase.Database;
using EZObjectPools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GradeListPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public GameObject viewStudentGradeItemPrefab;
    public GameObject studentList;

    private static EZObjectPool objectPool = null;

    private void Awake()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(viewStudentGradeItemPrefab, "ViewStudentGradeItemPrefab", 4, true, false, true);
        }
    }

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start GradesPanelScript, lab=" + lab.ID);
        GameObject item;

        studentList.transform.DetachChildren();

        try
        {
            if (FirebaseAuthManager.instance.IsInstructor())
            {
                var students = await ClassDatabase.GetLabClassStudentsAsync(lab.ID);

                foreach (var student in students)
                {
                    if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
                    {
                        item.transform.SetParent(studentList.transform);
                        item.transform.localScale = new Vector3(1f, 1f);
                        item.GetComponentInChildren<TextMeshProUGUI>().SetText(student.ToString());
                        item.GetComponent<ViewStudentGradeScript>().LoadAsync(student, lab);
                    }
                }
            }
            else
            {
                var exercises = await ClassDatabase.GetLabClassExercisesAsync(lab);

                foreach (var exer in exercises)
                {
                    if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
                    {
                        item.transform.SetParent(studentList.transform);
                        item.transform.localScale = new Vector3(1f, 1f);
                        item.GetComponentInChildren<TextMeshProUGUI>().SetText(exer.Name);
                        item.GetComponent<ViewStudentGradeScript>().LoadAsync(lab);
                    }
                }
            }
        }
        catch (AggregateException e)
        {
            Debug.LogError(FirebaseFunctions.GetFirebaseErrorMessage(e));
        }
    }
}
