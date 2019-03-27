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

    private GameObject activePanel = null;

    private void Awake()
    {
        SetActiveState(false);
    }

    public void OnBack()
    {
        this.gameObject.SetActive(false);
        SetActiveState(false);
    }

    public async void OnSubmit()
    {
        var fields = activePanel.GetComponentsInChildren<InputField>();
        var answer = new ExerciseAnswer();
        

        foreach (var field in fields)
        {
            field.interactable = false;
        }
    }

    public void OnSave()
    {

    }

    public void LoadAnswers(Exercise exercise,  ExerciseAnswer answer)
    {
        Debug.Log($"Start AnswerOverlayScript_LoadAnswers, exercise={exercise.ID} answer={answer.ID}");
        int activity;

        if (int.TryParse(exercise.ExperimentID, out activity))
        {
            LoadOverlay(activity, true);
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

    public void LoadOverlay(Exercise exercise, Student student)
    {
        LoadOverlay(GameManager.Instance.CurrentLabActivity - 1);
    }

    private void LoadOverlay(int activity, bool readOnly = false)
    {
        Debug.Log($"Start AnswerOverlayScript_LoadOverlay, activity={activity} readOnly={readOnly}");
        this.gameObject.SetActive(true);
        SetActiveState(false);

        if (exerPanels.Count > 0 && activity >= 0 && activity < exerPanels.Count)
        {
            activePanel = exerPanels[activity];

            var fields = activePanel.GetComponentsInChildren<InputField>();

            foreach(var field in fields)
            {
                field.interactable = !readOnly;
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
}
