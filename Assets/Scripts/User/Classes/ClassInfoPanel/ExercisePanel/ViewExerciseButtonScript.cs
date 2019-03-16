using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewExerciseButtonScript : MonoBehaviour
{
    public Exercise Exercise { get; set; }

    public void OnClick()
    {
        ExercisesPanelScript.Instance.LoadExerciseInfo(Exercise);
    }
}
