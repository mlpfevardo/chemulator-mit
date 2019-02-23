using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using EZObjectPools;
using UnityEngine.Events;

public class PartSelectorPanel : MonoBehaviour
{
    private static PartSelectorPanel instance;
    private static EZObjectPool objectPool;
    private GameObject buttonContainer;

    public class SelectorButton
    {
        public string title;
        public UnityAction action;
    }

    public static PartSelectorPanel Instance
    {
        get
        {
            if (!instance)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/PartSelector");
                var obj = GameObject.Instantiate(prefab);
                instance = obj.AddComponent<PartSelectorPanel>();
                instance.buttonContainer = instance.transform.Find("PartPanel/Panel/ButtonContainer").gameObject;

                instance.CloseSelector();
            }

            return instance;
        }
    }

    private void OnDestroy()
    {
        objectPool.ClearPool();
    }
    private void Awake()
    {
        if (!objectPool)
        {
            objectPool = EZObjectPool.CreateObjectPool(Resources.Load<GameObject>("Prefabs/GreenButton"), "GreenButtonPool", 2, true, true, true);
        }
    }

    public void ShowSelector(params SelectorButton[] otherActions)
    {
        this.gameObject.SetActive(true);

        foreach (var todo in otherActions)
        {
            AddButton(todo);
        }

        Time.timeScale = 0f;
    }

    private void AddButton(SelectorButton action)
    {
        GameObject obj;

        if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
        {
            var btn = obj.GetComponent<CustomUIButton>();

            btn.ClearListeners();
            btn.AddClickListener(CloseSelector);
            btn.AddClickListener(action.action);
            btn.SetText(action.title);

            obj.transform.SetParent(buttonContainer.transform, false);
            obj.transform.SetAsLastSibling();
        }
    }

    private void CloseSelector()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
