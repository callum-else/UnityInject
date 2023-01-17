using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RequiresDependencies<T> : MonoBehaviour, IRequiresDependencies<T>
{
    private List<Action> _actionsOnInitialize;

    protected bool IsInitialized { get; private set; }
    protected GameObject RootObject { get; private set; }

    public void InitializeWithDependencies(GameObject root, T dependencies)
    {
        RootObject = root;
        Initialize(dependencies);
        IsInitialized = true;

        if (_actionsOnInitialize != null)
        {
            foreach (var action in _actionsOnInitialize)
                action.Invoke();
        }
    }

    public abstract void Initialize(T dependencies);
    
    public void ExecuteOnInitialise(Action action)
    {
        if (IsInitialized)
        {
            action();
            return;
        }

        if (_actionsOnInitialize == null)
            _actionsOnInitialize = new List<Action>();

        _actionsOnInitialize.Add(action);
    }
}