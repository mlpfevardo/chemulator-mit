﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public GameObject buttonContainer;

    public delegate void OnPauseAction();
    public static event OnPauseAction OnPause;

    private EZObjectPool objectPool;
    public static bool block = false;

    private void Awake()
    {
        objectPool = EZObjectPool.CreateObjectPool(Resources.Load<GameObject>("Prefabs/GreenButton"), "GreenButtonPool", 2, true, true, true);
    }
    private void OnDestroy()
    {
        objectPool.ClearPool();
    }

    // Use this for initialization
    public void ShowPauseMenu()
    {
        if (!block)
        {
            TogglePauseMenu(true);
        }
        block = false;
    }

    public void HidePauseMenu()
    {
        TogglePauseMenu(false);

        foreach (Transform child in buttonContainer.transform)
        {
            child.gameObject.SetActive(false);
        }

        block = false;
    }

    public void DoQuit()
    {
        SimulationManager.instance.DoQuit();
    }

    public void AddButton(string title, UnityAction action)
    {
        GameObject obj;

        if (this.gameObject.activeSelf)
        {
            if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
            {
                var btn = obj.GetComponent<CustomUIButton>();
                btn.ClearListeners();
                btn.AddClickListener(HidePauseMenu);
                btn.AddClickListener(action);
                btn.SetText(title);

                obj.transform.SetParent(buttonContainer.transform, false);
            }
        }
        else
        {
            Debug.LogWarning("You are adding a button while the pause menu is not present.");
        }
    }

    private void TogglePauseMenu(bool show)
    {
        gameObject.SetActive(show);
        Time.timeScale = show ? 0f : 1.0f;

        if (show && OnPause != null)
        {
            OnPause();
        }

        if (!show && RuntimePlatform.Android == Application.platform)
        {
            block = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Input.ResetInputAxes();
            TogglePauseMenu(!gameObject.activeSelf);
        }
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
