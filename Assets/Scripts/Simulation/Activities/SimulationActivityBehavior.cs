using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SimulationActivityBehavior
{
    public abstract void Setup();
    public abstract string ID { get; }

    public virtual void Finish() { }

    public virtual void OnPause(object sender, EventArgs e) { }

    protected void Reset()
    {
        TableDropZone.Instance.RemoveObjects();
        SimulationManager.instance.ClearList();
        SimulationMixtureManager.instance.ResetData();
    }

    protected void Publish()
    {
        SimulationManager.instance.RefreshList();
    }

    public string GetInstructions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Instructions", ID + ".txt");

        if (Application.platform == RuntimePlatform.Android)
        {
            using (var www = new WWW(path))
            {
                while(!www.isDone) { }

                return www.text;
            }
        }
        else
        {
            IEnumerable<string> lines = System.IO.File.ReadLines(path);
            return string.Join("\n", lines);
        }
    }
}