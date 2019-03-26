using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditGradePanelScript : MonoBehaviour
{
    public TextMeshProUGUI studentText;
    public InputField inputGrade;
    public Button btnSubmit;
    public Button btnBack;
    public Button btnViewAnswers;

    private Student currentStudent;
    private StudentGrade currentStudentGrade;
    private Exercise currentExercise;
    private LabClass targetClass;

    public async Task LoadAsync(Student student, LabClass lab, Exercise exercise, StudentGrade studentGrade)
    {
        currentStudent = student;
        currentExercise = exercise;
        currentStudentGrade = studentGrade;
        targetClass = lab;
        inputGrade.text = studentGrade.Score.ToString();
        studentText.SetText("Grade of " + student.UserInfo.ToString());
    }

    public async void OnSubmit()
    {
        if (string.IsNullOrEmpty(inputGrade.text))
        {
            ModalPanel.Instance.ShowModalOK("Invalid Grade", "Grade should be a valid number");
            return;
        }
        else
        {
            SetInteractability(false);

            try
            {
                double d;

                if (double.TryParse(inputGrade.text, out d))
                {
                    currentStudentGrade.Score = d;
                    await GradeDatabase.UpdateGradeAsync(currentStudentGrade);
                    ModalPanel.Instance.ShowModalOK("Success", "Grade Successfully Saved", () => SetInteractability(true));
                }
                else
                {
                    ModalPanel.Instance.ShowModalOK("Unsuccessful", "Unable to submit grade due to invalid data");
                }
            }
            catch (AggregateException e)
            {
                ModalPanel.Instance.ShowModalOK("Error", FirebaseFunctions.GetFirebaseErrorMessage(e));
            }
        }
    }

    public void OnViewAnswers()
    {

    }

    public void OnBack()
    {
        GradesPanelScript.Instance.LoadGradeInfo(currentStudent, targetClass);
    }

    private void SetInteractability(bool interactable)
    {
        inputGrade.interactable = interactable;
        btnSubmit.interactable = interactable;
        btnBack.interactable = interactable;
        btnViewAnswers.interactable = interactable;
    }
}
