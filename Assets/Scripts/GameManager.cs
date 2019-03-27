using Assets.Scripts.Firebase.Database;
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
    public ExerciseAnswer CurrentActiveExerciseAnswer { get; private set; }

    public async void LoadLabActivity(int activity, LabClass labClass)
    {
        Debug.Log($"Start LoadLabActivity, activity={activity} labClass={labClass.ID}");
        CurrentLabActivity = activity;
        CurrentActiveClass = labClass;
        var exers = (await ClassDatabase.GetLabClassExercisesAsync(labClass)) as List<Exercise>;
        CurrentActiveExercise = exers.Find(e => e.ExperimentID == activity.ToString());

        var student = FirebaseAuthManager.instance.GetStudentInfo();
        CurrentActiveExerciseAnswer = await ExerciseAnswerDatabase.GetExerciseAnswer(FirebaseAuthManager.instance.ActiveUserInfo, CurrentActiveExercise);
        if (CurrentActiveExerciseAnswer == null)
        {
            CurrentActiveExerciseAnswer = new ExerciseAnswer()
            {
                ExerciseID = CurrentActiveExercise.ID,
                UserID = FirebaseAuthManager.instance.ActiveUserInfo.ID,
            };
            CurrentActiveExerciseAnswer.ID = await ExerciseAnswerDatabase.UpdateExerciseAnswer(CurrentActiveExerciseAnswer);
        }

        Debug.Log($"Set active exercise to {CurrentActiveExercise.ID}");

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
