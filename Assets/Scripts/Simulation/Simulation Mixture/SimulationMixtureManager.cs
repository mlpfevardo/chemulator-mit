using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using EZObjectPools;
using TMPro;

public class SimulationMixtureManager : MonoBehaviour
{
    private static Dictionary<string, HashSet<SimulationMixableBehavior>> mixturePool = new Dictionary<string, HashSet<SimulationMixableBehavior>>();
    private static Dictionary<SimulationMixableBehavior, List<SimulationMixableBehavior>> savedMixtures = new Dictionary<SimulationMixableBehavior, List<SimulationMixableBehavior>>();

    private static EZObjectPool objectPool;

    public static SimulationMixtureManager instance = null;

    public GameObject contextMenuPanel;
    public GameObject mixButtonPrefab;

    //public void ResetPool()
    //{
    //    mixturePool.Clear();
    //}

    //public void ResetMixtures()
    //{
    //    savedMixtures.Clear();
    //}

    public void ResetData()
    {
        savedMixtures.Clear();
        mixturePool.Clear();
    }
    #region Register Methods
    //public bool RegisterMixable(SimulationMixableBehavior mixable, Action<List<SimulationMixableBehavior>, DropZoneObjectHandler> action)
    //{
    //    return this.RegisterMixable(mixable) && this.RegisterMixAction(mixable, action);
    //}

    public bool RegisterMixable(SimulationMixableBehavior mixable)
    {
        if (mixturePool.ContainsKey(mixable.GetItemId()))
        {
            Debug.LogWarning("Mixture " + mixable.GetItemId() + " already registered");
            return false;
        }

        //var item = new MixableItem
        //{
        //    mixables = new HashSet<SimulationMixableBehavior>(),
        //};
        mixturePool.Add(mixable.GetItemId(), new HashSet<SimulationMixableBehavior>());

        return true;
    }

    public bool AddAllowableMixtureToMixable(SimulationMixableBehavior mixture, SimulationMixableBehavior mixable)
    {
        if (!mixturePool.ContainsKey(mixture.GetItemId()))
        {
            Debug.LogError("Mixture " + mixture.GetItemId() + " is not registered");
            return false;
        }

        mixturePool[mixture.GetItemId()].Add(mixable);
        //MixableItem savedMixture = mixturePool[mixture.GetItemId()];
        //savedMixture.mixables.Add(mixable);

        return true;
    }

    //public bool RegisterMixAction(SimulationMixableBehavior mixture, Action<List<SimulationMixableBehavior>, DropZoneObjectHandler> action)
    //{
    //    if (!mixturePool.ContainsKey(mixture.GetItemId()))
    //    {
    //        Debug.LogError("Mixture " + mixture.GetItemId() + " is not registered");
    //        return false;
    //    }

    //    MixableItem savedMixture = mixturePool[mixture.GetItemId()];
    //    savedMixture.doMixAction = action;

    //    return true;
    //}

    #endregion

    #region Public Methods
    public bool IsMixable(SimulationMixableBehavior targetMixture, SimulationMixableBehavior mixable)
    {
        return mixturePool.ContainsKey(targetMixture.GetItemId()) && mixturePool[targetMixture.GetItemId()].FirstOrDefault(m => m.GetItemId() == mixable.GetItemId()) != null; //mixturePool[targetMixture.GetItemId()].mixables.FirstOrDefault(a => a.GetItemId() == mixable.GetItemId()) != null;
    }

    public bool AddMixableToMixture(DropZoneObjectHandler mixtureObject, DraggableObjectBehavior draggedObject)
    {
        if (!mixturePool.ContainsKey(mixtureObject.MixtureItem.GetItemId()))
        {
            Debug.LogError("Mixture " + mixtureObject.MixtureItem.GetItemId() + " is not registered");
            return false;
        }

        if (!savedMixtures.ContainsKey(mixtureObject.MixtureItem))
        {
            savedMixtures.Add(mixtureObject.MixtureItem, new List<SimulationMixableBehavior>());
        }

        if (mixtureObject.MixtureItem.AutoMix)
        {
            if (mixtureObject.MixtureItem.DoMix(savedMixtures[mixtureObject.MixtureItem], mixtureObject, draggedObject, savedMixtures.ContainsKey(draggedObject.MixtureItem) ? savedMixtures[draggedObject.MixtureItem] : null))
            {
                savedMixtures[mixtureObject.MixtureItem].Add(draggedObject.MixtureItem);
            }
        }
        else
        {
            savedMixtures[mixtureObject.MixtureItem].Add(draggedObject.MixtureItem);
        }

        return true;
    }

    public bool HasMixables(DropZoneObjectHandler mixtureObject)
    {
        return savedMixtures.ContainsKey(mixtureObject.MixtureItem) && savedMixtures[mixtureObject.MixtureItem].Count > 0;
    }

    public bool Mix(DropZoneObjectHandler mixtureObject)
    {
        if (!mixturePool.ContainsKey(mixtureObject.MixtureItem.GetItemId()))
        {
            Debug.LogError("Mixture " + mixtureObject.MixtureItem.GetItemId() + " is not registered");
            return false;
        }

        mixtureObject.MixtureItem.DoMix(savedMixtures[mixtureObject.MixtureItem], mixtureObject);

        return true;
    }

    public List<SimulationMixableBehavior> GetSavedMixtures(SimulationMixableBehavior mixture)
    {
        if (!savedMixtures.ContainsKey(mixture))
        {
            return Enumerable.Empty<SimulationMixableBehavior>().ToList();
        }

        return savedMixtures[mixture];
    }

    public void ShowContextMenu(DropZoneObjectHandler dropZoneObject)
    {
        GameObject obj = null;
        Vector3 position = new Vector3(dropZoneObject.transform.position.x + 7, dropZoneObject.transform.position.y, dropZoneObject.transform.position.z);

        contextMenuPanel.transform.position = position;

        contextMenuPanel.SetActive(true);
        
        if (mixturePool.ContainsKey(dropZoneObject.MixtureItem.GetItemId()) && !string.IsNullOrEmpty(dropZoneObject.MixtureItem.MixButtonTitle))
        {
            if (!savedMixtures.ContainsKey(dropZoneObject.MixtureItem))
            {
                if (dropZoneObject.MixtureItem.MinAllowableMix == 0)
                {
                    savedMixtures.Add(dropZoneObject.MixtureItem, new List<SimulationMixableBehavior>());

                    obj = CreateMenuButton(position, () =>
                        {
                            SimulationMixtureManager.instance.Mix(dropZoneObject);
                        }, dropZoneObject.MixtureItem.MixButtonTitle);
                }
            }
            else if (savedMixtures[dropZoneObject.MixtureItem]?.Count >= dropZoneObject.MixtureItem.MinAllowableMix)
            {
                obj = CreateMenuButton(position, () =>
                {
                    SimulationMixtureManager.instance.Mix(dropZoneObject);
                }, dropZoneObject.MixtureItem.MixButtonTitle);
            }
        }

        if (dropZoneObject.MixtureItem.isRemovable())
        {
            obj = CreateMenuButton(position, () =>
            {
                dropZoneObject.gameObject.SetActive(false);
            }, "Remove");

            obj.transform.SetAsLastSibling();
        }

        if (obj == null)
        {
            CleanMenuPanel();
            return;
        }

        if (!RendererExtensions.IsFullyVisibleFrom(contextMenuPanel.GetComponent<RectTransform>(), Camera.main))
        {
            position.x -= 12;
            contextMenuPanel.transform.position = position;
            contextMenuPanel.transform.SetAsLastSibling();
        }
    }
    #endregion

    #region Private Functions
    private void OnDestroy()
    {
        //if (objectPool != null)
        //{
        //    objectPool.ClearPool();
        //}
    }
    private void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(this.gameObject);
        instance = this;
        objectPool = EZObjectPool.CreateObjectPool(mixButtonPrefab, "simulationContextButton", 2, true, true, true);
        contextMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && contextMenuPanel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(contextMenuPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            CleanMenuPanel();
        }
    }

    private GameObject CreateMenuButton(Vector3 position, UnityEngine.Events.UnityAction callback, string buttonText = "Mix")
    {
        GameObject obj = null;

        if (objectPool.TryGetNextObject(position, Quaternion.identity, out obj))
        {
            var btn = obj.GetComponent<Button>();
            obj.transform.SetParent(contextMenuPanel.transform, false);

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(CleanMenuPanel);
            btn.onClick.AddListener(callback);

            btn.GetComponent<CustomUIButton>().SetText(buttonText);
        }

        return obj;
    }

    private void CleanMenuPanel()
    {
        foreach (Transform item in contextMenuPanel.transform)
        {
            item.gameObject.SetActive(false);
        }

        contextMenuPanel.SetActive(false);
    }
    #endregion
}
