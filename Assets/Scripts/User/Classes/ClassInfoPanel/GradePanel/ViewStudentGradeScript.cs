using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ViewStudentGradeScript : MonoBehaviour
{
    private Student student;
    private LabClass lab;

    // Load Instructor View
    public async Task LoadAsync(Student student, LabClass labClass)
    {
        Debug.Log($"Start ViewStudentGradeScript, student={student.ID} labClass={labClass.ID}");

        this.student = student;
        this.lab = labClass;
    }

    // Load Student View
    public async Task LoadAsync(LabClass labClass)
    {
        Debug.Log($"Start ViewStudentGradeScript, labClass={labClass.ID}");
        this.student = null;
        this.lab = labClass;
    }

    public void OnClick()
    {

    }
}
