using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using TMPro;
using Assets.Scripts.Firebase.Database;

public class SignupPanelScript : MonoBehaviour {

    public GameObject SignupPanel;
    public GameObject LoginPanel;
    public GameObject SuccessPanel;
    public GameObject SignupFormPanel;
    public Text txtMessage;
    
    public InputField inputFirstName;
    public InputField inputLastName;
    public InputField inputEmail;
    public InputField inputPassword;
    public Dropdown ddMonth;
    public Dropdown ddDay;
    public Dropdown ddYear;
    public Toggle toggleStudent;
    public Toggle toggleInstructor;
    public RegistrationSuccessScript successPanel;

    private bool isBusy = false;

    private void Start()
    {
        SuccessPanel.SetActive(false);
        SignupFormPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !isBusy)
        {
            ReturnToLogin();
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
        Debug.Log("Start ProcessRegistration");
        FirebaseAuthManager.instance.email = inputEmail.text;
        FirebaseAuthManager.instance.password = inputPassword.text;
        isBusy = true;

        try
        {
            var result = await FirebaseAuthManager.instance.CreateUser();

            if (result != null)
            {
                Debug.Log("User registered success.");
                Debug.Log("Start database update");

                try
                {
                    var dt = DateTime.Parse((ddMonth.value + 1) + "/" + (ddDay.value + 1) + "/" + ddYear.captionText.text);

                    var user = new UserInfo
                    {
                        ID = result.UserId,
                        firstName = inputFirstName.text,
                        lastName = inputLastName.text,
                        email = inputLastName.text,
                        birthday = new UserBirthday
                        {
                            day = dt.Day,
                            month = dt.Month,
                            year = dt.Year
                        },
                        userType = toggleStudent.isOn ? UserType.Student : UserType.Instructor,
                    };

                    SuccessPanel.SetActive(true);
                    SignupFormPanel.SetActive(false);
                    successPanel.ShowLoadingMessage();

                    //await FirebaseDatabaseManager.Instance.RegisterUser(user);
                    await UserDatabase.RegisterUser(user);
                }
                catch (AggregateException e)
                {
                    txtMessage.text = GetFirebaseErrorMessage(e);
                    SuccessPanel.SetActive(false);
                    SignupFormPanel.SetActive(true);

                    try
                    {
                        await result.DeleteAsync();
                    }
                    catch (AggregateException ex)
                    {
                        Debug.Log("Unable to delete user with failed registration");
                        Debug.LogError(GetFirebaseErrorMessage(ex));
                    }

                    isBusy = false;
                    return;
                }
                catch (FormatException e)
                {
                    txtMessage.text = "An error occurred while processing the data.";
                    Debug.LogError(e.Message);

                    SuccessPanel.SetActive(false);
                    SignupFormPanel.SetActive(true);

                    try
                    {
                        await result.DeleteAsync();
                    }
                    catch (AggregateException ex)
                    {
                        Debug.Log("Unable to delete user with failed registration");
                        Debug.LogError(GetFirebaseErrorMessage(ex));
                    }

                    isBusy = false;
                    return;
                }

                FirebaseAuthManager.instance.SignOut();

                successPanel.ShowSuccessMessage();

                await Task.Delay(2500);
                ReturnToLogin();
            }
            else
            {
                Debug.Log("Failed to register user");
            }
        }
        catch(AggregateException e)
        {
            txtMessage.text = GetFirebaseErrorMessage(e);
        }

        isBusy = false;
    }

    private string GetFirebaseErrorMessage(AggregateException e)
    {
        foreach (var exception in e.Flatten().InnerExceptions)
        {
            if (exception is Firebase.FirebaseException)
            {
                Debug.LogWarning("Firebase: " + exception.Message);
                return exception.Message;
            }
            else
            {
                Debug.LogError(exception.Message);
                return "An unexpected error occurred";
            }
        }

        return "Unknown error";
    }

    private void ReturnToLogin()
    {
        SignupPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
}
