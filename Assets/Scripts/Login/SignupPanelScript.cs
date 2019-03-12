using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;

public class SignupPanelScript : MonoBehaviour {

    public GameObject SignupPanel;
    public GameObject LoginPanel;
    public Text txtMessage;
    
    public InputField inputFirstName;
    public InputField inputLastName;
    public InputField inputEmail;
    public InputField inputPassword;
    public Dropdown ddMonth;
    public Dropdown ddDay;
    public Dropdown ddYear;
    public ToggleGroup tgUserType;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SignupPanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
    }

    public void OnClickRegisterButton()
    {
        txtMessage.text = string.Empty;

        if (string.IsNullOrEmpty(inputFirstName.text) || 
            string.IsNullOrEmpty(inputEmail.text) ||
            string.IsNullOrEmpty(inputPassword.text))
        {
            txtMessage.text = "Please fill up all fields";
            return;
        }

        if (inputPassword.text.Length < 6)
        {
            txtMessage.text = "Password should be at least 6 characters";
            return;
        }

        ProcessRegistration();
    }

    private async void ProcessRegistration()
    {
        FirebaseAuthManager.instance.email = inputEmail.text;
        FirebaseAuthManager.instance.password = inputPassword.text;
        var result = await FirebaseAuthManager.instance.CreateUser();
        
        if (result)
        {
            //Debug.Log("COMPLETE");
            //FirebaseDatabase.DefaultInstance.GetReference("Users")
            Debug.Log("TODO: Save to database");

        }
        else
        {
            Debug.Log("Failed to register user");
        }
    }
}
