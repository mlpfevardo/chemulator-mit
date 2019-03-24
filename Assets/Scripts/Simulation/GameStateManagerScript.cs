using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class GameStateData
{
    public List<GameStateTableObject> elements;
    public float timer;
    public int activityId;
}

[System.Serializable]
public class GameStateTableObject
{
    public SimulationMixableBehavior mixtureItem;
    public Type type;
    public string iconPath;
    public float x;
    public float y;
}

public class GameStateManagerScript : MonoBehaviour
{
    public static GameStateManagerScript Instance { get; private set; }
    public GameObject dropZoneContainer;

    private static string fileName = "-simulationData.dat";

    private void Awake()
    {
        Instance = this;
    }

    public async Task SaveState()
    {
        RemoveSaveData();

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        var data = new GameStateData
        {
            elements = new List<GameStateTableObject>(),
            timer = GameTimerScript.Instance.GetTime(),
            activityId = GameManager.Instance.CurrentLabActivity,
        };

        foreach(Transform child in dropZoneContainer.transform)
        {
            var element = child.GetComponent<DropZoneObjectHandler>();
            
            data.elements.Add(new GameStateTableObject
            {
                type = element.MixtureItem.GetType(),
                x = child.position.x,
                y = child.position.y,
                mixtureItem = element.MixtureItem,
                iconPath = System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(element.MixtureItem.icon).Replace("Assets/Resources/", ""), null),
            });
        }

        string path = Path.Combine(Application.persistentDataPath, data.activityId + fileName);

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public async Task<GameStateData> LoadState()
    {
        Debug.Log("Start LoadState, activityId=" + GameManager.Instance.CurrentLabActivity);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.CurrentLabActivity + fileName);

        try
        {
            using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
            {
                return binaryFormatter.Deserialize(fileStream) as GameStateData;
            }
        }
        catch
        {
            Debug.LogError("Failed to decode saved file.");
        }

        return null;
    }

    public void RemoveSaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.CurrentLabActivity + fileName);
        File.Delete(path);
    }

    public bool HasSavedState()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.CurrentLabActivity + fileName);
        return File.Exists(path);
    }
}
