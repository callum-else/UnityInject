using UnityEngine;

public interface IRequiresDependencies<T>
{
    void InitializeWithDependencies(GameObject root, T dependencies);
    public virtual void Initialize() { }
}