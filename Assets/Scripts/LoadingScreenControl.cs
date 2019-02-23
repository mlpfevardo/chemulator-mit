using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour {


    public GameObject loadingScreenObject;
    public Slider slider;

    AsyncOperation async;

    public void LoadScreenSample(int sceneNumber)
    {
        StartCoroutine(LoadingScreen(sceneNumber));
    }

    IEnumerator LoadingScreen(int sceneNumber)
    {
        async = SceneManager.LoadSceneAsync(sceneNumber);
        loadingScreenObject.SetActive(true);
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = async.progress;
        }
        yield return null;
    }
}
