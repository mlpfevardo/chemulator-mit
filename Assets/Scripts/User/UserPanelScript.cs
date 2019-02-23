using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class UserPanelScript : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public GameObject welcomePanel;
    public GameObject simulationPanel;
    // Use this for initialization
    void Start()
    {
    }

    private void Awake()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            nameText.SetText(string.IsNullOrEmpty(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName) ?
                FirebaseAuth.DefaultInstance.CurrentUser.Email :
                FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        }
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.Login, true);
    }

    public void OnStartSimulationButtonClick()
    {
        welcomePanel.SetActive(false);
        simulationPanel.SetActive(true);
    }

    public void StartActivity(int id)
    {
        GameManager.Instance.LoadLabActivity(id);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (simulationPanel.activeSelf)
            {
                BackToWelcomePanel();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void BackToWelcomePanel()
    {
        welcomePanel.SetActive(true);
        simulationPanel.SetActive(false);
    }
}
