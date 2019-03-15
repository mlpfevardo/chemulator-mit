using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GradesPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start GradesPanelScript, lab=" + lab.ID);
    }
}
