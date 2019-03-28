using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OnLoginButtonClick : MonoBehaviour {

    public InputField emailField;
    public InputField passwordField;
    public Text messageText;
    public LoginPanelScript panelScript;

	public async void OnClick()
    {
        messageText.text = "";

        if (FirebaseAuthManager.instance != null)
        {
            panelScript.ToggleInteractability();

            if (!string.IsNullOrEmpty(emailField.text) && !string.IsNullOrEmpty(passwordField.text))
            {
                FirebaseAuthManager.instance.email = emailField.text;
                FirebaseAuthManager.instance.password = passwordField.text;
                
                var task = FirebaseAuthManager.instance.SigninWithEmailAsync();
                if (await Task.WhenAny(Task.Delay(30000), task) == task)
                {
                    var result = await task;

                    if (result.IsSuccessful)
                    {
                        Debug.Log("Login success " + result.Message);
                        SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.User, true);
                    }
                    else
                    {
                        Debug.Log("Login failed: " + result.Message);
                        if (result.Message.Contains("network error"))
                        {
                            messageText.text = "Network unavailable. Try again later";
                        }
                        else
                        {
                            messageText.text = "Invalid email or password";
                        }
                    }
                }
                else
                {
                    messageText.text = "Network timeout";
                }
            }
            else
            {
                Debug.Log("Empty fields");
                messageText.text = "Email and password cannot be empty";
            }

            panelScript.ToggleInteractability();
        }
        else
        {
            Debug.Log("Firebase not loaded onclick");
            messageText.text = "Network unavailable. Try again later";
        }
    }
}
