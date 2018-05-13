using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public GameObject _CachedGameObject { get; private set; }

    public Transform _CachedTransform { get; private set; }


    private void Awake()
    {
        InitializePanel();
    }

    private void OnEnable()
    {
        EnablePanel();
    }

    private void OnDisable()
    {
        DisablePanel();
    }

    private void OnDestroy()
    {
        DestroyPanel();
    }

    protected virtual void InitializePanel()
    {
        _CachedGameObject = gameObject;
        _CachedTransform = transform;
    }

    protected virtual void EnablePanel()
    {
        if (_CachedGameObject == null)
        {
            InitializePanel();
        }

        _CachedGameObject.SetActive(true);
    }

    protected virtual void DisablePanel()
    {
        _CachedGameObject.SetActive(false);
    }

    protected virtual void DestroyPanel()
    {
        _CachedGameObject = null;
        _CachedTransform = null;
    }
}
