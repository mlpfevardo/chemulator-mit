﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupHandler : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {

        if (FirebaseManager.instance == null || FirebaseAuthManager.instance == null)
        {
            Debug.Log("Firebase not initialized");
        }
        else
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        Camera.main.gameObject.SetActive(false);        

        if (FirebaseAuthManager.instance.IsAuthenticated())
        {
            Debug.Log("Authenticated");
        }
        else
        {
            Debug.Log("Not authenticated");
            //GameManager.Instance.LoadLabActivity(5);
            SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.Login, true);
        }
    }
}
