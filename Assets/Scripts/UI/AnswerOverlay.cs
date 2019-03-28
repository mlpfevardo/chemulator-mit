using Assets.Scripts.Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class AnswerOverlay : MonoBehaviour
{
    private GameObject answerOverlayObject;

    private static AnswerOverlay instance;

    public static AnswerOverlay Instance
    {
        get
        {
            if(!instance)
            {
                instance = new GameObject("AnswerOverlay").AddComponent<AnswerOverlay>();
                var prefab = Resources.Load<GameObject>("Prefabs/AnswerOverlay");
                instance.answerOverlayObject = GameObject.Instantiate(prefab);
                instance.answerOverlayObject.transform.SetParent(instance.transform);

                DontDestroyOnLoad(instance);
                instance.answerOverlayObject.SetActive(false);
            }

            return instance;
        }
    }

    public async void LoadOverlay(Exercise exercise)
    {
        if (!GameManager.Instance.CurrentActiveExerciseAnswer.IsStarted)
        {
            ModalPanel.Instance.ShowModalYesNo("Not Yet Started", "Opening the answer sheet will start your timer for this exercise. Continue?", () =>
            {
                answerOverlayObject.GetComponent<AnswerOverlayScript>().LoadOverlay(exercise);
                GameManager.Instance.CurrentActiveExerciseAnswer.IsStarted = true;
                GameManager.Instance.CurrentActiveExerciseAnswer.StartTime = DateTime.Now;
                ExerciseAnswerDatabase.UpdateExerciseAnswer(GameManager.Instance.CurrentActiveExerciseAnswer);
                answerOverlayObject.SetActive(true);
            }, () => { });
        }
        else
        {
            if (!GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted)
            {
                var span = DateTime.Now - GameManager.Instance.CurrentActiveExerciseAnswer.StartTime;
                if (span.TotalMinutes >= exercise.TimeLimit)
                {
                    ModalPanel.Instance.ShowModalOK("Time's Up", "The allotted time for this exercise has expired. Unsaved answer will be submitted");
                    GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted = true;
                    GameManager.Instance.CurrentActiveExerciseAnswer.SubmitTime = DateTime.Now;
                    await ExerciseAnswerDatabase.UpdateExerciseAnswer(GameManager.Instance.CurrentActiveExerciseAnswer);
                }
                else
                {
                    answerOverlayObject.GetComponent<AnswerOverlayScript>().LoadAnswers(exercise, GameManager.Instance.CurrentActiveExerciseAnswer, false);
                    answerOverlayObject.SetActive(true);
                }
            }
            else
            {
                ModalPanel.Instance.ShowModalOK("Answer Submitted", "Your answers for this exercise have been submitted. You can no longer edit your answers");
            }
        }
        
    }

    public void LoadAnswers(Exercise exercise, ExerciseAnswer answer)
    {
        answerOverlayObject.SetActive(true);
        answerOverlayObject.GetComponent<AnswerOverlayScript>().LoadAnswers(exercise, answer);
    }

    //public void Close()
    //{
    //    answerOverlayObject.SetActive(false);
    //}
}
