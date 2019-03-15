using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ViewClassInfoRootScript : MonoBehaviour, ILabClassInfoPanel
{
    public TextMeshProUGUI labName;

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Start ViewClassInfoRootScript, lab=" + lab.ID);
        labName.SetText(lab.Name);
    }
}
