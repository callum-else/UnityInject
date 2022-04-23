using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentResolver : MonoBehaviour
{
    private Dictionary<(Type, dynamic), dynamic> _resolutionCache = new Dictionary<(Type, dynamic), dynamic>();

    private enum ResolverValidationRules : byte
    {
        All = 1,
        ComponentNotFound = 2,
        ExcessComponentsFound = 4,
    }

    private void ValidateResults<TComponent>(List<TComponent> results, ResolverValidationRules validationRules)
    {
        if (validationRules.HasFlag(ResolverValidationRules.ComponentNotFound | ResolverValidationRules.All) && results.Count == 0)
        {
            throw new ComponentNotFoundException($"No component of type '{typeof(TComponent)}' could be found. " +
                "Check the component predicate and make sure the component you are looking for exists.");
        }

        if (validationRules.HasFlag(ResolverValidationRules.ExcessComponentsFound | ResolverValidationRules.All) && results.Count > 1)
        {
            throw new ExcessComponentsFoundException($"Multiple components of type '{typeof(TComponent)}' were found. Check the component predicate.");
        }
    }

    public TComponent ResolveOrDefault<TComponent>(Func<TComponent, bool> predicate = null, bool resolveInactive = true, bool cacheResult = true, bool getCachedComponents = true)
    {
        List<TComponent> foundComponents = ResolveComponents(predicate, resolveInactive, cacheResult, getCachedComponents);
        ValidateResults(foundComponents, ResolverValidationRules.ExcessComponentsFound);
        return foundComponents.FirstOrDefault();
    }

    public TComponent ResolveComponent<TComponent>(Func<TComponent, bool> predicate = null, bool resolveInactive = true, bool cacheResult = true, bool getCachedComponents = true)
    {
        List<TComponent> foundComponents = ResolveComponents(predicate, resolveInactive, cacheResult, getCachedComponents);
        ValidateResults(foundComponents, ResolverValidationRules.All);
        return foundComponents.FirstOrDefault();
    }

    public List<TComponent> ResolveComponents<TComponent>(Func<TComponent, bool> predicate = null, bool resolveInactive = true, bool cacheResult = true, bool getCachedComponents = true)
    {
        bool isCached = (cacheResult || getCachedComponents) && _resolutionCache.ContainsKey((typeof(TComponent), predicate));

        List<TComponent> foundComponents;
        if (getCachedComponents && isCached)
        {
            foundComponents = _resolutionCache[(typeof(TComponent), predicate)] as List<TComponent>;
            //Debug.Log($"GameObject {gameObject.name} resolved {foundComponents.GetType()}({foundComponents.Count}) using key ({typeof(TComponent)},{predicate})");
        }
        else
            foundComponents = GetComponentsInChildren<TComponent>(resolveInactive).ToList();

        if (predicate != null)
            foundComponents = foundComponents.Where(predicate).ToList();

        if (cacheResult && !isCached)
        {
            _resolutionCache.Add((typeof(TComponent), predicate), foundComponents);
            //Debug.Log($"GameObject {gameObject.name} cached {foundComponents.GetType()}({foundComponents.Count}) using key ({typeof(TComponent)},{predicate})");
        }

        return foundComponents;
    }

    public bool ComponentExists<TComponent>(Func<TComponent, bool> predicate = null, bool resolveInactive = true, bool cacheResult = true, bool getCachedComponents = true)
    {
        return ResolveComponents(predicate, resolveInactive, cacheResult, getCachedComponents).Any();
    }

    public bool ComponentExists<TComponent>(out TComponent component, Func<TComponent, bool> predicate = null, bool resolveInactive = true, bool cacheResult = true, bool getCachedComponents = true)
    {
        var foundComponents = ResolveComponents(predicate, resolveInactive, cacheResult, getCachedComponents);
        component = foundComponents.FirstOrDefault();
        return foundComponents.Any();        
    }

    public Transform GetRootTransform()
    {
        return transform;
    }

    public static ComponentResolver GetResolver(Transform source)
    {
        return source.GetComponentInParent<ComponentResolver>();
    }

    public static bool ComponentByIdPredicate<TSourceObject, TId>(TSourceObject sourceObject, TId componentId)
        where TSourceObject : Component
        where TId : struct
    {
        return sourceObject.TryGetComponent(out ComponentIdentifier<TId> componentIdentifier) && componentIdentifier.HasId(componentId);
    }
}
