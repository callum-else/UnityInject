using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentResolverMonoBehaviour : MonoBehaviour
{
    protected IComponentResolver _componentResolver { get; private set; }

    protected virtual void Awake()
    {
        _componentResolver ??= GetComponentInParent<IComponentResolver>();

        if (_componentResolver == null)
            throw new ComponentNotFoundException($"Component '{nameof(IComponentResolver)}' could not be found in gameobject '{gameObject.name}' or its parents.");
    }
}
