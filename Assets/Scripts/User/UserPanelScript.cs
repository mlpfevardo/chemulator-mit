using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using System.Threading.Tasks;

public class UserPanelScript : MonoBehaviour
{
    public static UserPanelScript Instance { get; private set; }
    public TextMeshProUGUI nameText;
    public GameObject welcomePanel;
    public GameObject simulationPanel;
    public GameObject enrollPanel;
    public GameObject classesPanel;
    public GameObject createClassPanel;

    public Button buttonEnrollClass;
    public Button buttonCreateClass;

    private bool hasRun = false;
    private bool defaultBackStack = true;

    private void Awake()
    {
        Instance = this;

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            nameText.SetText(FirebaseAuthManager.instance.ActiveUserInfo.ToString());

            switch(FirebaseAuthManager.instance.ActiveUserInfo.UserType)
            {
                case UserType.Student:
                    buttonEnrollClass.gameObject.SetActive(true);
                    buttonCreateClass.gameObject.SetActive(false);
                    break;
                case UserType.Instructor:
                    buttonEnrollClass.gameObject.SetActive(false);
                    buttonCreateClass.gameObject.SetActive(true);
                    break;
            }
        }

        if (!hasRun)
        {
            BackToWelcomePanel();
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

        simulationPanel.GetComponent<SimulationPanelScript>().LoadAsync();
    }

    public void OnEnrollButtonClick()
    {
        welcomePanel.SetActive(false);
        enrollPanel.SetActive(true);

        enrollPanel.GetComponent<ILoadableClass>().LoadAsync();
    }

    public void OnCreateClassButtonClick()
    {
        welcomePanel.SetActive(false);
        createClassPanel.SetActive(true);

        createClassPanel.GetComponent<CreateClassPanelScript>().LoadRoot();
    }

    public void OnViewClassButtonClick()
    {
        welcomePanel.SetActive(false);
        classesPanel.SetActive(true);

        classesPanel.GetComponent<ClassesPanelScript>().LoadAllClasses();
    }

    public void StartActivity(int id, LabClass labClass)
    {
        GameManager.Instance.LoadLabActivity(id, labClass);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && defaultBackStack)
        {
            if (!welcomePanel.activeSelf)
            {
                BackToWelcomePanel();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void OverrideBackStack(bool b)
    {
        defaultBackStack = b;
    }

    public void BackToWelcomePanel()
    {
        welcomePanel.SetActive(true);
        simulationPanel.SetActive(false);
        enrollPanel.SetActive(false);
        classesPanel.SetActive(false);
        createClassPanel.SetActive(false);
    }
}
