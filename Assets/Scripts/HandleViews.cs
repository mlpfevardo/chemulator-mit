using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandleViews : MonoBehaviour {

    // Use this for initialization
    public RectTransform proceduresPanel;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandlePanel()
    {
        proceduresPanel.DOAnchorPos(new Vector2(0, 0), 0.25f);
        //Time.timeScale = 0f;
    }

    public void HandlePanelBack()
    {
        proceduresPanel.DOAnchorPos(new Vector2(1280, 0), 0.25f);
        //Time.timeScale = 1f;
    }
}
