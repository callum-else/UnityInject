using System;

namespace UnityInject
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class DependencyInterfaceAttribute : Attribute
    {
    }
}