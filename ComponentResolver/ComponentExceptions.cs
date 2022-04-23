using UnityEngine;

public class ComponentNotFoundException : UnityException 
{
    public ComponentNotFoundException() { }

    public ComponentNotFoundException(string message) : base(message) { }
}

public class ExcessComponentsFoundException : UnityException
{
    public ExcessComponentsFoundException() { }

    public ExcessComponentsFoundException(string message) : base(message) { }
}