using Assets.Scripts.Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOverlayScript : MonoBehaviour
{
    public List<GameObject> exerPanels;
    public TextMeshProUGUI txtTitle;
    public Button btnSubmit;
    public Button btnSave;

    private GameObject activePanel = null;
    private Exercise activeExercise;

    private void Awake()
    {
        SetActiveState(false);
    }

    public void OnBack()
    {
        UserPanelScript.Instance?.OverrideBackStack(false);
        this.gameObject.SetActive(false);
        SetActiveState(false);
    }

    public async void OnSubmit()
    {
        var fields = activePanel.GetComponentsInChildren<InputField>();

        GameManager.Instance.CurrentActiveExerciseAnswer.Answers.Clear();

        foreach (var field in fields)
        {
            field.interactable = false;
            GameManager.Instance.CurrentActiveExerciseAnswer.Answers.Add(field.text);
        }

        try
        {
            GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted = true;
            GameManager.Instance.CurrentActiveExerciseAnswer.SubmitTime = DateTime.Now;
            await ExerciseAnswerDatabase.UpdateExerciseAnswer(GameManager.Instance.CurrentActiveExerciseAnswer);

            ModalPanel.Instance.ShowModalOK("Success", "Answers has been submitted successfully", OnBack);
        }
        catch (AggregateException e)
        {
            ModalPanel.Instance.ShowModalOK("Failed", "Failed to submit answer. ERR: REMOTE_FAIL");
            Debug.LogError("SaveLab failed: " + FirebaseFunctions.GetFirebaseErrorMessage(e));

            foreach (var field in fields)
            {
                field.interactable = true;
            }
        }
    }

    public async void OnSave()
    {
        if (!GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted)
        {
            var fields = activePanel.GetComponentsInChildren<InputField>();

            GameManager.Instance.CurrentActiveExerciseAnswer.Answers.Clear();

            foreach (var field in fields)
            {
                field.interactable = false;
                GameManager.Instance.CurrentActiveExerciseAnswer.Answers.Add(field.text);
            }

            try
            {
                GameManager.Instance.CurrentActiveExerciseAnswer.SubmitTime = DateTime.Now;
                await ExerciseAnswerDatabase.UpdateExerciseAnswer(GameManager.Instance.CurrentActiveExerciseAnswer);

                ModalPanel.Instance.ShowModalOK("Success", "Answers has been saved successfully", OnBack);
            }
            catch (AggregateException e)
            {
                ModalPanel.Instance.ShowModalOK("Failed", "Failed to save answer. ERR: REMOTE_FAIL");
                Debug.LogError("SaveLab failed: " + FirebaseFunctions.GetFirebaseErrorMessage(e));

                foreach (var field in fields)
                {
                    field.interactable = true;
                }
            }
        }
    }

    public void LoadAnswers(Exercise exercise,  ExerciseAnswer answer, bool isReadOnly = true)
    {
        Debug.Log($"Start AnswerOverlayScript_LoadAnswers, exercise={exercise.ID} answer={answer.ID}");
        int activity;

        if (int.TryParse(exercise.ExperimentID, out activity))
        {
            LoadOverlay(activity, isReadOnly);
            var fields = activePanel.GetComponentsInChildren<InputField>();

            for (int i = 0; i < answer.Answers.Count; i++)
            {
                try
                {
                    fields[i].text = answer.Answers[i];
                }
                catch
                {
                    Debug.LogWarning("Answer fields are not filled up. Terminating fill-up process");
                    break;
                }
            }
        }
        else
        {
            throw new FormatException("Invalid experiment id " + exercise.ExperimentID);
        }
    }

    public void LoadOverlay(Exercise exercise)
    {
        activeExercise = exercise;
        LoadAnswers(exercise, GameManager.Instance.CurrentActiveExerciseAnswer, false);
    }

    private void LoadOverlay(int activity, bool readOnly = false)
    {
        Debug.Log($"Start AnswerOverlayScript_LoadOverlay, activity={activity} readOnly={readOnly}");
        this.gameObject.SetActive(true);
        SetActiveState(false);

        UserPanelScript.Instance?.OverrideBackStack(true);

        btnSave.interactable = !readOnly;
        btnSubmit.interactable = !readOnly;

        if (exerPanels.Count > 0 && activity >= 0 && activity < exerPanels.Count)
        {
            activePanel = exerPanels[activity - 1];

            var fields = activePanel.GetComponentsInChildren<InputField>();

            foreach(var field in fields)
            {
                field.interactable = !readOnly;
                field.placeholder.GetComponent<Text>().text = readOnly ? "" : "Enter answer here...";
            }

            activePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to load answer overlay. No panels were set");
        }
    }

    private void SetActiveState(bool active)
    {
        foreach(var panel in exerPanels)
        {
            panel.SetActive(active);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }

        var span = DateTime.Now - GameManager.Instance.CurrentActiveExerciseAnswer.StartTime;
        var remainingTime = (GameManager.Instance.CurrentActiveExercise.TimeLimit - (span.TotalMinutes));

        if (remainingTime >= 0)
        {
            if (remainingTime < 1)
            {
                txtTitle?.SetText("Time Remaining: " + (int)(remainingTime * 60) + " seconds");
            }
            else
            {
                txtTitle?.SetText("Time Remaining: " + (int)remainingTime + " minutes");
            }
        }
        else
        {
            txtTitle?.SetText("Time Remaining: Time's up");
            if (!GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted)
            {
                GameManager.Instance.CurrentActiveExerciseAnswer.IsSubmitted = true;
                ModalPanel.Instance.ShowModalOK("Time's Up", "The allotted time for this exercise has expired. Unsaved answer will be submitted", OnSubmit);
            }
        }
    }
}
