using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public const string APPTITLE = "Chemulator";
    public const string APPVERSION = "0.1";

    [Header("UI Pages")]
    public RectTransform loginUI;
    public RectTransform signUpUI;

    [Header("Login UI")]
    public Text txtTitle;
    public Text txtVersion;
    public InputField inputEmailField;
    public InputField inputPasswordField;
    public Button btnLogin;
    public Button btnSignup;
    public Button btnExit;

    public enum UIScenes
    {
        None,
        Login,
        SignUp
    }

    private void Start()
    {
        txtVersion.text = "v" + APPVERSION;
        txtTitle.text = APPTITLE;

        //if (!AndroidNativeFunctions.isConnectInternet()) {
        //    AndroidNativeFunctions.ShowAlert("Unable to connect to the network.", "Network unavailable", null, null, "Ok", null);
        //}

        if (FirebaseManager.instance == null || FirebaseAuthManager.instance == null)
        {
            Debug.Log("Firebase not initialized");
        }
        else
        {
            Initialize();
        }
    }

    void Initialize()
    {
        Debug.Log("Firebase loaded");
        //FirebaseAuthManager.instance.API = this;
    }

    public void ShowError(string text)
    {
        
    }

    private void ShowUITextDialog(GameObject UI, string text)
    {

    }

    private void SignIn()
    {
        //btnLogin.interactable = false;
        //btnSignup.interactable = false;

        if (!string.IsNullOrEmpty(inputEmailField.text) && !string.IsNullOrEmpty(inputPasswordField.text))
        {
            FirebaseAuthManager.instance.email = inputEmailField.text;
            FirebaseAuthManager.instance.password = inputPasswordField.text;
            inputPasswordField.text = "";
            FirebaseAuthManager.instance.SigninWithEmailAsync();
        }
        else
        {
            Debug.Log("Empty login fields");
            btnLogin.interactable = true;
            btnSignup.interactable = true;
        }
    }

    public void SignInFinished(bool value, string output)
    {
        if (value == true)
        {
            Debug.Log("Signed in with " + output);
        }
        else
        {
            Debug.Log("Failed to sign in");
        }
    }

    private void RegisterSwitchPage()
    {
        SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.Register);
    }

    private void Update()
    {
        btnLogin.interactable = FirebaseAuthManager.instance.IsUIEnabled();
        btnSignup.interactable = FirebaseAuthManager.instance.IsUIEnabled();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    doGetPreviousScene();
        //}
    }

    public void SetScreen(UIScenes scene, bool addToStack)
    {
        if (FirebaseAuthManager.instance == null)
        {
            Debug.LogError("Failed to load scene. Auth Manager not found");
            return;
        }
    }

    public void SignOutFinished()
    {
        Debug.Log("Sign out success");
    }

    public void SignUpFinished(bool value, string message)
    {
        if (value)
        {
            Debug.Log("User created: " + message);
        }
        else
        {
            Debug.Log("Failed to create user: " + message);
        }
    }

    #region Buttons
    public void doSignIn()
    {
        SignIn();
    }

    public void doSwitchToSignup()
    {
        Debug.Log("Switching to Register");
        RegisterSwitchPage();
    }
    #endregion
}
