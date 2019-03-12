using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegistrationSuccessScript : MonoBehaviour {

    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondaryText;
    // Use this for initialization
    public void ShowLoadingMessage()
    {
        primaryText.SetText("Processing.... Please Wait....");
        secondaryText.gameObject.SetActive(false);
    }

    public void ShowSuccessMessage()
    {
        primaryText.SetText("You have been successfully registered!");
        secondaryText.gameObject.SetActive(true);
    }
}
