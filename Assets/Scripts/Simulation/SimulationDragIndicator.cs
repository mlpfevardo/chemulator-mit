using UnityEngine;
using UnityEngine.UI;
using SpriteGlow;
using System;

public class SimulationDragIndicator : MonoBehaviour, IGlowingObject
{
    private SimulationMixableBehavior parentItem = null;
    private GameObject parentObject = null;
    private GameObject lastCollider = null;

    public ObjectGlowState CurrentGlowState { get; set; }

    private void Update ()
    {
        // do a "floating" effect
        Vector3 tmpPos = transform.position;
        tmpPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 2f) * 0.05f;

        this.transform.position = tmpPos;
	}

    #region Public Methods
    public void SetParents(GameObject gameObject, SimulationMixableBehavior mixtureItem)
    {
        this.parentObject = gameObject;
        this.parentItem = mixtureItem;
    }

    public DraggableObjectBehavior GetLastCollider()
    {
        return lastCollider.GetComponent<DraggableObjectBehavior>();
    }
    #endregion

    #region Interface Implementations
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!SimulationManager.instance.IsAllowedToDrop(collision.collider.gameObject))
        {
            ChangeGlowColor(ObjectGlowState.Invalid);
        }
        else
        {
            ChangeGlowColor();
        }

        DropZoneObjectHandler dropZoneObject = collision.collider.GetComponent<DropZoneObjectHandler>();

        if (dropZoneObject != null && collision.gameObject != this.parentObject)
        {
            
            if (SimulationMixtureManager.instance.IsMixable(dropZoneObject.MixtureItem, this.parentItem))
            {
                ChangeGlowColor(ObjectGlowState.Valid);
            }
            else
            {
                ChangeGlowColor(ObjectGlowState.Invalid);
            }

            this.lastCollider = collision.collider.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (SimulationManager.instance.IsAllowedToDrop(collision.collider.gameObject))
        {
            ChangeGlowColor(ObjectGlowState.Invalid);
        }
        else
        {
            ChangeGlowColor();
        }

        DropZoneObjectHandler dropZoneObject = collision.collider.GetComponent<DropZoneObjectHandler>();

        if (dropZoneObject != null && collision.gameObject != this.parentObject)
        {
            Debug.Log(parentItem.itemName + " exit collision with " + dropZoneObject.MixtureItem.itemName);

            if(this.lastCollider == collision.collider.gameObject)
            {
                ChangeGlowColor();
                this.lastCollider = null;
            }
        }
    }

    public void ChangeGlowColor(ObjectGlowState state = ObjectGlowState.Default)
    {
        var glow = this.GetComponent<SpriteGlowEffect>();
        CurrentGlowState = state;

        switch (state)
        {
            case ObjectGlowState.Valid:
                glow.GlowColor = Color.green;
                break;
            case ObjectGlowState.Invalid:
                glow.GlowColor = Color.red;
                break;
            case ObjectGlowState.Default:
            default:
                glow.GlowColor = Color.yellow;
                break;
        }
    }

    #endregion

    private void OnDisable()
    {
        ChangeGlowColor();
        this.lastCollider = null;
    }
}
