using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EZObjectPools;
using System.Threading.Tasks;

public class TableDropZone : MonoBehaviour, IDropHandler {

    public GameObject itemIndicatorPrefab;
    public static TableDropZone Instance { get; private set; }

    private EZObjectPool objectPool;
    
    // Use this for initialization
    private void Awake()
    {
        objectPool = EZObjectPool.CreateObjectPool(itemIndicatorPrefab, "DropZoneObjectPool", 5, true, true, true);

        //if (!instance)
        //{
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        //}
    }

    private void OnDestroy()
    {
        //objectPool.ClearPool();
    }

    public void RemoveObjects()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void AddObject(SimulationMixableBehavior element, Vector3 position, Quaternion rotation)
    {
        Debug.Log($"Start TableDropZone_AddObject, element={element.GetItemId()} x={position.x} y={position.y}");
        GameObject item;

        if (objectPool.TryGetNextObject(position, Quaternion.identity, out item))
        {
            item.GetComponent<DropZoneObjectHandler>().Setup(element);
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(element.Scale, element.Scale);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item;

        var draggableObject = eventData.pointerDrag.GetComponent<DraggableObjectBehavior>();

        if (draggableObject != null)
        {
            if (draggableObject.DragIndicator.CurrentGlowState == ObjectGlowState.Default)
            {
                if (!draggableObject.moveToFinalPosition)
                {
                    if (objectPool.TryGetNextObject(draggableObject.DragIndicator.transform.position, Quaternion.identity, out item))
                    {
                        draggableObject.MixtureItem.OnDrop(item.transform);

                        item.GetComponent<DropZoneObjectHandler>().Setup(draggableObject);
                        item.transform.SetParent(this.transform);
                        item.transform.localScale = new Vector3(draggableObject.MixtureItem.Scale, draggableObject.MixtureItem.Scale);
                    }
                    else
                    {
                        Debug.Log("Unable to generate object for dropped item");
                    }
                }
                else
                {
                    draggableObject.transform.position = draggableObject.DragIndicator.transform.position;
                }
            }
            else if (draggableObject.DragIndicator.CurrentGlowState == ObjectGlowState.Valid)
            {
                // perform mix item operations
                // dropped object will be mixed with the collider
                if(draggableObject.DragIndicator.GetLastCollider() is DropZoneObjectHandler)
                {
                    SimulationMixtureManager.instance.AddMixableToMixture((draggableObject.DragIndicator.GetLastCollider() as DropZoneObjectHandler), draggableObject);
                    Debug.Log("Mixed " + draggableObject.DragIndicator.GetLastCollider().MixtureItem.itemName + " with " + draggableObject.MixtureItem.itemName);
                }
                else
                {
                    Debug.LogWarning("Unknown state detected");
                }
            }
        }
    }
}
