using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerOverlayScript : MonoBehaviour
{
    public List<GameObject> exerPanels;

    private GameObject activePanel = null;

    private void Awake()
    {
        SetActiveState(false);
    }

    public void LoadOverlay()
    {
        Debug.Log("Start AnswerOverlayScript_LoadOverlay");
        this.gameObject.SetActive(true);
        SetActiveState(false);

        if (exerPanels.Count > 0)
        {
            activePanel = exerPanels[GameManager.Instance.CurrentLabActivity - 1];
            activePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to load answer overlay. No panels were set");
        }
    }

    private void SetActiveState(bool active)
    {
        foreach(var panel in exerPanels)
        {
            panel.SetActive(active);
        }
    }
}
