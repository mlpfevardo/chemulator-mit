using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateExercisePanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public TextMeshProUGUI textLabName;
    public TextMeshProUGUI textTitle;
    public InputField inputName;
    public InputField inputAttempts;
    public InputField inputTimeLimit;
    public InputField inputInstructions;
    public Button btnCreate;
    public Button btnCancel;
    public Text txtMessage;

    public GameObject firstPagePanel;
    public GameObject secondPagePanel;
    public GameObject thirdPagePanel;

    private LabClass activeLab;
    private int attempts;
    private int timelimit;

    private Exercise activeExercise;

    public async Task LoadAsync(LabClass lab)
    {
        firstPagePanel.SetActive(true);
        secondPagePanel.SetActive(false);
        thirdPagePanel.SetActive(false);

        inputName.text = string.Empty;
        inputAttempts.text = string.Empty;
        inputTimeLimit.text = string.Empty;
        inputInstructions.text = string.Empty;

        activeLab = lab;
        activeExercise = null;

        textLabName.SetText(lab.Name);
        textTitle.SetText("Create Exercise");

        UserPanelScript.Instance.OverrideBackStack(true);
    }

    public async Task LoadAsync(Exercise exercise)
    {
        activeExercise = exercise;

        firstPagePanel.SetActive(true);
        secondPagePanel.SetActive(false);
        thirdPagePanel.SetActive(false);

        inputName.text = exercise.Name;
        inputAttempts.text = exercise.MaxAttempts.ToString();
        inputTimeLimit.text = exercise.TimeLimit.ToString();
        inputInstructions.text = exercise.Instructions;

        textLabName.SetText(exercise.Name);
        textTitle.SetText("Edit Exercise");

        activeLab = null;
        activeExercise = exercise;

        UserPanelScript.Instance.OverrideBackStack(true);
    }

    public void OnCancel()
    {
        ModalPanel.Instance.ShowModalYesNo("Confirm", "This exercise will not be created. Are you sure you want to cancel?", GoBack, () => { });
    }

    public void OnBackFirstPage()
    {
        txtMessage.text = string.Empty;
        secondPagePanel.SetActive(false);
        firstPagePanel.SetActive(true);
    }

    public void OnBackSecondPage()
    {
        txtMessage.text = string.Empty;
        thirdPagePanel.SetActive(false);
        secondPagePanel.SetActive(true);
    }

    public void OnGoSecondPage()
    {
        txtMessage.text = string.Empty;
        if (string.IsNullOrEmpty(inputName.text) || 
            string.IsNullOrEmpty(inputAttempts.text) ||
            string.IsNullOrEmpty(inputTimeLimit.text))
        {
            txtMessage.text = "Please fill up all fields";
            return;
        }

        if (!int.TryParse(inputAttempts.text, out attempts) || !int.TryParse(inputTimeLimit.text, out timelimit))
        {
            txtMessage.text = "Please enter a valid number for Max Attempts and Time Limit";
            return;
        }

        if (attempts <= 0 || timelimit <= 0)
        {
            txtMessage.text = "Please enter a valid number for Max Attempts and Time Limit";
            return;
        }

        firstPagePanel.SetActive(false);
        secondPagePanel.SetActive(true);
    }

    public void OnGoThirdPage()
    {
        txtMessage.text = string.Empty;
        if (string.IsNullOrEmpty(inputInstructions.text))
        {
            txtMessage.text = "Instructions cannot be empty";
            return;
        }

        secondPagePanel.SetActive(false);
        thirdPagePanel.SetActive(true);
        SetInteractability(true);
    }

    public async void SubmitForm()
    {
        SetInteractability(false);

        try
        {
            if (activeExercise == null)
            {
                await ExerciseDatabase.RegisterExercise(new Exercise
                {
                    ClassID = activeLab.ID,
                    MaxAttempts = attempts,
                    TimeLimit = timelimit,
                    Name = inputName.text,
                    Instructions = inputInstructions.text,
                });

                ModalPanel.Instance.ShowModalOK("Exercise Created", "The '" + inputName.text + "' exercise has been successfully created", GoBack);
            }
            else
            {
                await ExerciseDatabase.RegisterExercise(new Exercise
                {
                    ClassID = activeExercise.ClassID,
                    MaxAttempts = attempts,
                    TimeLimit = timelimit,
                    Name = inputName.text,
                    Instructions = inputInstructions.text,
                    ID = activeExercise.ID,
                });

                ModalPanel.Instance.ShowModalOK("Exercise Update", "This exercise has been successfully updated", GoBack);
            }
        }
        catch (AggregateException e)
        {
            txtMessage.text = FirebaseFunctions.GetFirebaseErrorMessage(e);
        }

        SetInteractability(true);
    }

    private async void GoBack()
    {
        UserPanelScript.Instance.OverrideBackStack(false);
        if (activeExercise == null)
        {
            await ExercisesPanelScript.Instance.LoadAsync(activeLab);
        }
        else
        {
            ExercisesPanelScript.Instance.LoadExerciseInfo(activeExercise);
        }
    }

    private void SetInteractability(bool interactable)
    {
        btnCreate.gameObject.SetActive(interactable);
        btnCancel.gameObject.SetActive(interactable);
        inputInstructions.gameObject.SetActive(interactable);
    }
}
