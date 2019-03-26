using Assets.Scripts.Firebase.Database;
using EZObjectPools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GradeInfoPanelScript : MonoBehaviour
{
    public GameObject gradeInfoItemPrefab;
    public GameObject gradeInfoList;
    public TextMeshProUGUI txtTitle;

    private static EZObjectPool objectPool = null;
    private LabClass currentLab;

    private void Awake()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(gradeInfoItemPrefab, "GradeInfoItemPrefab", 2, true, false, true);
        }
    }

    public async Task LoadAsync(Student student, LabClass labClass)
    {
        GameObject item;
        Debug.Log($"Start GradeInfoPanelScript, student={student.ID} labClass={labClass?.ID}");
        txtTitle.SetText("View Grade: " + student.UserInfo.ToString());

        currentLab = labClass;

        gradeInfoList.transform.DetachChildren();

        try
        {
            var exercises = await ClassDatabase.GetLabClassExercisesAsync(labClass);

            foreach (var exer in exercises)
            {
                if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
                {
                    item.transform.SetParent(gradeInfoList.transform);
                    item.transform.localScale = new Vector3(1f, 1f);

                    item.GetComponent<GradeInfoItemScript>().LoadAsync(student, labClass, exer);
                }
            }
        }
        catch (AggregateException e)
        {
            Debug.LogError(FirebaseFunctions.GetFirebaseErrorMessage(e));
        }
    }

    public void OnBack()
    {
        GradesPanelScript.Instance.LoadAsync(currentLab);
    }
}
