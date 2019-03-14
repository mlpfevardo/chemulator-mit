using Assets.Scripts.Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnrollPanelScript : MonoBehaviour
{
    public GameObject classInfoPanel;
    public TextMeshProUGUI textLabName;
    public TextMeshProUGUI textInstructorName;

    public InputField inputKey;
    public Button buttonEnroll;
    public Text textMessage;

    private void Awake()
    {
        classInfoPanel.SetActive(false);
    }

    public async void OnClickEnroll()
    {
        textMessage.text = "Please wait...";
        classInfoPanel.SetActive(false);

        SetInteractibility(false);

        try
        {
            LabClass lab = await ClassDatabase.GetLabClassAsync(inputKey.text);
            if (lab == null)
            {
                textMessage.text = "Invalid key";
            }
            else
            {
                bool b = await ClassDatabase.IsLabHasStudent(lab, FirebaseAuthManager.instance.ActiveUserInfo);

                if (!b)
                {
                    await ClassDatabase.RegisterLabStudent(lab, FirebaseAuthManager.instance.ActiveUserInfo);
                    textMessage.text = "";

                    textLabName.SetText(lab.Name);
                    var instructor = await ClassDatabase.GetLabInstructorAsync(lab);
                    textInstructorName.SetText(instructor.UserInfo.ToString());

                    inputKey.text = "";

                    classInfoPanel.SetActive(true);
                }
                else
                {
                    textMessage.text = "You are already enrolled in this class";
                }
            }
        }
        catch (AggregateException e)
        {
            textMessage.text = FirebaseFunctions.GetFirebaseErrorMessage(e);
        }

        SetInteractibility(true);
    }

    private void SetInteractibility(bool interact)
    {
        inputKey.interactable = interact;
        buttonEnroll.interactable = interact;
    }
}
