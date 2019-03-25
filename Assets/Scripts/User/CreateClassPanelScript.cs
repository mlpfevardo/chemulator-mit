using Assets.Scripts.Firebase.Database;
using MlkPwgen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CreateClassPanelScript : MonoBehaviour
{
    public GameObject resultPanel;
    public GameObject createClassPanel;

    public InputField inputClassKey;
    public InputField inputClassName;
    public Button buttonCreateClass;

    public Text textMessage;

    public const int KEYLENGTH = 10;

    public void LoadRoot()
    {
        resultPanel.SetActive(false);
        createClassPanel.SetActive(true);
    }

    public async void OnCreateClassButtonClick()
    {
        resultPanel.SetActive(false);

        if (string.IsNullOrEmpty(inputClassName.text))
        {
            textMessage.text = "Class name cannot be empty";
        }
        else
        {
            SetInteractability(false);

            textMessage.text = "Please wait....";

            try
            {
                Instructor info = await InstructorDatabase.GetInstructorInfoAsync(FirebaseAuthManager.instance.ActiveUserInfo);
                string result = await SaveLab(info);
                if (!string.IsNullOrEmpty(result))
                {
                    resultPanel.SetActive(true);
                    inputClassKey.text = result;
                    textMessage.text = string.Empty;
                    return;
                }
            }
            catch(AggregateException e)
            {
                textMessage.text = FirebaseFunctions.GetFirebaseErrorMessage(e);
            }

            SetInteractability(true);
        }
    }

    private void SetInteractability(bool interactable)
    {
        inputClassName.interactable = interactable;
        buttonCreateClass.interactable = interactable;
    }

    private async Task<string> SaveLab(Instructor instructor)
    {
        Debug.Log("Start SaveLab, instructor=" + instructor.ID);
        string key = PasswordGenerator.Generate(KEYLENGTH);

        try
        {
            LabClass lab = await ClassDatabase.GetLabClassAsync(key);
            while (lab != null)
            {
                key = PasswordGenerator.Generate(KEYLENGTH);
                lab = await ClassDatabase.GetLabClassAsync(key);
            }

            lab = new LabClass
            {
                ID = key,
                InstructorID = instructor.ID,
                Name = inputClassName.text,
            };

            await ClassDatabase.RegisterLabClassAsync(lab);
            textMessage.text = "Building exercises....";
            await ExerciseDatabase.BuildDefaultExercise(lab);

            return key;
        }
        catch(AggregateException e)
        {
            textMessage.text = FirebaseFunctions.GetFirebaseErrorMessage(e);
            Debug.LogError("SaveLab failed: " + key);
        }
        catch(Exception e)
        {
            textMessage.text = "Unexpected error occurred";
        }

        return string.Empty;
    }
}
