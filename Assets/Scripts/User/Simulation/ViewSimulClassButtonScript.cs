using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSimulClassButtonScript : MonoBehaviour
{
    public LabClass LabClass { get; set; }

    public void OnClick()
    {
        SimulationPanelScript.Instance.LoadSimulationList(LabClass);
    }
}
