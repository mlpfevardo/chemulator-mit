using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelScript : MonoBehaviour
{
    public static LoginPanelScript instance = null;
    public Button loginBtn;
    public Button signupBtn;
    public InputField userField;
    public InputField passField;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleInteractability()
    {
        loginBtn.interactable = !loginBtn.interactable;
        signupBtn.interactable = !signupBtn.interactable;
        userField.interactable = !userField.interactable;
        passField.interactable = !passField.interactable;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneStorageManager.Instance.LoadPreviousScene();
    //    }
    //}
}
