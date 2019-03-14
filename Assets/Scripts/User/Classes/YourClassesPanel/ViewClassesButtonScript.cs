using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewClassesButtonScript : MonoBehaviour
{
    public LabClass LabClass { get; set; }

    public void OnClick()
    {
        ClassesPanelScript.Instance.LoadClassInfoPanel(LabClass);
    }
}
