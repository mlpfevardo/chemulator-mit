using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ViewStudentGradeScript : MonoBehaviour
{
    private UserInfo user;
    private LabClass lab;

    // Load Instructor View
    public async Task LoadAsync(UserInfo user, LabClass labClass)
    {
        Debug.Log($"Start ViewStudentGradeScript, student={user.ID} labClass={labClass.ID}");

        this.user = user;
        this.lab = labClass;
    }

    //// Load Student View
    //public async Task LoadAsync(LabClass labClass)
    //{
    //    Debug.Log($"Start ViewStudentGradeScript, labClass={labClass.ID}");
    //    this.student = null;
    //    this.lab = labClass;
    //}

    public void OnClick()
    {
        GradesPanelScript.Instance.LoadGradeInfo(user, lab);
        //ClassInfoPanelScript.Instance.OnClickGrades();
    }
}
