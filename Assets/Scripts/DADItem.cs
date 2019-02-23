using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DADItem : MonoBehaviour {

    // Use this for initialization
    
    bool unbreakable = false;
    bool isHoldingObject = false;
    public GameObject item;
    //public delegate void DragEvent(DADItem daditem);
    //public static event DragEvent OnItemStartEvent;
    //public static event DragEvent OnItemDragEndEvent;

    void Start () {
        item = GetComponent<GameObject>();
    }

    private void Awake()
    {
        //item = GetComponent<GameObject>();

    }

    // Update is called once per frame
    void Update () {
		//if(Input.GetMouseButtonUp(0) )
       
	}

    

    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    void OnHoldItem()
    {
        if(Input.GetMouseButtonUp(0) && isHoldingObject == true)
        {
            item = Instantiate(item) as GameObject;
            item.transform.position = Input.mousePosition;
        }
    }

    private void OnMouseDrag()
    {
        
    }

    public void Drag()
    {
        //item = Instantiate(item) as GameObject;
        item.transform.position = Input.mousePosition;
    }
}
