using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
using Assets.Scripts.Firebase.Database;
using System.Threading.Tasks;
using TMPro;

public class RosterPanelScript : MonoBehaviour
{
    public GameObject rosterItemPrefab;
    public GameObject rosterList;

    //public static string activeClassKey = "106xW23zQ5";// string.Empty;

    private static EZObjectPool objectPool = null;

    private void Start()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(rosterItemPrefab, "RosterListPrefabs", 3, true, false, true);
        }
    }

    public async Task LoadRosterAsync(LabClass lab)
    {
        GameObject item;

        var students = await ClassDatabase.GetLabClassStudentsAsync(lab.ID);
        //activeClassKey = lab.ID;

        rosterList.transform.DetachChildren();

        foreach(var student in students)
        {
            if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out item))
            {
                item.transform.SetParent(rosterList.transform);
                item.transform.localScale = new Vector3(1f, 1f);
                item.GetComponent<TextMeshProUGUI>().SetText(student.ToString());
            }
        }
    }
}
