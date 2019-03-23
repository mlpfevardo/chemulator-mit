using EZObjectPools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneObjectHandler : DraggableObjectBehavior, IPointerClickHandler {
    public void Setup(DraggableObjectBehavior draggedObject)
    {
        this.moveToFinalPosition = true;

        // create a copy of the behavior data class for an independent access
        if (draggedObject.GetComponent<SimulationScrollButton>() != null)
        {
            this.MixtureItem = (SimulationMixableBehavior)Activator.CreateInstance(draggedObject.MixtureItem.GetType(), draggedObject.MixtureItem);
        }
        else
        {
            this.MixtureItem = draggedObject.MixtureItem;
        }

        this.MixtureItem.Parent = this.gameObject;

        SetIcon(this.MixtureItem.icon);
    }

    public void Setup(SimulationMixableBehavior element)
    {
        this.moveToFinalPosition = true;
        this.MixtureItem = element;
        this.MixtureItem.Parent = this.gameObject;

        SetIcon(this.MixtureItem.icon);
    }

    public void SetIcon(Sprite icon)
    {
        this.GetComponent<SpriteRenderer>().sprite = icon;
    }

    #region Interface Implementations
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        this.GetComponent<Renderer>().enabled = false;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        this.GetComponent<Renderer>().enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // show menu if we got a valid click, instead of drag
        if (this.GetComponent<Renderer>().enabled)
        {
            SimulationMixtureManager.instance.ShowContextMenu(this);
        }
    }
    #endregion
}
