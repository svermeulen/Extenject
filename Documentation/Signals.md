# Signals

## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Introduction](#introduction)
  - [Motivation / Theory](#motivation--theory)
  - [Signals Quick Start](#signals-quick-start)
  - [Signals Declaration](#signals-declaration)
  - [Declaration Binding Syntax](#declaration-binding-syntax)
  - [Signal Firing](#signal-firing)
  - [Binding Signals with BindSignal](#binding-signals-with-bindsignal)
  - [SignalBusInstaller](#signalbusinstaller)
  - [When To Use Signals](#when-to-use-signals)
- [Advanced](#advanced)
  - [Abstract Signals](#abstract-signals)
  - [Signals With Subcontainers](#signals-with-subcontainers)
  - [Asynchronous Signals](#asynchronous-signals)
  - [Signal Settings](#signal-settings)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

* Introduction
    * <a href="#theory">Theory</a>
    * <a href="#quick-start">Signals Quick Start</a>
    * <a href="#declaration">Signals Declaration</a>
    * <a href="#declaration-syntax">Declaration Binding Syntax</a>
    * <a href="#firing">Signal Firing</a>
    * <a href="#bindsignal">Binding Signals with BindSignal</a>
    * <a href="#signalbusinstaller">SignalBusInstaller</a>
    * <a href="#when-to-use-signals">When To Use Signals</a>
* Advanced
    * <a href="#abstract-signals">Abstract Signals</a>
    * <a href="#use-with-subcontainers">Signals With Subcontainers</a>
    * <a href="#async-signals">Asynchronous Signals</a>
    * <a href="#settings">Signal Settings</a>
    * <a href="#identifiers">Identifiers</a>

## Introduction

### Motivation / Theory

Given two classes A and B that need to communicate, your options are usually:

1. Directly call a method on B from A.  In this case, A is strongly coupled with B.
2. Inverse the dependency by having B observe an event on A.  In this case, B is strongly coupled with A

As a third option, in some cases it might actually be better for neither one to know about the other. This way your code is kept as loosely coupled as possible.  You can achieve this by having A and B interact with an intermediary object (in this case, zenject signals) instead of directly with each other.

Note also that while the result will be more loosely coupled, this isn't always going to be better.  Signals can be misused just like any programming pattern, so you have to consider each case for whether it's a good candidate for them or not.

### Signals Quick Start

If you just want to get up and running immediately, see the following example which shows basic usage:

```csharp

public class UserJoinedSignal
{
    public string Username;
}

public class GameInitializer : IInitializable
{
    readonly SignalBus _signalBus;

    public GameInitializer(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Fire(new UserJoinedSignal() { Username = "Bob" });
    }
}

public class Greeter
{
    public void SayHello(UserJoinedSignal userJoinedInfo)
    {
        Debug.Log("Hello " + userJoinedInfo.Username + "!");
    }
}

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<UserJoinedSignal>();

        Container.Bind<Greeter>().AsSingle();

        Container.BindSignal<UserJoinedSignal>()
            .ToMethod<Greeter>(x => x.SayHello).FromResolve();

        Container.BindInterfacesTo<GameInitializer>().AsSingle();
    }
}
```

To run, just copy and paste the code above into a new file named `GameInstaller` then create an empty scene with a new scene context and attach the new installer.

There are several ways of creating signal handlers.  Another approach would be the following

```csharp
public class Greeter : IInitializable, IDisposable
{
    readonly SignalBus _signalBus;

    public Greeter(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<UserJoinedSignal>(OnUserJoined);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<UserJoinedSignal>(OnUserJoined);
    }

    void OnUserJoined(UserJoinedSignal args)
    {
        SayHello(args.Username);
    }

    public void SayHello(string userName)
    {
        Debug.Log("Hello " + userName + "!");
    }
}

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<UserJoinedSignal>();

        // Here, we can get away with just binding the interfaces since they don't refer
        // to each other
        Container.BindInterfacesTo<Greeter>().AsSingle();
        Container.BindInterfacesTo<GameInitializer>().AsSingle();
    }
}
```

As one final alternative approach, you could also combine zenject signals with the UniRx library and do it like this instead:


```csharp
public class Greeter : IInitializable, IDisposable
{
    readonly SignalBus _signalBus;
    readonly CompositeDisposable _disposables = new CompositeDisposable();

    public Greeter(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.GetStream<UserJoinedSignal>()
            .Subscribe(x => SayHello(x.Username)).AddTo(_disposables);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void SayHello(string userName)
    {
        Debug.Log("Hello " + userName + "!");
    }
}
```

Note that if you go this route that you need to enable UniRx integration as described <a href="../README.md#unirx-integration">here</a>.

As you can see in the the above examples, you can either directly bind a handler method to a signal in an installer using `BindSignal` (first example) or you can have your signal handler attach and detach itself to the signal (second and third examples)

Details of how this works are explained in the following sections.

### Signals Declaration

Before declaring a signal you need to create a class that will represent it.  For example:

```csharp
public class PlayerDiedSignal
{
}
```

Any parameters passed along with the signal should be added as public members or properties.  For example:

```csharp
public class WeaponEquippedSignal
{
    public Player Player;
    public IWeapon Weapon;
}
```

You might also consider making the signal classes immutable, so our WeaponEquippedSignal might be better written as this instead:

```csharp
public class WeaponEquippedSignal
{
    public WeaponEquippedSignal(Player player, IWeapon weapon)
    {
        Player = player;
        Weapon = weapon;
    }

    public IWeapon Weapon
    {
        get; private set;
    }

    public Player Player
    {
        get; private set;
    }
}
```

This isn't necessary but you might consider doing this to ensure that any signal handlers do not attempt to change the signal parameter values, which could negatively affect other signal handler behaviour.

After we have created our signal class we just need to declare it in an installer somewhere:

```csharp
public override void InstallBindings()
{
    Container.DeclareSignal<PlayerDiedSignal>();
}
```

Any objects that are in the container where it's declared, or any sub container, can now listen on the signal and also fire it.

### Declaration Binding Syntax

The format of the DeclareSignal statement is the following:

<pre>
Container.DeclareSignal&lt;<b>SignalType</b>&gt;()
    .WithId(<b>Identifier</b>)
    .<b>(RequireSubscriber|OptionalSubscriber|OptionalSubscriberWithWarning)</b>()
    .<b>(RunAsync|RunSync)</b>()
    .WithTickPriority(<b>TickPriority</b>)
    .(<b>Copy</b>|<b>Move</b>)Into(<b>All</b>|<b>Direct</b>)SubContainers();
</pre>

Where:

- **SignalType** - The custom class that represents the signal

* **Identifier** = The value to use to uniquely identify the binding.  This can be ignored in most cases, but can be useful in cases where you want to define multiple distinct signals using the same signal type.

- **RequireSubscriber**/**OptionalSubscriber**/**OptionalSubscriberWithWarning** - These values control how the signal should behave when it fired but there are no subscribers associated with it.  Unless it is over-ridden in <a href="#settings">ZenjectSettings</a>, the default is OptionalSubscriber, which will do nothing in this case.  When RequireSubscriber is set, exceptions will be thrown in the case of zero subscribers.  OptionalSubscriberWithWarning is half way in between where it will issue a console log warning instead of an exception.  Which one you choose depends on how strict you prefer your application to be, and whether it matters if the given signal is actually handled or not.

- **RunAsync**/**RunSync** - These values control whether the signal is fired synchronously or asynchronously:

    **RunSync** - This means the that when the signal is fired by calling `SignalBus.Fire` that all the subscribed handler methods are immediately invoked.

    **RunAsync** - This means that when a signal is fired, the subscribed methods will not be invoked until later (as specified by the TickPriority parameter).

    Note that Unless It is over-ridden in <a href="#settings">ZenjectSettings</a>, the default value is to run synchronously.   See <a href="#async-signals">here</a> for a discussion of asynchronous signals and why you might sometimes want to use that instead.

* **TickPriority** = The tick priority to execute the signal handler methods at.  Note that this is only applicable when using **RunAsync**.

* (**Copy**|**Move**)Into(**All**|**Direct**)SubContainers = Same behaviour as described in <a href="../README.md#binding">main section on binding</a>.

    Note that the default value for **RunSync**/**RunAsync** and **RequireSubscriber**/**OptionalSubscriber** can be overridden by changing <a href="#settings">ZenjectSettings</a>

### Signal Firing

To fire the signal, you add a reference to the `SignalBus` class, and then call the `Fire` method like this:

```csharp
public class UserJoinedSignal
{
}

public class UserManager
{
    readonly SignalBus _signalBus;

    public UserManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void DoSomething()
    {
        _signalBus.Fire<UserJoinedSignal>();
    }
}
```

Or, if the signal has parameters then you will want to create a new instance of it, like this:

```csharp
public class UserJoinedSignal
{
    public string Username;
}

public class UserManager
{
    readonly SignalBus _signalBus;

    public UserManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void DoSomething()
    {
        _signalBus.Fire(new UserJoinedSignal() { Username = "Bob" });
    }
}
```

When `Fire()` is called, SignalBus expects the signal to be declared and exception will be thrown if the signal is not declared. If you want to call `Fire()` regardless of the signal declaration, use `TryFire()` method instead that ignores undeclared signals. You can use `TryFire()` looks like this:

```csharp
public class UserJoinedSignal
{
}

public class UserManager
{
    readonly SignalBus _signalBus;

    public UserManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void DoSomething()
    {
        // Generic version
        _signalBus.TryFire<UserJoinedSignal>(); // Nothing happens if UserJoinedSignal is NOT declared

        // Non-Generic version
        _signalBus.TryFire(new UserJoinedSignal()); // Nothing happens if UserJoinedSignal is NOT declared
    }
}
```

### Binding Signals with BindSignal

As mentioned above, in addition to being able to directly subscribe to signals on the signal bus (via `SignalBus.Subscribe` or `SignalBus.GetStream`) you can also directly bind a signal to a handling class inside an installer.  This approach has advantages and disadvantages compared to directly subscribing in a handling class so again comes down to personal preference.

The format of the BindSignal command is:

<pre>
Container.BindSignal&lt;<b>SignalType</b>&gt;()
    .WithId(<b>Identifier</b>)
    .ToMethod(<b>Handler</b>)
    .From(<b>ConstructionMethod</b>)
    .(<b>Copy</b>|<b>Move</b>)Into(<b>All</b>|<b>Direct</b>)SubContainers();
</pre>

Where:

- **SignalType** - The custom class that represents the signal

* **Identifier** = The value to use to uniquely identify the binding.  This can be ignored in most cases.  Note that when using signal identifiers you have to use the same identifier for DeclareSignal as well (and Fire, Subscribe, etc.)

- **ConstructionMethod** - When binding to an instance method above, you also need to define where this instance comes from.  See the section on Handler below for more detail

* (**Copy**|**Move**)Into(**All**|**Direct**)SubContainers = Same behaviour as described in <a href="../README.md#binding">main section on binding</a>.

- **Handler** - The method that should be triggered when the signal fires.  This has several variations:

**1. Static method**

```csharp
Container.BindSignal<UserJoinedSignal>().ToMethod(s => Debug.Log("Hello user " + s.Username));
```

Note that the method can also be parameterless:

```csharp
Container.BindSignal<UserJoinedSignal>().ToMethod(() => Debug.Log("Received UserJoinedSignal signal"))
```

Note also that in this case, there is no option to provide a value for `From` since there is no instance needed

**2. Instance method directly**

For example:

```csharp
public class Greeter
{
    public void SayHello(UserJoinedSignal signal)
    {
        Debug.Log("Hello " + signal.Username + "!");
    }
}

Container.Bind<Greeter>().AsSingle();
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).FromResolve();
```

In this case we want the signal to trigger the `Greeter.SayHello` method.  Note that we need to supply a value for `From` in this case because an instance is needed to call the given method on.

Similar to static methods you could also bind to a method without parameters:

```csharp
public class Greeter
{
    public void SayHello()
    {
        Debug.Log("Hello there!");
    }
}

Container.Bind<Greeter>().AsSingle();
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).FromResolve();
```

We are using `FromResolve` however we could use any kind of construction method we want as well.  Under the hood, `FromResolve` actually expands to the following:

```csharp
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).From(x => x.FromResolve().AsCached());
```

There is also another shortcut `FromNew` for cases where the handler classes is not accessed anywhere else in the container

```csharp
// These are both equivalent
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).FromNew();
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).From(x => x.AsCached());
```

So, if we didn't need the Greeter class to be injected anywhere else, we could have also implemented it as follows:

```csharp
public class Greeter
{
    public void SayHello(UserJoinedSignal signal)
    {
        Debug.Log("Hello " + signal.Username + "!");
    }
}

Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>(x => x.SayHello).FromNew();
```

This way, we don't need a separate binding for Greeter at all.   You can provide many other kinds of arguments to `From` as well, including binding to a lazily instantiated `MonoBehaviour`, a factory method, a custom factory, a facade in a subcontainer, etc.

**3. Instance method with mapping**

There might also be cases where the arguments to the handling method directly contain the signal arguments.  For example:

```csharp
public class Greeter
{
    public void SayHello(string username)
    {
        Debug.Log("Hello " + username + "!");
    }
}
```

In this case you could bind the signal to a method that does a mapping of the parameters for us:

```csharp
Container.Bind<Greeter>().AsSingle();
Container.BindSignal<UserJoinedSignal>().ToMethod<Greeter>((x, s) => x.SayHello(s.Username)).FromResolve()
```

### SignalBusInstaller

Signals are an optional feature of Zenject.  When importing Zenject, if you do not want to include signals you can simply uncheck the `OptionalExtras/Signals` folder.  As a result of this, signals are not enabled automatically, so you have to explicitly install them yourself by calling `SignalBusInstaller.Install(Container)` in one of your installers.

You could either do this just one time in a `ProjectContext` installer, or you could do this in each scene in a `SceneContext` installer.  Note that you only need to do this once, and then you can use signals in the container that you pass to `SignalBusInstaller,` as well as any subcontainers, which is why if you install to `ProjectContext` you do not need to install to `SceneContext.`

### When To Use Signals

Signals are most appropriate as a communication mechanism when:

1. There might be multiple interested receivers listening to the signal
2. The sender doesn't need to get a result back from the receiver
3. The sender doesn't even really care if it gets received.  In other words, the sender should not rely on some state changing when the signal is called for subsequent sender logic to work correctly.  Ideally signals can be thought as "fire and forget" events
4. The sender triggers the signal infrequently or at unpredictable times

These are just rules of thumb, but useful to keep in mind when using signals.  The less logically coupled the sender is to the response behaviour of the receivers, the more appropriate it is compared to other forms of communication such as direct method calls, interfaces, C# event class members, etc.  This is also one reason you might consider using <a href="#async-signals">asynchronous signals</a>

When event driven program is abused, it is possible to find yourself in "callback hell" where events are triggering other events etc. and which make the entire system impossible to understand.  So signals in general should be used with caution.  Personally I like to use signals for high level game-wide events and then use other forms of communication (unirx streams, c# events, direct method calls, interfaces) for most other things.

## Advanced

### Abstract Signals

One of the problems of the signals is that when you subscribe to their types you are coupling your concrete signal types to the subscribers

For example, Lets say I have a player and i want to save the game when i finish a level.
Ok easy, I create ``SignalLevelCompleted`` and then I subscribe it to my ``SaveGameSystem``
then I also want to save when i reach a checkpoint, again i create ``SignalCheckpointReached``
and then I subscribe it to my ``SaveGameSystem``
you are begining to get something like this...
```csharp
public class Example
{
   SignalBus signalBus;
   public Example(Signalbus signalBus) => this.signalBus = signalBus;
   
   public void CheckpointReached() => signalBus.Fire<SignalCheckpointReached>();
   
   public void CompleteLevel() => signalBus.Fire<SignalLevelCompleted>();
}

public class SaveGameSystem
{
   public SaveGameSystem(SignalBus signalBus)
   {
      signalBus.Subscribe<SignalCheckpointReached>(x => SaveGame());
      signalBus.Subscribe<SignalLevelCompleted>(x => SaveGame());
   }
   
   void SaveGame() { /*Saves the game*/ }
}

//in your installer
Container.DeclareSignal<SignalLevelCompleted>();
Container.DeclareSignal<SignalCheckpointReached>();

//your signal types
public struct SignalCheckpointReached{}
public struct SignalLevelCompleted{}
```

And then you realize you are coupling the types``signalLevelCompleted`` and ``SignalCheckpointReached``to ``SaveGameSystem``. 
``SaveGameSystem`` shouldn't know about those "non related with saving" events...

So let's give the power of interfaces to signals!
So i have the ``SignalCheckpointReached`` and ``SignalLevelCompleted`` both implementing **``ISignalGameSaver``**
and my ``SaveGameSystem`` just Subscribes to **``ISignalGameSaver``** for saving the game
So when i fire any of those signals the ``SaveGameSystem`` saves the game.
Then you have something like this...
```csharp
public class Example
{
   SignalBus signalBus;
   public Example(Signalbus signalBus) => this.signalBus = signalBus;
   
   public void CheckpointReached() => signalBus.AbstractFire<SignalCheckpointReached>();
   
   public void CompleteLevel() => signalBus.AbstractFire<SignalLevelCompleted>();
}

public class SaveGameSystem
{
   public SaveGameSystem(SignalBus signalBus)
   {
      signalBus.Subscribe<ISignalGameSaver>(x => SaveGame());
   }
   
   void SaveGame() { /*Saves the game*/ }
}

//in your installer
Container.DeclareSignalWithInterfaces<SignalLevelCompleted>();
Container.DeclareSignalWithInterfaces<SignalCheckpointReached>();

//your signal types
public struct SignalCheckpointReached : ISignalGameSaver{}
public struct SignalLevelCompleted : ISignalGameSaver{}

public interface ISignalGameSaver{}
```

Now your ``SaveGameSystem`` doesnt knows about CheckPoints nor Level events, and just reacts to signals that save the game.
The main difference is in the Signal declaration and Firing
 - ``DeclareSignalWithInterfaces`` works like ``DeclareSignal`` but it declares the interfaces too.
 - ``AbstractFire`` is the same that ``Fire`` but it fires the interfacesjust if you have Declared the signal with interfaces 
 otherwise it will throw an exception.

Ok, let's show even more power.
Now i create another signal for the WorldDestroyed Achievement "SignalWorldDestroyed"
But i also want my SoundSystem to play sounds when i reach a checkpoint and/or unlock an Achievement
So the code could look like this.
```csharp
public class Example
{
   SignalBus signalBus;
   public Example(Signalbus signalBus) => this.signalBus = signalBus;
   
   public void CheckpointReached() => signalBus.AbstractFire<SignalCheckpointReached>();
   
   public void DestroyWorld() => signalBus.AbstractFire<SignalWorldDestroyed>();
}

public class SoundSystem
{
   public SoundSystem(SignalBus signalBus)
   {
      signalBus.Subscribe<ISignalSoundPlayer>(x => PlaySound(x.soundId));
   }
   
   void PlaySound(int soundId) { /*Plays the sound with the given id*/ }
}

public class AchievementSystem
{
   public AchievementSystem(SignalBus signalBus)
   {
      signalBus.Subscribe<ISignalAchievementUnlocker>(x => UnlockAchievement(x.achievementKey));
   }
   
   void UnlockAchievement(string key) { /*Unlocks the achievement with the given key*/ }
}

//in your installer
Container.DeclareSignalWithInterfaces<SignalCheckpointReached>();
Container.DeclareSignalWithInterfaces<SignalWorldDestroyed>();

//your signal types
public struct SignalCheckpointReached : ISignalGameSaver, ISignalSoundPlayer
{ 
   public int SoundId { get => 2} //or configured in a scriptable with constants instead of hardcoded
}
public struct SignalWorldDestroyed : ISignalAchievementUnlocker, ISignalSoundPlayer
{
   public int SoundId { get => 4}
   public string AchievementKey { get => "WORLD_DESTROYED"}
}

//Your signal interfaces
public interface ISignalGameSaver{}
public interface ISignalSoundPlayer{ int SoundId {get;}}
public interface ISignalAchievementUnlocker{ string AchievementKey {get;}}
```

It offers a lot of modularity and abstraction for signals,
you fire a concrete signal telling what you did and give them functionality trough Interface implementations

### Signals With Subcontainers

Signals are only visible at the container level where they are declared and below.  For example, you might use Unity's multi-scene support and split up your game into a GUI scene and an Environment scene.  In the GUI scene you might fire a signal indicating that the GUI popup overlay has been opened/closed, so that the Environment scene can pause/resume activity.  One way of achieving this would be to declare a signal in a ProjectContext installer (or a shared <a href="../README.md#scene-parenting">scene parent</a>), then subscribe to it in the Environment scene, and then fire it from the GUI scene.

### Asynchronous Signals

In some cases it might be desirable to run a given signal asynchronously.  Asynchronous signals have the following advantages:

1. The update-order that the signal handlers are triggered might be more predictable.  When using synchronous signals, the signal handler methods are executed at the same time that the signal is fired, which could be triggered at any time during the frame, or in some cases multiple places if the signal is fired multiple times.  This can lead to some update-order issues.  With async signals, the signal handlers are always executed at the same time in the frame as configured by the TickPriority.

2. Asynchronous signals can encourage less coupling between the sender and receiver, which is often what you want.  As explained <a href="#when-to-use-signals">above</a>, signals work best when they are used for "fire and forget" events where the sender doesn't care about the behaviour of any listeners.   By making a signal async, it can enforce this separation because the signal handler methods will be executed later, and therefore the sender actually cannot make direct use of the result of the handlers behaviour.

3. Unexpected state changes can occur while firing just one signal.  For example, an object A might trigger a signal which would trigger some logic that would eventually cause A to be deleted.  If the signal was executed synchronously, then the call stack could eventually return to object A where the signal was fired, and object A might then attempt to execute commands afterwards that causes problems (since object A will have already been deleted)

This is not to say that asynchronous signals are superious to synchronous signals.  Asynchronous signals have their own risks as well.

1. Debugging can be more difficult, because it isn't clear from the stack trace where the signal was fired.

2. Some parts of the state can be out of sync with each other.   If a class A fires an async signal that requires a response from class B, then there will be some period between when the signal was fired and the handler method in class B was invoked, where B is out of sync with A, which can lead to some bugs.

3. The overall system might be more complex than when using synchronous signals and therefore harder to understand.

### Signal Settings

Most of the default settings for signals can be overriden via a settings property that is on the `ProjectContext`.  It can also be configured on a per-container level by setting the `DiContainer.Settings` property.  For signals this includes the following:

**Default Sync Mode** - This value controls the default value for the `DeclareSignal` property `RunSync`/`RunAsync` when it is left unspecified.  By default it is set to synchronous so will assume `RunSync` when unspecified by a call to `DeclareSignal`.  So if you are a fan of async signals then you could set this to async to assume async instead.

**Missing Handler Default Response** - This value controls the default value when **RequireSubscriber**/**OptionalSubscriber**/**OptionalSubscriberWithWarning** is not specified for a call to `DeclareSignal`.  By default it is set to **OptionalSubscriber**.

**Require Strict Unsubscribe** - When true, this will cause exceptions to be thrown if the scene ends and there are still signal handlers that have not yet unsubscribed yet.  By default it is false.

**Default Async Tick Priority** - This value controls the default tick priority when `RunAsync` is used with `DeclareSignal` but `WithTickPriority` is left unset.  By default it is set to 1, which will cause the signal handlers to be invoked right after all the normal tickables have been called.  This default is chosen because it will ensure that the signal is handled in the same frame that it is triggered, which can be important if the signal affects how the frame is rendered.
