# Factories

## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Dynamically Creating Objects with Factories](#dynamically-creating-objects-with-factories)
- [Introduction:](#introduction)
  - [Theory](#theory)
  - [Example](#example)
  - [Binding Syntax](#binding-syntax)
  - [Abstract Factories](#abstract-factories)
- [Advanced:](#advanced)
  - [Custom Factories](#custom-factories)
  - [Using IFactory directly](#using-ifactory-directly)
  - [Custom Factory Interface](#custom-factory-interface)
  - [Prefab Factory](#prefab-factory)
  - [Implementing IValidatable](#implementing-ivalidatable)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Dynamically Creating Objects with Factories

One of the things that often confuses people new to dependency injection is the question of how to create new objects dynamically, after the app/game has fully started up.  For example, if you are writing a game in which you are spawning new enemies throughout the game, then you will want to construct new instances of the 'Enemy' class, and you will want to ensure that this object gets injected with dependencies just like all the objects that are part of the initial object graph.  The recommended way to achieve this is to use Factories.

Similar to the main documentation, I recommend at least reading the Introduction section and then skipping around in Advanced if necessary

## Introduction:

### Theory

Remember that an important part of dependency injection is to reserve use of the container to strictly the "Composition Root Layer".  The container class `(DiContainer)` is included as a dependency in itself automatically so there is nothing stopping you from ignoring this rule and injecting the container into any classes that you want.  For example, the following code will work:

```csharp
public class Enemy
{
    DiContainer Container;

    public Enemy(DiContainer container)
    {
        Container = container;
    }

    public void Update()
    {
        ...
        var player = Container.Resolve<Player>();
        WalkTowards(player.Position);
        ...
        etc.
    }
}
```

However, the above code is an example of an anti-pattern.  This will work, and you can use the container to get access to all other classes in your app, however if you do this you will not really be taking advantage of the power of dependency injection.  This is known, by the way, as [Service Locator Pattern](https://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/).

Note that the only exception to this rule is within factories and installers.  Again, factories and installers make up what we refer to as the "composition root layer".

Of course, the dependency injection way of doing this would be the following:

```csharp
public class Enemy
{
    Player _player;

    public Enemy(Player player)
    {
        _player = player;
    }

    public void Update()
    {
        ...
        WalkTowards(_player.Position);
        ...
    }
}
```

But now, every place that needs to create a new `Enemy` instance needs to also supply an instance of `Player`, and we are back at the problem mentioned <a href="../README.md#theory">in the main theory section</a>.  So to address this, factories must be used to create every dynamic instance to ensure that these extra dependencies are filled in by zenject.

### Example

The recommended way to do this in Zenject is the following:

```csharp
public class Player
{
}

public class Enemy
{
    readonly Player _player;

    public Enemy(Player player)
    {
        _player = player;
    }

    public class Factory : PlaceholderFactory<Enemy>
    {
    }
}

public class EnemySpawner : ITickable
{
    readonly Enemy.Factory _enemyFactory;

    public EnemySpawner(Enemy.Factory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public void Tick()
    {
        if (ShouldSpawnNewEnemy())
        {
            var enemy = _enemyFactory.Create();
            // ...
        }
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<EnemySpawner>().AsSingle();
        Container.Bind<Player>().AsSingle();
        Container.BindFactory<Enemy, Enemy.Factory>();
    }
}
```

By using `Enemy.Factory` above instead of `new Enemy`, all the dependencies for the Enemy class (such as the Player) will be automatically filled in.

We can also add runtime parameters to our factory.  For example, let's say we want to randomize the speed of each Enemy to add some interesting variation to our game.  Our enemy class becomes:

```csharp
public class Enemy
{
    readonly Player _player;
    readonly float _speed;

    public Enemy(float speed, Player player)
    {
        _player = player;
        _speed = speed;
    }

    public class Factory : PlaceholderFactory<float, Enemy>
    {
    }
}

public class EnemySpawner : ITickable
{
    readonly Enemy.Factory _enemyFactory;

    public EnemySpawner(Enemy.Factory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public void Tick()
    {
        if (ShouldSpawnNewEnemy())
        {
            var newSpeed = Random.Range(MIN_ENEMY_SPEED, MAX_ENEMY_SPEED);
            var enemy = _enemyFactory.Create(newSpeed);
            // ...
        }
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<EnemySpawner>().AsSingle();
        Container.Bind<Player>().AsSingle();
        Container.BindFactory<float, Enemy, Enemy.Factory>();
    }
}
```

The dynamic parameters that are provided to the `Enemy` constructor are declared by providing extra generic arguments to the `PlaceholderFactory<>` base class of `Enemy.Factory`.  `PlaceholderFactory<>` contains a `Create` method with the given parameter types, which can then be called by other classes such `EnemySpawner`.

`Enemy.Factory` is always intentionally left empty and simply derives from the built-in Zenject `PlaceholderFactory<>` class, which handles the work of using the DiContainer to construct a new instance of `Enemy`.  It is called `PlaceholderFactory` because it doesn't actually control how the object is created directly.  The way that the object is created is declared in an installer in the same way it is declared for non-factory dependencies.  For example, if our `Enemy` class was a MonoBehaviour on a prefab, we could install it like this instead:

```csharp
public class Enemy : MonoBehaviour
{
    Player _player;

    // Note that we can't use a constructor anymore since we are a MonoBehaviour now
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    public class Factory : PlaceholderFactory<Enemy>
    {
    }
}

public class TestInstaller : MonoInstaller
{
    public GameObject EnemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<EnemySpawner>().AsSingle();
        Container.Bind<Player>().AsSingle();
        Container.BindFactory<Enemy, Enemy.Factory>().FromComponentInNewPrefab(EnemyPrefab);
    }
}
```

And similarly if you want to instantiate your dynamic object via `FromMethod`, `FromNewComponentOnNewGameObject`, `FromInstance`, `FromSubContainerResolve`, etc. (see <a href="../README.md#binding">binding section</a> for full details)

Using `FromSubContainerResolve` can be particularly useful if your dynamically created object has a lot of its own dependencies.  You can have it behave like a "Facade"  (see the <a href="SubContainers.md">subcontainers section</a> for details on nested containers / facades)

<a id="using-factories-directly"></a>
There is no requirement that the `Enemy.Factory` class be a nested class within `Enemy,` however we have found this to be a very useful convention.  In both of the above examples we could install it like this instead, and bypass the need for a nested factory class:

```
Container.BindFactory<Enemy, PlaceholderFactory<Enemy>>()
```

However, this comes with several drawbacks:

1.  Changing the parameter list is not detected at compile time.  If the `PlaceholderFactory<Enemy>` is injected directly all over the code base, when we add a speed parameter and therefore change it to `PlaceholderFactory<float, Enemy>`, then we will get runtime errors when zenject fails to find the `PlaceholderFactory<Enemy>` dependency (or validation errors if you use validation).   However, if we use a derived `Enemy.Factory` class, then if we later decide to derive from `PlaceholderFactory<float, Enemy>` instead, we will get compiler errors instead (at every place that calls the `Create` method) which is easier to catch

2.  It's less verbose.  Injecting `Enemy.Factory` everywhere is much more readable than `PlaceholderFactory<float, Enemy>`, especially as the parameter list grows.

This is why we recommend this convention of using a nested factory class instead.

Other things to be aware of:

- Validation can be especially useful for dynamically created objects, because otherwise you may not catch the error until the factory is invoked at some point during runtime  (see the <a href="../README.md#object-graph-validation">validation section</a> for more details on validation)

- Note that for dynamically instantiated MonoBehaviours (for example when using `FromComponentInNewPrefab` with `BindFactory)` injection should always occur before `Awake` and `Start`, so a common convention we recommend is to use `Awake`/`Start` for initialization logic and use the inject method strictly for saving dependencies (ie. similar to constructors for non-monobehaviours)

- Unlike non-factory injection, you can have multiple runtime parameters declared with the same type.  In this case, the order that the values are given to the factory will be matched to the parameter order - assuming that you are using constructor or method injection.  However, note that this is not the case with field or property injection.  In those cases the order that values are injected is not guaranteed to follow the declaration order, since these fields are retrieved using `Type.GetFields` which does not guarantee order as described <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.getfields?redirectedfrom=MSDN&view=netcore-3.1#System_Type_GetFields">here</a>

### Binding Syntax

<pre>
Container.BindFactory&lt;<b>ContractType</b>, <b>PlaceholderFactoryType</b>&gt;()
    .WithId(<b>Identifier</b>)
    .WithFactoryArguments(<b>Factory Arguments</b>)
    .To&lt;<b>ResultType</b>&gt;()
    .From<b>ConstructionMethod</b>()
    .As<b>Scope</b>()
    .WithArguments(<b>Arguments</b>)
    .OnInstantiated(<b>InstantiatedCallback</b>)
    .When(<b>Condition</b>)
    .NonLazy()
    .(<b>Copy</b>|<b>Move</b>)Into(<b>All</b>|<b>Direct</b>)SubContainers();
</pre>

Where:

* **ContractType** = The contract type returned from the factory `Create` method

* **PlaceholderFactoryType** = The class deriving from `PlaceholderFactory<>`

* **WithFactoryArguments** = If you want to inject extra arguments into your placeholder factory derived class, you can include them here.  Note that `WithArguments` applies to the actual instantiated type and not the factory.

* **Scope** = Note that unlike for non-factory bindings, the default is AsCached instead of AsTransient, which is almost always what you want for factories, so in most cases you can leave this unspecified.

Other bind methods have the same functionality as <a href="../README.md#binding">non factory bindings</a>.

### Abstract Factories

The above description of factories is great for most cases, however, there are times you do not want to depend directly on a concrete class and instead want your factory to return an interface instead.  This kind of factory is called an Abstract Factory.

Let's create an example scenario, where we have multiple different implementations of a given interface:

```csharp

public interface IPathFindingStrategy
{
    ...
}

public class AStarPathFindingStrategy : IPathFindingStrategy
{
    ...
}

public class RandomPathFindingStrategy : IPathFindingStrategy
{
    ...
}
```

For the sake of this example, let's also assume that we have to create the instance of IPathFindingStrategy at runtime.  Otherwise it would be as simple as executing `Container.Bind<IPathFindingStrategy>().To<TheImplementationWeWant>().AsSingle();` in one of our installers.

This is done in a very similar way that non-Abstract factories work.  One difference is that we can't include the factory as a nested class inside the interface (not allowed in C#) but otherwise it's no different:

```csharp
public class PathFindingStrategyFactory : PlaceholderFactory<IPathFindingStrategy>
{
}

public class GameController : IInitializable
{
    PathFindingStrategyFactory _strategyFactory;
    IPathFindingStrategy _strategy;

    public GameController(PathFindingStrategyFactory strategyFactory)
    {
        _strategyFactory = strategyFactory;
    }

    public void Initialize()
    {
        _strategy = _strategyFactory.Create();
        // ...
    }
}

public class GameInstaller : MonoInstaller
{
    public bool UseAStar;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameController>().AsSingle();

        if (UseAStar)
        {
            Container.BindFactory<IPathFindingStrategy, PathFindingStrategyFactory>().To<AStarPathFindingStrategy>();
        }
        else
        {
            Container.BindFactory<IPathFindingStrategy, PathFindingStrategyFactory>().To<RandomPathFindingStrategy>();
        }
    }
}
```
## Advanced:

### Custom Factories

*Ok, but what if I don't know what type I want to create until after the application has started?  Or what if I have special requirements for constructing instances of the Enemy class that are not covered by any of the construction methods?* 

In these cases you can create what we call a 'custom factory', and then directly call `new Enemy` or use the <a href="../README.md#dicontainer-methods">methods on DiContainer</a>, or use any method you need to create your object.  For example, continuing the previous factory example, let's say that you wanted to be able to change a runtime value (difficulty) that determines what kinds of enemies get created.

```csharp
public enum Difficulties
{
    Easy,
    Hard,
}

public interface IEnemy
{
}

public class EnemyFactory : PlaceholderFactory<IEnemy>
{
}

public class Demon : IEnemy
{
}

public class Dog : IEnemy
{
}

public class DifficultyManager
{
    public Difficulties Difficulty
    {
        get;
        set;
    }
}

public class CustomEnemyFactory : IFactory<IEnemy>
{
    DiContainer _container;
    DifficultyManager _difficultyManager;

    public CustomEnemyFactory(DiContainer container, DifficultyManager difficultyManager)
    {
        _container = container;
        _difficultyManager = difficultyManager;
    }

    public IEnemy Create()
    {
        if (_difficultyManager.Difficulty == Difficulties.Hard)
        {
            return _container.Instantiate<Demon>();
        }

        return _container.Instantiate<Dog>();
    }
}

public class GameController : IInitializable
{
    readonly EnemyFactory _enemyFactory;

    public GameController(EnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public void Initialize()
    {
        var enemy = _enemyFactory.Create();
        // ...
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameController>().AsSingle();
        Container.Bind<DifficultyManager>().AsSingle();
        Container.BindFactory<IEnemy, EnemyFactory>().FromFactory<CustomEnemyFactory>();
    }
}
```

In other words, create a new class that derives from `IFactory<Enemy>` and then use the `FromFactory` method in a binding to hook it up.

You could also directly call `new Dog()` and `new Demon()` here instead of using the DiContainer (though in that case `Dog` and `Demon` would not have their members injected).

Note that `FromFactory<CustomEnemyFactory>()` is really shorthand for `FromIFactory(b => b.To<CustomEnemyFactory>().AsCached());` as explained in <a href="../README.md#binding">binding section</a>.  Using `FromIFactory` instead of `FromFactory` is a more powerful way of specifying custom factories because the custom factory can be created using any construction method you want, including `FromSubContainerResolve`, `FromInstance`, `FromComponentInNewPrefab`, etc.

One problem with our `CustomEnemyFactory` above is that it doesn't get validated correctly.  If we add dependencies to the `Demon` or `Dog` classes, and those dependencies are not bound in any installers, then we will not find out until runtime.  So unless we test every difficulty level, it might take some time before becoming aware of this problem.

So a better way to do this would be the following:

```csharp
public class CustomEnemyFactory : IFactory<IEnemy>
{
    Dog.Factory _dogFactory;
    Demon.Factory _demonFactory;
    DifficultyManager _difficultyManager;

    public CustomEnemyFactory(
        DifficultyManager difficultyManager, Dog.Factory dogFactory, Demon.Factory demonFactory)
    {
        _dogFactory = dogFactory;
        _demonFactory = demonFactory;
        _difficultyManager = difficultyManager;
    }

    public IEnemy Create()
    {
        if (_difficultyManager.Difficulty == Difficulties.Hard)
        {
            return _demonFactory.Create();
        }

        return _dogFactory.Create();
    }
}
```

With the above change, any dependencies that are missing from the demon or dog constructor parameter list will be caught during validation, instead of at runtime.

Note that if you insist on using the DiContainer methods directly, you can still validate the dependencies you require by making your factory implement `IValidatable` as explained <a href="#implementing-validatable">here</a>.

### Using IFactory directly

If you don't want to define any extra factory classes at all, you can inject `IFactory<>` directly into any using classes, and then use the `BindIFactory` method to hook it up to a construction method.  To re-use the above example, that would look like this:

```csharp

public class GameController : IInitializable
{
    IFactory<IPathFindingStrategy> _strategyFactory;
    IPathFindingStrategy _strategy;

    public GameController(IFactory<IPathFindingStrategy> strategyFactory)
    {
        _strategyFactory = strategyFactory;
    }

    public void Initialize()
    {
        _strategy = _strategyFactory.Create();
        // ...
    }
}

public class GameInstaller : MonoInstaller
{
    public bool UseAStar;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameController>().AsSingle();

        if (UseAStar)
        {
            Container.BindIFactory<IPathFindingStrategy>().To<AStarPathFindingStrategy>();
        }
        else
        {
            Container.BindIFactory<IPathFindingStrategy>().To<RandomPathFindingStrategy>();
        }
    }
}
```

This can be simpler than deriving from `PlaceholderFactory` in some cases, however, it has the <a href="#using-factories-directly">same problems</a> that are mentioned above when using `PlaceholderFactory` directly (that is, it is more error prone when changing the parameter list, and it can be more verbose in some cases)

### Custom Factory Interface

In some cases, you might want to avoid becoming directly coupled to the factory class, and would prefer to use a base class or a custom interface instead.  You can do that by using the `BindFactoryCustomInterface` method instead of `BindFactory` like this:

```csharp

public interface IMyFooFactory : IFactory<Foo>
{
}

public class Foo
{
    public class Factory : PlaceholderFactory<Foo>, IMyFooFactory
    {
    }
}

public class Runner : IInitializable
{
    readonly IMyFooFactory _fooFactory;

    public Runner(IMyFooFactory fooFactory)
    {
        _fooFactory = fooFactory;
    }

    public void Initialize()
    {
        var foo = _fooFactory.Create();
        // ...
    }
}

public class FooInstaller : MonoInstaller<FooInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactoryCustomInterface<Foo, Foo.Factory, IMyFooFactory>();
    }
}
```

Note that there is an equivalent method for memory pools called `BindMemoryPoolCustomInterface` as well

### Prefab Factory

In some cases you might want the code that is calling the Create method to also provide the prefab to use for the new object.  You could directly call `DiContainer.InstantiatePrefabForComponent` but this would violate our rule of only injecting DiContainer into the 'composition root layer' (ie. factories and installers), so it would be better to write a custom factory like this instead:

```csharp
public class Foo
{
    public class Factory : PlaceholderFactory<UnityEngine.Object, Foo>
    {
    }
}

public class FooFactory : IFactory<UnityEngine.Object, Foo>
{
    readonly DiContainer _container;

    public FooFactory(DiContainer container)
    {
        _container = container;
    }

    public Foo Create(UnityEngine.Object prefab)
    {
        return _container.InstantiatePrefabForComponent<Foo>(prefab);
    }
}

public override void InstallBindings()
{
    Container.BindFactory<UnityEngine.Object, Foo, Foo.Factory>().FromFactory<FooFactory>();
}
```

However, this kind of custom factory is common enough that there is a helper class included for this purpose called PrefabFactory.  So you could just do this instead:

```csharp
public class Foo
{
    public class Factory : PlaceholderFactory<UnityEngine.Object, Foo>
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public GameObject Prefab;

    public override void InstallBindings()
    {
        Container.BindFactory<UnityEngine.Object, Foo, Foo.Factory>().FromFactory<PrefabFactory<Foo>>();
    }
}
```

A similar helper class is provided when instantiating a prefab from a resource path.  For example:

```csharp
public class Foo
{
    public class Factory : PlaceholderFactory<string, Foo>
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public GameObject Prefab;

    public override void InstallBindings()
    {
        Container.BindFactory<string, Foo, Foo.Factory>().FromFactory<PrefabResourceFactory<Foo>>();
    }
}
```

One thing to be aware of when using PrefabResource or PrefabResourceFactory is that validation does not run in those cases.  So if our Foo class above was missing a dependency then we would not find this out until run time.  This is not possible because the prefab is needed for validation.

### Implementing IValidatable

If you do need to use the DiContainer instantiate methods directly, but you still want to validate the dynamically created object graphs, you can still do that, by implementing the `IValidatable` interface.  To re-use the same example from above, that would look like this:

```csharp
public class CustomEnemyFactory : IFactory<IEnemy>, IValidatable
{
    DiContainer _container;
    DifficultyManager _difficultyManager;

    public CustomEnemyFactory(DiContainer container, DifficultyManager difficultyManager)
    {
        _container = container;
        _difficultyManager = difficultyManager;
    }

    public IEnemy Create()
    {
        if (_difficultyManager.Difficulty == Difficulties.Hard)
        {
            return _container.Instantiate<Demon>();
        }

        return _container.Instantiate<Dog>();
    }

    public void Validate()
    {
        _container.Instantiate<Dog>();
        _container.Instantiate<Demon>();
    }
}

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<IEnemy, EnemyFactory>().FromFactory<CustomEnemyFactory>();
    }
}
```

Note that it is not necessary to bind the `IValidatable` interface to our factory.  Simply by implementing the `IValidatable` interface, and also having our factory be part of the object graph, is enough for the `Validate` method to get called.

Within the `Validate` method, to manually validate dynamic object graphs, you simply instantiate them.  Note that this will not actually instantiate these objects (these calls actually return null here).  The point is to do a "dry run" without actually instantiating anything, to prove out the full object graph.  For more details on validation see the <a href="../README.md#object-graph-validation">validation section</a>.

