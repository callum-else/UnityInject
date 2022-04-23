using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentIdentifier<TId> : MonoBehaviour
    where TId : struct
{
    [SerializeField] private TId _componentId;

    public TId Id
    {
        get => _componentId;
    }

    public bool HasId(TId id)
    {
        return id.Equals(_componentId);
    }
}
