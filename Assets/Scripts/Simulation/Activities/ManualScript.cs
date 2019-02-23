using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManualScript : MonoBehaviour
{
    public static ManualScript instance = null;
    public TextMeshProUGUI text;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public void ShowManual()
    {
        gameObject.SetActive(true);
        text.SetText(SimulationManager.instance.ActiveActivity.GetInstructions());
        Time.timeScale = 0f;
    }

    public void CloseManual()
    {
        text.SetText("");
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameObject.activeSelf)
            {
                CloseManual();
                if (RuntimePlatform.Android == Application.platform)
                {
                    PauseManager.block = true;
                }
            }
            Input.ResetInputAxes();
        }
    }
}

