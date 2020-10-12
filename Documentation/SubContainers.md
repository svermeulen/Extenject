# Sub-Containers And Facades

## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Introduction](#introduction)
- [Hello World Example For Sub-Containers/Facade](#hello-world-example-for-sub-containersfacade)
- [Creating Sub-Containers on GameObject's by using Game Object Context](#creating-sub-containers-on-gameobjects-by-using-game-object-context)
- [Creating Game Object Context's Dynamically](#creating-game-object-contexts-dynamically)
- [Creating Game Object Context's Dynamically With Parameters](#creating-game-object-contexts-dynamically-with-parameters)
- [GameObjectContext Example Without MonoBehaviours](#gameobjectcontext-example-without-monobehaviours)
- [Using ByInstaller / ByMethod with Kernel](#using-byinstaller--bymethod-with-kernel)
- [Using ByInstaller / ByMethod with Kernel and BindFactory](#using-byinstaller--bymethod-with-kernel-and-bindfactory)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Introduction

In some cases it can be very useful to use multiple containers in the same application.  For example, if you are creating a word processor it might be useful to have a sub-container for each tab that represents a separate document.  This way, you could bind a bunch of classes `AsSingle()` within the sub-container and they could all easily reference each other as if they were all singletons.  Then you could instantiate multiple sub-containers to be used for each document, with each sub-container having unique instances of all the classes that handle each specific document.

Another example might be if you are designing an open-world space ship game, you might want each space ship to have it's own container that contains all the class instances responsible for running that specific spaceship.

This is actually how ProjectContext bindings work.  There is one container for the entire project, and when a unity scene starts up, the container within each SceneContext is created "underneath" the ProjectContext container.  All the bindings that you add in your scene MonoInstaller are bound to your SceneContext container.  This allows the dependencies in your scene to automatically get injected with ProjectContext bindings, because sub-containers automatically inherit all the bindings in their parent (and grandparent, etc.).

A common design pattern that we like to use in relation to sub-containers is the <a href="https://en.wikipedia.org/wiki/Facade_pattern">Facade pattern</a>.  This pattern is used to abstract away a related group of dependencies so that it can be used at a higher level when used by other modules in the code base.  This is relevant here because often when you are defining sub-containers in your application it is very useful to also define a Facade class that is used to interact with this sub-container as a whole.  So, to apply it to the spaceship example above, you might have a SpaceshipFacade class that represents very high-level operations on a spaceship such as "Start Engine", "Take Damage", "Fly to destination", etc.  And then internally, the SpaceshipFacade class can delegate the specific handling of all the parts of these requests to the relevant single-responsibility dependencies that exist within the sub-container.

Subcontainers can also be incredibly powerful as a way to organize big parts of your code base into distinct modules.  You can manage dependencies at a higher level - as dependencies between modules instead of just between classes.  Without subcontainers, as your code base grows larger and larger, having everything exist as singletons at the same level can get unwieldy over time, since every class can depend on every other class just by adding a constructor parameter for it.  By instead grouping related classes into their own subcontainers and forcing any other interested classes to interact via a facade class, it can be much easier to manage and understand the overall dependencies between parts of the code.  You can think of each facade class as a contract for an entire subsystem of classes, with the implementation of the contract hidden away underneath the subcontainer, where you can have many different low-level classes each with their own single responsibility.  The result will be more loosely coupled code in general, which will make it much easier to refactor / maintain / test / understand.

Let's do some examples in the following sections.

## Hello World Example For Sub-Containers/Facade

```csharp
public class Greeter
{
    readonly string _message;

    public Greeter(string message)
    {
        _message = message;
    }

    public void DisplayGreeting()
    {
        Debug.Log(_message);
    }
}

public class GameController : IInitializable
{
    readonly Greeter _greeter;

    public GameController(Greeter greeter)
    {
        _greeter = greeter;
    }

    public void Initialize()
    {
        _greeter.DisplayGreeting();
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameController>().AsSingle();
        Container.Bind<Greeter>().FromSubContainerResolve().ByMethod(InstallGreeter).AsSingle();
    }

    void InstallGreeter(DiContainer subContainer)
    {
        subContainer.Bind<Greeter>().AsSingle();
        subContainer.BindInstance("Hello world!");
    }
}
```

The important thing to understand here is that any bindings that we add inside the `InstallGreeter` method will only be visible to objects within this sub-container.  The only exception is the Facade class (in this case, Greeter) since it is bound to the parent container using the FromSubContainerResolve binding.  In other words, in this example, the string "Hello World" is only visible by the Greeter class.

Note the following:
- You should always add a bind statement for whatever class is given to FromSubContainerResolve within the sub-container install method - otherwise you'll get exceptions and validation will fail.
- Unlike the above example, it is often better to use `ByInstaller` instead of `ByMethod`.  This is because when you use `ByMethod` it is easy to accidentally reference the Container instead of the subContainer.  Also, by using `ByInstaller` you can pass arguments into the Installer itself.
- Subcontainers can also be used in a similar way to spawn facades dynamically by using BindFactory with FromSubContainerResolve
- There are some drawbacks to this approach when it comes to using MonoBehaviour's or when using interfaces such as IInitializable / ITickable / IDisposable.  See <a href="#byinstaller-bymethod-with-kernel">here</a> for advanced usage.

## Creating Sub-Containers on GameObject's by using Game Object Context

One issue with the <a href="#hello-world-for-facades">sub-container hello world example</a> above is that it does not work very well for MonoBehaviour classes.  There is nothing preventing us from adding MonoBehaviour bindings such as FromComponentInNewPrefab, FromNewComponentOnNewGameObject, etc. to our ByInstaller/ByMethod sub-container - however these will cause these new game objects to be added to the root of the scene heirarchy, so we'll have to manually track the lifetime of these objects ourselves by calling GameObject.Destroy on them when the Facade is destroyed.  Also, there is no way to have GameObject's that exist in our scene at the start but also exist within our sub-container.  Also, using ByInstaller and ByMethod like above does not support the use of interfaces such as IInitializable / ITickable / IDisposable inside the subcontainer.  These problems can be solved by using Game Object Context.

For this example, let's try to actually implement something similar to the open world space ship game described in <a href="#introduction">the sub-container introduction</a>:

* Create a new scene
* Add the following files to your project:

```csharp
using Zenject;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipHealthHandler _healthHandler;

    [Inject]
    public void Construct(ShipHealthHandler healthHandler)
    {
        _healthHandler = healthHandler;
    }

    public void TakeDamage(float damage)
    {
        _healthHandler.TakeDamage(damage);
    }
}
```

```csharp
using UnityEngine;
using Zenject;

public class GameRunner : ITickable
{
    readonly Ship _ship;

    public GameRunner(Ship ship)
    {
        _ship = ship;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ship.TakeDamage(10);
        }
    }
}
```

```csharp
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameRunner>().AsSingle();
    }
}
```

```csharp
using Zenject;
using UnityEngine;

public class ShipHealthHandler : MonoBehaviour
{
    float _health = 100;

    public void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 100), "Health: " + _health);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }
}
```

```csharp
using UnityEngine;
using System.Collections;

public class ShipInputHandler : MonoBehaviour
{
    [SerializeField]
    float _speed = 2;

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += Vector3.forward * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position -= Vector3.forward * _speed * Time.deltaTime;
        }
    }
}
```

* Right Click inside the Hierarchy tab and select `Zenject -> Scene Context`
* Drag the GameInstaller class to the SceneContext game object
* Add a new row to the Installers property of the SceneContext
* Drag the GameInstaller component to the new row under Installers
* Right Click again inside the Hierarchy tab and select `Zenject -> Game Object Context`
* Rename the new game object GameObjectContext to Ship
* Drag the Ship MonoBehaviour to the Ship GameObject in our Scene. The Ship class will be used as a <a href="https://en.wikipedia.org/wiki/Facade_pattern">Facade</a> for our ship and will be used by other systems to interact with the ship at a high level
* Also add the `ShipInputHandler` component to the Ship game object
* Right click on the Ship GameObject and select 3D Object -> Cube.  This will serve as the placeholder model for our ship.
* Add new game object under ship called HealthHandler, and add the `ShipHealthHandler` component to it
* Your scene should look like this:

<img src="Images/ShipFacadeExample1.png?raw=true" alt="Ship Facade Example"/>

* The idea here is that everything at or underneath the Ship game object should be considered inside it's own sub-container.  When we're done, we should be able to add multiple ships to our scene, each with their own components ShipHealthHandler, ShipInputHandler, etc. that can treat each other as singletons.
* Try to validate your scene by pressing `CTRL+ALT+V`.  You should get an error that looks like this: `Unable to resolve type 'ShipHealthHandler' while building object with type 'Ship'.`
* This is because the ShipHealthHandler component has not been added to our sub-container.  To address this:
    * Click on the HealthHandler game object and then click Add Component and type Zenject Binding (see <a href="../README.md#scene-bindings">here</a> for details on this feature)
    * Drag the Ship Health Handler Component to the Components field of Zenject Binding
* Validate again by pressing `CTRL+ALT+V`.  You should now get this error instead: `Unable to resolve type 'Ship' while building object with type 'GameRunner'.` 
* Our Ship component also needs to be added to the container.  To address this, once again:
    * Click on the Ship game object and then click Add Component and type Zenject Binding
    * Drag the Ship Component to the Components field of Zenject Binding
* If we attempt to validate again you should notice the same error occurs.  This is because by default, ZenjectBinding only adds its components to the nearest container - in this case Ship.  This is not what we want though.  We want Ship to be added to the scene container because we want to use it as the Facade for our sub-container.  We can do this just by checking the "Use Scene Context" flag on the ZenjectBinding.  We can also explicitly choose the context to use by using the Context property but it is easier to use this flag if we're only interested in using the scene context.
* Validation should now pass successfully.
* If you run the scene now, you should see a health display in the middle of the screen, and you should be able to press Space bar to apply damage, and the up/down arrows to move the ship

Also note that we can add installers to our ship sub-container in the same way that we add installers to our Scene Context - just by dropping them into the Installers property of GameObjectContext.

In this example we used MonoBehaviour's for everything but this is just one of several ways to implement Facades/Subcontainers.  In the <a href="../README.md#zenject-philophy">spirit of not enforcing any one way of doing things</a>, we also present <a href="#using-game-object-contexts-no-monobehaviours">another approach</a> below that doesn't use any MonoBehaviour's at all.

## Creating Game Object Context's Dynamically

Continuing with the ship example <a href="#using-game-object-contexts">above</a>, let's pretend that we now want to create ships dynamically, after the game has started.

* First, create a prefab for the entire `Ship` GameObject that we created above.
* Then just add the following changes

```csharp
public class Ship : MonoBehaviour
{
    ...

    public class Factory : PlaceholderFactory<Ship>
    {
    }
}
```

```csharp
public class GameRunner : ITickable
{
    readonly Ship.Factory _shipFactory;

    Vector3 lastShipPosition;

    public GameRunner(Ship.Factory shipFactory)
    {
        _shipFactory = shipFactory;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ship = _shipFactory.Create();
            ship.transform.position = lastShipPosition;

            lastShipPosition += Vector3.forward * 2;
        }
    }
}
```

```csharp
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    GameObject ShipPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameRunner>().AsSingle();

        Container.BindFactory<Ship, Ship.Factory>().FromSubContainerResolve().ByNewContextPrefab(ShipPrefab);
    }
}
```

After doing this, make sure to drag and drop the newly created Ship prefab into the ShipPrefab property of GameInstaller in the inspector

Now if we run our scene, we can hit Space to add multiple Ship's to our scene.  You can also add ships directly to the scene at edit time just like before and they should work the same.   Note however that the ZenjectBinding component we added with the "Use Scene Context" flag checked will have no effect for the dynamically created ships, but will be used for the ships added at edit time.  So if you duplicate the ship in the scene hierarchy and then add a `List<Ship>` constructor parameter to one of your classes, you'll get the initial list of Ships but not the dynamically created ones that were added via the factory.

## Creating Game Object Context's Dynamically With Parameters

Let's make this even more interesting by passing a parameter into our ship facade.  Let's make the speed of the ship configurable from within the GameController class.

* Change our classes to the following:

```csharp
public class GameRunner : ITickable
{
    readonly Ship.Factory _shipFactory;

    Vector3 lastShipPosition;

    public GameRunner(Ship.Factory shipFactory)
    {
        _shipFactory = shipFactory;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ship = _shipFactory.Create(Random.RandomRange(2, 20));
            ship.transform.position = lastShipPosition;

            lastShipPosition += Vector3.forward * 2;
        }
    }
}
```

```csharp
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    GameObject ShipPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameRunner>().AsSingle();

        Container.BindFactory<float, Ship, Ship.Factory>().FromSubContainerResolve().ByNewContextPrefab<ShipInstaller>(ShipPrefab);
    }
}
```

```csharp
using Zenject;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipHealthHandler _healthHandler;

    [Inject]
    public void Construct(ShipHealthHandler healthHandler)
    {
        _healthHandler = healthHandler;
    }

    public void TakeDamage(float damage)
    {
        _healthHandler.TakeDamage(damage);
    }

    public class Factory : PlaceholderFactory<float, Ship>
    {
    }
}
```

```csharp
using UnityEngine;
using System.Collections;
using Zenject;

public class ShipInputHandler : MonoBehaviour
{
    [Inject]
    float _speed;

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += Vector3.forward * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position -= Vector3.forward * _speed * Time.deltaTime;
        }
    }
}
```

Also, add this new file:

```csharp
using System;
using Zenject;

public class ShipInstaller : MonoInstaller
{
    [Inject]
    float _speed;

    public override void InstallBindings()
    {
        Container.BindInstance(_speed).WhenInjectedInto<ShipInputHandler>();
    }
}
```

After that compiles, add ShipInstaller to the Ship prefab and also drag it to the Installers field of the GameObjectContext.

Note the changes that we made here:
- ShipInputHandler now has the speed injected instead of using Unity's SerializeField.
- The nested Factory class in Ship has a float parameter added to it
- In GameInstaller, the binding for the factory is different
- In GameRunner, we now need to pass a float parameter to the factory's create method

One important difference with creating a Sub-Container using a factory, is that the parameters you supply to the factory are not necessarily forwarded to the facade class.  In this example, the parameter is a float value for speed, which we want to forward to the ShipInputHandler class instead.  That is why these parameters are always forwarded to an installer for the sub-container, so that you can decide for yourself at install time what to do with the parameter.  Another reason for this is that in some cases the parameter might be used to choose different bindings.

One problem with the above is that it will not work with the ships that we add during edit time, since the injected `_speed` value in ShipInstaller will not be found.  We can address this by making it optional and then also exposing it to the inspector like this:

```csharp
using System;
using Zenject;
using UnityEngine;

public class ShipInstaller : MonoInstaller
{
    [SerializeField]
    [InjectOptional]
    float _speed;

    public override void InstallBindings()
    {
        Container.BindInstance(_speed).WhenInjectedInto<ShipInputHandler>();
    }
}
```

This way, you can drop the Ship prefab into the scene and control the speed in the inspector, but you can also create them dynamically and pass the speed into the factory as a parameter.

For a more real-world example see the SpaceFighter sample project which makes heavy use of Game Object Contexts.

## GameObjectContext Example Without MonoBehaviours

If you're like me, then you might want to minimize all the use of MonoBehaviour in the above example.   It comes down to personal preference, but sometimes it's simpler to just use plain C# classes when possible.  In this example, we'll change the example above so that the ship prefab is just the model used for the ship (which in this case is just a cube):


```csharp
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    GameObject ShipPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameRunner>().AsSingle();

        Container.BindFactory<float, ShipFacade, ShipFacade.Factory>()
            .FromSubContainerResolve().ByNewPrefabInstaller<ShipInstaller>(ShipPrefab);
    }
}
```

```csharp
using UnityEngine;
using Zenject;

public class GameRunner : ITickable
{
    readonly ShipFacade.Factory _shipFactory;

    Vector3 lastShipPosition;

    public GameRunner(ShipFacade.Factory shipFactory)
    {
        _shipFactory = shipFactory;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ship = _shipFactory.Create(Random.Range(2.0f, 20.0f));
            ship.Transform.position = lastShipPosition;

            lastShipPosition += Vector3.forward * 2;
        }
    }
}
```

```csharp
using UnityEngine;
using Zenject;

public class ShipFacade
{
    readonly ShipHealthHandler _healthHandler;

    public ShipFacade(ShipHealthHandler healthHandler)
    {
        _healthHandler = healthHandler;
    }

    public void TakeDamage(float damage)
    {
        _healthHandler.TakeDamage(damage);
    }

    [Inject]
    public Transform Transform
    {
        get; private set;
    }

    public class Factory : PlaceholderFactory<float, ShipFacade>
    {
    }
}
```

```csharp
using UnityEngine;

public class ShipHealthHandler : MonoBehaviour
{
    float _health = 100;

    public void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 100), "Health: " + _health);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }
}
```

```csharp
using UnityEngine;
using Zenject;

public class ShipInputHandler : ITickable
{
    readonly Transform _transform;
    readonly float _speed;

    public ShipInputHandler(
        float speed,
        Transform transform)
    {
        _transform = transform;
        _speed = speed;
    }

    public void Tick()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _transform.position += Vector3.forward * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _transform.position -= Vector3.forward * _speed * Time.deltaTime;
        }
    }
}
```

```csharp
using UnityEngine;
using Zenject;

public class ShipInstaller : Installer<ShipInstaller>
{
    readonly float _speed;

    public ShipInstaller(
        [InjectOptional]
        float speed)
    {
        _speed = speed;
    }

    public override void InstallBindings()
    {
        Container.Bind<ShipFacade>().AsSingle();
        Container.Bind<Transform>().FromComponentOnRoot();
        Container.BindInterfacesTo<ShipInputHandler>().AsSingle();
        Container.BindInstance(_speed).WhenInjectedInto<ShipInputHandler>();
        Container.Bind<ShipHealthHandler>().FromNewComponentOnRoot().AsSingle();
    }
}
```

Note the following changes:
- In GameInstaller, we are now using ByNewPrefabInstaller instead of ByNewContextPrefab.  This will automatically add the GameObjectContext on to the given prefab, and then attach the given installer to it.  This allows us to make the ShipInstaller type Installer instead of MonoInstaller
- Since we are no longer using MonoBehaviour's we no longer have access to the Transform, so we have to add a binding for this as well.  To do this we use FromComponentOnRoot, which will grab the transform from the root of the context which in this case will be the root of the newly instantiated prefab
- The only exception is ShipHealthHandler, which still needs to be a MonoBehaviour because it uses OnGUI to render to the screen, so we have to use FromNewComponentOnRoot in that case
- We can now use constructor injection instead of field/method injection for all our classes

Another benefit to this approach compared to the initial approach we took is that it can be easier to follow in some ways purely by reading the code.  You can read GameInstaller and see that it creates a subcontainer using ShipInstaller, and then you can read ShipInstaller to see all the dependencies that are inside the subcontainer.  When using ByNewContextPrefab, we would have to leave the code and go back to unity, then find the prefab and check which installers are on it, and also look through the hierarchy for ZenjectBinding components, which can be much more difficult to follow

## Using ByInstaller / ByMethod with Kernel

In some cases you might not want to use GameObjectContext at all and instead just use ByInstaller or ByMethod like in the <a href="#hello-world-for-facades">Hello World example</a> above.  You might also want to use interfaces such as ITickable / IInitializable / IDisposable inside your subcontainer.  However, unlike when using GameObjectContext, this doesn't work out-of-the-box.

For example, you might try doing this:

```csharp
public class GoodbyeHandler : IDisposable
{
    public void Dispose()
    {
        Debug.Log("Goodbye World!");
    }
}

public class HelloHandler : IInitializable
{
    public void Initialize()
    {
        Debug.Log("Hello world!");
    }
}

public class Greeter
{
    public Greeter()
    {
        Debug.Log("Created Greeter!");
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Greeter>().FromSubContainerResolve().ByMethod(InstallGreeter).AsSingle().NonLazy();
    }

    void InstallGreeter(DiContainer subContainer)
    {
        subContainer.Bind<Greeter>().AsSingle();

        subContainer.BindInterfacesTo<GoodbyeHandler>().AsSingle();
        subContainer.BindInterfacesTo<HelloHandler>().AsSingle();
    }
}
```

However, while we will find that our `Greeter` class is created (due to the fact we're using `NonLazy`) and the text "Created Greeter!" is printed to the console, the Hello and Goodbye messages are not.  This is because events such as Initialize / Tick / Dispose are not automatically forwarded to ByInstaller/ByMethod subcontainers by default.  So one way to fix this would be to change it to the following:

```csharp
public class Greeter : Kernel
{
    public Greeter()
    {
        Debug.Log("Created Greeter");
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Greeter>()
            .FromSubContainerResolve().ByMethod(InstallGreeter).AsSingle();
    }
}
```

Now if we run it, we should see the Hello message, then if we stop playing we should see the Goodbye message.

The reason this works is because we are now binding `IInitializable`, `IDisposable`, and `ITickable` on the root container to the Greeter class by executing `Container.BindInterfacesAndSelfTo<Greeter>()`.  Greeter inherits from Kernel, which inherits from all these interfaces and also handles forwarding these calls to the IInitializable's / ITickable's / IDisposable's in our sub container.  Note that we no longer need to use NonLazy here because any bindings to these interfaces are always created.

The Initialize / Tick / Dispose events work out-of-the-box for GameObjectContext and SceneContext because in those cases a kernel is added automatically.  It is only for these non-MonoBehaviour subcontainers that we need to be explicit about whether a kernel should be added or not.

Note also that we can control the tick priority of our subcontainer by changing the execution order for the Greeter class like this:

```csharp
public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Greeter>()
            .FromSubContainerResolve().ByMethod(InstallGreeter).AsSingle();

        Container.BindExecutionOrder<Greeter>(-1);
    }
}
```

This approach of deriving from Kernel works, however, it can also add some extra weight to our Facade class that we don't want.  As an alternative, you can also use the bind method WithKernel like this:

```csharp
public class Greeter
{
    public Greeter()
    {
        Debug.Log("Created Greeter");
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Greeter>()
            .FromSubContainerResolve().ByMethod(InstallGreeter).WithKernel().AsSingle();
    }

    void InstallGreeter(DiContainer subContainer)
    {
        subContainer.Bind<Greeter>().AsSingle();

        subContainer.BindInterfacesTo<GoodbyeHandler>().AsSingle();
        subContainer.BindInterfacesTo<HelloHandler>().AsSingle();
    }
}
```

Using this approach, you do not need to derive from Kernel and also use BindInterfacesAndSelfTo.  You can simply add WithKernel to the bind statement and the Initialize / Tick / Dispose events will be triggered properly inside your subcontainer.

When using WithKernel like this, you can also call another bind method that will define a default transform parent for any game objects that are instantiated inside the sub container.  For example:

```csharp
public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Greeter>().FromSubContainerResolve().ByMethod(InstallGreeter).WithKernel().WithDefaultGameObjectParent("Greeter").AsSingle();
    }
}
```

Now, if we instantiate any game objects inside the Greeter subcontainer and we do not specify an explicit parent, then they will be placed underneath a new game object named "Greeter".  This can help for organization purposes when reading the scene in the scene heirarchy.

Note also that with this approach, you can no longer use `BindExecutionOrder<Greeter>` to adjust the execution order of the subcontainer.  However, if you later decide that you need to customize the execution order then you can do that by passing a custom kernel-derived class to the WithKernel method and then using BindExecutionOrder with that class.  For example:

```csharp
public class GreeterKernel : Kernel
{
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Greeter>().FromSubContainerResolve().ByMethod(InstallGreeter).WithKernel<GreeterKernel>().AsSingle();
        Container.BindExecutionOrder<GreeterKernel>(-1);
    }
}
```

## Using ByInstaller / ByMethod with Kernel and BindFactory

You might want to define a subcontainer using ByInstaller or ByMethod but create it dynamically, and you might also want to use the interfaces such as IInitializable / ITickable / IDisposable inside this subcontainer. Starting with the example in the previous section, you might change it to this:

```csharp
public class Runner : IInitializable
{
    readonly Greeter.Factory _greeterFactory;

    Greeter _greeter;

    public Runner(Greeter.Factory greeterFactory)
    {
        _greeterFactory = greeterFactory;
    }

    public void Initialize()
    {
        _greeter = _greeterFactory.Create();
    }
}

public override void InstallBindings()
{
    Container.BindInterfacesTo<Runner>().AsSingle();

    Container.BindFactory<Greeter, Greeter.Factory>()
        .FromSubContainerResolve().ByMethod(InstallGreeter).AsSingle().NonLazy();
}
```

If you run this, you should see the "Created Greeter" message but no message for Hello or Goodbye.  The problem is that in this case nothing is triggering these events.  In the non-factory example above, these events were triggered from the parent container because we were using BindInterfacesAndSelfTo for the Greeter binding.  So for dynamically created subcontainers, if we need these events to be triggered we have to not only derive our facade from Kernel but we have to call them explicitly as well, like this:

```csharp
public class Runner : IInitializable, IDisposable, ITickable
{
    readonly Greeter.Factory _greeterFactory;

    Greeter _greeter;

    public Runner(Greeter.Factory greeterFactory)
    {
        _greeterFactory = greeterFactory;
    }

    public void Initialize()
    {
        _greeter = _greeterFactory.Create();
        _greeter.Initialize();
    }

    public void Dispose()
    {
        _greeter.Dispose();
    }

    public void Tick()
    {
        _greeter.Tick();
    }
}
```

