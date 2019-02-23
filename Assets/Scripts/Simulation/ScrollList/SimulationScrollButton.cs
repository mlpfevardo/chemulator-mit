using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EZObjectPools;
using TMPro;

public class SimulationScrollButton : DraggableObjectBehavior
{
    private SimulationScrollListHandler scrollList;

    public void Setup(SimulationMixableBehavior currentItem, SimulationScrollListHandler currentScrollList)
    {
        MixtureItem = currentItem;
        
        // setup image and text
        try
        {
            var imageComponents = this.GetComponentsInChildren<Image>();

            // don't look for components on the button itself, just on the children
            foreach (Image image in imageComponents)
            {
                if (image.gameObject != this.gameObject)
                {
                    image.sprite = currentItem.icon;
                    break;
                }
            }

            this.GetComponentInChildren<TextMeshProUGUI>().text = currentItem.itemName;
        }
        catch (NullReferenceException)
        {
            Debug.LogError(this.name + " button does not have an Image or Text child component");
        }

        scrollList = currentScrollList;
    }
}
