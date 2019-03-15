using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
using Assets.Scripts.Firebase.Database;
using System.Threading.Tasks;
using TMPro;
using System;

public class RosterPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public GameObject rosterItemPrefab;
    public GameObject rosterList;

    private static EZObjectPool objectPool = null;

    private void Start()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(rosterItemPrefab, "RosterListPrefabs", 3, true, false, true);
        }
    }

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start RosterPanelScript, lab=" + lab.ID);
        GameObject item;

        rosterList.transform.DetachChildren();

        try
        {
            var students = await ClassDatabase.GetLabClassStudentsAsync(lab.ID);

            foreach (var student in students)
            {
                if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
                {
                    item.transform.SetParent(rosterList.transform);
                    item.transform.localScale = new Vector3(1f, 1f);
                    item.GetComponent<TextMeshProUGUI>().SetText(student.ToString());
                }
            }
        }
        catch(AggregateException e)
        {
            Debug.LogError(FirebaseFunctions.GetFirebaseErrorMessage(e));
        }
    }
}
