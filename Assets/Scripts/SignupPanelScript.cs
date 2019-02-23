using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignupPanelScript : MonoBehaviour {

    public GameObject SignupPanel;
    public GameObject LoginPanel;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SignupPanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
    }
}
