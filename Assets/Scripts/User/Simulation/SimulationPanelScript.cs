using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using EZObjectPools;
using Assets.Scripts.Firebase.Database;
using TMPro;

public class SimulationPanelScript : MonoBehaviour, ILoadableClass
{
    private static EZObjectPool objectPool = null;
    public GameObject viewSimulClassButtonPrefab;
    public GameObject buttonPanelContent;

    public GameObject expListPanel;
    public GameObject classListPanel;
    public GameObject classListContent;

    public static SimulationPanelScript Instance { get; private set; }

    private LabClass activeClass;

    private void Awake()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(viewSimulClassButtonPrefab, "ViewSimulClassButtonPrefab", 3, true, false, true);
        }

        Instance = this;
    }

    public async Task LoadAsync()
    {
        expListPanel.SetActive(false);
        classListPanel.SetActive(true);

        GameObject obj;
        classListContent.transform.DetachChildren();

        var classes = await UserDatabase.GetLabClasses(FirebaseAuthManager.instance.ActiveUserInfo);

        foreach (var lab in classes)
        {
            if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
            {
                obj.transform.SetParent(classListContent.transform);
                obj.transform.localScale = new Vector3(1f, 1f);

                obj.GetComponent<ViewSimulClassButtonScript>().LabClass = lab;
                obj.GetComponentInChildren<TextMeshProUGUI>().SetText(lab.Name);
            }
        }
    }

    public async Task LoadSimulationList(LabClass labClass)
    {
        expListPanel.SetActive(true);
        classListPanel.SetActive(false);

        activeClass = labClass;
    }

    public void StartActivity(int id)
    {
        UserPanelScript.Instance.StartActivity(id, activeClass);
    }
}
