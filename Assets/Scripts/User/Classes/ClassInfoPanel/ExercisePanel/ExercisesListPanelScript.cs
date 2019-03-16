using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Firebase.Database;
using EZObjectPools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExercisesListPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public Button btnCreateExercise;
    public GameObject exercisesList;
    public GameObject exerciseItemPrefab;

    private static EZObjectPool objectPool = null;

    private void Start()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(exerciseItemPrefab, "ExerciseItemprefab", 3, true, false, true);
        }
    }

    public async Task LoadAsync(LabClass lab)
    {
        GameObject item;

        Debug.Log("Loading ExercisesListPanel, lab=" + lab.ID);

        btnCreateExercise.gameObject.SetActive(FirebaseAuthManager.instance.IsInstructor());

        exercisesList.transform.DetachChildren();

        try
        {
            var exercises = await ClassDatabase.GetLabClassExercisesAsync(lab);

            foreach (var exer in exercises)
            {
                if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
                {
                    item.transform.SetParent(exercisesList.transform);
                    item.transform.localScale = new Vector3(1f, 1f);
                    item.GetComponentInChildren<TextMeshProUGUI>().SetText(exer.Name);

                    item.GetComponent<ViewExerciseButtonScript>().Exercise = exer;
                }
            }
        }
        catch (AggregateException e)
        {
            Debug.LogError(FirebaseFunctions.GetFirebaseErrorMessage(e));
        }
    }
}
