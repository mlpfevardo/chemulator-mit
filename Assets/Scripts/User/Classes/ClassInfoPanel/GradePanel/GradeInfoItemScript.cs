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
    private Student currentStudent;
    private StudentGrade currentGrade;
    private Exercise currentExercise;

    public async Task<double> LoadAsync(Student student, LabClass labClass, Exercise exercise)
    {
        Debug.Log($"Start GradeInfoItemScript, student={student?.ID} exercise={exercise?.ID}");

        currentExercise = exercise;
        txtTitle.SetText(exercise.Name);
        txtGrade.SetText("Grade: 0");
        Setup(student, labClass);

        currentGrade = await GradeDatabase.GetGradeInfoAsync(student, exercise);
        txtGrade.SetText("Grade: " + currentGrade.Score.ToString());

        return currentGrade.Score;
    }

    private void Setup(Student student, LabClass labClass)
    {
        currentLab = labClass;
        currentStudent = student;

        if (FirebaseAuthManager.instance.IsInstructor())
        {
            btnEdit.gameObject.SetActive(FirebaseAuthManager.instance.GetInstructorInfo()?.ID == labClass.InstructorID);

            btnEdit.onClick.RemoveAllListeners();
            btnEdit.onClick.AddListener(() =>
            {
                GradesPanelScript.Instance.LoadEditGrade(currentStudent, currentLab, currentExercise, currentGrade);
            });
        }
        else
        {
            btnEdit.gameObject.SetActive(false);
        }
    }
}
