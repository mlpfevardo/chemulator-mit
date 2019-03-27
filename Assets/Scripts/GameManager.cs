using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //public static GameManager instance = null;
    private static GameManager instance = null;

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

    public int CurrentLabActivity { get; private set; } = 1;

    public LabClass CurrentActiveClass { get; private set; }
    public Exercise CurrentActiveExercise { get; private set; }

    public void LoadLabActivity(int activity, LabClass labClass)
    {
        Debug.Log($"Start LoadLabActivity, activity={activity} labClass={labClass.ID}");
        CurrentLabActivity = activity;
        CurrentActiveClass = labClass;

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
