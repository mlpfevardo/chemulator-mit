using Assets.Scripts.Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradeInfoItemScript : MonoBehaviour
{
    public Button btnEdit;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtGrade;

    private LabClass currentLab;
    private UserInfo currentUser;
    private StudentGrade currentGrade;
    private Exercise currentExercise;

    public async Task<double> LoadAsync(UserInfo user, LabClass labClass, Exercise exercise)
    {
        Debug.Log($"Start GradeInfoItemScript, user={user?.ID} exercise={exercise?.ID}");

        currentExercise = exercise;
        txtTitle.SetText(exercise.Name);
        txtGrade.SetText("Grade: 0");
        Setup(user, labClass);

        currentGrade = await GradeDatabase.GetGradeInfoAsync(user, exercise);
        txtGrade.SetText("Grade: " + currentGrade.Score.ToString());

        return currentGrade.Score;
    }

    private void Setup(UserInfo user, LabClass labClass)
    {
        currentLab = labClass;
        currentUser = user;

        if (FirebaseAuthManager.instance.IsInstructor())
        {
            btnEdit.gameObject.SetActive(FirebaseAuthManager.instance.GetInstructorInfo()?.ID == labClass.InstructorID);

            btnEdit.onClick.RemoveAllListeners();
            btnEdit.onClick.AddListener(() =>
            {
                GradesPanelScript.Instance.LoadEditGrade(currentUser, currentLab, currentExercise, currentGrade);
            });
        }
        else
        {
            btnEdit.gameObject.SetActive(false);
        }
    }
}
