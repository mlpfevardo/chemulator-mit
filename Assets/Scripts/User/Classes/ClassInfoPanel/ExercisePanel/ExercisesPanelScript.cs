using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExercisesPanelScript : MonoBehaviour, ILabClassInfoPanel
{
    public static ExercisesPanelScript Instance { get; private set; }
    public GameObject exerciseListPanel;
    public GameObject createExercisePanel;
    public GameObject exerciseInfoPanel;

    private LabClass activeLab;

    private void Awake()
    {
        Instance = this;
    }

    public Task ReLoadAsync()
    {
        return LoadAsync(activeLab);
    }

    public async Task LoadAsync(LabClass lab)
    {
        Debug.Log("Load ExercisesPanelScript, lab=" + lab.ID);
        activeLab = lab;

        exerciseInfoPanel.SetActive(false);
        //createExercisePanel.SetActive(false);
        exerciseListPanel.SetActive(true);

        await exerciseListPanel.GetComponent<ExercisesListPanelScript>().LoadAsync(lab);
    }

    public async void LoadExerciseInfo(Exercise exercise)
    {
        //createExercisePanel.SetActive(false);
        exerciseListPanel.SetActive(false);
        exerciseInfoPanel.SetActive(true);

        await exerciseInfoPanel.GetComponent<ExerciseInfoPanelScript>().LoadAsync(exercise);
    }

    public async void LoadCreateExercisePanel(Exercise exercise)
    {
        exerciseInfoPanel.SetActive(false);
        exerciseListPanel.SetActive(false);
        createExercisePanel.SetActive(true);

        await createExercisePanel.GetComponent<CreateExercisePanelScript>().LoadAsync(exercise);
    }

    public async void LoadCreateExercisePanel()
    {
        exerciseInfoPanel.SetActive(false);
        exerciseListPanel.SetActive(false);
        createExercisePanel.SetActive(true);

        await createExercisePanel.GetComponent<CreateExercisePanelScript>().LoadAsync(activeLab);
    }
}
