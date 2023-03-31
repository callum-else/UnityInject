using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityInject
{
    public abstract class RequiresDependencies<T> : MonoBehaviour, IRequiresDependencies<T>
    {
        protected bool IsInitialized { get; private set; }
        protected GameObject RootObject { get; private set; }

        public void InitializeWithDependencies(GameObject root, T dependencies)
        {
            RootObject = root;
            Initialize(dependencies);
            IsInitialized = true;
        }

        public abstract void Initialize(T dependencies);

        public virtual void OnInitialized() { }
    }
}