
## <a id="async-bindings"></a>Async Extensions <smal><sub>*Experimental*</sub>


## Table Of Contents

* Introduction
    * <a href="#async-and-di">Async in DI</a>
    * <a href="#example">Example</a>
* Advanced
    * <a href="#static-memory-pool">Static Memory Pools</a>
    * <a href="#usingstatement">Using statements and dispose pattern</a>



## Introduction
### <a id="async-and-di"></a>Async in DI

In dependency injection, the injector resolves dependencies of the target class only once, often after class is first created. In other words, injection is a one time process that does not track the injected dependencies to update them later on. If a dependency is not ready at the moment of injection; either binding would not resolve in case of optional bindings or fail completely throwing an error.

This is creates a dilemma when implementing dependencies that are resolved asyncroniously. You can design around the DI limitations by carefully architecting your code so that the injection happens after async process is completed. This requires careful planning, increased complexity in setup. It is also prone to errors.

Alternatively you can inject a intermediary object that tracks the result of the async operation. When you need to access the dependency, you can use this intermediary object to check if async task is completed and get the resulting object. With the experimental async support, we would like to provide ways to tackle this problem in Extenject. You can find Async extensions in **Plugins/Zenject/OptionalExtras/Async** folder.

### <a id="example"></a>Example

Lets see how we can inject async dependencies through an intermediary object. Async extensions implements `AsyncInject<T>` as this intermediary. You can use it as follows. 


```csharp
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
         Container.BindAsync<IFoo>().FromMethod(async () =>
            {
                await Task.Delay(100);
                return (IFoo) new Foo();
            }).AsCached();
    }
}

public class Bar : IInitializable, IDisposable
{
    readonly AsyncInject<IFoo> _asyncFoo;
    IFoo _foo;
    public Bar(AsyncInject<IFoo> asyncFoo)
    {
        _asyncFoo = asyncFoo;
    }

    public void Initialize()
    {
        if (!_asyncFoo.TryGetResult(out _foo))
        {
            _asyncFoo.Completed += OnFooReady;
        }
    }
       
    private void OnFooReady(IFoo foo)
    {
        _foo = foo;
    }

    public void Dispose()
    {
        _asyncFoo.Completed -= OnFooReady;
    }
}
```

Here we use `BindAsync<IFoo>().FromMethod()` to pass an async method delegate that waits for 100 ms and then returns a newly created `Foo` object. This method can be any other method with `async Task<T> Method()` signature. `BindAsync<T>` extension provides a separate binder for async operations. This binder is currently limited to a few `FromX()` providers. Features like Pooling and Factories are not supported at the moment.

With above binding `AsyncInject<IFoo>` object is added to the container. Since scope is set to `AsCached()` the operation will run only once and `AsyncInject<IFoo>` will keep the result. It is important to note that async operation will not start before this binding is resolved. If you want async operation to start immediately after installing use `NonLazy()` option. 

Once injected to `Bar`, we can check whether the return value of the async operation is already available by `TryGetResult`. method. This method will return false if there is no result to return. If result is not ready yet, we can listen to `Completed` event to get the return value when async operation completes.

Alternatively we can use following methods to check result.
```csharp
// Use HasResult to check if result exists 
if (_asyncFoo.HasResult)
{
    // Result will throw error if prematurely used. 
    var foo = _asyncFoo.Result;
}

// AsyncInject<T> provides custom awaiter
IFoo foo = await _asyncFoo;
```