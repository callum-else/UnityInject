using UnityEngine;

namespace UnityInject
{
    public interface IRequiresDependencies<T>
    {
        void InitializeWithDependencies(GameObject root, T dependencies);
        public virtual void Initialize() { }
        public virtual void OnInitialized() { }
    }
}