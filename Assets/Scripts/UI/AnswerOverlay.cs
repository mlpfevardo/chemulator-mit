using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class AnswerOverlay : MonoBehaviour
{
    private GameObject answerOverlayObject;

    private static AnswerOverlay instance;

    public static AnswerOverlay Instance
    {
        get
        {
            if(!instance)
            {
                instance = new GameObject("AnswerOverlay").AddComponent<AnswerOverlay>();
                var prefab = Resources.Load<GameObject>("Prefabs/AnswerOverlay");
                instance.answerOverlayObject = GameObject.Instantiate(prefab);
                instance.answerOverlayObject.transform.SetParent(instance.transform);

                DontDestroyOnLoad(instance);
                instance.answerOverlayObject.SetActive(false);
            }

            return instance;
        }
    }

    public void LoadOverlay(Exercise exercise, Student student)
    {
        answerOverlayObject.SetActive(true);
        answerOverlayObject.GetComponent<AnswerOverlayScript>().LoadOverlay(exercise, student);
    }

    public void LoadAnswers(Exercise exercise, ExerciseAnswer answer)
    {
        answerOverlayObject.SetActive(true);
        answerOverlayObject.GetComponent<AnswerOverlayScript>().LoadAnswers(exercise, answer);
    }

    //public void Close()
    //{
    //    answerOverlayObject.SetActive(false);
    //}
}
