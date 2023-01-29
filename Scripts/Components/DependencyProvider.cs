using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace UnityInject
{
    public abstract class DependencyProvider : MonoBehaviour
    {
        private UnityEvent<MonoBehaviour> _publishDependencies;
        private IEnumerable<Type> _implementedTypes;
        private bool _hasInjected;

        private void Awake()
        {
            Inject();
        }

        private void OnEnable()
        {
            if (!_hasInjected)
                Inject();
        }

        private void Inject()
        {
            _hasInjected = true;
            _publishDependencies = new UnityEvent<MonoBehaviour>();

            _implementedTypes = GetType().GetInterfaces()
                .Where(x => x.CustomAttributes.Any(x => x.AttributeType == typeof(DependencyInterfaceAttribute)))
                .Select(x => typeof(IRequiresDependencies<>).MakeGenericType(x))
                .ToList();

            foreach (var type in _implementedTypes)
            {
                var components = transform.GetComponentsInChildren(type, true).Cast<MonoBehaviour>();
                foreach (var component in components)
                {
                    _publishDependencies.AddListener((x) => type
                        .GetMethod(nameof(IRequiresDependencies<MonoBehaviour>.InitializeWithDependencies))
                        .Invoke(component, new object[] { x.gameObject, x }));
                }
            }

            _publishDependencies.Invoke(this);
        }
    }
}
