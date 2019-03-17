using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExerciseCanvasScript : MonoBehaviour
{
    public static ExerciseCanvasScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public async Task LoadAsync(LabClass lab)
    {

    }

    public void OnSubmit()
    {
        ModalPanel.Instance.ShowModalOK("Success", "Submission success", () =>
        {

        });
    }
}
