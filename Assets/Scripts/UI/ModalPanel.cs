using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ModalPanel : MonoBehaviour
{
    private TextMeshProUGUI question;
    private TextMeshProUGUI title;
    private Button yesButton;
    private Button noButton;
    private GameObject modalPanelObject;

    private static ModalPanel instance;

    public static ModalPanel Instance
    {
        get
        {
            if(!instance)
            {
                instance = new GameObject("ModalPanel").AddComponent<ModalPanel>();
                var prefab = Resources.Load<GameObject>("Prefabs/Modal");
                instance.modalPanelObject = GameObject.Instantiate(prefab);
                instance.modalPanelObject.transform.SetParent(instance.transform);

                instance.yesButton = instance.modalPanelObject.transform.Find("Panel/ModalDialogPanel/ButtonPanel/OkButton").GetComponent<Button>();
                instance.noButton = instance.modalPanelObject.transform.Find("Panel/ModalDialogPanel/ButtonPanel/NoButton").GetComponent<Button>();
                instance.question = instance.modalPanelObject.transform.Find("Panel/ModalDialogPanel/QuestionPanel/QuestionText").GetComponent<TextMeshProUGUI>();
                instance.title = instance.modalPanelObject.transform.Find("Panel/ModalDialogPanel/TitlePanel/TitleText").GetComponent<TextMeshProUGUI>();

                DontDestroyOnLoad(instance);
                instance.modalPanelObject.SetActive(false);
            }

            return instance;
        }
    }

    public void ShowModalOK(string title, string question, UnityAction action = null)
    {
        ShowModal(title, question, "ok", null, action, null);
    }

    public void ShowModalYesNo(string title, string question, UnityAction yesAction, UnityAction noAction)
    {
        ShowModal(title, question, "Yes", "No", yesAction, noAction);
    }

    private void ShowModal(string title, string question, string okText, string noText, UnityAction yesEvent, UnityAction noEvent)
    {
        Time.timeScale = 0f;

        modalPanelObject.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = okText;
        yesButton.onClick.AddListener(ClosePanel);
        if (yesEvent != null)
        {
            yesButton.onClick.AddListener(yesEvent);
        }

        noButton.onClick.AddListener(ClosePanel);
        if (noEvent != null)
        {
            noButton.GetComponentInChildren<TextMeshProUGUI>().text = noText;
            noButton.onClick.AddListener(noEvent);
            noButton.gameObject.SetActive(true);
        }
        else
        {
            noButton.gameObject.SetActive(false);
        }

        this.question.text = question;
        this.title.text = title;
    }

    private void ClosePanel()
    {
        Time.timeScale = 1.0f;
        modalPanelObject.SetActive(false);
    }
}
