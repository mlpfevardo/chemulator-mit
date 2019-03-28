﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public abstract class SimulationMixableBehavior 
{
    private string itemId = Guid.NewGuid().ToString();
    public string itemName;
    [System.NonSerialized]
    public Sprite icon;
    
    [System.NonSerialized]
    public GameObject Parent;
    public string MixButtonTitle { get; set; } = String.Empty;
    public int MinAllowableMix { get; set; } = 1;
    public bool AutoMix { get; set; } = false;
    public float Scale { get; set; } = 30f;

    public string GetItemId()
    {
        return this.itemId;
    }

    public SimulationMixableBehavior() { }

    public SimulationMixableBehavior(SimulationMixableBehavior otherItem)
    {
        this.itemName = otherItem.itemName;
        this.icon = otherItem.icon;
        this.itemId = otherItem.GetItemId();
        this.Parent = otherItem.Parent;
        this.MixButtonTitle = otherItem.MixButtonTitle;
        this.MinAllowableMix = otherItem.MinAllowableMix;
        this.AutoMix = otherItem.AutoMix;
        this.Scale = otherItem.Scale;
    }

    public virtual bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
    {
        return true;
    }
    public virtual bool isRemovable()
    {
        return true;
    }

    public virtual void OnDropToSink()
    {
        Debug.Log("Dropped to sink");
    }

    public virtual void OnDrop(Transform transform)
    {
        
    }
}