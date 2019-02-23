using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStorageManager : MonoBehaviour {

    //public static SceneStorageManager instance = null;
    private static SceneStorageManager instance;

    private List<Scenes> scenes = new List<SceneStorageManager.Scenes>();
    private string nextScene;
    private bool resetNextScene = false;
    private bool isLoading = false;

    public static SceneStorageManager Instance
    {
        get
        {
            if (!instance)
            {
                var obj = new GameObject("SceneManager");
                instance = obj.AddComponent<SceneStorageManager>();
                Debug.Log("Instance");
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    public enum Scenes
    {
        None,
        Login,
        Register,
        Simulation,
        User
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public void ChangeScene(SceneStorageManager.Scenes scene, bool reset = false)
    {
        resetNextScene = reset;
        if (reset)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            scenes.Clear();
        }
        scenes.Add(scene);

        LoadScene(scene);
    }

    public void LoadPreviousScene(bool quitOnEnd = true)
    {
        SceneStorageManager.Scenes previousScene = SceneStorageManager.Scenes.None;
        string sceneName = null;

        if (scenes.Count > 1)
        {
            previousScene = scenes[scenes.Count - 2];
            scenes.RemoveAt(scenes.Count - 1);
        }
        else
        {
            if (quitOnEnd)
            {
                Debug.Log("Exiting application");
                if (!Application.isEditor)
                {
                    Application.Quit();
                }
                return;
            }
            else
            {
                previousScene = scenes[0];
            }
        }

        sceneName = GetSceneName(previousScene);
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        Debug.Log("Loading previous scene " + previousScene);
        LoadScene(previousScene);
    }

    private void LoadScene(SceneStorageManager.Scenes scene, bool reset = false)
    {
        if (isLoading)
        {
            Debug.Log("Currently switching please wait");
            return;
        }

        string sceneName = GetSceneName(scene);
        Debug.Log("Loaded scene " + sceneName);

        if (!string.IsNullOrEmpty(sceneName))
        {
            resetNextScene = reset;

            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            isLoading = true;

            async.completed += (obj) =>
            {
                isLoading = false;
                Debug.Log("loaded");
                //if (resetNextScene)
                //{

                //}
                //var asOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                //asOp.completed += (b) =>
                //{
                //    isLoading = false;
                //    SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
                //    Debug.Log("Loaded");
                //    nextScene = null;
                //};
            };

            //if (reset)
            //{
            //    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            //}
            //else
            //{
            //    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //}
            //async.completed += Async_completed;

            ////async.allowSceneActivation = false;
        }
    }

    //private void Async_completed(AsyncOperation obj)
    //{
    //    Scene nextScene = SceneManager.GetSceneByName(loadingScene);

    //    if (resetNextScene)
    //    {
    //        Scene active = SceneManager.GetActiveScene();
    //        activeOperation = SceneManager.UnloadSceneAsync(active);
    //    }

    //    SceneManager.SetActiveScene(nextScene);
    //    loadingScene = null;
    //    Debug.Log("Scene loaded");
    //}

    private void Update()
    {
        //Scene nextScene = SceneManager.GetSceneByName(loadingScene);

        //if (nextScene.isLoaded)
        //{
            
        //}
    }

    private string GetSceneName(SceneStorageManager.Scenes scene)
    {
        nextScene = null;
        switch (scene)
        {
            case Scenes.Login:
                nextScene = "LoginPanel";
                break;
            case Scenes.Register:
                nextScene = "SignupPanel";
                break;
            case Scenes.Simulation:
                nextScene = "SimulationPanel";
                break;
            case Scenes.User:
                nextScene = "UserPanel";
                break;
        }

        return nextScene;
    }
}
