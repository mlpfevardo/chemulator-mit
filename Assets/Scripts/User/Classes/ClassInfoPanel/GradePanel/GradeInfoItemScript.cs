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

    public async Task LoadAsync(Student student, LabClass labClass, Exercise exercise)
    {
        Debug.Log($"Start GradeInfoItemScript, student={student?.ID} exercise={exercise?.ID}");

        txtTitle.SetText(exercise.Name);
        
        

        Setup(student, labClass);
    }

    public async Task LoadAsync(Student student, LabClass labClass, Experiment experiment)
    {
        Debug.Log($"Start GradeInfoItemScript, student={student?.ID} experiment={experiment}");

        txtTitle.SetText(experiment.Name);

        Setup(student, labClass);
    }

    private void Setup(Student student, LabClass labClass)
    {
        currentLab = labClass;
        currentStudent = student;

        if (FirebaseAuthManager.instance.IsInstructor())
        {
            btnEdit.gameObject.SetActive(FirebaseAuthManager.instance.GetInstructorInfo()?.ID == labClass.InstructorID);
        }
        else
        {
            btnEdit.gameObject.SetActive(false);
        }
    }
}
