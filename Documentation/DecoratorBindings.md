# Decorator Bindings

## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Introduction](#introduction)
- [Example](#example)
- [Binding Syntax](#binding-syntax)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->


## Introduction 

Another feature of Zenject that can open up interesting design possibilities is decorator bindings.  This allows you to easily implement the <a href="https://en.wikipedia.org/wiki/Decorator_pattern">decorator pattern</a>.

## Example

As an example, let's say we have a class that contains stats for a given enemy in our game, and that we have two enemies (an orc and a demon):

```csharp
public interface IEnemyStats
{
    float Damage
    {
        get;
    }

    float Health
    {
        get;
    }
}

public class OrcStats : IEnemyStats
{
    public float Damage
    {
        get { return 1; }
    }

    public float Health
    {
        get { return 50; }
    }
}

public class DemonStats : IEnemyStats
{
    public float Damage
    {
        get { return 7; }
    }

    public float Health
    {
        get { return 20; }
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IEnemyStats>().To<OrcStats>().AsSingle();
    }
}
```

Let's also say that we occasionally want to upgrade random units to add some variation to the game.  We can use decorators for this:

```csharp
public class WeaponUpgradeEnemyDecorator : IEnemyStats
{
    readonly IEnemyStats _stats;

    public WeaponUpgradeEnemyDecorator(IEnemyStats stats)
    {
        _stats = stats;
    }

    public float Damage
    {
        get { return _stats.Damage + 2; }
    }

    public float Health
    {
        get { return _stats.Health; }
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IEnemyStats>().To<OrcStats>().AsSingle();
        Container.Decorate<IEnemyStats>().With<WeaponUpgradeEnemyDecorator>();
    }
}
```

By using `Container.Decorate` like we did here, any using code can continue using `IEnemyStats` the same as before, but with the upgrade to cause more damage.  This upgrade can also be applied to any IEnemyStats derived class so could be for an Orc or a Demon.

We can also chain decorators together, so if wanted to apply a different upgrade for health we could do that at the same time:

```csharp
public class ShieldUpgradeEnemyDecorator : IEnemyStats
{
    readonly IEnemyStats _stats;

    public ShieldUpgradeEnemyDecorator(IEnemyStats stats)
    {
        _stats = stats;
    }

    public float Damage
    {
        get { return _stats.Damage; }
    }

    public float Health
    {
        get { return _stats.Health + 20; }
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IEnemyStats>().To<OrcStats>().AsSingle();
        Container.Decorate<IEnemyStats>().With<WeaponUpgradeEnemyDecorator>();
        Container.Decorate<IEnemyStats>().With<ShieldUpgradeEnemyDecorator>();
    }
}
```

So now our orc stats will get an upgrade for both damage and health.

Note that the order that we apply our decorators doesn't matter in this case but could in other cases.  The decorators will be applied in the order that they are added, so in this case, it would look like `ShieldUpgradeEnemyDecorator(WeaponUpgradeEnemyDecorator(OrcStats()))`

Another simple way of using decorators would be to do things like add extra logging, or verification of output values, or profiling to existing interfaces.  For example:

```csharp
public interface ISaveGameHandler
{
    void SaveGame();
}

public class SaveGameHandler : ISaveGameHandler
{
    public void SaveGame()
    {
        // Some long running operation
    }
}

public class SaveGameProfilerDecorator : ISaveGameHandler
{
    readonly ISaveGameHandler _handler;

    public SaveGameProfilerDecorator(ISaveGameHandler handler)
    {
        _handler = handler;
    }

    public void SaveGame()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _handler.SaveGame();
        stopwatch.Stop();
        Debug.Log(string.format("Took {0:0.00} seconds to save the game!", stopwatch.Elapsed.TotalSeconds));
    }
}

public void InstallBindings()
{
    Container.Bind<ISaveGameHandler>().To<SaveGameHandler>().AsSingle();
    Container.Decorate<ISaveGameHandler>().With<SaveGameProfilerDecorator>();
}
```

## Binding Syntax

<pre>
Container.Decorate&lt;<b>ContractType</b>&gt;()
    .With&lt;<b>DecoratorType</b>&gt;()
    .From<b>ConstructionMethod</b>()
    .WithArguments(<b>Arguments</b>)
    .(<b>Copy</b>|<b>Move</b>)Into(<b>All</b>|<b>Direct</b>)SubContainers();
</pre>

Where: 

* **ContractType** = The type that is being decoratored.  An object of this type will be injected into the DecoratorType class.

* **DecoratorType** = The decorator class.  This should be a concrete type and also should take as an injected parameter/field an object of type ContractType.

The other values have the same effect described <a href="../README.md#binding">here</a>.

Note that we can define any From construction method we want here - we don't have to default to FromNew like in the examples above.
