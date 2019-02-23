using EZObjectPools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationScrollListHandler : MonoBehaviour {

    private List<SimulationMixableBehavior> itemsList = new List<SimulationMixableBehavior>();
    public Transform contentPanel;
    public GameObject buttonPreFab;

    EZObjectPool objectPool;

	// Use this for initialization
	private void Start () {
        RefreshDisplay();
	}

    private void Awake()
    {
        objectPool = EZObjectPool.CreateObjectPool(buttonPreFab, "simulationPanelButton", 5, true, true, true);
    }

    private void OnDestroy()
    {
        objectPool.ClearPool();
    }

    public void RefreshDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    //public void AddList(List<SimulationMixableBehavior> list)
    //{
    //    if (list != null)
    //    {
    //        itemsList.AddRange(list);

    //        RefreshDisplay();
    //    }
    //}

    public void AddItem(SimulationMixableBehavior item)
    {
        itemsList.Add(item);
    }

    public void ResetList()
    {
        RemoveButtons();
        itemsList.Clear();
    }
	
	private void AddButtons()
    {
        foreach (var item in itemsList)
        {
            GameObject newButton;

            if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out newButton))
            {
                newButton.transform.SetParent(contentPanel, false);
                newButton.transform.localPosition = Vector3.zero;

                SimulationScrollButton button = newButton.GetComponent<SimulationScrollButton>();
                button.Setup(item, this);
            }
            else
            {
                Debug.Log("Failed to draw simulation button");
            }
        }
    }

    private void RemoveButtons()
    {     
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    //private void AddItem(SimulationMixableBehavior itemToAdd, SimulationScrollListHandler scrollList)
    //{
    //    scrollList.itemsList.Add(itemToAdd);
    //}

    //private void RemoveItem(SimulationMixableBehavior itemToRemove, SimulationScrollListHandler scrollList)
    //{
    //    for (int i = scrollList.itemsList.Count - 1; i >= 0; i--)
    //    {
    //        if (scrollList.itemsList[i] == itemToRemove)
    //        {
    //            scrollList.itemsList.RemoveAt(i);
    //        }
    //    }
    //}
}
