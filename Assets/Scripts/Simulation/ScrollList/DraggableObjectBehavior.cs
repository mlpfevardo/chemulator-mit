using EZObjectPools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableObjectBehavior : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject dragIndicatorPrefab;
    public bool moveToFinalPosition = false;

    protected SimulationDragIndicator dragIndicator = null;
    protected static EZObjectPool objectPool = null;

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool removeOnEnd = false;

    #region Properties
    public SimulationMixableBehavior MixtureItem { get; set; } = null;
    public SimulationDragIndicator DragIndicator
    {
        get { return dragIndicator; }
    }
    #endregion

    public void SetRemoveOnEnd()
    {
        if (this.GetType() != typeof(SimulationScrollButton))
        {
            removeOnEnd = true;
        }
    }

    private void Awake()
    {
        //Debug.Log("Running");
        //if (objectPool == null)
        //{
        objectPool = EZObjectPool.CreateObjectPool(dragIndicatorPrefab, "DraggableSpritePools", 5, true, true, true);
        //}
        //objectPool.InstantiatePool();
    }

    //private void OnDestroy()
    //{
    //    Debug.Log("destroyed " + name);
    //    //if (objectPool != null)
    //    //{
    //    //    //objectPool.DeletePool(true);
    //    //    objectPool = null;
    //    //}
    //}

    #region Interface Implementations
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        GameObject dragObject;

        screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPoint.z));

        Debug.Log("Dragging " + MixtureItem.GetItemId() + "/" + MixtureItem.itemName);

        if (MixtureItem == null)
        {
            Debug.LogError("Unable to generate drag indicator. ScrollListItem not assigned. Did you initialized this object properly?");
        }
        else if (objectPool.TryGetNextObject(cursorPoint, Quaternion.identity, out dragObject))
        {
            this.dragIndicator = dragObject.GetComponent<SimulationDragIndicator>();
            if (dragIndicator == null)
            {
                Debug.LogError(dragObject.name + " is not a valid drag indicator.");
                return;
            }

            SpriteRenderer image = dragIndicator.GetComponentInChildren<SpriteRenderer>();
            image.sprite = MixtureItem.icon;
            dragIndicator.transform.SetParent(this.GetComponentInParent<Canvas>().transform);

            PolygonCollider2D collider = dragIndicator.GetComponent<PolygonCollider2D>();

            dragIndicator.GetComponent<CanvasGroup>().blocksRaycasts = false;
            dragIndicator.GetComponent<SimulationDragIndicator>().SetParents(this.gameObject, MixtureItem);

            dragObject.transform.localScale = new Vector3(MixtureItem.Scale, MixtureItem.Scale);

            // compensate for offset of screen point to world point
            offset = dragIndicator.transform.position - cursorPoint;
        }
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (dragIndicator != null)
        {
            Vector3 cursorPoint = new Vector3(eventData.position.x, eventData.position.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

            dragIndicator.transform.position = cursorPosition;
        }
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (this.dragIndicator != null)
        {
            //if (this.moveToFinalPosition)// && this.dragIndicator.CurrentGlowState == ObjectGlowState.Valid)
            //{
            //    this.gameObject.SetActive(false);
            //}
            if (this.removeOnEnd)
            {
                this.gameObject.SetActive(false);
                this.removeOnEnd = false;
            }

            this.dragIndicator.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.dragIndicator.gameObject.SetActive(false);
            this.dragIndicator = null;
        }
    }
    #endregion
}
