# Memory Pools

## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Introduction](#introduction)
  - [Example](#example)
  - [Binding Syntax](#binding-syntax)
  - [Resetting Items In Pool](#resetting-items-in-pool)
  - [Runtime Parameters](#runtime-parameters)
  - [Factories, Pools, and the Dispose Pattern](#factories-pools-and-the-dispose-pattern)
  - [Memory Pools for GameObjects](#memory-pools-for-gameobjects)
- [Advanced](#advanced)
  - [Static Memory Pools](#static-memory-pools)
  - [Using statements and dispose pattern](#using-statements-and-dispose-pattern)
  - [List Pool](#list-pool)
  - [Dispose Block](#dispose-block)
  - [PoolableMemoryPool](#poolablememorypool)
  - [Abstract Memory Pools](#abstract-memory-pools)
  - [Subcontainers/Facades And Memory Pools](#subcontainersfacades-and-memory-pools)
  - [Instantiating Memory Pools Directly](#instantiating-memory-pools-directly)
  - [Pool Cleanup Checker](#pool-cleanup-checker)
  - [Memory Pool Monitor](#memory-pool-monitor)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Introduction

### Example

Before understanding memory pools it would be helpful to understand factories, so please read <a href="Factories.md">the introduction to factories</a> first.

It doesn't take long when developing games in Unity before you realize that proper memory management is very important if you want your game to run smoothly (especially on mobile).  Depending on the constraints of the platform and the type of game you are working on, it might be very important to avoid unnecessary heap allocations as much as possible.  One very effective way to do this is to use memory pools.

As an example let's look at at a case where we are dynamically creating a class:

```csharp
public class Foo
{
    public class Factory : PlaceholderFactory<Foo>
    {
    }
}

public class Bar
{
    readonly Foo.Factory _fooFactory;
    readonly List<Foo> _foos = new List<Foo>();

    public Bar(Foo.Factory fooFactory)
    {
        _fooFactory = fooFactory;
    }

    public void AddFoo()
    {
        _foos.Add(_fooFactory.Create());
    }

    public void RemoveFoo()
    {
        _foos.RemoveAt(0);
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();
        Container.BindFactory<Foo, Foo.Factory>();
    }
}
```

Here, every time we call `Bar.AddFoo` it will always allocate new heap memory. And every time we call `Bar.RemoveFoo`, the `Bar` class will stop referencing one of the instances of `Foo,` and therefore the memory for that instance will be marked for garbage collection.  If this happens enough times, then eventually the garbage collector will kick in and you will get a (sometimes very noticeable) spike in your game.

We can fix this spike by using memory pools instead:

```csharp
public class Foo
{
    public class Pool : MemoryPool<Foo>
    {
    }
}

public class Bar
{
    readonly Foo.Pool _fooPool;
    readonly List<Foo> _foos = new List<Foo>();

    public Bar(Foo.Pool fooPool)
    {
        _fooPool = fooPool;
    }

    public void AddFoo()
    {
        _foos.Add(_fooPool.Spawn());
    }

    public void RemoveFoo()
    {
        var foo = _foos[0];
        _fooPool.Despawn(foo);
        _foos.Remove(foo);
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();
        Container.BindMemoryPool<Foo, Foo.Pool>();
    }
}
```

As you can see, this works very similarly to factories, except that the terminology is a bit different (`Pool` instead of `Factory`, `Spawn` instead of `Create`) and unlike factories, you have to return the instance to the pool rather than just stop referencing it.

With this new implementation above, there will be some initial heap allocation with every call to `AddFoo()`, but if you call `RemoveFoo()` then `AddFoo()` in sequence this will re-use the previous instance and therefore save you a heap allocation.

This is better, but we might still want to avoid the spikes from the initial heap allocations as well.  One way to do this is to do all the heap allocations all at once as your game is starting up:

```csharp
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();
        Container.BindMemoryPool<Foo, Foo.Pool>().WithInitialSize(10);
    }
}
```

When we use `WithInitialSize` like this in the Bind statement for our pool, 10 instances of `Foo` will be created immediately on startup to seed the pool.

### Binding Syntax

The syntax for memory pools are almost identical to factories, with a few new bind methods such as `WithInitialSize` and `ExpandBy`.  Also, unlike `BindFactory`, it is not necessary to specify the parameters to the factory as generic arguments to `BindMemoryPool`

Just like with factories, the recommended convention is to use a public nested class named Pool (though this is just a convention)

```csharp
public class Foo
{
    public class Pool : MemoryPool<Foo>
    {
    }
}
```

Parameters are added by adding generic arguments:

```csharp
public class Foo
{
    public class Pool : MemoryPool<string, int, Foo>
    {
    }
}
```

The full format of the binding is the following:

<pre>
Container.BindMemoryPool&lt;<b>ObjectType, MemoryPoolType</b>&gt;()
    .With<b>(InitialSize|FixedSize)</b>
    .WithMaxSize(<b>MaxSize</b>)
    .ExpandBy<b>(OneAtATime|Doubling)</b>()
    .WithFactoryArguments(<b>Factory Arguments</b>)
    .To&lt;<b>ResultType</b>&gt;()
    .WithId(<b>Identifier</b>)
    .From<b>ConstructionMethod</b>()
    .As<b>Scope</b>()
    .WithArguments(<b>Arguments</b>)
    .OnInstantiated(<b>InstantiatedCallback</b>)
    .When(<b>Condition</b>)
    .CopyIntoAllSubContainers()
    .NonLazy();
</pre>

Where:

* **InitialSize** - This will determine how many items are seeded to the pool on startup.   It can be helpful to set a value here to avoid spikes caused by allocations during gameplay.

* **FixedSize** - When set, the pool will initially be seeded with the amount given here, and if that amount is exceeded then exceptions will be thrown.

* **MaxSize** - When set, if enough items are returned to the pool that the pool size exceeds this value, then the remaining items will be destroyed. This can be useful to ensure that the pool takes a maximum amount of memory.

* **ObjectType** = The type of the class that is being instantiated by the memory pool

* **MemoryPoolType** = The type of the MemoryPool derived class, which is often a nested class of `ObjectType`.

* **ExpandBy** = Determines the behaviour to invoke when the pool reaches its maximum size.  Note that when specifying `WithFixedSize`, this option is not available.  The options are:

    * **ExpandByOneAtATime** - Only allocate new instances one at a time as necessary
    * **ExpandByDoubling** - When the pool is full and a new instance is requested, the pool will double in size before returning the requested instance.  This approach can be useful if you prefer having large infrequent allocations to many small frequent allocations

* **WithFactoryArguments** = If you want to inject extra arguments into your MemoryPool derived class, you can include them here.  Note that `WithArguments` applies to the actual instantiated type and not the memory pool.

* **Scope** = Note that unlike for normal bindings, the default is AsCached instead of AsTransient, which is almost always what you want, so in most cases you can leave this unspecified.  It is only in some rare cases where you need to have unique pools for each class that uses the pool.

The rest of the bind methods behave the same as the normal bind methods documented <a href="../README.md#binding">here</a>

### Resetting Items In Pool

One very important thing to be aware of when using memory pools instead of factories is that you must make sure to completely "reset" the given instance.  This is necessary otherwise you might have state from a previous "life" of the instance bleeding in to the behaviour of the new instance.

You can reset the object by implementing any of the following methods in your memory pool derived class:

```csharp
public class Foo
{
    public class Pool : MemoryPool<Foo>
    {
        protected override void OnCreated(Foo item)
        {
            // Called immediately after the item is first added to the pool
        }

        protected override void OnDestroyed(Foo item)
        {
            // Called immediately after the item is removed from the pool without also being spawned
            // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
        }

        protected override void OnSpawned(Foo item)
        {
            // Called immediately after the item is removed from the pool
        }

        protected override void OnDespawned(Foo item)
        {
            // Called immediately after the item is returned to the pool
        }

        protected override void Reinitialize(Foo foo)
        {
            // Similar to OnSpawned
            // Called immediately after the item is removed from the pool
            // This method will also contain any parameters that are passed along
            // to the memory pool from the spawning code
        }
    }
}
```

In most cases, you will probably only have to implement the Reinitialize method.   For example, let's introduce some state to our first example by adding a Position value to `Foo`:

```csharp
public class Foo
{
    Vector3 _position = Vector3.zero;

    public void Move(Vector3 delta)
    {
        _position += delta;
    }

    public class Pool : MemoryPool<Foo>
    {
        protected override void Reinitialize(Foo foo)
        {
            foo._position = Vector3.zero;
        }
    }
}
```

Note that our pool class is free to access private variables inside `Foo` because of the fact that it is a nested class.

Or, if we wanted to avoid the duplication in `Foo` and `Foo.Pool`, we could do it this way:

```csharp
public class Foo
{
    Vector3 _position;

    public Foo()
    {
        Reset();
    }

    public void Move(Vector3 delta)
    {
        _position += delta;
    }

    void Reset()
    {
        _position = Vector3.zero;
    }

    public class Pool : MemoryPool<Foo>
    {
        protected override void Reinitialize(Foo foo)
        {
            foo.Reset();
        }
    }
}
```

### Runtime Parameters

Just like Factories, you can also pass runtime parameters when spawning new instances of your pooled classes.  The difference is, instead of the parameters being injected into the class, they are passed to the `Reinitialize` method:

```csharp
public class Foo
{
    Vector3 _position;
    Vector3 _velocity;

    public Foo()
    {
        Reset(Vector3.zero);
    }

    public void Tick()
    {
        _position += _velocity * Time.deltaTime;
    }

    void Reset(Vector3 velocity)
    {
        _position = Vector3.zero;
        _velocity = Vector3.zero;
    }

    public class Pool : MemoryPool<Vector3, Foo>
    {
        protected override void Reinitialize(Vector3 velocity, Foo foo)
        {
            foo.Reset(velocity);
        }
    }
}

public class Bar
{
    readonly Foo.Pool _fooPool;
    readonly List<Foo> _foos = new List<Foo>();

    public Bar(Foo.Pool fooPool)
    {
        _fooPool = fooPool;
    }

    public void AddFoo()
    {
        float maxSpeed = 10.0f;
        float minSpeed = 1.0f;

        _foos.Add(_fooPool.Spawn(
            Random.onUnitSphere * Random.Range(minSpeed, maxSpeed)));
    }

    public void RemoveFoo()
    {
        var foo = _foos[0];
        _fooPool.Despawn(foo);
        _foos.Remove(foo);
    }
}
```

### Factories, Pools, and the Dispose Pattern

The approach that is outlined above works fairly well but has the following drawbacks:

- Every time we make a class poolable we always need to add boilerplate code where we have to subclass `MemoryPool` and then call an instance method 'Reset' on our object, passing along any parameters to it.  It would be easier if this was automated somehow instead of duplicated for every pooled object.

- Any code that is spawning pooled objects has to maintain a reference to the pool class so that it can call the Despawn method.  This code doesn't really care about whether the object is pooled or not.  The fact that the object is pooled is more of an implementation detail, and therefore it would be better if this was abstracted away from the code that is using it.

- Every time we want to convert some non-pooled objects to use a pool we have to change a lot of code.  We have to remove the `PlaceholderFactory` derived class, and then change all the using code to call `Spawn` instead of `Create`, and then also remember to call `Despawn`

We can solve these problems by using `PlaceholderFactory` and the Dispose Pattern.  Any code can call the factory Create method just like for non-pooled objects and then call Dispose to automatically return the object to the pool.

For example:

```csharp
public class Foo : IPoolable<IMemoryPool>, IDisposable
{
    IMemoryPool _pool;

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public class Factory : PlaceholderFactory<Foo>
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactory<Foo, Foo.Factory>().FromPoolableMemoryPool(x => x.WithInitialSize(2));
    }
}
```

To accomplish this, we use a nested PlaceholderFactory derived class just like for non-pooled objects, and then implement the `IPoolable<IMemoryPool>` interface.  This will require that we define `OnSpawned` and `OnDespawned` methods which will handle the `Reset` logic that we used in previous examples.

We can then also implement `IDisposable` and then return ourselves to the given pool whenever `Dispose` is called.

Then when binding the factory, we can use the `FromPoolableMemoryPool` method to configure the pool with an initial seed value, max size, and expand method as well as the method that is used to construct the object.

<a id="aot-error"></a>
Note: If you encounter IL2CPP AOT issues with the above approach, you may have to make the memory pool class more explicit by doing the following instead:

```csharp
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactory<Foo, Foo.Factory>().FromPoolableMemoryPool<Foo, FooPool>(x => x.WithInitialSize(2));
    }

    public class FooPool : PoolableMemoryPool<IMemoryPool, Foo>
    {
    }
}
```

This occurs because sometimes IL2CPP will not include the `PoolableMemoryPool` automatically

### Memory Pools for GameObjects

Memory pools for GameObjects works similarly.  For example:

```csharp
public class Foo : MonoBehaviour
{
    Vector3 _velocity;

    [Inject]
    public void Construct()
    {
        Reset(Vector3.zero);
    }

    public void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    void Reset(Vector3 velocity)
    {
        transform.position = Vector3.zero;
        _velocity = velocity;
    }

    public class Pool : MonoMemoryPool<Vector3, Foo>
    {
        protected override void Reinitialize(Vector3 velocity, Foo foo)
        {
            foo.Reset(velocity);
        }
    }
}

public class Bar
{
    readonly Foo.Pool _fooPool;
    readonly List<Foo> _foos = new List<Foo>();

    public Bar(Foo.Pool fooPool)
    {
        _fooPool = fooPool;
    }

    public void AddFoo()
    {
        float maxSpeed = 10.0f;
        float minSpeed = 1.0f;

        _foos.Add(_fooPool.Spawn(
            Random.onUnitSphere * Random.Range(minSpeed, maxSpeed)));
    }

    public void RemoveFoo()
    {
        var foo = _foos[0];
        _fooPool.Despawn(foo);
        _foos.Remove(foo);
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public GameObject FooPrefab;

    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();

        Container.BindMemoryPool<Foo, Foo.Pool>()
            .WithInitialSize(2)
            .FromComponentInNewPrefab(FooPrefab)
            .UnderTransformGroup("Foos");
    }
}
```

The main difference here is that `Foo.Pool` now derives from `MonoMemoryPool` instead of `MemoryPool`.  `MonoMemoryPool` is a helper class that will automatically enable and disable the game object for us when it is added/removed from the pool.  The implementation for `MonoMemoryPool` is simply this:

```csharp
public abstract class MonoMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue>
    where TValue : Component
{
    Transform _originalParent;

    protected override void OnCreated(TValue item)
    {
        item.gameObject.SetActive(false);
        // Record the original parent which will be set to whatever is used in the UnderTransform method
        _originalParent = item.transform.parent;
    }

    protected override void OnDestroyed(TValue item)
    {
        GameObject.Destroy(item.gameObject);
    }

    protected override void OnSpawned(TValue item)
    {
        item.gameObject.SetActive(true);
    }

    protected override void OnDespawned(TValue item)
    {
        item.gameObject.SetActive(false);

        if (item.transform.parent != _originalParent)
        {
            item.transform.SetParent(_originalParent, false);
        }
    }
}
```

Therefore, if you override one of these methods you will have to make sure to call the base version as well.

Also, worth noting is the fact that for this logic to work, our MonoBehaviour must be at the root of the prefab, since otherwise only the transform associated with `Foo` and any children will be disabled.

You can also use the `Dispose` Pattern here using a similar approach outlined above for non-MonoBehaviours, which would look like this:

```csharp
public class Foo : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
{
    Vector3 _velocity;
    IMemoryPool _pool;

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    public void OnDespawned()
    {
        _pool = null;
        _velocity = Vector3.zero;
    }

    public void OnSpawned(Vector3 velocity, IMemoryPool pool)
    {
        transform.position = Vector3.zero;
        _pool = pool;
        _velocity = velocity;
    }

    public class Factory : PlaceholderFactory<Vector3, Foo>
    {
    }
}

public class Bar
{
    readonly Foo.Factory _fooFactory;
    readonly List<Foo> _foos = new List<Foo>();

    public Bar(Foo.Factory fooFactory)
    {
        _fooFactory = fooFactory;
    }

    public void AddFoo()
    {
        float maxSpeed = 10.0f;
        float minSpeed = 1.0f;

        var foo = _fooFactory.Create(
            Random.onUnitSphere * Random.Range(minSpeed, maxSpeed));

        foo.transform.SetParent(null);

        _foos.Add(foo);
    }

    public void RemoveFoo()
    {
        if (_foos.Any())
        {
            var foo = _foos[0];
            foo.Dispose();
            _foos.Remove(foo);
        }
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public GameObject FooPrefab;

    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();

        Container.BindFactory<Vector3, Foo, Foo.Factory>().FromMonoPoolableMemoryPool(
            x => x.WithInitialSize(2).FromComponentInNewPrefab(FooPrefab).UnderTransformGroup("FooPool"));
    }
}
```

Note that unlike in other examples, we derive from `PlaceholderFactory`, implement `IDisposable`, and we use `FromMonoPoolableMemoryPool` instead of `FromPoolableMemoryPool`.  Also be careful of potential <a href="#aot-error">AOT errors</a> with this approach.

## Advanced

### Static Memory Pools

Another approach to memory pools is to not bother installing the memory pool at all and instead store the pool statically using the `StaticMemoryPool` class.  For example:

```csharp
public class Foo
{
    public static readonly StaticMemoryPool<Foo> Pool =
        new StaticMemoryPool<Foo>(OnSpawned, OnDespawned);

    static void OnSpawned(Foo that)
    {
        // Initialize
    }

    static void OnDespawned(Foo that)
    {
        // Reset
    }
}

public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var foo = Foo.Pool.Spawn();

        // Use foo

        Foo.Pool.Despawn(foo);
    }
}
```

In this case, the pool is accessed directly as a static member of the `Foo` class.   This approach can be useful for objects that do not have dependencies, or for cases where you don't want to bother with always needing to install it everywhere.   However, something to be aware of is that unlike with normal memory pools, the objects in the pool will remain in memory even after changing scenes, unless the pool is cleared manually by calling `Foo.Pool.Clear`.

Once again you can also use the `Dispose` Pattern for this approach as well.  For example:

```csharp
public class Foo : IDisposable
{
    public static readonly StaticMemoryPool<Foo> Pool =
        new StaticMemoryPool<Foo>(OnSpawned, OnDespawned);

    public void Dispose()
    {
        Pool.Despawn(this);
    }

    static void OnSpawned(Foo that)
    {
        // Initialize
    }

    static void OnDespawned(Foo that)
    {
        // Reset
    }
}

public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var foo = Foo.Pool.Spawn();

        // Use foo

        foo.Dispose();
    }
}
```

You can also include runtime parameters in your pooled object using `StaticMemoryPool` using generic arguments, similar to normal memory pools:

```csharp
public class Foo : IDisposable
{
    public static readonly StaticMemoryPool<string, Foo> Pool =
        new StaticMemoryPool<string, Foo>(OnSpawned, OnDespawned);

    public string Value
    {
        get; private set;
    }

    public void Dispose()
    {
        Pool.Despawn(this);
    }

    static void OnSpawned(string value, Foo that)
    {
        that.Value = value;
    }

    static void OnDespawned(Foo that)
    {
        that.Value = null;
    }
}
```

Also, similar to normal memory pools, you can use the `IPoolable` interface in combination with `PoolableStaticMemoryPool` to avoid some boilerplate code and use instance methods instead of static methods:

```csharp
public class Foo : IPoolable<string>, IDisposable
{
    public static readonly PoolableStaticMemoryPool<string, Foo> Pool =
        new PoolableStaticMemoryPool<string, Foo>();

    public string Data
    {
        get; private set;
    }

    public void Dispose()
    {
        Pool.Despawn(this);
    }

    public void OnSpawned(string data)
    {
        Data = data;
    }

    public void OnDespawned()
    {
        Data = null;
    }
}
```

### Using statements and dispose pattern

There are several drawbacks to the following approach:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var foo = Foo.Pool.Spawn();

        // Use foo

        foo.Dispose();
    }
}
```

1.  We have to always remember to call `Dispose()` at the end of the method.  We could easily forget to do this and cause many allocations to occur per frame.

1.  If we want to have multiple return statements within the function, we have to duplicate the cleanup code for each case, which can be even more error prone

1.  If an exception occurs in between the `Spawn` and `Dispose`, then the object will not be returned to the pool.

An easy way to solve these problems would be to add a try-finally block:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var foo = Foo.Pool.Spawn();

        try
        {
            // Use foo
        }
        finally
        {
            foo.Dispose();
        }
    }
}
```

Or, equivalently, we could add a using statement:


```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var foo = Foo.Pool.Spawn())
        {
            // Use foo
        }
    }
}
```

These approaches guarantee that the Foo object will be returned to the pool, regardless of whether an exception is thrown or the method exits early.  This is another reason why using the `Dispose` pattern for memory pooled objects is useful.

### List Pool

Static memory pools are especially useful for common data structures such as lists or dictionaries.  Zenject includes some standard memory pools for this exact purpose which you can use.  For example, let's say you are writing a `MonoBehaviour` that needs to iterate over every component on a game object every frame.  You might implement it like this:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var components = this.GetComponents(typeof(Component));

        foreach (var component in components)
        {
            // Some logic
        }
    }
}
```

However, if you run a scene with this MonoBehaviour added to it, and open up the unity profiler, you will see that there is around 48 bytes allocated per frame.  We can get rid of that by using the `Zenject.ListPool` class and doing this instead:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        var components = ListPool<Component>.Instance.Spawn();

        this.GetComponents(typeof(Component), components);

        foreach (var component in components)
        {
            // Some logic
        }

        ListPool<Component>.Instance.Despawn(components);
    }
}
```

Zenject also includes `DictionaryPool`, `HashSetPool`, and `ArrayPool` classes that can be used similarly.

### Dispose Block

Zenject also provides the `DisposeBlock` class which is simply a collection of `IDisposable` objects that are disposed of all at once when `DisposeBlock.Dispose` is called.  It can also be useful when combined with the using statement for cases where you are allocating multiple temporary instances from the same pool or multiple pools.  For example, let's say we needed to spawn multiple temporary objects in our `PoolExample` class.  We could do it this way:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var foo = Foo.Pool.Spawn())
        using (var bar = Bar.Pool.Spawn())
        {
            // Some logic
        }
    }
}
```

This will work but is not scalable if we are spawning more than a few objects.  So a better alternative might be to use the DisposeBlock class instead like this:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var block = DisposeBlock.Spawn())
        {
            var foo = Foo.Pool.Spawn();
            var bar = Bar.Pool.Spawn();

            block.Add(foo);
            block.Add(bar);

            // Some logic
        }
    }
}
```

We can simplify this further by using the `DisposeBlock.Spawn` method:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var block = DisposeBlock.Spawn())
        {
            var foo = block.Spawn(Foo.Pool);
            var bar = block.Spawn(Bar.Pool);

            // Some logic
        }
    }
}
```

We can also use DisposeBlock to improve our `ListPool` example above and avoid the need to explicitly call Despawn:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var block = DisposeBlock.Spawn())
        {
            var components = block.Spawn(ListPool<Component>.Instance);

            this.GetComponents(typeof(Component), components);

            foreach (var component in components)
            {
                // Some logic
            }
        }
    }
}
```

Since spawning lists are such a common operation, DisposeBlock includes a helper method for it, so it can be simplified to this instead:

```csharp
public class PoolExample : MonoBehaviour
{
    public void Update()
    {
        using (var block = DisposeBlock.Spawn())
        {
            var components = block.SpawnList<Component>();

            this.GetComponents(typeof(Component), components);

            foreach (var component in components)
            {
                // Some logic
            }
        }
    }
}
```

### PoolableMemoryPool

If you prefer not to follow the dispose pattern explained above, but would also like to avoid the boilerplate code from the original approach using a `Reset` method, then you can do that too by using `PoolableMemoryPool` or `MonoPoolableMemoryPool`.

For example:

```csharp
public class Foo : IPoolable<string>
{
    public string Data
    {
        get; private set;
    }

    public void OnDespawned()
    {
        Data = null;
    }

    public void OnSpawned(string data)
    {
        Data = data;
    }

    public class Pool : PoolableMemoryPool<string, Foo>
    {
    }
}
```

The implementation of `PoolableMemoryPool` is very simple and just calls the instance methods on the `IPoolable` class:

```csharp
public class PoolableMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue>
    where TValue : IPoolable<TParam1>
{
    protected override void OnDespawned(TValue item)
    {
        item.OnDespawned();
    }

    protected override void Reinitialize(TParam1 p1, TValue item)
    {
        item.OnSpawned(p1);
    }
}
```

If you prefer, you could also make the `OnSpawned` and `OnDespawned` methods private by using the c# feature 'explicit interface implementation' which will only allow calling the `OnSpawned` and `OnDespawned` methods via the `IPoolable` interface:

```csharp
public class Foo : IPoolable<string>
{
    public string Data
    {
        get; private set;
    }

    void IPoolable<string>.OnDespawned()
    {
        Data = null;
    }

    void IPoolable<string>.OnSpawned(string data)
    {
        Data = data;
    }

    public class Pool : PoolableMemoryPool<string, Foo>
    {
    }
}
```

### Abstract Memory Pools

Just like <a href="Factories.md#abstract-factories">abstract factories</a>, sometimes you might want to create a memory pool that returns an interface, with the concrete type decided inside an installer.  This works very similarly to abstract factories.  For example:

```csharp
public interface IFoo
{
}

public class Foo1 : IFoo
{
}

public class Foo2 : IFoo
{
}

public class FooPool : MemoryPool<IFoo>
{
}

public class Bar
{
    readonly FooPool _fooPool;
    readonly List<IFoo> _foos = new List<IFoo>();

    public Bar(FooPool fooPool)
    {
        _fooPool = fooPool;
    }

    public void AddFoo()
    {
        _foos.Add(_fooPool.Spawn());
    }

    public void RemoveFoo()
    {
        var foo = _foos[0];
        _fooPool.Despawn(foo);
        _foos.Remove(foo);
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public bool Use1;

    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();

        if (Use1)
        {
            Container.BindMemoryPool<IFoo, FooPool>().WithInitialSize(10).To<Foo1>();
        }
        else
        {
            Container.BindMemoryPool<IFoo, FooPool>().WithInitialSize(10).To<Foo2>();
        }
    }
}
```

We might also want to add a `Reset()` method to the `IFoo` interface as well here, and call that on `Reinitialize()`

### Subcontainers/Facades And Memory Pools

You might wonder, if you are using dynamic subcontainers and facades, what is the best way to put the entire subcontainer in a pool?   Consider the following example:

```csharp
public class EnemyFacade : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    IMemoryPool _pool;

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public class Factory : PlaceholderFactory<EnemyFacade>
    {
    }
}

public class EnemyMoveHandler : MonoBehaviour
{
    Settings _settings;
    float _velocity;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _velocity += Time.deltaTime * _settings.Acceleration;
        }

        transform.position += Vector3.forward * _velocity * Time.deltaTime;
    }

    [Serializable]
    public class Settings
    {
        public float Acceleration;
    }
}

public class Runner : ITickable
{
    readonly EnemyFacade.Factory _enemyFactory;

    EnemyFacade _enemy;

    public Runner(EnemyFacade.Factory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _enemy == null)
        {
            _enemy = _enemyFactory.Create();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _enemy != null)
        {
            _enemy.Dispose();
            _enemy = null;
        }
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public EnemyMoveHandler.Settings EnemyMoveHandlerSettings;
    public GameObject EnemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(EnemyMoveHandlerSettings);
        Container.BindInterfacesTo<Runner>().AsSingle();

        Container.BindFactory<EnemyFacade, EnemyFacade.Factory>()
            .FromMonoPoolableMemoryPool<EnemyFacade>(b => b
                .WithInitialSize(1)
                .FromSubContainerResolve()
                .ByNewPrefabMethod(EnemyPrefab, InstallEnemy)
                .UnderTransformGroup("Enemies"));
    }

    void InstallEnemy(DiContainer subContainer)
    {
        subContainer.Bind<EnemyFacade>().FromNewComponentOnRoot().AsSingle();
        subContainer.Bind<EnemyMoveHandler>().FromNewComponentOnRoot().AsSingle().NonLazy();
    }
}
```

If we add this code to a project, then we should have a scene where the user can press space to spawn an instance of Enemy, and hit escape to despawn it.  Then, once spawned, the user can hit up arrow to apply some velocity to the enemy object.

If you are re-creating this example, note that it doesn't matter what you use for the `EnemyPrefab` setting on `TestInstaller` to represent our enemy.  I just added a cube by selecting `GameObject -> 3D Object -> Cube` from the menu and created a prefab directly from that.

This works but as you will see if you run it, there is a problem with it.  After spawning a new enemy, adding a velocity to it, despawning it, and then spawning it again, the velocity remains unchanged.  When we spawn something from a pool, we always want the state to be set to default values.  In this example it is quite simple and there is only two classes in our subcontainer, but in a real world application, there could be many classes, all with their own state that needs to be reset by the despawn event.

We could fix this by having the reset methods on the `EnemyFacade` explicitly call other reset methods on other objects, but it would be nice if there was a more generic way to do this.  So instead of that, what you can do is add the built-in `PoolableManager` class to the subcontainer and then implement the `IPoolable` interface on any classes that need to be reset:

```csharp
public class EnemyMoveHandler : MonoBehaviour, IPoolable
{
    Settings _settings;
    float _velocity;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
    }

    public void OnDespawned()
    {
    }

    public void OnSpawned()
    {
        _velocity = 0;
        transform.position = Vector3.zero;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _velocity += Time.deltaTime * _settings.Acceleration;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _velocity -= Time.deltaTime * _settings.Acceleration;
        }

        transform.position += Vector3.forward * _velocity * Time.deltaTime;
    }

    [Serializable]
    public class Settings
    {
        public float Acceleration;
    }
}

public class EnemyFacade : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [Inject]
    PoolableManager _poolableManager;

    IMemoryPool _pool;

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        _poolableManager.TriggerOnSpawned();
    }

    public void OnDespawned()
    {
        _pool = null;
        _poolableManager.TriggerOnDespawned();
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public class Factory : PlaceholderFactory<EnemyFacade>
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public EnemyMoveHandler.Settings EnemyMoveHandlerSettings;
    public GameObject EnemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(EnemyMoveHandlerSettings);
        Container.BindInterfacesTo<Runner>().AsSingle();

        Container.BindFactory<EnemyFacade, EnemyFacade.Factory>()
            .FromMonoPoolableMemoryPool<EnemyFacade>(b => b
                .WithInitialSize(2)
                .FromSubContainerResolve()
                .ByNewPrefabMethod(EnemyPrefab, InstallEnemy)
                .UnderTransformGroup("Enemies"));
    }

    static void InstallEnemy(DiContainer subContainer)
    {
        subContainer.Bind<EnemyFacade>().FromNewComponentOnRoot().AsSingle();
        subContainer.BindInterfacesAndSelfTo<EnemyMoveHandler>().FromNewComponentOnRoot().AsSingle().NonLazy();
        subContainer.Bind<PoolableManager>().AsSingle();
    }
}
```

Note the following:

- We've installed `PoolableManager` in our subcontainer
- In EnemyFacade, we have to call the `TriggerOnSpawned` and `TriggerOnDespawned` methods on PoolableManager to trigger the `OnSpawned` and `OnDespawned` methods for the rest of the subcontainer classes
- You can also pass generic arguments to `PoolableManager<>` to allow passing runtime parameters to the `TriggerOnSpawned` so they are forwarded on to your `IPoolable<>` derived classes

Note also that the order that the `IPoolable` classes are triggered will use the same execution order that is set with the `BindExecutionOrder` method, just like the other standard interfaces.  Also note that the `OnDespawned` methods are called in the reverse order compared to `OnSpawned`.

### Instantiating Memory Pools Directly

For complex scenarios involving custom factories, it might be desirable to directly instantiate memory pools.  In this case, you just have to make sure to provide an `IFactory<>` derived class to be used for creating new instances and all the settings information that would normally be provided via the bind statements.  For example:

```csharp
public class BarFactory : IFactory<Bar>
{
    public Bar Create()
    {
        ...
        [Custom creation logic]
        ...
    }
}

var settings = new MemoryPoolSettings(
    // Initial size
    1, 
    // Max size
    int.MaxValue, 
    PoolExpandMethods.Double);

var pool = _container.Instantiate<MemoryPool<Bar>>(
    new object[] { settings, new MyBarFactory<Bar>() });
```

### Pool Cleanup Checker

One mistake that can sometimes occur when using memory pools is that the spawned objects are not returned to the pool properly.  This can happen if the `Spawn` method is called but then the programmer forgets to add a matching `Despawn`/`Dispose`.

This isn't always a problem and can sometimes even be intentional.  Assuming the spawned object is not a MonoBehaviour, once the object is no longer used it will be automatically destroyed by the C# garbage collector anyway, so forgetting a `Despawn` in that case will not produce a memory leak.  The only drawback is that when the object is not returned to the pool, it means the pool has to spawn more objects next time which can be slightly less efficient.

Also, of course, if you object has custom dispose logic that is important to run then that will be missed as well which could be a problem.

To detect these mistakes, Zenject includes an optional class that you can install that will throw exceptions when there are actively spawned objects when the scene is closed.  Just add the following to one of your scene installers:

```csharp
Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle()
```

And then any time you forget to call `Despawn`, you will get errors reported in the console after ending play mode in Unity.  All this class does is check that each pool has zero active items during the `LateDispose` event.

Also, since this is intentional in some cases, you can suppress the error reporting for specific pools by passing in a list of types as an argument to PoolCleanupChecker:

```csharp
Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle()
Container.BindInstance(
    new List<Type>()
    {
        typeof(Foo.Pool),
        typeof(Bar.Pool),
    })
    .WhenInjectedInto<PoolCleanupChecker>();
```

### Memory Pool Monitor

Zenject also includes an experimental editor window that can be used to monitor the sizes of all the memory pools in the scene.  You can open it by clicking `Window -> Zenject Pool Monitor` inside Unity and should look like this:

<img src="Images/PoolMonitor.png?raw=true" alt="Memory Pool Monitor"/>

This can be useful if you want to make sure that the pools you're using are all reasonable sizes.   It can also be useful as a way to choose a good size to use with the `WithInitializeSize` method to ensure that all allocations occur at once during a loading screen rather than incrementally during gameplay.

Also, you can use the Expand button and the Clear button along with Unity's Profiler window to see how much the pool contributes to overall memory usage.

