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
    public GameObject simulationPanelLabBtnPrefab;
    public GameObject buttonPanelContent;

    private void Awake()
    {
        if (objectPool == null)
        {
            objectPool = EZObjectPool.CreateObjectPool(simulationPanelLabBtnPrefab, "UserSimulationPanelLabBtn", 5, true, false, true);
        }
    }

    public async Task LoadAsync()
    {
        GameObject obj;
        int id;
        IEnumerable<Experiment> experiments = await ExperimentDatabase.GetExperimentsAsync();

        Debug.Log("Start SimulationPanelScript");

        buttonPanelContent.transform.DetachChildren();

        foreach(Experiment exp  in experiments)
        {
            if (int.TryParse(exp.ID, out id))
            {
                if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
                {
                    obj.transform.SetParent(buttonPanelContent.transform);
                    obj.GetComponentInChildren<TextMeshProUGUI>().SetText(exp.Name);
                    
                    obj.GetComponent<Button>().onClick.RemoveAllListeners();
                    obj.GetComponent<Button>().onClick.AddListener(() => UserPanelScript.Instance.StartActivity(id));
                }
            }
            else
            {
                Debug.LogWarning("Failed to convert experiment id " + exp.ID + ". Invalid format.");
            }
        }
    }
}
