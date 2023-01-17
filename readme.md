# UnityInject
UnityInject is a dependency injection system for the Unity game engine that allows for the easy management of dependencies between components in your project. It promotes good software design practices by enforcing separation of concerns, making your code more modular and easier to maintain.

# Benefits
- Allows for more maintainable and testable code by promoting separation of concerns.
- Improves code reusability by making it easier to swap out components that have dependencies.
- Reduces coupling between components by making dependencies explicit.
- Makes it easy to manage dependencies for complex systems with many components.

# How it works
UnityInject works by using interfaces marked with the `DependencyInterface` attribute to define the dependencies required by a component, and classes that implement these interfaces to provide the dependencies.

The `DependencyProvider` abstract class is used on the 'root' game object containing your dependencies. It uses reflection to find all the components in the children of the root game object's transform that implement the `IRequiresDependencies<T>` interface, and calls their `InitializeWithDependencies` method, passing the dependencies provided by the interface.

Components that require dependencies inherit from the `RequiresDependencies<T>` abstract class, and they implement the `Initialize` method to set their dependencies.

# Usage
In order to use UnityInject in your project, you will need to do the following:

1. Create an interface that defines the dependencies required by a component, and mark it with the DependencyInterface attribute.
```csharp
[DependencyInterface]
public interface IDependencies
{
    public Collider Collider { get; }
    public Rigidbody Rigidbody { get; }
}
```

2. Create a class that implements the interface created in step 1, and inherit from the `DependencyProvider` abstract class. This class will provide the dependencies to other components.
```csharp
public class Dependencies : DependencyProvider, IDependencies
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    public Collider Collider => _collider;
    public Rigidbody Rigidbody => _rigidbody;
}
```

3. Place the class created in step 2 on the 'root' game object containing your dependencies.
4. Create a component that requires the dependencies defined in step 1, and inherit from the `RequiresDependencies<T>` abstract class, where `T` is the interface created in step 1.
```csharp
public class ExampleComponent : RequiresDependencies<IDependencies>
{
    private Collider _collider;
    private Rigidbody _rigidbody;

    public override void Initialize(IDependencies dependencies)
    {
        _collider = dependencies.Collider;
        _rigidbody = dependencies.Rigidbody;
    }
}
```

1. Attach the component created in step 4 to a game object inside the scope of your 'root' game object. When the scene starts, it will automatically receive the dependencies defined in step 2.