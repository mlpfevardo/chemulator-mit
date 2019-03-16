using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Firebase.Database;
using TMPro;
using UnityEngine;

public class ExerciseInfoPanelScript : MonoBehaviour
{
    public TextMeshProUGUI txtMaxAttempt;
    public TextMeshProUGUI txtTimeLimit;
    public TextMeshProUGUI txtName;
    public GameObject btnEdit;
    public GameObject btnDelete;
    public GameObject btnViewAnswer;
    private Exercise activeExercise;

    public GameObject overviewPanel;
    public GameObject answerPanel;


    public async Task LoadAsync(Exercise exercise)
    {
        Debug.Log("Start ExerciseInfoPanel, exercise=" + exercise.ID);

        answerPanel.SetActive(false);
        overviewPanel.SetActive(true);

        activeExercise = exercise;
        txtMaxAttempt.text = $"Max Attempt: {exercise.MaxAttempts}";
        txtTimeLimit.text = $"Time Limit: {exercise.TimeLimit} minute(s)";
        txtName.text = exercise.Name;

        btnDelete.gameObject.SetActive(FirebaseAuthManager.instance.IsInstructor());
        btnEdit.gameObject.SetActive(FirebaseAuthManager.instance.IsInstructor());
    }

    public void OnEditButtonClick()
    {
        if (FirebaseAuthManager.instance.IsInstructor())
        {
            ExercisesPanelScript.Instance.LoadCreateExercisePanel(activeExercise);
        }
    }

    public void OnDeleteButtonClick()
    {
        ModalPanel.Instance.ShowModalYesNo("Delete", "Are you sure you want to delete this exercise? This action cannot be undone.", async () =>
        {
            try
            {
                await ExerciseDatabase.RemoveExercise(activeExercise);

                ModalPanel.Instance.ShowModalOK("Delete Success", "Exercise successfully deleted.", async () =>
                {
                    await ExercisesPanelScript.Instance.ReLoadAsync();
                });
            }
            catch(AggregateException e)
            {
                ModalPanel.Instance.ShowModalOK("Delete Failed", "Failed to delete this exercise: " + FirebaseFunctions.GetFirebaseErrorMessage(e));
            }
        }, () => { });
    }
}
