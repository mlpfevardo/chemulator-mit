using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SinkPanelScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject item;

        var draggableObject = eventData.pointerDrag.GetComponent<DraggableObjectBehavior>();

        if (draggableObject != null)
        {
            draggableObject.MixtureItem.OnDropToSink();
        }
    }
}
