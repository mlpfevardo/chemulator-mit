using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
using Assets.Scripts.Firebase.Database;
using TMPro;

public class YourClassesPanelScript : MonoBehaviour
{
    public GameObject viewClassButtonPrefab;
    public GameObject classListContent;

    private static EZObjectPool objectPool = null;

    private void Start()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(viewClassButtonPrefab, "ViewClassButtonPrefab", 2, true, false, true);
        }
    }

    public async void LoadClasses()
    {
        GameObject obj;
        var classes = await UserDatabase.GetLabClasses(FirebaseAuthManager.instance.ActiveUserInfo);

        classListContent.transform.DetachChildren();

        foreach (var lab in classes)
        {
            if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
            {
                obj.transform.SetParent(classListContent.transform);
                obj.transform.localScale = new Vector3(1f, 1f);

                obj.GetComponent<ViewClassesButtonScript>().LabClass = lab;
                obj.GetComponentInChildren<TextMeshProUGUI>().SetText(lab.Name);
            }
        }
    }
}
