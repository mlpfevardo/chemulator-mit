using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //public static GameManager instance = null;
    private static GameManager instance = null;

    private int requestedLabActivity = 1;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    public int CurrentLabActivity
    {
        get { return requestedLabActivity; }
    }

    public void LoadLabActivity(int activity)
    {
        requestedLabActivity = activity;

        SceneStorageManager.Instance.ChangeScene(SceneStorageManager.Scenes.Simulation, true);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
