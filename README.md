![Extenject Header Image](Documentation/Images/ExtenjectMainHeader.png)

[![Gitter](https://img.shields.io/static/v1?label=Gitter&labelColor=ED1965&message=Support&color=grey&logo=Gitter&logoColor=White&url=https://gitter.im/Extenject/community)](https://gitter.im/Extenject/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/svermeulen/Extenject?color=green)](https://github.com/svermeulen/Extenject/releases)
![GitHub Release Date](https://img.shields.io/github/release-date/svermeulen/Extenject)
![GitHub contributors](https://img.shields.io/github/contributors/svermeulen/Extenject)
![GitHub last commit](https://img.shields.io/github/last-commit/svermeulen/Extenject)
![Continuous Integration](https://github.com/svermeulen/Extenject/workflows/Continuous%20Integration/badge.svg?branch=master)
![GitHub](https://img.shields.io/github/license/svermeulen/Extenject)

# Extenject: extensions, bug fixes and updates for Zenject 

This project is simply a fork of [Zenject](https://github.com/modesttree/zenject) with the goal of being actively maintained.  I am the primary author and was the primary maintainer until my access was removed after leaving my position at the company Modest Tree.  It is called Extenject to respect Modest Tree's [trademark claim](https://github.com/modesttree/Zenject/commit/2cbbf11b344d083cc697d8b248acf41520d72da3) on the name Zenject.

# Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

  - [Lawsuit](#lawsuit)
  - [Support](#support)
  - [Introduction](#introduction)
  - [Features](#features)
  - [Installation *](#installation-)
  - [History](#history)
  - [Documentation](#documentation)
- [What is Dependency Injection?](#what-is-dependency-injection)
  - [Theory](#theory)
  - [Misconceptions](#misconceptions)
- [Introduction to Zenject API](#introduction-to-zenject-api)
  - [Hello World Example](#hello-world-example)
  - [Injection](#injection)
  - [Register Mappings to the DI Container](#register-mappings-to-the-di-container)
    - [Binding](#binding)
    - [Construction Methods](#construction-methods)
  - [Installers](#installers)
  - [Using Non-MonoBehaviour Classes](#using-non-monobehaviour-classes)
    - [ITickable](#itickable)
    - [IInitializable](#iinitializable)
    - [IDisposable](#idisposable)
    - [BindInterfacesTo and BindInterfacesAndSelfTo](#bindinterfacesto-and-bindinterfacesandselfto)
    - [Using the Unity Inspector To Configure Settings](#using-the-unity-inspector-to-configure-settings)
  - [Object Graph Validation](#object-graph-validation)
  - [Scene Bindings](#scene-bindings)
  - [General Guidelines / Recommendations / Gotchas / Tips and Tricks](#general-guidelines--recommendations--gotchas--tips-and-tricks)
  - [Further Reading](#further-reading)
- [Advanced](#advanced)
  - [Binding](#binding-1)
    - [Game Object Bind Methods](#game-object-bind-methods)
    - [Optional Binding](#optional-binding)
    - [List Bindings](#list-bindings)
    - [Global Bindings Using Project Context](#global-bindings-using-project-context)
    - [Identifiers](#identifiers)
    - [Non Generic Bindings](#non-generic-bindings)
    - [Convention Based Binding](#convention-based-binding)
    - [Decorator Bindings](#decorator-bindings)
  - [Scriptable Object Installer](#scriptable-object-installer)
  - [Runtime Parameters For Installers](#runtime-parameters-for-installers)
  - [Using Zenject Outside Unity Or For DLLs](#using-zenject-outside-unity-or-for-dlls)
  - [Zenject Settings](#zenject-settings)
  - [Signals](#signals)
  - [Factories: Creating Objects Dynamically](#factories-creating-objects-dynamically)
  - [Memory Pools](#memory-pools)
  - [Update / Initialization Order](#update--initialization-order)
  - [Zenject Order Of Operations](#zenject-order-of-operations)
  - [Injecting data across scenes](#injecting-data-across-scenes)
  - [Scene Parenting Using Contract Names](#scene-parenting-using-contract-names)
  - [Default Scene Parents](#default-scene-parents)
  - [ZenAutoInjecter](#zenautoinjecter)
  - [Scene Decorators](#scene-decorators)
  - [Sub-Containers And Facades](#sub-containers-and-facades)
  - [Writing Automated Unit Tests / Integration Tests](#writing-automated-unit-tests--integration-tests)
  - [Philosophy Of Zenject](#philosophy-of-zenject)
  - [Just-In-Time Resolving Using LazyInject&lt;&gt;](#just-in-time-resolving-using-lazyinjectltgt)
  - [Open Generic Types](#open-generic-types)
  - [Notes About Destruction/Dispose Order](#notes-about-destructiondispose-order)
  - [UniRx Integration](#unirx-integration)
  - [Auto-Mocking using Moq](#auto-mocking-using-moq)
  - [Creating Unity EditorWindow's with Zenject](#creating-unity-editorwindows-with-zenject)
  - [Optimization Recommendations/Notes](#optimization-recommendationsnotes)
  - [Reflection Baking](#reflection-baking)
    - [Baking External DLLs](#baking-external-dlls)
    - [Under the hood](#under-the-hood)
    - [Coverage Settings](#coverage-settings)
  - [Upgrade Guide for Zenject 6](#upgrade-guide-for-zenject-6)
  - [DiContainer Methods](#dicontainer-methods)
    - [DiContainer.Instantiate](#dicontainerinstantiate)
    - [DiContainer.Bind](#dicontainerbind)
    - [DiContainer.Resolve](#dicontainerresolve)
    - [DiContainer.Inject](#dicontainerinject)
    - [DiContainer.QueueForInject](#dicontainerqueueforinject)
    - [DiContainer Unbind / Rebind](#dicontainer-unbind--rebind)
    - [Other DiContainer methods](#other-dicontainer-methods)
  - [Frequently Asked Questions](#frequently-asked-questions)
    - [Isn't this overkill?  I mean, is using statically accessible singletons really that bad?](#isnt-this-overkill--i-mean-is-using-statically-accessible-singletons-really-that-bad)
    - [Is there a way to integrate with the upcoming Unity ECS?](#is-there-a-way-to-integrate-with-the-upcoming-unity-ecs)
    - [Does this work on AOT platforms such as iOS and WebGL?](#does-this-work-on-aot-platforms-such-as-ios-and-webgl)
    - [How is performance?](#how-is-performance)
    - [How do I use Unity style Coroutines in normal C# classes?](#how-do-i-use-unity-style-coroutines-in-normal-c-classes)
    - [Are there any more sample projects to look at?](#are-there-any-more-sample-projects-to-look-at)
    - [What games/applications/libraries are using Zenject?](#what-gamesapplicationslibraries-are-using-zenject)
    - [I keep getting errors complaining about circular reference!  How to address this?](#i-keep-getting-errors-complaining-about-circular-reference--how-to-address-this)
  - [Cheat Sheet](#cheat-sheet)
  - [Further Help](#further-help)
  - [Release Notes](#release-notes)
  - [License](#license)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Lawsuit
```

Extenject has been removed from Asset Store because of a copyright claim by 
Modest Tree, so the only way to obtain it currently is through the 
https://github.com/svermeulen/Extenject/releases.

Modest Tree has also blocked me from contributing to zenject github issues and 
removed me from the zenject gitter chat, so to get my help please report 
issues https://github.com/svermeulen/Extenject/issues/new, 
and discuss in the https://gitter.im/Extenject/community instead

They have also filed a lawsuit against me and Unity tells me that they will 
not re-enable Extenject in the Asset Store until the lawsuit is complete, 
which might not occur until 2020.

I have been advised not to comment on details of the lawsuit right now 
however you can read their filing and my defense (both of which are public) 
for details if interested.  The lawsuit also relates to other MIT licensed 
open source projects I created such as Projeny and Unity3dAsyncAwaitUtil.
```

[![Projeny](https://img.shields.io/badge/Projeny-181717?logo=GitHub&logoColor=white)](https://github.com/modesttree/Projeny)
[![Unity3dAsyncAwaitUtil](https://img.shields.io/badge/Unity3dAsyncAwaitUtil-181717?logo=GitHub&logoColor=white)](hhttps://github.com/modesttree/Unity3dAsyncAwaitUtil)
[![Modest Tree filing against me](https://img.shields.io/static/v1?label=Public%20PDF&message=Modest%20Tree%20Filing%20Against%20Me&color=red&url=https://drive.google.com/open?id=1pVVCuwsJfhQ-jj2cIe5oF6zNwHITCHbF)](https://drive.google.com/open?id=1pVVCuwsJfhQ-jj2cIe5oF6zNwHITCHbF)
[![My Defense Fiiing](https://img.shields.io/static/v1?label=Public%20PDF&message=My%20Defense%20Filing&color=success&url=https://drive.google.com/open?id=1CYy2g46b2XiDoIsWb75SgLJoIGakGiyz)](https://drive.google.com/open?id=1CYy2g46b2XiDoIsWb75SgLJoIGakGiyz)

## Support

This project is supported via donations.  If you or your team have found it useful, please consider supporting further development through [patreon](https://www.patreon.com/extenject) or [paypal](https://paypal.me/stevevermeulen)


## Introduction

Note that if you are looking for the older documentation for Zenject you can find that here:  [Zenject 3.x](https://github.com/svermeulen/Extenject/tree/f0dd30ad451dcbc3eb17e636455a6c89b14ad537), [Zenject 4.x](https://github.com/svermeulen/Extenject/tree/0b4a15b1e6e680c94fd34a2d7420eb41e320b21b) and [Zenject 5.x](https://github.com/svermeulen/Extenject/tree/dc019e31dbae09eb53c1638be00f7f002898956c)

Zenject is a lightweight highly performant dependency injection framework built specifically to target Unity 3D (however it can be used outside of Unity as well).  It can be used to turn your application into a collection of loosely coupled parts with highly segmented responsibilities.  Zenject can then glue the parts together in many different configurations to allow you to easily write, re-use, refactor and test your code in a scalable and extremely flexible way.

Tested in Unity 3D on the following platforms: 
* PC/Mac/Linux
* iOS
* Android
* WebGL
* PS4 (with IL2CPP backend)
* Windows Store (including 8.1, Phone 8.1, Universal 8.1 and Universal 10 - both .NET and IL2CPP backend)

IL2CPP is supported, however there are some gotchas - see [here](#aot-support) for details

This project is open source.

For general troubleshooting / support, please post to [stack overflow](https://stackoverflow.com/questions/ask) using the tag 'zenject', or post in the [zenject google group](https://groups.google.com/forum/#!forum/zenject/)

Or, if you have found a bug, you are also welcome to create an issue on the [github page](https://github.com/svermeulen/Extenject), or a pull request if you have a fix / extension.  There is also a [gitter chat](https://gitter.im/Extenject/community) that you can join for real time discussion.  Finally, you can also email me directly at sfvermeulen@gmail.com or follow me on twitter at [@steve_verm](https://twitter.com/steve_verm)

## Features

* Injection
    * Supports both normal C# classes and MonoBehaviours
    * Constructor injection
    * Field injection
    * Property injection
    * Method injection
* Conditional binding (eg. by type, by name, etc.)
* Optional dependencies
* Support for creating objects after initialization using factories
* Nested Containers aka Sub-Containers
* Injection across different Unity scenes to pass information from one scene to the next
* Scene parenting, to allow one scene to inherit the bindings from another
* Support for global, project-wide bindings to add dependencies for all scenes
* Convention based binding, based on class name, namespace, or any other criteria
* Ability to validate object graphs at editor time (including dynamic object graphs created via factories)
* Automatic binding on components in the scene using the `ZenjectBinding` component
* Auto-Mocking using the Moq library
* Built-in support for memory pools
* Support for decorator pattern using decorator bindings
* Support for automatically mapping open generic types
* Built in support for unit test, integration tests, and scene tests
* Just-in-time injection using the LazyInject<> construct
* Support for multiple threads for resolving/instantiating
* Support for 'reflection baking' to eliminate costly reflection operations completely by directly modifying the generated assemblies
* Automatic injection of game objects using ZenAutoInjecter component

## Installation ![GitHub release (latest by date)](https://img.shields.io/github/v/release/svermeulen/Extenject?color=green)

[![GitHub releases](https://img.shields.io/static/v1?style=for-the-badge&label=GitHub%20Releases&labelColor=181717&message=Downloads&color=green&logo=GitHub&logoColor=white)](https://github.com/svermeulen/Extenject/releases)
[![Unity](https://img.shields.io/static/v1?style=for-the-badge&label=Unity%20Asset%20Store&labelColor=181717&message=Download&color=green&logo=Unity&logoColor=white)](https://assetstore.unity.com/packages/tools/utilities/extenject-dependency-injection-ioc-157735)
[![Unity Package](https://img.shields.io/static/v1?style=for-the-badge&label=Unity%20Package&labelColor=181717&message=UPM&color=green&logo=Unity&logoColor=white)](https://github.com/svermeulen/Extenject/issues/2)

You can install Zenject using any of the following methods

1.  __From [Releases Page](https://github.com/svermeulen/Extenject/releases)__
    Here you can choose between the following:

    * **Zenject-WithAsteroidsDemo.vX.X.unitypackage** - This is equivalent to what you find in the Asset Store and contains both sample games "Asteroids" and "SpaceFighter" as part of the package.  All the source code for Zenject is included here.
    * **Zenject.vX.X.unitypackage** - Same as above except without the Sample projects.
    * **Zenject-NonUnity.vX.X.zip** - Use this if you want to [use Zenject outside of Unity](#using-outside-unity) (eg. just as a normal C# project)


1.  __From the [Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/extenject-dependency-injection-framework-157735)__

    * Normally this should be the same as what you find in the [Releases section](https://github.com/svermeulen/Extenject/releases), but may also be slightly out of date since Unity Asset Store can take a week or so to review submissions sometimes.

1.  __UPM Branch__

    * This option is a [feature request](https://github.com/svermeulen/Extenject/issues/24). The package will be released when Unity is ready. Unity is not giving any insights on the development status. But the expectation is in the first or second release of 2020.
    * If you can not wait. There is an alternative. But you will need the Unity extension found [here](https://github.com/mob-sakai/UpmGitExtension). And the package found [here](https://github.com/starikcetin/Extenject/tree/upm).

1.  __From Source__

    * After syncing the git repo, note that you will have to build the `Zenject-Usage.dll` by building the solution at `AssemblyBuild\Zenject-usage\Zenject-usage.sln`.  Or, if you prefer you can get `Zenject-Usage.dll` from Releases section instead
    * Then you can copy the `UnityProject/Assets/Plugins/Zenject` directory to your own Unity3D project.

Note that when importing Zenject into your unity project, you can uncheck any folder underneath the OptionalExtras folder for cases where you don't want to include it, or if you just want the core zenject functionality, you can uncheck the entire OptionalExtras directory.

## History

Unity is a fantastic game engine, however the approach that new developers are encouraged to take does not lend itself well to writing large, flexible, or scalable code bases.  In particular, the default way that Unity manages dependencies between different game components can often be awkward and error prone.

This project was started because at the time there wasn't any DI frameworks for Unity, and having used DI frameworks outside of Unity (eg.  Ninject) and seeing the benefits, I felt it was important to remedy that.

Finally, I will just say that if you don't have experience with DI frameworks, and are writing object oriented code, then trust me, you will thank me later!  Once you learn how to write properly loosely coupled code using DI, there is simply no going back.

## Documentation

The Zenject documentation is split up into the following sections.  It is split up into two parts (Introduction and Advanced) so that you can get up and running as quickly as possible.  I would recommend at least skimming through the Introduction section before beginning, but then feel free to jump around in the advanced section as necessary

Another great starting point is to watch [this youtube series on zenject](https://www.youtube.com/watch?v=IS2YUIb_w_M&list=PLKERDLXpXl_jNJPY2czQcfPXW4BJaGZc_) created by Infallible Code.

You might also benefit from playing with the provided sample projects (which you can find by opening `Zenject/OptionalExtras/SampleGame1` or `Zenject/OptionalExtras/SampleGame2`).

If you are a DI veteran, then it might be worth taking a look at the [cheatsheet](#cheat-sheet) at the bottom of this page, which should give you an idea of the syntax, which might be all you need to get started.

The tests may also be helpful to show usage for each specific feature (which you can find at `Zenject/OptionalExtras/UnitTests` and `Zenject/OptionalExtras/IntegrationTests`)

Also see [further reading section](#further-reading) for some external zenject tutorials provided elsewhere.

# What is Dependency Injection? 

## Theory

What follows is a general overview of Dependency Injection from my perspective.  However, it is kept light, so I highly recommend seeking other resources for more information on the subject, as there are many other people (often with better writing ability) that have written about the theory behind it.

When writing an individual class to achieve some functionality, it will likely need to interact with other classes in the system to achieve its goals.  One way to do this is to have the class itself create its dependencies, by calling concrete constructors:

```csharp
public class Foo
{
    ISomeService _service;

    public Foo()
    {
        _service = new SomeService();
    }

    public void DoSomething()
    {
        _service.PerformTask();
       … 
    }
}
```

This works fine for small projects, but as your project grows it starts to get unwieldy.  The class Foo is tightly coupled to class 'SomeService'.  If we decide later that we want to use a different concrete implementation then we have to go back into the Foo class to change it.

After thinking about this, often you come to the realization that ultimately, Foo shouldn't bother itself with the details of choosing the specific implementation of the service.  All Foo should care about is fulfilling its own specific responsibilities.  As long as the service fulfills the abstract interface required by Foo, Foo is happy.  Our class then becomes:

```csharp
public class Foo
{
    ISomeService _service;

    public Foo(ISomeService service)
    {
        _service = service;
    }

    public void DoSomething()
    {
        _service.PerformTask();
        ...
    }
}
```

This is better, but now whatever class is creating Foo (let's call it Bar) has the problem of filling in Foo's extra dependencies:

```csharp
public class Bar
{
    public void DoSomething()
    {
        var foo = new Foo(new SomeService());
        foo.DoSomething();
        ...
    }
}
```

And class Bar probably also doesn't really care about what specific implementation of SomeService Foo uses.  Therefore we push the dependency up again:

```csharp
public class Bar
{
    ISomeService _service;

    public Bar(ISomeService service)
    {
        _service = service;
    }

    public void DoSomething()
    {
        var foo = new Foo(_service);
        foo.DoSomething();
        ...
    }
}
```

So we find that it is useful to push the responsibility of deciding which specific implementations of which classes to use further and further up in the 'object graph' of the application.  Taking this to an extreme, we arrive at the entry point of the application, at which point all dependencies must be satisfied before things start.  The dependency injection lingo for this part of the application is called the 'composition root'.  It would normally look like this:

```csharp
var service = new SomeService();
var foo = new Foo(service);
var bar = new Bar(service);
var qux = new Qux(bar);

.. etc.
```

DI frameworks such as Zenject simply help automate this process of creating and handing out all these concrete dependencies, so that you don't need to explicitly do so yourself like in the above code.

## Misconceptions

There are many misconceptions about DI, due to the fact that it can be tricky to fully wrap your head around at first.  It will take time and experience before it fully 'clicks'.

As shown in the above example, DI can be used to easily swap different implementations of a given interface (in the example this was ISomeService).  However, this is only one of many benefits that DI offers.

More important than that is the fact that using a dependency injection framework like Zenject allows you to more easily follow the '[Single Responsibility Principle](http://en.wikipedia.org/wiki/Single_responsibility_principle)'.  By letting Zenject worry about wiring up the classes, the classes themselves can just focus on fulfilling their specific responsibilities.

<a id="overusinginterfaces"></a>Another common mistake that people new to DI make is that they extract interfaces from every class, and use those interfaces everywhere instead of using the class directly.  The goal is to make code more loosely coupled, so it's reasonable to think that being bound to an interface is better than being bound to a concrete class.  However, in most cases the various responsibilities of an application have single, specific classes implementing them, so using interfaces in these cases just adds unnecessary maintenance overhead.  Also, concrete classes already have an interface defined by their public members.  A good rule of thumb instead is to only create interfaces when the class has more than one implementation or in cases where you intend to have multiple implemenations in the future (this is known, by the way, as the [Reused Abstraction Principle](http://codemanship.co.uk/parlezuml/blog/?postid=934))

Other benefits include:

* Refactorability - When code is loosely coupled, as is the case when using DI properly, the entire code base is much more resilient to changes.  You can completely change parts of the code base without having those changes wreak havoc on other parts.
* Encourages modular code - When using a DI framework you will naturally follow better design practices, because it forces you to think about the interfaces between classes.
* Testability - Writing automated unit tests or user-driven tests becomes very easy, because it is just a matter of writing a different 'composition root' which wires up the dependencies in a different way.  Want to only test one subsystem?  Simply create a new composition root.  Zenject also has some support for avoiding code duplication in the composition root itself (using Installers - described below).

Also see [here](#isthisoverkill) and [here](#zenject-philophy) for further discussion and justification for using a DI framework.

# Introduction to Zenject API

## Hello World Example

```csharp
using Zenject;
using UnityEngine;
using System.Collections;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Hello World!");
        Container.Bind<Greeter>().AsSingle().NonLazy();
    }
}

public class Greeter
{
    public Greeter(string message)
    {
        Debug.Log(message);
    }
}
```

You can run this example by doing the following:

* Create a new scene in Unity
* Right Click inside the Hierarchy tab and select `Zenject -> Scene Context`
* Right Click in a folder within the Project Tab and Choose `Create -> Zenject -> MonoInstaller`.  Name it TestInstaller.cs
* Add your TestInstaller script to the scene (as its own GameObject or on the same GameObject as the SceneContext, it doesn't matter)
* Add a reference to your TestInstaller to the properties of the SceneContext by adding a new row in the inspector of the "Installers" property (press the + button) and then dragging TestInstaller to it
* Open up TestInstaller and paste the above code into it
* Validate your scene by either selecting Edit -> Zenject -> Validate Current Scene or hitting CTRL+ALT+V.  (note that this step isn't necessary but good practice to get into)
* Run
* Note also, that you can use the shortcut `CTRL+SHIFT+R` to "validate then run".  Validation is usually fast enough that this does not add a noticeable overhead to running your game, and when it does detect errors it is much faster to iterate on since you avoid the startup time.
* Observe unity console for output

The SceneContext MonoBehaviour is the entry point of the application, where Zenject sets up all the various dependencies before kicking off your scene.  To add content to your Zenject scene, you need to write what is referred to in Zenject as an 'Installer', which declares all the dependencies and their relationships with each other.  All dependencies that are marked as "NonLazy" are automatically created after the installers are run, which is why the Greeter class that we added above gets created on startup.  If this doesn't make sense to you yet, keep reading!

## Injection

There are many different ways of declaring dependencies on the container, which are documented in the [next section](#binding).  There are also several ways of having these dependencies injected into your classes. These are:

1 - **Constructor Injection**

```csharp
public class Foo
{
    IBar _bar;

    public Foo(IBar bar)
    {
        _bar = bar;
    }
}
```

2 - **Field Injection**

```csharp
public class Foo
{
    [Inject]
    IBar _bar;
}
```

Field injection occurs immediately after the constructor is called.  All fields that are marked with the `[Inject]` attribute are looked up in the container and given a value.  Note that these fields can be private or public and injection will still occur.

3 - **Property Injection**

```csharp
public class Foo
{
    [Inject]
    public IBar Bar
    {
        get;
        private set;
    }
}
```

Property injection works the same as field injection except is applied to C# properties.  Just like fields, the setter can be private or public in this case.

4 - **Method Injection**

```csharp
public class Foo
{
    IBar _bar;
    Qux _qux;

    [Inject]
    public void Init(IBar bar, Qux qux)
    {
        _bar = bar;
        _qux = qux;
    }
}
```

Method Inject injection works very similarly to constructor injection.

Note the following:

- Inject methods are the recommended approach for MonoBehaviours, since MonoBehaviours cannot have constructors.
- There can be any number of inject methods.  In this case, they are called in the order of Base class to Derived class.  This can be useful to avoid the need to forward many dependencies from derived classes to the base class via constructor parameters, while also guaranteeing that the base class inject methods complete first, just like how constructors work.
- Inject methods are called after all other injection types.  It is designed this way so that these methods can be used to execute initialization logic which might make use of injected fields or properties.  Note also that you can leave the parameter list empty if you just want to do some initialization logic only.
- You can safely assume that the dependencies that you receive via inject methods will themselves already have been injected (the only exception to this is in the case where you have circular dependencies).  This can be important if you use inject methods to perform some basic initialization, since in that case you may need the given dependencies to be initialized as well
- Note however that it is usually not a good idea to use inject methods for initialization logic.  Often it is better to use IInitializable.Initialize or Start() methods instead, since this will allow the entire initial object graph to be created first.

**Recommendations**

Best practice is to prefer constructor/method injection compared to field/property injection.
* Constructor injection forces the dependency to only be resolved once, at class creation, which is usually what you want.  In most cases you don't want to expose a public property for your initial dependencies because this suggests that it's open to changing.
* Constructor injection guarantees no circular dependencies between classes, which is generally a bad thing to do.  Zenject does allow circular dependencies when using other injections types however such as method/field/property injection
* Constructor/Method injection is more portable for cases where you decide to re-use the code without a DI framework such as Zenject.  You can do the same with public properties but it's more error prone (it's easier to forget to initialize one field and leave the object in an invalid state)
* Finally, Constructor/Method injection makes it clear what all the dependencies of a class are when another programmer is reading the code.  They can simply look at the parameter list of the method.  This is also good because it will be more obvious when a class has too many dependencies and should therefore be split up (since its constructor parameter list will start to seem long)

## Register Mappings to the DI Container 

The core of a dependency injection framework is the DI container. In it's simplest form it's an object which contains a dictionary that holds all the _registrations_. 
In this section we are going to cover the 'register a new mapping' part. In Zenject it's called _binding_. As it creates a binding between an _abstraction_ to a _concrete type_. 

### Binding

---

Every dependency injection framework is ultimately just a framework to bind types to instances.

In Zenject, dependency mapping is done by adding bindings to something called a container. The container should then 'know' how to create all the object instances in your application, by recursively resolving all dependencies for a given object.

When the container is asked to construct an instance of a given type, it uses C# reflection to find the list of constructor arguments, and all fields/properties that are marked with an [Inject] attribute. It then attempts to resolve each of these required dependencies, which it uses to call the constructor and create the new instance.

Each Zenject application therefore must tell the container how to resolve each of these dependencies, which is done via Bind commands.  For example, given the following class:

```csharp
public class Foo
{
    IBar _bar;

    public Foo(IBar bar)
    {
        _bar = bar;
    }
}
```

You can wire up the dependencies for this class with the following:

```csharp
Container.Bind<Foo>().AsSingle();
Container.Bind<IBar>().To<Bar>().AsSingle();
```

This tells Zenject that every class that requires a dependency of type Foo should use the same instance, which it will automatically create when needed.  And similarly, any class that requires the IBar interface (like Foo) will be given the same instance of type Bar.

The full format for the bind command is the following.  Note that in most cases you will not use all of these methods and that they all have logical defaults when unspecified

<pre>
Container.Bind&lt;<b>ContractType</b>&gt;()
    .WithId(<b>Identifier</b>)
    .To&lt;<b>ResultType</b>&gt;()
    .From<b>ConstructionMethod</b>()
    .As<b>Scope</b>()
    .WithArguments(<b>Arguments</b>)
    .OnInstantiated(<b>InstantiatedCallback</b>)
    .When(<b>Condition</b>)
    .(<b>Copy</b>|<b>Move</b>)Into(<b>All</b>|<b>Direct</b>)SubContainers()
    .NonLazy()
    .IfNotBound();
</pre>

Where:

* **ContractType** = The type that you are creating a binding for.

    * This value will correspond to the type of the field/parameter that is being injected.

* **ResultType** = The type to bind to.

    * Default: **ContractType**
    * This type must either to equal to **ContractType** or derive from **ContractType**.  If unspecified, it assumes ToSelf(), which means that the **ResultType** will be the same as the **ContractType**.  This value will be used by whatever is given as the **ConstructionMethod** to retrieve an instance of this type

* **Identifier** = The value to use to uniquely identify the binding.  This can be ignored in most cases, but can be quite useful in cases where you need to distinguish between multiple bindings with the same contract type.  See [here](#identifiers) for details.

* **ConstructionMethod** = The method by which an instance of **ResultType** is created/retrieved.  See [this section](#construction-methods) for more details on the various construction methods.

    * Default: FromNew()
    * Examples: eg. FromGetter, FromMethod, FromResolve, FromComponentInNewPrefab, FromSubContainerResolve, FromInstance, etc.

* **Scope** = This value determines how often (or if at all) the generated instance is re-used across multiple injections.

    * Default: AsTransient.  Note however that not all bindings have a default, so an exception will be thrown if not supplied.  The bindings that do not require the scope to be set explicitly are any binding with a construction method that is a search rather than creating a new object from scratch (eg. FromMethod, FromComponentX, FromResolve, etc.)
    * It can be one of the following:
        1. **AsTransient** - Will not re-use the instance at all.  Every time **ContractType** is requested, the DiContainer will execute the given construction method again
        2. **AsCached** - Will re-use the same instance of **ResultType** every time **ContractType** is requested, which it will lazily generate upon first use
        3. **AsSingle** - Exactly the same as AsCached, except that it will sometimes throw exceptions if there already exists a binding for **ResultType**.  It is simply a way to ensure that the given **ResultType** is unique within the container.  Note however that it will only guarantee that there is only one instance across the given container, which means that using AsSingle with the same binding in a sub-container could generate a second instance.

    * In most cases, you will likely want to just use AsSingle, however AsTransient and AsCached have their uses too.

* **Arguments** = A list of objects to use when constructing the new instance of type **ResultType**.  This can be useful as an alternative to adding other bindings for the arguments in the form `Container.BindInstance(arg).WhenInjectedInto<ResultType>()`
* **InstantiatedCallback** = In some cases it is useful to be able customize an object after it is instantiated.  In particular, if using a third party library, it might be necessary to change a few fields on one of its types.  For these cases you can pass a method to OnInstantiated that can customize the newly created instance.  For example:

    ```csharp
    Container.Bind<Foo>().AsSingle().OnInstantiated<Foo>(OnFooInstantiated);

    void OnFooInstantiated(InjectContext context, Foo foo)
    {
        foo.Qux = "asdf";
    }
    ```

    Or, equivalently:

    ```csharp
    Container.Bind<Foo>().AsSingle().OnInstantiated<Foo>((ctx, foo) => foo.Bar = "qux");
    ```

    Note that you can also bind a custom factory using FromFactory that directly calls Container.InstantiateX before customizing it for the same effect, but OnInstantiated can be easier in some cases

* **Condition** = The condition that must be true for this binding to be chosen.  See [here](#conditional-bindings) for more details.
* (**Copy**|**Move**)Into(**All**|**Direct**)SubContainers = This value can be ignored for 99% of users.  It can be used to automatically have the binding inherited by subcontainers.  For example, if you have a class Foo and you want a unique instance of Foo to be automatically placed in the container and every subcontainer, then you could add the following binding:

    ```csharp
    Container.Bind<Foo>().AsSingle().CopyIntoAllSubContainers()
    ```

    In other words, the result will be equivalent to copying and pasting the `Container.Bind<Foo>().AsSingle()` statement into the installer for every sub-container.

    Or, if you only wanted Foo in the subcontainers and not the current container:

    ```csharp
    Container.Bind<Foo>().AsSingle().MoveIntoAllSubContainers()
    ```

    Or, if you only wanted Foo to be in the immediate child subcontainer, and not the subcontainers of these subcontainers:

    ```csharp
    Container.Bind<Foo>().AsSingle().MoveIntoDirectSubContainers()
    ```

* **NonLazy** = Normally, the **ResultType** is only ever instantiated when the binding is first used (aka "lazily").  However, when NonLazy is used, **ResultType** will immediately be created on startup.

* **IfNotBound** = When this is added to a binding and there is already a binding with the given contract type + identifier, then this binding will be skipped.

### Construction Methods

1. **FromNew** - Create via the C# new operator. This is the default if no construction method is specified.

    ```csharp
    // These are both the same
    Container.Bind<Foo>();
    Container.Bind<Foo>().FromNew();
    ```

1. **FromInstance** - Add a given instance to the container.  Note that the given instance will not be injected in this case.  If you also want your instance to be injected at startup, see [QueueForInject](#dicontainer-methods-queueforinject)

    ```csharp
    Container.Bind<Foo>().FromInstance(new Foo());

    // You can also use this short hand which just takes ContractType from the parameter type
    Container.BindInstance(new Foo());

    // This is also what you would typically use for primitive types
    Container.BindInstance(5.13f);
    Container.BindInstance("foo");

    // Or, if you have many instances, you can use BindInstances
    Container.BindInstances(5.13f, "foo", new Foo());
    ```

1. **FromMethod** - Create via a custom method

    ```csharp
    Container.Bind<Foo>().FromMethod(SomeMethod);

    Foo SomeMethod(InjectContext context)
    {
        ...
        return new Foo();
    }
    ```

1. **FromMethodMultiple** - Same as FromMethod except allows returning multiple instances at once (or zero).

    ```csharp
    Container.Bind<Foo>().FromMethodMultiple(GetFoos);

    IEnumerable<Foo> GetFoos(InjectContext context)
    {
        ...
        return new Foo[]
        {
            new Foo(),
            new Foo(),
            new Foo(),
        }
    }
    ```

1. **FromFactory** - Create instance using a custom factory class.  This construction method is similar to `FromMethod` except can be cleaner in cases where the logic is more complicated or requires dependencies (since the factory itself can have dependencies injected)

    ```csharp
    class FooFactory : IFactory<Foo>
    {
        public Foo Create()
        {
            // ...
            return new Foo();
        }
    }

    Container.Bind<Foo>().FromFactory<FooFactory>()
    ```

1. **FromIFactory** - Create instance using a custom factory class.  This is a more generic and more powerful alternative to FromFactory, because you can use any kind of construction method you want for your custom factory (unlike FromFactory which assumes `FromNew().AsCached()`)

    For example, you could use a factory that is a scriptable object like this:

    ```csharp
    class FooFactory : ScriptableObject, IFactory<Foo>
    {
        public Foo Create()
        {
            // ...
            return new Foo();
        }
    }

    Container.Bind<Foo>().FromIFactory(x => x.To<FooFactory>().FromScriptableObjectResource("FooFactory")).AsSingle();
    ```

    Or, you might want to have your custom factory be placed in a subcontainer like this:

    ```csharp
    public class FooFactory : IFactory<Foo>
    {
        public Foo Create()
        {
            return new Foo();
        }
    }

    public override void InstallBindings()
    {
        Container.Bind<Foo>().FromIFactory(x => x.To<FooFactory>().FromSubContainerResolve().ByMethod(InstallFooFactory)).AsSingle();
    }

    void InstallFooFactory(DiContainer subContainer)
    {
        subContainer.Bind<FooFactory>().AsSingle();
    }
    ```

    Also note that the following two lines are equivalent:

    ```csharp
    Container.Bind<Foo>().FromFactory<FooFactory>().AsSingle();
    Container.Bind<Foo>().FromIFactory(x => x.To<FooFactory>().AsCached()).AsSingle();
    ```

1. **FromComponentInNewPrefab** - Instantiate the given prefab as a new game object, inject any MonoBehaviour's on it, and then search the result for type **ResultType** in a similar way that `GetComponentInChildren` works

    ```csharp
    Container.Bind<Foo>().FromComponentInNewPrefab(somePrefab);
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    Note that if there are multiple matches for **ResultType** on the prefab it will only match the first one encountered just like how GetComponentInChildren works.  So if you are binding a prefab and there isn't a specific MonoBehaviour/Component that you want to bind to, you can just choose Transform and it will match the root of the prefab.

1. **FromComponentsInNewPrefab** - Same as FromComponentInNewPrefab except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere.  Can be thought of as similar to `GetComponentsInChildren`

1. **FromComponentInNewPrefabResource** - Instantiate the given prefab (found at the given resource path) as a new game object, inject any MonoBehaviour's on it, and then search the result for type **ResultType** in a similar way that `GetComponentInChildren` works (in that it will return the first matching value found)

    ```csharp
    Container.Bind<Foo>().FromComponentInNewPrefabResource("Some/Path/Foo");
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromComponentsInNewPrefabResource** - Same as FromComponentInNewPrefab except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere.  Can be thought of as similar to `GetComponentsInChildren`

1. **FromNewComponentOnNewGameObject** - Create a new empty game object and then instantiate a new component of the given type on it.

    ```csharp
    Container.Bind<Foo>().FromNewComponentOnNewGameObject();
    ```

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromNewComponentOnNewPrefab** - Instantiate the given prefab as a new game object and also instantiate a new instance of the given component on the root of the new game object.  NOTE: It is not necessary that the prefab contains a copy of the given component.

    ```csharp
    Container.Bind<Foo>().FromNewComponentOnNewPrefab(somePrefab);
    ```

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromNewComponentOnNewPrefabResource** - Instantiate the given prefab (found at the given resource path) and also instantiate a new instance of the given component on the root of the new game object.  NOTE: It is not necessary that the prefab contains a copy of the given component.

    ```csharp
    Container.Bind<Foo>().FromNewComponentOnNewPrefabResource("Some/Path/Foo");
    ```

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromNewComponentOn** - Instantiate a new component of the given type on the given game object

    ```csharp
    Container.Bind<Foo>().FromNewComponentOn(someGameObject);
    ```

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromNewComponentSibling** - Instantiate a new component of the given on the current transform.  The current transform here is taken from the object being injected, which must therefore be a MonoBehaviour derived type.

    ```csharp
    Container.Bind<Foo>().FromNewComponentSibling();
    ```

    Note that if the given component type is already attached to the current transform that this will just return that instead of creating a new component.  As a result, this bind statement functions similar to Unity's [RequireComponent] attribute.

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    Also note that if a non-MonoBehaviour requests the given type, an exception will be thrown, since there is no current transform in that case.

1. **FromComponentInHierarchy** - Look up the component within the scene hierarchy associated with the current context, as well as the hierarchy associated with any parent contexts.  Works similar to `GetComponentInChildren` in that it will return the first matching value found

    ```csharp
    Container.Bind<Foo>().FromComponentInHierarchy().AsSingle();
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    In the most common case where the context is a SceneContext, this will search the entire scene hierarchy (except any sub-contexts such as GameObjectContext).  In other words, when the current context is a scene context, it will behave similar to `GameObject.FindObjectsOfType`.  Note that since this could be a big search, it should be used with caution, just like `GameObject.FindObjectsOfType` should be used with caution.

    In the case where the context is GameObjectContext, it will only search within and underneath the game object root (and any parent contexts).

    In the case where the context is ProjectContext, it will only search within the project context prefab

1. **FromComponentsInHierarchy** - Same as FromComponentInHierarchy except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere.  Can be thought of as similar to `GetComponentsInChildren`

1. **FromComponentSibling** - Look up the given component type by searching over the components that are attached to the current transform.  The current transform here is taken from the object being injected, which must therefore be a MonoBehaviour derived type. 

    ```csharp
    Container.Bind<Foo>().FromComponentSibling();
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    Note that if a non-MonoBehaviour requests the given type, an exception will be thrown, since there is no current transform in that case.

1. **FromComponentsSibling** - Same as FromComponentSibling except will match multiple values or zero values.

1. **FromComponentInParents** - Look up the component by searching the current transform or any parent for the given component type.  The current transform here is taken from the object being injected, which must therefore be a MonoBehaviour derived type. 

    ```csharp
    Container.Bind<Foo>().FromComponentInParents();
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    Note that if a non-MonoBehaviour requests the given type, an exception will be thrown, since there is no current transform in that case.

1. **FromComponentsInParents** - Same as FromComponentInParents except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere

1. **FromComponentInChildren** - Look up the component by searching the current transform or any child transform for the given component type.  The current transform here is taken from the object being injected, which must therefore be a MonoBehaviour derived type.   Similar to `GetComponentInChildren` in that it will return the first matching value found

    ```csharp
    Container.Bind<Foo>().FromComponentInChildren();
    ```

    **ResultType** must either be an interface or derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

    Note that if a non-MonoBehaviour requests the given type, an exception will be thrown, since there is no current transform in that case.

1. **FromComponentsInChildren** - Same as FromComponentInChildren except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere.  Can be thought of as similar to `GetComponentsInChildren`

1. **FromNewComponentOnRoot** - Instantiate the given component on the root of the current context.  This is most often used with GameObjectContext.

    ```csharp
    Container.Bind<Foo>().FromNewComponentOnRoot();
    ```

    **ResultType** must derive from UnityEngine.MonoBehaviour / UnityEngine.Component in this case

1. **FromResource** - Create by calling the Unity3d function `Resources.Load` for **ResultType**.  This can be used to load any type that `Resources.Load` can load, such as textures, sounds, prefabs, etc.

    ```csharp
    Container.Bind<Texture>().WithId("Glass").FromResource("Some/Path/Glass");
    ```

1. **FromResources** - Same as FromResource except will match multiple values or zero values.  You might use it for example and then inject `List<ContractType>` somewhere

1. **FromScriptableObjectResource** - Bind directly to the scriptable object instance at the given resource path.  NOTE:  Changes to this value while inside unity editor will be saved persistently.  If this is undesirable, use FromNewScriptableObjectResource.

    ```csharp
    public class Foo : ScriptableObject
    {
    }

    Container.Bind<Foo>().FromScriptableObjectResource("Some/Path/Foo");
    ```

1. **FromNewScriptableObjectResource** - Same as FromScriptableObjectResource except it will instantiate a new copy of the given scriptable object resource.  This can be useful if you want to have multiple distinct instances of the given scriptable object resource, or if you want to ensure that the saved values for the scriptable object are not affected by changing at runtime.

1. **FromResolve** - Get instance by doing another lookup on the container (in other words, calling `DiContainer.Resolve<ResultType>()`).  Note that for this to work, **ResultType** must be bound in a separate bind statement.  This construction method can be especially useful when you want to bind an interface to another interface, as shown in the below example

    ```csharp
    public interface IFoo
    {
    }

    public interface IBar : IFoo
    {
    }

    public class Foo : IBar
    {
    }

    Container.Bind<IFoo>().To<IBar>().FromResolve();
    Container.Bind<IBar>().To<Foo>();
    ```

1. **FromResolveAll** - Same as FromResolve except will match multiple values or zero values.

1. **FromResolveGetter&lt;ObjectType&gt;** - Get instance from the property of another dependency which is obtained by doing another lookup on the container (in other words, calling `DiContainer.Resolve<ObjectType>()` and then accessing a value on the returned instance of type **ResultType**).  Note that for this to work, **ObjectType** must be bound in a separate bind statement.

    ```csharp
    public class Bar
    {
    }

    public class Foo
    {
        public Bar GetBar()
        {
            return new Bar();
        }
    }

    Container.Bind<Foo>();
    Container.Bind<Bar>().FromResolveGetter<Foo>(x => x.GetBar());
    ```

1. **FromResolveAllGetter&lt;ObjectType&gt;** - Same as FromResolveGetter except will match multiple values or zero values.

1. **FromSubContainerResolve** - Get **ResultType** by doing a lookup on a subcontainer.  Note that for this to work, the sub-container must have a binding for **ResultType**.  This approach can be very powerful, because it allows you to group related dependencies together inside a mini-container, and then expose only certain classes (aka ["Facades"](https://en.wikipedia.org/wiki/Facade_pattern)) to operate on this group of dependencies at a higher level.  For more details on using sub-containers, see [this section](#sub-containers-and-facades).  There are several ways to define the subcontainer:

    1. **ByNewPrefabMethod** - Initialize subcontainer by instantiating a new prefab.  Note that unlike `ByNewContextPrefab`, this bind method does not require that there be a GameObjectContext attached to the prefab.  In this case the GameObjectContext is added dynamically and then run with the given installer method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabMethod(MyPrefab, InstallFoo);

        void InstallFoo(DiContainer subContainer)
        {
            subContainer.Bind<Foo>();
        }
        ```

        Note that instead of passing in a prefab directly, you can also pass in a getter method.  For example:

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabMethod(ChooseFooPrefab, InstallFoo);

        UnityEngine.Object ChooseFooPrefab(InjectContext context) {
            return FooPrefabs[Random.Range(0, FooPrefabs.Length)];
        }
        ```

    1. **ByNewPrefabInstaller** - Initialize subcontainer by instantiating a new prefab.  Same as ByNewPrefabMethod, except it initializes the dynamically created GameObjectContext with the given installer rather than a method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabInstaller<FooInstaller>(MyPrefab);

        class FooInstaller : Installer
        {
            public override void InstallBindings()
            {
                Container.Bind<Foo>();
            }
        }
        ```

        Note that instead of passing in a prefab directly, you can also pass in a getter method.  For example:

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabInstaller<FooInstaller>(ChooseFooPrefab);

        UnityEngine.Object ChooseFooPrefab(InjectContext context) {
            return FooPrefabs[Random.Range(0, FooPrefabs.Length)];
        }
        ```

    1. **ByNewPrefabResourceMethod** - Initialize subcontainer instantiating a new prefab obtained via `Resources.Load`.  Note that unlike `ByNewPrefabResource`, this bind method does not require that there be a GameObjectContext attached to the prefab.  In this case the GameObjectContext is added dynamically and then run with the given installer method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabResourceMethod("Path/To/MyPrefab", InstallFoo);

        void InstallFoo(DiContainer subContainer)
        {
            subContainer.Bind<Foo>();
        }
        ```

    1. **ByNewPrefabResourceInstaller** - Initialize subcontainer instantiating a new prefab obtained via `Resources.Load`.  Same as ByNewPrefabResourceMethod, except it initializes the dynamically created GameObjectContext with the given installer rather than a method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewPrefabResourceInstaller<FooInstaller>("Path/To/MyPrefab");

        class FooInstaller : Installer<FooInstaller>
        {
            public override void InstallBindings()
            {
                Container.Bind<Foo>();
            }
        }
        ```

    1. **ByNewGameObjectInstaller** - Initialize subcontainer by instantiating a empty game object, attaching GameObjectContext, and then installing using the given installer.  This approach is very similar to ByInstaller except has the following advantages:

        - Any ITickable, IInitializable, IDisposable, etc. bindings will be called properly
        - Any new game objects that are instantiated inside the subcontainer will be parented underneath the game object context object
        - You can destroy the subcontainer by just destroying the game object context game object

    1. **ByNewGameObjectMethod** - Same as ByNewGameObjectInstaller except the subcontainer is initialized based on the given method rather than an installer type.

    1. **ByMethod** - Initialize the subcontainer by using a method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByMethod(InstallFooFacade);

        void InstallFooFacade(DiContainer subContainer)
        {
            subContainer.Bind<Foo>();
        }
        ```

        Note that when using ByMethod, if you want to use zenject interfaces such as ITickable, IInitializable, IDisposable inside your subcontainer then you have to also use the WithKernel bind method like this:

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByMethod(InstallFooFacade).WithKernel();

        void InstallFooFacade(DiContainer subContainer)
        {
            subContainer.Bind<Foo>();
            subContainer.Bind<ITickable>().To<Bar>();
        }
        ```

    1. **ByInstaller** - Initialize the subcontainer by using a class derived from `Installer`.  This can be a cleaner and less error-prone alternative than `ByMethod`, especially if you need to inject data into the installer itself.  Less error prone because when using ByMethod it is common to accidentally use Container instead of subContainer in your method.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByInstaller<FooFacadeInstaller>();

        class FooFacadeInstaller : Installer
        {
            public override void InstallBindings()
            {
                Container.Bind<Foo>();
            }
        }
        ```

        Note that when using ByInstaller, if you want to use zenject interfaces such as ITickable, IInitializable, IDisposable inside your subcontainer then you have to also use the WithKernel bind method like this:

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByInstaller<FooFacadeInstaller>().WithKernel();
        ```

    1. **ByNewContextPrefab** - Initialize subcontainer by instantiating a new prefab.  Note that the prefab must contain a `GameObjectContext` component attached to the root game object.  For details on `GameObjectContext` see [this section](#sub-containers-and-facades).

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewContextPrefab(MyPrefab);

        // Assuming here that this installer is added to the GameObjectContext at the root
        // of the prefab.  You could also use a ZenjectBinding in the case where Foo is a MonoBehaviour
        class FooFacadeInstaller : MonoInstaller
        {
            public override void InstallBindings()
            {
                Container.Bind<Foo>();
            }
        }
        ```

    1. **ByNewContextPrefabResource** - Initialize subcontainer instantiating a new prefab obtained via `Resources.Load`.  Note that the prefab must contain a `GameObjectContext` component attached to the root game object.

        ```csharp
        Container.Bind<Foo>().FromSubContainerResolve().ByNewContextPrefabResource("Path/To/MyPrefab");
        ```

    1. **ByInstance** - Initialize the subcontainer by directly using a given instance of DiContainer that you provide yourself.  This is only useful in some rare edge cases.

1. **FromSubContainerResolveAll** - Same as FromSubContainerResolve except will match multiple values or zero values.

## Installers

Often, there is some collections of related bindings for each sub-system and so it makes sense to group those bindings into a re-usable object.  In Zenject this re-usable object is called an 'installer'.  You can define a new installer as follows:

```csharp
public class FooInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();
        Container.BindInterfacesTo<Foo>().AsSingle();
        // etc...
    }
}
```

You add bindings by overriding the InstallBindings method, which is called by whatever `Context` the installer has been added to (usually this is `SceneContext`).  MonoInstaller is a MonoBehaviour so you can add FooInstaller by attaching it to a GameObject.  Since it is a GameObject you can also add public members to it to configure your installer from the Unity inspector.  This allows you to add references within the scene, references to assets, or simply tuning data (see [here](#using-the-unity-inspector-to-configure-settings) for more information on tuning data).

Note that in order for your installer to be triggered it must be attached to the Installers property of the `SceneContext` object.  Installers are installed in the order given to `SceneContext` (with scriptable object installers first, then mono installers, then prefab installers) however this order should not usually matter (since nothing should be instantiated during the install process).

In many cases you want to have your installer derive from MonoInstaller, so that you can have inspector settings.  There is also another base class called simply `Installer` which you can use in cases where you do not need it to be a MonoBehaviour.

You can also call an installer from another installer.  For example:

```csharp
public class BarInstaller : Installer<BarInstaller>
{
    public override void InstallBindings()
    {
        ...
    }
}

public class FooInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BarInstaller.Install(Container);
    }
}
```

Note that in this case BarInstaller is of type `Installer<>` (note the generic arguments) and not MonoInstaller, which is why we can simply call `BarInstaller.Install(Container)` and don't require that BarInstaller be added to our scene already.  Any calls to BarInstaller.Install will immediately instantiate a temporary instance of BarInstaller and then call InstallBindings on it.  This will repeat for any installers that this installer installs.  Note also that when using the `Installer<>` base class, we always must pass in ourself as the generic argument to `Installer<>`.  This is necessary so that the `Installer<>` base class can define the static method `BarInstaller.Install`.  It is also designed this way to support runtime parameters (described below).

One of the main reasons we use installers as opposed to just having all our bindings declared all at once for each scene, is to make them re-usable.  This is not a problem for installers of type `Installer<>` because you can simply call `FooInstaller.Install` as described above for every scene you wish to use it in, but then how would we re-use a MonoInstaller in multiple scenes?

There are three ways to do this.

1. **Prefab instances within the scene**.  After attaching your MonoInstaller to a gameobject in your scene, you can then create a prefab out of it.  This is nice because it allows you to share any configuration that you've done in the inspector on the MonoInstaller across scenes (and also have per-scene overrides if you want).  After adding it in your scene you can then drag and drop it on to the Installers property of a `Context`

1. **Prefabs**.  You can also directly drag your installer prefab from the Project tab into the InstallerPrefabs property of SceneContext.  Note that in this case you cannot have per-scene overrides like you can when having the prefab instantiated in your scene, but can be nice to avoid clutter in the scene.

1. **Prefabs within Resources folder**.  You can also place your installer prefabs underneath a Resoures folder and install them directly from code by using the Resources path.  For details on usage see [here](#runtime-parameters-for-installers).

Another option in addition to `MonoInstaller` and `Installer<>` is to use `ScriptableObjectInstaller` which has some unique advantages (especially for settings) - for details see [here](#scriptableobject-installer).

When calling installers from other installers it is common to want to pass parameters into it.  See [here](#runtime-parameters-for-installers) for details on how that is done.

## Using Non-MonoBehaviour Classes

### ITickable

---

In some cases it is preferable to avoid the extra weight of MonoBehaviours in favour of just normal C# classes.  Zenject allows you to do this much more easily by providing interfaces that mirror functionality that you would normally need to use a MonoBehaviour for.

For example, if you have code that needs to run per frame, then you can implement the `ITickable` interface:

```csharp
public class Ship : ITickable
{
    public void Tick()
    {
        // Perform per frame tasks
    }
}
```

Then, to hook it up in an installer:

```csharp
Container.Bind<ITickable>().To<Ship>().AsSingle();
```

Or if you don't want to have to always remember which interfaces your class implements, you can use the [shortcut described here](#bindinterfacesto-and-bindinterfacesandselfto)

Note that the order that the Tick() is called in for all ITickables is also configurable, as outlined [here](#update--initialization-order).

Also note that there are interfaces `ILateTickable` and `IFixedTickable` which mirror Unity's LateUpdate and FixedUpdated methods

### IInitializable

---

If you have some initialization that needs to occur on a given object, you could include this code in the constructor.  However, this means that the initialization logic would occur in the middle of the object graph being constructed, so it may not be ideal.

A better alternative is to implement `IInitializable`, and then perform initialization logic in an `Initialize()` method. 

Then, to hook it up in an installer:

```csharp
Container.Bind<IInitializable>().To<Foo>().AsSingle();
```

Or if you don't want to have to always remember which interfaces your class implements, you can use the [shortcut described here](#bindinterfacesto-and-bindinterfacesandselfto)

The `Foo.Initialize` method would then be called after the entire object graph is constructed and all constructors have been called.

Note that the constructors for the initial object graph are called during Unity's Awake event, and that the `IInitializable.Initialize` methods are called immediately on Unity's Start event.  Using `IInitializable` as opposed to a constructor is therefore more in line with Unity's own recommendations, which suggest that the Awake phase be used to set up object references, and the Start phase be used for more involved initialization logic.

This can also be better than using constructors or `[Inject]` methods because the initialization order is customizable in a similar way to `ITickable`, as explained [here](#update--initialization-order).

```csharp
public class Ship : IInitializable
{
    public void Initialize()
    {
        // Initialize your object here
    }
}
```

`IInitializable` works well for start-up initialization, but what about for objects that are created dynamically via factories?  (see [this section](#creating-objects-dynamically-using-factories) for what I'm referring to here).  For these cases you will most likely want to either use an `[Inject]` method or an explicit Initialize method that is called after the object is created.  For example:

```csharp
public class Foo
{
    [Inject]
    IBar _bar;

    [Inject]
    public void Initialize()
    {
        ...
        _bar.DoStuff();
        ...
    }
}
```

### IDisposable

---

If you have external resources that you want to clean up when the app closes, the scene changes, or for whatever reason the context object is destroyed, you can declare your class as `IDisposable` like below:

```csharp
public class Logger : IInitializable, IDisposable
{
    FileStream _outStream;

    public void Initialize()
    {
        _outStream = File.Open("log.txt", FileMode.Open);
    }

    public void Log(string msg)
    {
        _outStream.WriteLine(msg);
    }

    public void Dispose()
    {
        _outStream.Close();
    }
}
```

Then in your installer you can include:

```csharp
Container.Bind(typeof(Logger), typeof(IInitializable), typeof(IDisposable)).To<Logger>().AsSingle();
```

Or you can use the [BindInterfaces shortcut](#bindinterfacesto-and-bindinterfacesandselfto):

```csharp
Container.BindInterfacesAndSelfTo<Logger>().AsSingle();
```

This works because when the scene changes or your unity application is closed, the unity event `OnDestroy()` is called on all MonoBehaviours, including the SceneContext class, which then triggers `Dispose()` on all objects that are bound to `IDisposable`.

You can also implement the `ILateDisposable` interface which works similar to `ILateTickable` in that it will be called after all `IDisposable` objects have been triggered.  However, for most cases you're probably better off setting an explicit [execution order](#update--initialization-order) instead if the order is an issue.

### BindInterfacesTo and BindInterfacesAndSelfTo

---

If you end up using the `ITickable`, `IInitializable`, and `IDisposable` interfaces as described above, you will often end up with code like this:

```csharp
Container.Bind(typeof(Foo), typeof(IInitializable), typeof(IDisposable)).To<Logger>().AsSingle();
```

This can be a bit verbose sometimes.  Also, it is not ideal because if I later on decide that Foo doesn't need a `Tick()` or a `Dispose()` then I have to keep the installer in sync.

A better idea might be to just always use the interfaces like this:

```csharp
Container.Bind(new[] { typeof(Foo) }.Concat(typeof(Foo).GetInterfaces())).To<Foo>().AsSingle();
```

This pattern is useful enough that Zenject includes a custom bind method for it.  The above code is equivalent to:

```csharp
Container.BindInterfacesAndSelfTo<Foo>().AsSingle();
```

Now, we can add and remove interfaces to/from Foo and the installer remains the same.

In some cases you might *only* want to bind the interfaces, and keep Foo hidden from other classes.  In that case you would use the BindInterfacesTo method instead:

```csharp
Container.BindInterfacesTo<Foo>().AsSingle()
```

Which, in this case, would expand to:

```csharp
Container.Bind(typeof(IInitializable), typeof(IDisposable)).To<Foo>().AsSingle();
```

### Using the Unity Inspector To Configure Settings

---

One implication of writing most of your code as normal C# classes instead of MonoBehaviour's is that you lose the ability to configure data on them using the inspector.  You can however still take advantage of this in Zenject by using the following pattern:

```csharp
public class Foo : ITickable
{
    readonly Settings _settings;

    public Foo(Settings settings)
    {
        _settings = settings;
    }

    public void Tick()
    {
        Debug.Log("Speed: " + _settings.Speed);
    }

    [Serializable]
    public class Settings
    {
        public float Speed;
    }
}
```

Then, in an installer:

```csharp
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public Foo.Settings FooSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(FooSettings);
        Container.BindInterfacesTo<Foo>().AsSingle();
    }
}
```

Or, equivalently:

```csharp
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public Foo.Settings FooSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Foo>().AsSingle().WithArguments(FooSettings);
    }
}
```

Now, if we run our scene we can change the speed value to tune the Foo class in real time.

Another (arguably better) way to do this is to use `ScriptableObjectInstaller` instead of `MonoInstaller,` which have the added advantage that you can change your settings at runtime and have those changes automatically persist when play mode is stopped.  See [here](#scriptableobject-installer) for details.

## Object Graph Validation

**Overview**

The usual workflow when setting up bindings using a DI framework is something like this:

* Add some number of bindings in code
* Execute your app
* Observe a bunch of DI related exceptions
* Modify your bindings to address problem
* Repeat

This works ok for small projects, but as the complexity of your project grows it is often a tedious process.  The problem gets worse if the startup time of your application is particularly bad, or when the exceptions only occur from factories at various points at runtime.  What would be great is some tool to analyze your object graph and tell you exactly where all the missing bindings are, without requiring the cost of firing up your whole app.

You can do this in Zenject out-of-the-box by executing the menu item `Edit -> Zenject -> Validate Current Scene` or simply hitting `SHIFT+ALT+V` with the scenes open that you want to validate.  This will execute all installers for the current scene, with the result being a fully bound container.   It will then iterate through the object graphs and verify that all bindings can be found (without actually instantiating any of them).  In other words, it executes a 'dry run' of the normal startup procedure.  Under the hood, this works by storing dummy objects in the container in place of actually instantiating your classes.

Alternatively, you can execute the menu item `Edit -> Zenject -> Validate Then Run` or simply hitting `CTRL+ALT+R`.  This will validate the scenes you have open and then if validation succeeds, it will start play mode.  Validation is usually pretty fast so this can be a good alternative to always just hitting play, especially if your game has a costly startup time.

Note that this will also include factories and memory pools, which is especially helpful because those errors might not be caught until sometime after startup.

There are a few things to be aware of:

- No actual logic code is executed - only install bindings is called.  This means that if you have logic inside installers other than bind commands that these will be executed as well and may cause issues when running validation (if that logic requires that the container return actual values)
- **null** values are injected into the dependencies that are actually instantiated such as installers (regardles of what was binded)

You might want to inject some classes even in validation mode.  In that case you can mark them with `[ZenjectAllowDuringValidation]`.

Also note that some validation behaviour is configurable in [zenjectsettings](#zenjectsettings)

**Custom validatables**

If you want to add your own validation logic, you can do this simply by having one of your classes inherit from `IValidatable`.  After doing this, as long as your class is bound in an installer somewhere, it will be instantiated during validation and then its `Validate()` method will be called.  Note however that any dependencies it has will be injected as null (unless marked with `[ZenjectAllowDuringValidation]` attribute).

Inside the Validate method you can throw exceptions if you want validation to fail, or you can just log information to the console.  One common thing that occurs in custom validatables is to instantiate types that would not otherwise be validated.  By instantiating them during validation it will ensure that all their dependencies can be resolved.

For example, if you create a custom factory that directly instantiates a type using `Container.Instantiate<Foo>()`, then `Foo` will not be validated, so you will not find out if it is missing some dependencies until runtime.  However you can fix this by having your factory implement `IValidatable` and then calling `Container.Instantiate<Foo>()` inside the `Validate()` method.

## Scene Bindings

In many cases, you have a number of MonoBehaviours that have been added to the scene within the Unity editor (ie. at editor time not runtime) and you want to also have these MonoBehaviours added to the Zenject Container so that they can be injected into other classes.

The usual way this is done is to add public references to these objects within your installer like this:

```csharp
public class Foo : MonoBehaviour
{
}

public class GameInstaller : MonoInstaller
{
    public Foo foo;

    public override void InstallBindings()
    {
        Container.BindInstance(foo);
        Container.Bind<IInitializable>().To<GameRunner>().AsSingle();
    }
}

public class GameRunner : IInitializable
{
    readonly Foo _foo;

    public GameRunner(Foo foo)
    {
        _foo = foo;
    }

    public void Initialize()
    {
        ...
    }
}
```

This works fine however in some cases this can get cumbersome.  For example, if you want to allow an artist to add any number of `Enemy` objects to the scene, and you also want all those `Enemy` objects added to the Zenject Container.  In this case, you would have to manually drag each one to the inspector of one of your installers.  This is very error prone since its easy to forget one, or to delete the `Enemy` game object but forget to delete the null reference in the inspector for your installer, etc.

Another way to do this would be to use the FromComponentInHierarchy bind method like this:

```csharp
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Foo>().FromComponentInHierarchy().AsTransient();
        Container.Bind<IInitializable>().To<GameRunner>().AsSingle();
    }
}
```

Now, whenever a dependency of type Foo is required, zenject will search the entire scene for any MonoBehaviours of type Foo.  This will function very similarly to use Unity's [FindObjectsOfType](https://docs.unity3d.com/ScriptReference/Object.FindObjectsOfType.html) method every time you want to look up a certain dependency.  Note that because this method can be a very heavy operation, you probably want to mark it AsCached or AsSingle like this instead:

```csharp
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Foo>().FromComponentInHierarchy().AsCached();
        Container.Bind<IInitializable>().To<GameRunner>().AsSingle();
    }
}
```

This way, you only incur the performance hit for the search once the first time it is needed instead of every time it is injected to any class.  Note also that we can `FromComponentsInHierarchy` (note the plural) in cases where we expect there to be multiple Foos.

Yet another way to do this is to use the `ZenjectBinding` component.  You can do this by adding a `ZenjectBinding` MonoBehaviour to the same game object that you want to be automatically added to the Zenject container.

For example, if you have a MonoBehaviour of type `Foo` in your scene, you can just add `ZenjectBinding` alongside it, and then drag the Foo component into the Component property of the ZenjectBinding component.

<img src="Documentation/Images/AutoBind1.png?raw=true" alt="ZenjectBinding"/>

Then our installer becomes:

```csharp
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInitializable>().To<GameRunner>().AsSingle();
    }
}
```

The `ZenjectBinding` component has the following properties:

* **Bind Type** - This will determine what '[contract type](#binding)' to use.  It can be set to any of the following values:

    1. **Self**

    This is equivalent to the first example where we did this:

    ```csharp
    Container.Bind<Foo>().FromInstance(_foo);
    ```

    Or, equivalently:

    ```csharp
    Container.BindInstance(_foo);
    ```

    So if we duplicate this game object to have multiple game objects with `Foo` on them (as well as the `ZenjectBinding`), they will all be bound to the Container this way.  So after doing this, we would have to change `GameRunner` above to take a `List<Foo>` otherwise we would get Zenject exceptions (see [here](#list-bindings) for info on list bindings).

    2. **AllInterfaces**

    This bind type is equivalent to the following:

    ```csharp
    Container.BindInterfacesTo(_foo.GetType()).FromInstance(_foo);
    ```

    Note however, in this case, that `GameRunner` must ask for type `IFoo` in its constructor.  If we left `GameRunner` asking for type `Foo` then Zenject would throw exceptions, since the `BindInterfaces` method only binds the interfaces, not the concrete type.  If you want the concrete type as well then you can use:

    3. **AllInterfacesAndSelf**

    This bind type is equivalent to the following:

    ```csharp
    Container.BindInterfacesAndSelfTo(_foo.GetType()).FromInstance(_foo);
    ```

    This is the same as `AllInterfaces` except we can directly access Foo using type `Foo` instead of needing an interface.

    4. **BaseType**

    This bind type is equivalent to the following:

    ```csharp
    Container.Bind(_foo.GetType().BaseType()).FromInstance(_foo)
    ```

* **Identifier** - This value can be left empty most of the time.  It will determine what is used as the [identifier](#binding) for the binding.   For example, when set to "Foo1", it is equivalent to doing the following:

    ```csharp
    Container.BindInstance(_foo).WithId("Foo1");
    ```

* **Use Scene Context** - This is optional but useful in cases where you want to bind a dependency that is inside a GameObjectContext to the SceneContext.  You could also drag the SceneContext to the Context properly but this flag is a bit easier.

* **Context** - This is completely optional and in most cases should be left unset.  This will determine which `Context` to apply the binding to.  If left unset, it will use whatever context the GameObject is in.  In most cases this will be `SceneContext,` but if it's inside a `GameObjectContext` it will be bound into the `GameObjectContext` container instead.  One important use case for this field is to allow dragging the `SceneContext` into this field, for cases where the component is inside a `GameObjectContext.`  This allows you to treat this MonoBehaviour as a [Facade](https://en.wikipedia.org/wiki/Facade_pattern) for the entire sub-container given by the `GameObjectContext.`

## General Guidelines / Recommendations / Gotchas / Tips and Tricks

* **Do not use GameObject.Instantiate if you want your objects to have their dependencies injected**
    * If you want to instantiate a prefab at runtime and have any MonoBehaviour's automatically injected, we recommend using a [factory](#creating-objects-dynamically-using-factories).  You can also instantiate a prefab by directly using the DiContainer by calling any of the [InstantiatePrefab](#dicontainer-methods-instantiate) methods.  Using these ways as opposed to GameObject.Instantiate will ensure any fields that are marked with the `[Inject]` attribute are filled in properly, and all `[Inject]` methods within the prefab are called.

* **Best practice with DI is to *only* reference the container in the composition root "layer"**
    * Note that factories are part of this layer and the container can be referenced there (which is necessary to create objects at runtime).  See [here](#creating-objects-dynamically-using-factories) for more details on this.

* **Do not use IInitializable, ITickable and IDisposable for dynamically created objects**
    * Objects that are of type `IInitializable` are only initialized once - at startup during Unity's `Start` phase.  If you create an object through a factory, and it derives from `IInitializable`, the `Initialize()` method will not be called.  You should use `[Inject]` methods in this case or call Initialize() explicitly yourself after calling Create.
    * The same applies to `ITickable` and `IDisposable`.  Deriving from these will do nothing unless they are part of the original object graph created at startup.
    * If you have dynamically created objects that have an `Update()` method, it is usually best to call `Update()` on those manually, and often there is a higher level manager-like class in which it makes sense to do this from.  If however you prefer to use `ITickable` for dynamically objects you can declare a dependency to `TickableManager` and add/remove it explicitly as well.

* **Using multiple constructors**
    * You can have multiple constructors however you must mark one of them with the `[Inject]` attribute so Zenject knows which one to use.  If you have multiple constructors and none of them are marked with `[Inject]` then Zenject will guess that the intended constructor is the one with the least amount of arguments.

* **Lazily instantiated objects and the object graph**
    * Zenject does not immediately instantiate every object defined by the bindings that you've set up in your installers.  It will only instantiate those bindings that are marked `NonLazy`.  All other bindings are only instantiated when they are needed.  All the `NonLazy` objects as well as all their dependencies form the 'initial object graph' of the application.  Note that this automatically includes all types that implement `IInitializable,` `ITickable,` `IDisposable,` etc.   So if you have a binding that is not being created because nothing in the initial object graph references it, then you can make this explicit by adding `NonLazy` to your binding

* <a id="do-not-add-bindings-after-install"></a>**Restrict the use of bind commands to the 'composition root' only**.  In other words, do not make calls to `Container.Bind`, `Container.Rebind`, or `Container.Unbind` after the install phase is completed.  This important because immediately after install completes the initial object graph of your application is constructed, and needs access to the full set of bindings.

* <a id="bad-execution-order"></a>**The order that things occur in is wrong, like injection is occurring too late, or Initialize() event is not called at the right time, etc.**
    * It may be because the 'script execution order' of the Zenject classes `ProjectKernel` or `SceneKernel` or `SceneContext` is incorrect.  These classes should always have the earliest or near earliest execution order.  This should already be set by default (since this setting is included in the `cs.meta` files for these classes).  However if you are compiling Zenject yourself or have a unique configuration, you may want to make sure, which you can do by going to "Edit -> Project Settings -> Script Execution Order" and confirming that these classes are at the top, before the default time.

## Further Reading

Tutorials Provided Elsewhere:
* [Getting Started With Zenject](https://www.youtube.com/watch?v=IS2YUIb_w_M&list=PLKERDLXpXl_jNJPY2czQcfPXW4BJaGZc_)
* [Udemy course](https://www.udemy.com/dependency-injection-in-unity3d-using-zenject/)
* [Chris' Tutorials on Zenject](https://www.youtube.com/watch?v=Bcj35ceGCn0&list=PLyH-qXFkNSxnJbZLrxF0jWGyHB-8Kcd5q)
* [A better architecture for Unity projects](http://www.gamasutra.com/blogs/RubenTorresBonet/20180703/316442/A_better_architecture_for_Unity_projects.php)
* [Development For Winners](https://grofit.gitbooks.io/development-for-winners/content/)

# Advanced

## Binding

### Game Object Bind Methods

---

For bindings that create new game objects (eg. `FromComponentInNewPrefab`, `FromNewComponentOnNewGameObject`, etc.) there are also two extra bind methods.

* **WithGameObjectName** = The name to give the new Game Object associated with this binding.

    ```csharp
    Container.Bind<Foo>().FromComponentInNewPrefabResource("Some/Path/Foo").WithGameObjectName("Foo1");
    Container.Bind<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName("Foo1");
    ```

* **UnderTransformGroup(string)** = The name of the transform group to place the new game object under.  This is especially useful for factories, which can be used to create many copies of a prefab, so it can be nice to have them automatically grouped together within the scene hierarchy.

    ```csharp
    Container.BindFactory<Bullet, Bullet.Factory>()
        .FromComponentInNewPrefab(BulletPrefab)
        .UnderTransformGroup("Bullets");
    ```

* **UnderTransform(Transform)** = The actual transform to place the new game object under.

    ```csharp
    Container.BindFactory<Bullet, Bullet.Factory>()
        .FromComponentInNewPrefab(BulletPrefab)
        .UnderTransform(BulletTransform);
    ```

* **UnderTransform(Method)** = A method to provide the transform to use.

    ```csharp
    Container.BindFactory<Foo, Foo.Factory>()
        .FromComponentInNewGameObject()
        .UnderTransform(GetParent);

    Transform GetParent(InjectContext context)
    {
        if (context.ObjectInstance is Component)
        {
            return ((Component)context.ObjectInstance).transform;
        }

        return null;
    }
    ```

    This example will automatically parent the Foo GameObject underneath the game object that it is being injected into, unless the injected object is not a MonoBehaviour in which case it will leave Foo at the root of the scene hierarchy.

### Optional Binding

---

You can declare some dependencies as optional as follows:

```csharp
public class Bar
{
    public Bar(
        [InjectOptional]
        IFoo foo)
    {
        ...
    }
}
...

// You can comment this out and it will still work
Container.Bind<IFoo>().AsSingle();
```

Or, if you have an identifier as well:

```csharp
public class Bar
{
    public Bar(
        [Inject(Optional = true, Id = "foo1")]
        IFoo foo)
    {
        ...
    }
}
```

If an optional dependency is not bound in any installers, then it will be injected as null.

If the dependency is a primitive type (eg. `int,` `float`, `struct`) then it will be injected with its default value (eg. 0 for ints).

You may also assign an explicit default using the standard C# way such as:

```csharp
public class Bar
{
    public Bar(int foo = 5)
    {
        ...
    }
}
...

// Can comment this out and 5 will be used instead
Container.BindInstance(1);
```

Note also that the `[InjectOptional]` is not necessary in this case, since it's already implied by the default value.

Alternatively, you can define the primitive parameter as nullable, and perform logic depending on whether it is supplied or not, such as:

```csharp
public class Bar
{
    int _foo;

    public Bar(
        [InjectOptional]
        int? foo)
    {
        if (foo == null)
        {
            // Use 5 if unspecified
            _foo = 5;
        }
        else
        {
            _foo = foo.Value;
        }
    }
}

...

// Can comment this out and it will use 5 instead
Container.BindInstance(1);
```

#### Conditional Bindings

---

In many cases you will want to restrict where a given dependency is injected.  You can do this using the following syntax:

```csharp
Container.Bind<IFoo>().To<Foo1>().AsSingle().WhenInjectedInto<Bar1>();
Container.Bind<IFoo>().To<Foo2>().AsSingle().WhenInjectedInto<Bar2>();
```

Note that `WhenInjectedInto` is simple shorthand for the following, which uses the more general `When()` method:

```csharp
Container.Bind<IFoo>().To<Foo>().AsSingle().When(context => context.ObjectType == typeof(Bar));
```

The InjectContext class (which is passed as the `context` parameter above) contains the following information that you can use in your conditional:

* `Type ObjectType` - The type of the newly instantiated object, which we are injecting dependencies into.  Note that this is null for root calls to `Resolve<>` or `Instantiate<>`
* `object ObjectInstance` - The newly instantiated instance that is having its dependencies filled.  Note that this is only available when injecting fields or into `[Inject]` methods and null for constructor parameters
* `string Identifier` - This will be null in most cases and set to whatever is given as a parameter to the `[Inject]` attribute.  For example, `[Inject(Id = "foo")] _foo` will result in `Identifier` being equal to the string "foo".
* `object ConcreteIdentifier` - This will be null in most cases and set to whatever is given as the identifier in the `WithConcreteIdentifier` bind method
* `string MemberName` - The name of the field or parameter that we are injecting into.  This can be used, for example, in the case where you have multiple constructor parameters that are strings.  However, using the parameter or field name can be error prone since other programmers may refactor it to use a different name.  In many cases it's better to use an explicit identifier
* `Type MemberType` - The type of the field or parameter that we are injecting into.
* `InjectContext ParentContext` - This contains information on the entire object graph that precedes the current class being created.  For example, dependency A might be created, which requires an instance of B, which requires an instance of C.  You could use this field to inject different values into C, based on some condition about A.  This can be used to create very complex conditions using any combination of parent context information.  Note also that `ParentContext.MemberType` is not necessarily the same as ObjectType, since the ObjectType could be a derived type from `ParentContext.MemberType`
* `bool Optional` - True if the `[InjectOptional]` parameter is declared on the field being injected

### List Bindings

---

When Zenject finds multiple bindings for the same type, it interprets that to be a list.  So, in the example code below, `Bar` would get a list containing a new instance of `Foo1,` `Foo2`, and `Foo3`:

```csharp
// In an installer somewhere
Container.Bind<IFoo>().To<Foo1>().AsSingle();
Container.Bind<IFoo>().To<Foo2>().AsSingle();
Container.Bind<IFoo>().To<Foo3>().AsSingle();

...

public class Bar
{
    public Bar(List<IFoo> foos)
    {
    }
}
```

The order of the list will be the same as the order in which they were added with a `Bind` method.  The only exception is when you use subcontainers, since in that case the list will be ordered first by the associated subcontainer, with the first set of instances taken from the bottom most subcontainer, and then the parent, then the grandparent, etc.

### Global Bindings Using Project Context

---

This all works great for each individual scene, but what if you have dependencies that you wish to persist permanently across all scenes?  In Zenject you can do this by adding installers to a `ProjectContext` object.

To do this, first you need to create a prefab for the `ProjectContext,` and then you can add installers to it.  You can do this most easily by selecting the menu item `Edit -> Zenject -> Create Project Context`. You should then see a new asset in the folder `Assets/Resources` called 'ProjectContext'.  Alternatively, you can right click somewhere in Projects tab and select `Create -> Zenject -> ProjectContext`.

If you click on this it will appear nearly identically to the inspector for `SceneContext`.  The easiest way to configure this prefab is to temporarily add it to your scene, add Installers to it, then click "Apply" to save it back to the prefab before deleting it from your scene.  In addition to installers, you can also add your own custom MonoBehaviour classes to the ProjectContext object directly.

Then, when you start any scene that contains a `SceneContext`, your `ProjectContext` object will always be initialized first.  All the installers you add here will be executed and the bindings that you add within them will be available for use in all scenes within your project.  The `ProjectContext` game object is set as [DontDestroyOnLoad](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html) so it will not be destroyed when changing scenes.

Note also that this only occurs once.  If you load another scene from the first scene, your `ProjectContext` will not be called again and the bindings that it added previously will persist into the new scene.  You can declare `ITickable` / `IInitializable` / `IDisposable` objects in your project context installers in the same way you do for your scene installers with the result being that `IInitializable.Initialize` is called only once across each play session and `IDisposable.Dispose` is only called once the application is fully stopped.

The reason that all the bindings you add to a global installer are available for any classes within each individual scene, is because the Container in each scene uses the `ProjectContext` Container as it's "parent".  For more information on nested containers see [here](#sub-containers-and-facades).

`ProjectContext` is a very convenient place to put objects that you want to persist across scenes.  However, the fact that it's completely global to every scene can lead to some unintended behaviour.  For example, this means that even if you write a simple test scene that uses Zenject, it will load the `ProjectContext,` which may not be what you want.  To address these problems it is often better to use Scene Parenting instead, since that approach allows you to be selective in terms of which scenes inherit the same common bindings.  See [here](#scene-parenting) for more details on that approach.

Note also that by default, any game objects that are instantiated inside ProjectContext will be parented underneath it by default.  If you'd prefer that each newly instantiated object is instead placed at the root of the scene hierarchy (but still marked DontDestroyOnLoad) then you can change this by unchecking the flag 'Parent New Objects Under Context' in the inspector of ProjectContext.

### Identifiers

---

You can also give an ID to your binding if you need to have distinct bindings for the same type, and you don't want it to just result in a `List<>`.  For example:

```csharp
Container.Bind<IFoo>().WithId("foo").To<Foo1>().AsSingle();
Container.Bind<IFoo>().To<Foo2>().AsSingle();

...

public class Bar1
{
    [Inject(Id = "foo")]
    IFoo _foo;
}

public class Bar2
{
    [Inject]
    IFoo _foo;
}
```

In this example, the `Bar1` class will be given an instance of `Foo1`, and the `Bar2` class will use the default version of `IFoo` which is bound to `Foo2`.

Note also that you can do the same thing for constructor/inject-method arguments as well:

```csharp
public class Bar
{
    Foo _foo;

    public Bar(
        [Inject(Id = "foo")]
        Foo foo)
    {
    }
}
```

In many cases, the ID is created as a string, however you can actually use any type you like for this.  For example, it's sometimes useful to use an enum instead:

```csharp
enum Cameras
{
    Main,
    Player,
}

Container.Bind<Camera>().WithId(Cameras.Main).FromInstance(MyMainCamera);
Container.Bind<Camera>().WithId(Cameras.Player).FromInstance(MyPlayerCamera);
```

You can also use custom types, as long as they implement the `Equals` operator.

### Non Generic Bindings

---

In some cases you may not know the exact type you want to bind at compile time.  In these cases you can use the overload of the `Bind` method which takes a `System.Type` value instead of a generic parameter.

```csharp
// These two lines will result in the same behaviour
Container.Bind(typeof(Foo));
Container.Bind<Foo>();
```

Note also that when using non generic bindings, you can pass multiple arguments:

```csharp
Container.Bind(typeof(Foo), typeof(Bar), typeof(Qux)).AsSingle();

// The above line is equivalent to these three:
Container.Bind<Foo>().AsSingle();
Container.Bind<Bar>().AsSingle();
Container.Bind<Qux>().AsSingle();
```

The same goes for the To method:

```csharp
Container.Bind<IFoo>().To(typeof(Foo), typeof(Bar)).AsSingle();

// The above line is equivalent to these two:
Container.Bind<IFoo>().To<Foo>().AsSingle();
Container.Bind<IFoo>().To<Bar>().AsSingle();
```

You can also do both:

```csharp
Container.Bind(typeof(IFoo), typeof(IBar)).To(typeof(Foo1), typeof(Foo2)).AsSingle();

// The above line is equivalent to these:
Container.Bind<IFoo>().To<Foo>().AsSingle();
Container.Bind<IFoo>().To<Bar>().AsSingle();
Container.Bind<IBar>().To<Foo>().AsSingle();
Container.Bind<IBar>().To<Bar>().AsSingle();
```

This can be especially useful when you have a class that implements multiple interfaces:

```csharp
Container.Bind(typeof(ITickable), typeof(IInitializable), typeof(IDisposable)).To<Foo>().AsSingle();
```

Though in this particular example there is already a built-in shortcut method for this:

```csharp
Container.BindInterfacesTo<Foo>().AsSingle();
```

### Convention Based Binding

---

Convention based binding can come in handy in any of the following scenarios:

- You want to define a naming convention that determines how classes are bound to the container (eg. using a prefix, suffix, or regex)
- You want to use custom attributes to determine how classes are bound to the container
- You want to automatically bind all classes that implement a given interface within a given namespace or assembly

Using "convention over configuration" can allow you to define a framework that other programmers can use to quickly and easily get things done, instead of having to explicitly add every binding within installers.  This is the philosophy that is followed by frameworks like Ruby on Rails, ASP.NET MVC, etc.  Of course, there are both advantages and disadvantages to this approach.

They are specified in a similar way to [Non Generic bindings](#non-generic-bindings), except instead of giving a list of types to the `Bind()` and `To()` methods, you describe the convention using a Fluent API.  For example, to bind `IFoo` to every class that implements it in the entire codebase:

```csharp
Container.Bind<IFoo>().To(x => x.AllTypes().DerivingFrom<IFoo>());
```

Note that you can use the same Fluent API in the `Bind()` method as well, and you can also use it in both `Bind()` and `To()` at the same time.

For more examples see the [examples](#convention-binding-examples) section below.  The full format is as follows:

<pre>
x.<b>InitialList</b>().<b>Conditional</b>().<b>AssemblySources</b>()
</pre>

#### Where:

* **InitialList** = The initial list of types to use for our binding.  This list will be filtered by the given **Conditional**s.  It can be one of the following (fairly self explanatory) methods:

    1. **AllTypes**
    1. **AllNonAbstractClasses**
    1. **AllAbstractClasses**
    1. **AllInterfaces**
    1. **AllClasses**

* **Conditional** = The filter to apply to the list of types given by **InitialList**.  Note that you can chain as many of these together as you want, and they will all be applied to the initial list in sequence.  It can be one of the following:

    1. **DerivingFrom**<T> - Only match types deriving from `T`
    1. **DerivingFromOrEqual**<T> - Only match types deriving from or equal to `T`
    1. **WithPrefix**(value) - Only match types with names that start with `value`
    1. **WithSuffix**(value) - Only match types with names that end with `value`
    1. **WithAttribute**<T> - Only match types that have the attribute `[T]` above their class declaration
    1. **WithoutAttribute**<T> - Only match types that do not have the attribute `[T]` above their class declaration
    1. **WithAttributeWhere**<T>(predicate) - Only match types that have the attribute `[T]` above their class declaration AND in which the given predicate returns true when passed the attribute.  This is useful so you can use data given to the attribute to create bindings
    1. **InNamespace**(value) - Only match types that are in the given namespace
    1. **InNamespaces**(value1, value2, etc.) - Only match types that are in any of the given namespaces
    1. **MatchingRegex**(pattern) - Only match types that match the given regular expression
    1. **Where**(predicate) - Finally, you can also add any kind of conditional logic you want by passing in a predicate that takes a `Type` parameter

* **AssemblySources** = The list of assemblies to search for types when populating **InitialList**.  It can be one of the following:

    1. **FromAllAssemblies** - Look up types in all loaded assemblies.  This is the default when unspecified.
    1. **FromAssemblyContaining**<T> - Look up types in whatever assembly the type `T` is in
    1. **FromAssembliesContaining**(type1, type2, ..) - Look up types in all assemblies that contains any of the given types
    1. **FromThisAssembly** - Look up types only in the assembly in which you are calling this method
    1. **FromAssembly**(assembly) - Look up types only in the given assembly
    1. **FromAssemblies**(assembly1, assembly2, ...) - Look up types only in the given assemblies
    1. **FromAssembliesWhere**(predicate) - Look up types in all assemblies that match the given predicate

#### Examples:

Note that you can chain together any combination of the below conditionals in the same binding.  Also note that since we aren't specifying an assembly here, Zenject will search within all loaded assemblies.

1. Bind `IFoo` to every class that implements it in the entire codebase:

    ```csharp
    Container.Bind<IFoo>().To(x => x.AllTypes().DerivingFrom<IFoo>());
    ```

    Note that this will also have the same result:

    ```csharp
    Container.Bind<IFoo>().To(x => x.AllNonAbstractTypes());
    ```

    This is because Zenject will skip any bindings in which the concrete type does not actually derive from the base type.  Also note that in this case we have to make sure we use `AllNonAbstractTypes` instead of just `AllTypes`, to ensure that we don't bind `IFoo` to itself

1. Bind an interface to all classes implementing it within a given namespace

    ```csharp
    Container.Bind<IFoo>().To(x => x.AllTypes().DerivingFrom<IFoo>().InNamespace("MyGame.Foos"));
    ```

1. Auto-bind `IController` every class that has the suffix "Controller" (as is done in ASP.NET MVC):

    ```csharp
    Container.Bind<IController>().To(x => x.AllNonAbstractTypes().WithSuffix("Controller"));
    ```

    You could also do this using `MatchingRegex`:

    ```csharp
    Container.Bind<IController>().To(x => x.AllNonAbstractTypes().MatchingRegex("Controller$"));
    ```

1. Bind all types with the prefix "Widget" and inject into Foo

    ```csharp
    Container.Bind<object>().To(x => x.AllNonAbstractTypes().WithPrefix("Widget")).WhenInjectedInto<Foo>();
    ```

1. Auto-bind the interfaces that are used by every type in a given namespace

    ```csharp
    Container.Bind(x => x.AllInterfaces())
        .To(x => x.AllNonAbstractClasses().InNamespace("MyGame.Things"));
    ```

    This is equivalent to calling `Container.BindInterfacesTo<T>()` for every type in the namespace "MyGame.Things".  This works because, as touched on above, Zenject will skip any bindings in which the concrete type does not actually derive from the base type.  So even though we are using `AllInterfaces` which matches every single interface in every single loaded assembly, this is ok because it will not try and bind an interface to a type that doesn't implement this interface.

### Decorator Bindings

Decorator Bindings has it's own documentation [here](Documentation/DecoratorBindings.md).

## Scriptable Object Installer

Another alternative to [deriving from MonoInstaller or Installer](#installers) when implementing your own installers, is to derive from the `ScriptableObjectInstaller` class.  This is most commonly used to store game settings.  This approach has the following advantages:

* Any changes you make to the properties of the installer will persist after you stop play mode.  This can be very useful when tweaking runtime parameters.  For other installer types as well as any MonoBehaviour's in your scene, any changes to the inspector properties at runtime will be undone when play mode is stopped.  However, there is a 'gotcha' to be aware of:  Any changes to these settings in code will also be saved persistently (unlike with settings on MonoInstaller's).  So if you go this route you should treat all settings objects as read-only to avoid this from happening.
* You can very easily swap out multiple instances of the same installer.  For example, using the below example, you might have an instance of `GameSettingsInstaller` called `GameSettingsEasy`, and another one called `GameSettingsHard`, etc.

Example:

* Open Unity
* Right click somewhere in the Project tab and select `Create -> Zenject -> ScriptableObjectInstaller`
* Name it GameSettingsInstaller
* Right click again in the same location
* Select the newly added menu item `Create -> Installers -> GameSettingsInstaller`
* Following the approach to settings outlined [here](#using-the-unity-inspector-to-configure-settings), you might then replace it with the following:

```csharp
public class GameSettings : ScriptableObjectInstaller
{
    public Player.Settings Player;
    public SomethingElse.Settings SomethingElse;
    // ... etc.

    public override void InstallBindings()
    {
        Container.BindInstances(Player, SomethingElse, etc.);
    }
}

public class Player : ITickable
{
    readonly Settings _settings;
    Vector3 _position;

    public Player(Settings settings)
    {
        _settings = settings;
    }

    public void Tick()
    {
        _position += Vector3.forward * _settings.Speed;
    }

    [Serializable]
    public class Settings
    {
        public float Speed;
    }
}
```

* Now, you should be able to run your game and adjust the Speed value that is on the `GameSettingsInstaller` asset at runtime, and have that change saved permanently

## Runtime Parameters For Installers

Often when calling installers from other installers it is desirable to be able to pass parameters.  You can do this by adding generic arguments to whichever installer base class you are using with the types for the runtime parameters. For example, when using a non-MonoBehaviour Installer:

```csharp
public class FooInstaller : Installer<string, FooInstaller>
{
    string _value;

    public FooInstaller(string value)
    {
        _value = value;
    }

    public override void InstallBindings()
    {
        ...

        Container.BindInstance(_value).WhenInjectedInto<Foo>();
    }
}

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        FooInstaller.Install(Container, "asdf");
    }
}

```

Or when using a MonoInstaller prefab:

```csharp
public class FooInstaller : MonoInstaller<string, FooInstaller>
{
    string _value;

    // Note that in this case we can't use a constructor
    [Inject]
    public void Construct(string value)
    {
        _value = value;
    }

    public override void InstallBindings()
    {
        ...

        Container.BindInstance(_value).WhenInjectedInto<Foo>();
    }
}

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // For this to work, there must be a prefab with FooInstaller attached to it at
        // Resources/My/Custom/ResourcePath.prefab
        FooInstaller.InstallFromResource("My/Custom/ResourcePath", Container, "asdf")

        // If a resource path is not provided then it is assumed to exist at resource path
        // 'Resources/Installers/FooInstaller'
        // For example:
        // FooInstaller.InstallFromResource(Container, "asdf");
    }
}
```

`ScriptableObjectInstaller` works the same as `MonoInstaller` in this regard.

## Using Zenject Outside Unity Or For DLLs

If you are building some code as DLLs and then including them in Unity, you can still add bindings for those classes inside your installers, with the only limitation being that you have to use constructor injection.  If you want to use the other inject approaches such as member injection or method injection, then you can do that too, however in that case you will need to add a reference for your project to `Zenject-Usage.dll` which can be found in the `Zenject\Source\Usage` directory.  This DLL also includes the standard interfaces such as `ITickable,` `IInitializable,` etc. so you can use those as well.

You can also use Zenject for non-unity projects by downloading `Zenject-NonUnity.zip` from the [releases section](https://github.com/svermeulen/Extenject/releases)

Finally, if you are attempting to run unit tests outside of Unity using the Unity generated solution, you might encounter run time errors in the Zenject code when it attempts to access the Unity API.  You can disable this behaviour by adding a define for `ZEN_TESTS_OUTSIDE_UNITY` in the generated solution.

## Zenject Settings

A lot of the default behaviour in Zenject can be customized via a settings property on the ProjectContext.  This includes the following:

- **Validation Error Response** - This value controls the behaviour that is triggered when zenject encounters a validation error.  It can be set to either 'Log' or 'Throw'.  The difference here is that when set to 'Log' there will be multiple validation errors printed every time validation is run, whereas if set to 'Throw' only the first validation error will be output to the console.  When unset the default value is 'Log'.  'Throw' is also sometimes useful if running validation inside unit tests.

- **Validation Root Resolve Method** - When validation is triggered for a given scene, the DiContainer will do a 'dry run' and pretend to instantiate the entire object graph as defined by the installers in the scene.   However, by default it will only validate the 'roots' of the object graph - that is, the 'NonLazy' bindings or the bindings which are injected into the 'NonLazy' bindings.  As an option, you can change this behaviour to 'All' which will validate all bindings, even those that are not currently used.

- **Display Warning When Resolving During Install** - This value will control whether a warning is issued to the console when either a Resolve or an Instantiate is triggered during the install phase which looks like this:

```
Zenject Warning: It is bad practice to call Inject/Resolve/Instantiate before all the Installers have completed!  This is important to ensure that all bindings have properly been installed in case they are needed when injecting/instantiating/resolving.  Detected when operating on type 'Foo'.
```

So if you often encounter this warning and are aware of the implications of what you're doing then you might set this value to false to suppress it.

- **Ensure Deterministic Destruction Order On Application Quit** - When set to true, this will ensure that all GameObject's and IDisposables are destroyed in a predictable order when the application is closed.  By default it is set to false, because there are some undesirable implications to enabling this feature as described in [this section](#destruction-order).

These settings can also be configured on a per DiContainer basis by changing the DiContainer.Settings property.  Changing this property will affect the given container as well as any subcontainers.

There are also settings for the signals system which are documented [here](#settings).

## Signals

See [here](Documentation/Signals.md).

## Factories: Creating Objects Dynamically

See [here](Documentation/Factories.md).

## Memory Pools

See [here](Documentation/MemoryPools.md).

## Update / Initialization Order

In many cases, especially for small projects, the order that classes update or initialize in does not matter.  However, in larger projects update or initialization order can become an issue.  This can especially be an issue in Unity, since it is often difficult to predict in what order the `Start()`, `Awake()`, or `Update()` methods will be called in.  Unfortunately, Unity does not have an easy way to control this (besides in `Edit -> Project Settings -> Script Execution Order`, though that can be awkward to use)

In Zenject, by default, ITickables and IInitializables are called in the order that they are added, however for cases where the update or initialization order does matter, there is another way that is sometimes better:  By specifying their priorities explicitly in the installer.  For example, in the sample project you can find this code in the scene installer:

```csharp
public class AsteroidsInstaller : MonoInstaller
{
    ...

    void InitExecutionOrder()
    {
        // In many cases you don't need to worry about execution order,
        // however sometimes it can be important
        // If for example we wanted to ensure that AsteroidManager.Initialize
        // always gets called before GameController.Initialize (and similarly for Tick)
        // Then we could do the following:
        Container.BindExecutionOrder<AsteroidManager>(-10);
        Container.BindExecutionOrder<GameController>(-20);

        // Note that they will be disposed of in the reverse order given here
    }

    ...

    public override void InstallBindings()
    {
        ...
        InitExecutionOrder();
        ...
    }

}
```

This way, you won't hit a wall at the end of the project due to some unforeseen order-dependency.

Note here that the value given to `BindExecutionOrder` will apply to `ITickable` / `IInitializable` and `IDisposable` (with the order reversed for `IDisposable`'s).

You can also assign priorities for each specific interface separately like this:

```csharp
Container.BindInitializableExecutionOrder<Foo>(-10);
Container.BindInitializableExecutionOrder<Bar>(-20);

Container.BindTickableExecutionOrder<Foo>(10);
Container.BindTickableExecutionOrder<Bar>(-80);
```

Any ITickables, IInitializables, or IDisposables that are not assigned a priority are automatically given the priority of zero.  This allows you to have classes with explicit priorities executed either before or after the unspecified classes.  For example, the above code would result in `Foo.Initialize` being called before `Bar.Initialize`.

## Zenject Order Of Operations

What follows below is a more detailed view of what happens when running a scene that uses Zenject.  This can be useful to know to fully understand exactly how Zenject works.

* Unity Awake() phase begins
    * SceneContext.Awake() method is called.  This should always be the first thing executed in your scene.  It should work this way by default (see [here](#bad-execution-order) if you are noticing otherwise).
    * Project Context is initialized. Note that this only happens once per play session.  If a previous scene already initialized the ProjectContext, then this step is skipped
        * All injectable MonoBehaviour's on the ProjectContext prefab are passed to the container via [DiContainer.QueueForInject](#dicontainer-methods-queueforinject)
        * ProjectContext iterates through all the Installers that have been added to its prefab via the Unity Inspector, runs injects on them, then calls InstallBindings() on each.  Each Installer calls some number of Bind methods on the DiContainer.
        * ProjectContext then constructs all the non-lazy root objects, which includes any classes that derive from ITickable / IInitializable or IDisposable, as well as those classes that are added with a `NonLazy()` binding.
        * All instances that were added via [DiContainer.QueueForInject](#dicontainer-methods-queueforinject) are injected
    * Scene Context is initialized.
        * All injectable MonoBehaviour's in the entire scene are passed to the SceneContext container via [DiContainer.QueueForInject](#dicontainer-methods-queueforinject)
        * SceneContext iterates through all the Installers that have been added to it via the Unity Inspector, runs injects on them, then calls InstallBindings() on each.  Each Installer calls some number of Bind<> methods on the DiContainer.
        * SceneContext then constructs all the non-lazy root objects, which includes any classes that derive from ITickable / IInitializable or IDisposable, as well as those classes that are added with a `NonLazy()` binding.
        * All instances that were added via [DiContainer.QueueForInject](#dicontainer-methods-queueforinject) are injected
    * If any required dependencies cannot be resolved, a ZenjectResolveException is thrown
    * All other MonoBehaviour's in the scene have their Awake() method called
* Unity Start() phase begins
    * ProjectKernel.Start() method is called.  This will trigger the Initialize() method on all `IInitializable` objects in the order specified in the ProjectContext installers.
    * SceneKernel.Start() method is called.  This will trigger the Initialize() method on all `IInitializable` objects in the order specified in the SceneContext installers.
    * All other MonoBehaviour's in your scene have their Start() method called
* Unity Update() phase begins
    * ProjectKernel.Update() is called, which results in Tick() being called for all `ITickable` objects (in the order specified in the ProjectContext installers)
    * SceneKernel.Update() is called, which results in Tick() being called for all `ITickable` objects (in the order specified in the SceneContext installers)
    * All other MonoBehaviour's in your scene have their Update() method called
* These same steps repeated for LateUpdate and ILateTickable
* At the same time, These same steps are repeated for FixedUpdate according to the physics timestep
* Unity scene is unloaded
    * Dispose() is called on all objects mapped to `IDisposable` within all the GameObjectContext's (see [here](#implementing-idisposable) for details)
    * Dispose() is called on all objects mapped to `IDisposable` within the SceneContext installers (see [here](#implementing-idisposable) for details)
* App is exitted
    * Dispose() is called on all objects mapped to `IDisposable` within the ProjectContext installers (see [here](#implementing-idisposable) for details)

## Injecting data across scenes

In some cases it's useful to pass arguments from one scene to another.  The way Unity allows us to do this by default is fairly awkward.  Your options are to create a persistent GameObject and call DontDestroyOnLoad() to keep it alive when changing scenes, or use global static classes to temporarily store the data.

Let's pretend you want to specify a 'level' string to the next scene.  You have the following class that requires the input:

```csharp
public class LevelHandler : IInitializable
{
    readonly string _startLevel;

    public LevelHandler(
        [InjectOptional]
        string startLevel)
    {
        if (startLevel == null)
        {
            _startLevel = "default_level";
        }
        else
        {
            _startLevel = startLevel;
        }
    }

    public void Initialize()
    {
        ...
        [Load level]
        ...
    }
}
```

You can load the scene containing `LevelHandler` and specify a particular level by using the following syntax:

```csharp

public class Foo
{
    readonly ZenjectSceneLoader _sceneLoader;

    public Foo(ZenjectSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void AdvanceScene()
    {
        _sceneLoader.LoadScene("NameOfSceneToLoad", LoadSceneMode.Single, (container) =>
            {
                container.BindInstance("custom_level").WhenInjectedInto<LevelHandler>();
            });
    }
}
```

The bindings that we add here inside the lambda will be added to the container as if they were inside an installer in the new scene.

Note that you can still run the scene directly, in which case it will default to using "default_level".  This is possible because we are using the `InjectOptional` flag.

An alternative and arguably cleaner way to do this would be to customize the installer itself rather than the `LevelHandler` class.  In this case we can write our `LevelHandler` class like this (without the `[InjectOptional]` flag).

```csharp
public class LevelHandler : IInitializable
{
    readonly string _startLevel;

    public LevelHandler(string startLevel)
    {
        _startLevel = startLevel;
    }

    public void Initialize()
    {
        ...
        [Load level]
        ...
    }
}
```

Then, in the installer for our scene we can include the following:

```csharp
public class GameInstaller : Installer
{
    [InjectOptional]
    public string LevelName = "default_level";

    ...

    public override void InstallBindings()
    {
        ...
        Container.BindInstance(LevelName).WhenInjectedInto<LevelHandler>();
        ...
    }
}
```

Then, instead of injecting directly into the `LevelHandler` we can inject into the installer instead.

```csharp

public class Foo
{
    readonly ZenjectSceneLoader _sceneLoader;

    public Foo(ZenjectSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void AdvanceScene()
    {
        _sceneLoader.LoadScene("NameOfSceneToLoad", (container) =>
            {
                container.BindInstance("custom_level").WhenInjectedInto<GameInstaller>();
            });
    }
}
```

The `ZenjectSceneLoader` class also allows for more complex scenarios, such as loading a scene as a "child" of the current scene, which would cause the new scene to inherit all the dependencies in the current scene.  However, it is often better to use [Scene Contract Names](#scene-parenting) for this instead.

## Scene Parenting Using Contract Names

Putting bindings inside ProjectContext is a fast and easy way to add common long-lasting dependencies that are shared across scenes.  However, in many cases you have bindings that you only want to be shared between specific scenes, so using ProjectContext doesn't work since in that case, the bindings we add there are global to every single scene in our entire project.

As an example, let's pretend that we are working on a spaceship game, and we want to create one scene to serve as the environment (involving planets, asteroids, stars, etc.) and we want to create another scene to represent the ship that the player is in.  We also want all the classes in the ship scene to be able to reference bindings declared in the environment scene.  Also, we want to be able to define multiple different versions of both the ship scene and the environment scene.  To achieve all this, we will use a Zenject feature called 'Scene Contract Names'.

We will start by using Unity's support for [multi-scene editting](https://docs.unity3d.com/Manual/MultiSceneEditing.html), and dragging both our environment scene and our ship scene into the Scene Hierarchy tab.  Then we will select the SceneContext in the environment scene and add a 'Contract Name'.  Let's call it 'Environment'.  Then all we have to do now is select the SceneContext inside the ship scene and set its 'Parent Contract Name' to the same value ('Environment').  Now if we press play, all the classes in the ship scene can access the declared bindings in the environment scene.

The reason we use a name field here instead of explicitly using the scene name is to support swapping out the various environment scenes for different implementations.  In this example, we might define several different environments, all using the same Contract Name 'Environment', so that we can easily mix and match them with different ship scenes just by dragging the scenes we want into the scene hierarchy then hitting play.

It is called 'Contract Name' because all the environment scenes will be expected to follow a certain 'contract' by the ship scenes.  For example, the ship scenes might require that regardless of which environment scene was loaded, there is a binding for 'AsteroidManager' containing the list of asteroids that the ship must avoid.

Note that you do not need to load the environment scene and the ship scene at the same time for this to work.  For example, you might want to have a menu embedded inside the environment to allow the user to choose their ship before starting.  So you could create a menu scene and load that after the environment scene.   Then once the user chooses their ship, you could load the associated ship scene by calling the unity method `SceneManager.LoadScene` (making sure to use `LoadSceneMode.Additive`).

Also note that the Validate command can be used to quickly verify the different multi-scene setups.  If you find that scenes are unloaded when you do this see [here](https://github.com/modesttree/Zenject/issues/168).

Also, I should mention that Unity currently doesn't have a built-in way to save and restore multi-scene setups.  We use a simple editor script for this that you can find [here](https://gist.github.com/svermeulen/8927b29b2bfab4e84c950b6788b0c677) if interested.

## Default Scene Parents

One drawback to using scene parent contract names instead of ProjectContext to add bindings shared across scenes, is that you always have to remember to configure the scene hierarchy inside Unity to contain the correct scenes before running it.  You can't simply open different scenes and hit Play like you can with ProjectContext.  So to address this, zenject allows specifying a default value for the different contract names in cases where an existing value is not already loaded.

To take our example from [above](#scene-parenting), we'd like to be able to open Ship scene and immediately hit play without needing to place an environment scene above that first.  To do this, right click in your Projects tab and select `Create -> Zenject -> Default Scene Contract Config`.  Note that you will have to do this by right clicking on a folder named Resources for this to work.

After adding this you can click on the `ZenjectDefaultSceneContractConfig` object and add any number of defaults by typing in contract names and then dragging in scene files from the Project tab into the Scene property.  After doing this, you should be able to directly run the Ship scene and the default environment scene will automatically be loaded above that.

Note that the default scene for a given contract will also be used when using [scene decorators](#scenes-decorator)

Note that this is an editor only feature.  The default contract names will not be used in builds.  In those cases you will have to explicitly load the correct parent scenes yourself in code.

## ZenAutoInjecter

As explained in the [section on factories](#creating-objects-dynamically-using-factories), any object that you create dynamically needs to be created through zenject in order for it to be injected.  You cannot simply execute `GameObject.Instantiate(prefab)`, or call `new Foo()`.

However, this is sometimes problematic especially when using other third party libraries.  For example, some networking libraries work by automatically instantiating prefabs to sync state across multiple clients.  And it is still desirable in these cases to execute zenject injection.

So to solve these cases, we've added a helper MonoBehaviour that you can add to these kinds of objects called `ZenAutoInjecter`.  If you add this MonoBehaviour to your prefab, then you should be able to call `GameObject.Instantiate` and injection should happen automatically.

After adding this component you might notice that there is a field on it called 'Container Source'.  This value will determine how it calculates which container to use for the injection and has the following options:

1. SearchInHiearchy - This will find the container to use by searching the scene hierarchy where the prefab is instantiated.  So if the prefab is instantiated underneath a GameObjectContext, it will use the container associated with the GameObjectContext.  If it is instantiated underneath a DontDestroyOnLoad object, then it will use the ProjectContext container.  And otherwise will use the SceneContext container for whatever scene it is instantiated in.

1. SceneContext - Don't bother searching the hierarchy and instead just always use the current SceneContext container.

1. ProjectContext - Don't bother searching the hierarchy and instead just always use the ProjectContext container.

## Scene Decorators

Scene Decorators offer another approach to using multiple scenes together with zenject in addition to [scene parenting](#scene-parenting) described above.  The difference is that with scene decorators, the multiple scenes in question will all share the same Container and therefore all scenes can access bindings in all other scenes (unlike with scene parenting where only the child can access the parent bindings and not vice versa).

Another way to think about scene decorators is that it is a more advanced way doing the process described [for injecting data across scenes](#injecting-data-across-scenes).  That is, they can be used to add behaviour to another scene without actually changing the installers in that scene.

Usually, when you want to customize different behaviour for a given scene depending on some conditions, you would use boolean or enum properties on MonoInstallers, which would then be used to add different bindings depending on the values set.  However, the scene decorator approach can be cleaner sometimes because it doesn't involve changing the main scene.

For example, let's say we want to add some special keyboard shortcuts to your main production scene for testing purposes.  In order to do this using decorators, you would do the following:

* Open the main production scene
* Right click on the far right menu beside the scene name within the scene hierarchy and select Add New Scene
* Drag the scene so it's above the main scene
* Right Click inside the new scene and select `Zenject -> Decorator Context`
* Select the Decorator Context and set the 'Decorated Contract Name' field to 'Main'
* Select the SceneContext in the main scene and add a contract name with the same value ('Main')
* Create a new C# script with the following contents, then add this MonoBehaviour to your decorator scene as a gameObject, then drag it to the `Installers` property of `SceneDecoratorContext`

```csharp
public class ExampleDecoratorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ITickable>().To<TestHotKeysAdder>().AsSingle();
    }
}

public class TestHotKeysAdder : ITickable
{
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hotkey triggered!");
        }
    }
}
```

Note the following:

- If you run your scene it should now behave exactly like the main scene except with the added functionality in your decorator installer.  Also note that while not shown here, both scenes can access each other's bindings as if everything was in the same scene.

- The Validate command (`CTRL+ALT+V`) can be used to quickly verify the different multi-scene setups.  If you find that scenes are unloaded when you do this see [here](https://github.com/modesttree/Zenject/issues/168).

- Decorator scenes must be loaded before the scenes that they are decorating.

- Unity currently doesn't have a built-in way to save and restore multi-scene setups.  We use a simple editor script for this that you can find [here](https://gist.github.com/svermeulen/8927b29b2bfab4e84c950b6788b0c677) if interested.

- Finally, if you want to save yourself some time you could add a [default scene](#default-scene-parents) for the contract name that you are using above

## Sub-Containers And Facades

See [here](Documentation/SubContainers.md).

## Writing Automated Unit Tests / Integration Tests

See [here](Documentation/WritingAutomatedTests.md).

## Philosophy Of Zenject

One thing that is helpful to be aware of in terms of understanding the design of Zenject is that unlike many other frameworks it is **not opinionated**.  Many frameworks, such as ROR, ECS, ASP.NET MVC, etc. make rigid design choices for you that you have to follow.  The only assumption that Zenject makes is that you are writing object oriented code and otherwise, how you design your code is entirely up to you.

In my view, dependency injection is pretty fundamental to object oriented programming.  And without a dependency injection framework, the composition root quickly becomes a headache to maintain.  Therefore dependency injection frameworks are fairly fundamental as well.

And that's all Zenject strives to be - a dependency injection framework that targets Unity.  There are certainly quite a few features in Zenject, but they are all optional.  If you want, you can follow traditional unity development and use MonoBehaviours for every class, with the one exception that you use `[Inject]` instead of `[SerializeField]`.  Or you can abandon MonoBehaviours's completely and use the included interfaces such as ITickable and IInitializable.  It is up to you which design you want to use.

Of course, using a DI framework has some disadvantages compared to more rigid frameworks.  The main drawback is that it can be more challenging for new developers to get up and running quickly in a code base, because they need to understand the specific architecture chosen by the previous developers.  Whereas with rigid frameworks developers are given a very clear pathway to follow and so can be productive more quickly.  It is more difficult to make huge design mistakes when using a rigid framework.  However, with this rigidity also comes limitations, because whatever design decisions that are enforced by the framework might not necessarily be ideal for every single problem.

## Just-In-Time Resolving Using LazyInject&lt;&gt;

In some cases it can be useful to delay the creation of certain dependencies until after startup.  You can use the `LazyInject<>` construct for this.

For example, let's imagine a scenario like this:

```csharp
public class Foo
{
    public void Run()
    {
        ...
    }
}

public class Bar
{
    Foo _foo;

    public Bar(Foo foo)
    {
        _foo = foo;
    }

    public void Run()
    {
        _foo.Run();
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Foo>().AsSingle();
        Container.Bind<Bar>().AsSingle();
    }
}
```

Let's also imagine that we would only like to create an instance of Foo if it's actually used (that is, when the `Bar.Run` method is called).  As it stands above, `Foo` would always be created every time that `Bar` is created, even if `Bar.Run` is never called.  We can fix this by changing `Bar` to the following:

```csharp
public class Bar
{
    LazyInject<Foo> _foo;

    public Bar(LazyInject<Foo> foo)
    {
        _foo = foo;
    }

    public void Run()
    {
        _foo.Value.Run();
    }
}
```

Now, by using `LazyInject<>` instead, the Foo class will not be created until Bar.Run is first called.  After that, it will use the same instance of Foo.

Note that the installers remain the same in both cases.  Any injected dependency can be made lazy by simply wrapping it in `LazyInject<>`.

## Open Generic Types

Zenject also has a feature that allow you to automatically fill in open generic arguments during injection.  For example:

```csharp
public class Bar<T>
{
    public int Value
    {
        get; set;
    }
}

public class Foo
{
    public Foo(Bar<int> bar)
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(Bar<>)).AsSingle();
        Container.Bind<Foo>().AsSingle().NonLazy();
    }
}
```

Note that when binding a type with open generic arguments, you must use the non generic version of the Bind() method.  As you can see in the example, when binding an open generic type, it will match whatever the injected parameter/field/property requires.  You can also bind one open generic type to another open generic type like this

```csharp
public interface IBar<T>
{
    int Value
    {
        get; set;
    }
}

public class Bar<T> : IBar<T>
{
    public int Value
    {
        get; set;
    }
}

public class Foo
{
    public Foo(IBar<int> bar)
    {
    }
}

public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IBar<>)).To(typeof(Bar<>)).AsSingle();
        Container.Bind<Foo>().AsSingle().NonLazy();
    }
}
```

This can sometimes open up some interesting design possibilities so good to be aware of.

## Notes About Destruction/Dispose Order

If you add bindings for classes that implement `IDisposable`, then you can control the order that they are disposed in by setting the [execution order](#update--initialization-order).  However this is not the case for GameObjects in the scene.

Unity has a concept of "script execution order" however this value does not affect the order that OnDestroy is executed.  The root-level game objects might be destroyed in any order and this includes the SceneContext as well.

One way to make this more predictable is to place everything underneath SceneContext.   For cases where a deterministic destruction order is needed this can be very helpful, because it will at least guarantee that the bound IDisposables get disposed of first before any of the game objects in the scene.  You can also toggle the setting on SceneContext "Parent New Objects Under Scene Context" to automatically parent all dynamically instantiated objects under SceneContext as well.

Another issue that can sometimes arise in terms of destruction order is the order that the scenes are unloaded in and also the order that the DontDestroyOnLoad objects (including ProjectContext) are unloaded in.

Unfortunately, Unity does not guarantee a deterministic destruction order in this case either, and you will find that sometimes when exiting your application, the DontDestroyOnLoad objects are actually destroyed before the scenes, or you will find that a scene that was loaded first was also the first to be destroyed which is usually not what you want.

If the scene destruction order is important to you, then you might consider also changing the ZenjectSetting `Ensure Deterministic Destruction Order On Application Quit` to true.  When this is set to true, this will cause all scenes to be forcefully destroyed during the OnApplicationQuit event, using a more sensible order than what unity does by default.  It will first destroy all scenes in the reverse order that they were loaded in (so that earlier loaded scenes are destroyed later) and will finish by destroying the DontDestroyOnLoad objects which include project context.

The reason this setting is not set to true by default is because it can cause crashes on Android as discussed [here](https://github.com/modesttree/Zenject/issues/301).

## UniRx Integration

[UniRx](https://github.com/neuecc/UniRx) is a library that brings Reactive Extensions to Unity.  It can greatly simplify your code by thinking of some kinds of communication between classes as 'streams' of data.  For more details see the [UniRx docs](https://github.com/neuecc/UniRx).

Zenject integration with UniRx is disabled by default.  To enable, you must add the define `ZEN_SIGNALS_ADD_UNIRX` to your project, which you can do by selecting Edit -> Project Settings -> Player and then adding `ZEN_SIGNALS_ADD_UNIRX` in the "Scripting Define Symbols" section

With zenject version 7.0.0, you'll also have to change the Zenject.asmdef file to the following:

```
{
    "name": "Zenject",
    "references": [
        "UniRx"
    ]
}
```

With `ZEN_SIGNALS_ADD_UNIRX` enabled, you can observe zenject signals via UniRx streams as explained in the [signals docs](Documentation/Signals.md), and you can also observe zenject events such as Tick, LateTick, and FixedTick etc. on the `TickableManager` class.  One example usage is to ensure that certain events are only handled a maximum of once per frame:

```csharp
public class User
{
    public string Username;
}

public class UserManager
{
    readonly List<User> _users = new List<User>();
    readonly Subject<User> _userAddedStream = new Subject<User>();

    public IReadOnlyList<User> Users
    {
        get { return _users; }
    }

    public IObservableRx<User> UserAddedStream
    {
        get { return _userAddedStream; }
    }

    public void AddUser(User user)
    {
        _users.Add(user);
        _userAddedStream.OnNext(user);
    }
}

public class UserDisplayWindow : IInitializable, IDisposable
{
    readonly TickableManager _tickManager;
    readonly CompositeDisposable _disposables = new CompositeDisposable();
    readonly UserManager _userManager;

    public UserDisplayWindow(
        UserManager userManager,
        TickableManager tickManager)
    {
        _tickManager = tickManager;
        _userManager = userManager;
    }

    public void Initialize()
    {
        _userManager.UserAddedStream.Sample(_tickManager.TickStream)
            .Subscribe(x => SortView()).AddTo(_disposables);
    }

    void SortView()
    {
        // Sort the displayed user list
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
```

In this case we have some costly operation that we want to run every time some data changes (in this case, sorting), and all it does is affect how something is rendered (in this case, a displayed list of user names).  We could implement ITickable and then set a boolean flag every time the data changes, then perform the update inside Tick(), but this isn't really the reactive way of doing things, so we use Sample() instead.

## Auto-Mocking using Moq

See [here](Documentation/AutoMocking.md).

## Creating Unity EditorWindow's with Zenject

If you need to add your own Unity plugin, and you want to create your own EditorWindow derived class, then you might consider using Zenject to help manage this code as well.  Let's go through an example of how you might do this:

1. Right click underneath an Editor folder in your project view then select `Create -> Zenject -> Editor Window`.  Let's call it TimerWindow.
2. Open your new editor window by selecting the menu item `Window -> TimerWindow`.
3. Right now it is empty, so let's add some content to it.  Open it up and replace the contents with the following:

```csharp
public class TimerWindow : ZenjectEditorWindow
{
    TimerController.State _timerState = new TimerController.State();

    [MenuItem("Window/TimerWindow")]
    public static TimerWindow GetOrCreateWindow()
    {
        var window = EditorWindow.GetWindow<TimerWindow>();
        window.titleContent = new GUIContent("TimerWindow");
        return window;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(_timerState);
        Container.BindInterfacesTo<TimerController>().AsSingle();
    }
}

class TimerController : IGuiRenderable, ITickable, IInitializable
{
    readonly State _state;

    public TimerController(State state)
    {
        _state = state;
    }

    public void Initialize()
    {
        Debug.Log("TimerController initialized");
    }

    public void GuiRender()
    {
        GUI.Label(new Rect(25, 25, 200, 200), "Tick Count: " + _state.TickCount);

        if (GUI.Button(new Rect(25, 50, 200, 50), "Restart"))
        {
            _state.TickCount = 0;
        }
    }

    public void Tick()
    {
        _state.TickCount++;
    }

    [Serializable]
    public class State
    {
        public int TickCount;
    }
}
```

In the InstallBindings method for your ZenjectEditorWindow, you can add IInitializable, ITickable, and IDisposable bindings just like you do within your scenes.  There is also a new interface called `IGuiRenderable` that you can use to draw content to the window by using Unity's immediate mode gui.

Note that every time your code is compiled again within Unity, your editor window is reloaded.  InstallBindings is called again and all your classes are created again from scratch.  This means that any state information you may have stored in member variables will be reset.  However, the member fields in EditorWindow derived class itself is serialized, so you can take advantage of this to have state persist across re-compiles.  In the example above, we are able to have the current tick count persist by wrapping it in a Serializable class and including this as a member inside our EditorWindow.

Something else to note is that the rate at which the ITickable.Tick method gets fired can change depending on what you have on focus.  If you run our timer window, then select another window other than Unity, you can see what I mean.  (Tick Count increments much more slowly)

## Optimization Recommendations/Notes

1. Use [memory pools](#memory-pools) with an initial size.  This should restrict all the costly instantiate operations to scene startup and allow you to avoid any performance spikes once the game starts.  Or, if you want to be really thorough, you could use a fixed size, which would trigger exceptions when the pool size limit is reached.

2. Use [reflection baking](#reflection-baking).  This is often simply a matter of enabling it and forgetting it, and can eliminate as much as 45% of the time spent running zenject code during scene startup.

3. Use Unity's Profiler.  When Unity's profiler is open, Zenject automatically adds profiling samples for all the common zenject interface operations including IInitializable.Initialize, ITickable.Tick, IDisposable.Dispose, etc.  in a similar way that unity does this automatically for all MonoBehaviour methods.  So, if you implement ITickable then you should see Foo.Tick in the profiler where Foo is one of your classes.

One common frustration people have when they start to profile their Zenject project is that it can be difficult to distinguish between time spent in Zenject versus time spent in their own code.  And therefore it can be tempting to blame Zenject.  This is especially the case when looking at the costs related to scene startup, since the SceneContext.Awake method appears to eat up a lot of the frame.  SceneContext.Awake is where the install is triggered, and also where the entire object graph is constructed, so contains within it a combination of zenject code and also user code.

To help determine how much of this initial performance hit is zenject compared to your own code, you can enable the define ZEN_INTERNAL_PROFILING in your player settings.  After doing this, if you run the scene you should see a more detailed break-down of all the costs incurred while invoking SceneContext.Awake.  After enabling this flag and running your scene, the profiling output should appear in the console like this:

```
SceneContext.Awake detailed profiling: Total time tracked: 3104.19 ms.  Details:
  67.2% (02960x) (2086 ms) User Code
  19.4% (01243x) (0602 ms) Type Analysis - Direct Reflection
  06.1% (02928x) (0189 ms) DiContainer.Resolve
  02.3% (00003x) (0071 ms) Other
  02.3% (00259x) (0071 ms) DiContainer.Bind
  01.3% (01112x) (0041 ms) DiContainer.Instantiate
  00.7% (01032x) (0023 ms) Searching Hierarchy
  00.4% (01243x) (0011 ms) Type Analysis - Calling Baked Reflection Getter
  00.3% (01852x) (0010 ms) DiContainer.Inject
```

Or, if you enable reflection baking, then the 'Direct Reflection' costs should be eliminated and it should appear more like this:

```
SceneContext.Awake detailed profiling: Total time tracked: 2357.43 ms.  Details:
  79.1% (02964x) (1865 ms) User Code
  06.4% (02928x) (0151 ms) DiContainer.Resolve
  06.2% (01243x) (0145 ms) Type Analysis - Calling Baked Reflection Getter
  02.8% (00003x) (0067 ms) Other
  02.4% (00259x) (0057 ms) DiContainer.Bind
  01.5% (01112x) (0034 ms) DiContainer.Instantiate
  00.8% (01852x) (0020 ms) DiContainer.Inject
  00.8% (01032x) (0018 ms) Searching Hierarchy
```

As you can see, in this case 79% of the costs incurred by calling SceneContext.Awake method were related to our game code (labelled here as User Code) rather than zenject.

Note that when not using reflection baking, the costs associated with 'Direct Reflection' above should mostly only occur at startup.  This is because after the first time these costs are incurred, the results are cached.

You can also get minor gains in speed and minor reductions in memory allocations by defining `ZEN_STRIP_ASSERTS_IN_BUILDS` in build settings.  This will cause all asserts to be stripped out of builds.  However, note that debugging any zenject related errors within builds will be made significantly more difficult by doing this.

For some benchmarks on Zenject versus other DI frameworks, see [here](https://github.com/svermeulen/IocPerformance) (see the charts at the bottom in particular).

## Reflection Baking

One easy way to squeeze extra performance out of Zenject is to enable a feature called Reflection Baking.  This will move some of the costs associated with analyzing the types in your codebase (aka reflection) from runtime to build time.  In one of our products at Modest Tree, turning on baking resulted in a 45% reduction in zenject startup time (which amounted to around 424 milliseconds saved).  Results vary project to project depending on how many types are used and the target platform, but is often noticeable.

Reflection Baking will also reduce the time taken to instantiate new objects.  This is especially true on IL2CPP platforms, where instantiating via reflection is typically slower due to restrictions there.

To enable for your project, simply right click somewhere in the project tab and select Create -> Zenject -> Reflection Baking Settings. Now if you build your project again, reflection costs inside Zenject should be mostly eliminated.

By default, reflection baking will modify all the generated assemblies in your project.  These include all the assemblies that Unity generates and places in the Library/ScriptAssemblies folder, and does not include any assemblies that are placed underneath the Assets directory (however you can also apply reflection baking there too as a [separate step](#reflection-baking-external-dlls))

In many cases you will want to limit which areas of the code reflection baking is applied to.  You can do this by selecting the reflection baking settings object, unchecking the `All Generated Assemblies` flag, and then explicitly adding the assemblies you want to use to the `Include Assemblies` property.  Or you can leave the `All Generated Assemblies` flag set to true and instead limit which assemblies are changed by adding one or more regular expressions to the Namespace Patterns field, and also changing the `Exclude Assemblies` property.  For example, if all of your game code lives underneath the `ModestTree.SpaceFighter` namespace, then you could ensure that the reflection baking only applies there by adding `^ModestTree.SpaceFighter` as a namespace pattern.  Note that zenject will automatically add a namespace pattern for itself so it is not necessary for you to do this (however, it is necessary to add the zenject assemblies to `Include Assemblies` if you do not have `All Generated Assemblies` checked)

By default, reflection baking will only apply to builds and will not be in effect while testing inside the unity editor.  If you want to temporarily disable baking you can uncheck `Is Enabled In Builds` in the inspector.  You can also force baking to apply while inside unity editor by checking `Is Enabled in Editor`.  However note that this will slow down compile times so probably will not be worth it, but can be useful when profiling to see the effect that the baking has.  Also note that if you are using Unity 2017 LTS then reflection baking can only be used by builds due to Unity API limitations.

### Baking External DLLs

When using reflection baking as described above, by adding a reflection baking settings object, this will only apply reflection baking to the C# files that are dropped directly into your unity project.  If you are using external dlls, and want to have reflection baking applied there as well, then you can do this by adding a post-build step.  There is a command line tool that you can find in the github repo by opening the `zenject\NonUnityBuild\Zenject.sln` file and building the "Zenject-ReflectionBakingCommandLine" project.

### Under the hood

Reflection Baking uses a library called [Cecil](https://github.com/jbevain/cecil) to do IL Weaving on the generated assemblies.  What this means is that after Unity generates the DLLs for the source files in your project, Zenject will edit those DLLs directly to embed zenject operations directly into your classes.   Normally, zenject has to iterate over every field, property, methods, and constructors of your classes to find what needs to be injected, and this is where the reflection costs are incurred.  However, once a class has reflection baking applied, then zenject just needs to call a static method on your class to retrieve all the information that it needs, which can be much faster.

### Coverage Settings

There are two settings on ProjectContext related to reflection baking that can be useful to be aware of.  `Editor Reflection Baking Coverage Mode` and `Builds Reflection Baking Coverage Mode`.  These values will determine what behaviour zenject should use when encountering types that do not have baked reflection information (Editor for when inside unity editor and Builds for when running inside generated builds).  The choices are:

1. Fallback To Direct Reflection - This will cause zenject to incur the necessary reflection operations when baking info is not found.  This is the default.
1. No Check Assume Full Coverage - With this value set, if no reflection baking information is found for a given type, then Zenject will assume that the type does not contain any reflection information.  This can be useful in cases where there is a lot of third party code that does not have reflection baking applied, but also does not use zenject in any way.   When coverage mode is set to (1), then this can be costly because Zenject will still analyze the third party code using reflection operations.  Note that when this is set, you will need to ensure that reflection baking is always applied everywhere that uses zenject.
1. Fallback To Direct Reflection With Warning - With this value set, when Zenject encounters a type that does not have reflection baking applied, it will use costly reflection operations, but will also issue a warning.  This can be useful if your intention is to get full coverage with reflection baking, but you don't want to use mode (2) and cause things to completely break when certain types are missed by the baking process

## Upgrade Guide for Zenject 6

The biggest backwards-incompatible change in Zenject 6 is that the signals system was re-written from scratch and works quite differently now.  However - if you want to continue using the previous signals implementation you can get a zenject-6-compatible version of that [here](https://github.com/svermeulen/ZenjectSignalsOld). So to use that, just import zenject 6 and make sure to uncheck the `OptionalExtras/Signals` folder, and then add the ZenjectSignalsOld folder to your project from that link.

Another backwards-incompatible change in zenject 6 is that AsSingle can no longer be used across multiple bind statements when mapping to the same instance.  In Zenject 5.x and earlier, you could do the following:

```csharp
public interface IFoo
{
}

public class Foo : IFoo
{
}

public void InstallBindings()
{
    Container.Bind<Foo>().AsSingle();
    Container.Bind<IFoo>().To<Foo>().AsSingle();
}
```

However, if you attempt this in Zenject 6, you will get runtime errors.  The zenject 6 way of doing this is now this:

```csharp
public void InstallBindings()
{
    Container.Bind(typeof(Foo), typeof(IFoo)).To<Foo>().AsSingle();
}
```

Or, alternatively you could do it this way as well:

```csharp
public void InstallBindings()
{
    Container.Bind<Foo>().AsSingle();
    Container.Bind<IFoo>().To<Foo>().FromResolve();
}
```

The reason this was changed was because supporting the previous way of using AsSingle had large implementation costs and was unnecessary given these other ways of doing things.

Another change that may cause issues is that for every binding that is a lookup there is both a plural form and a non plural form.  This includes the following:

- FromResource / FromResources
- FromResolve / FromResolveAll
- FromComponentInNewPrefab / FromComponentsInNewPrefab
- FromComponentInNewPrefabResource / FromComponentsInNewPrefabResource
- FromComponentInHierarchy / FromComponentsInHierarchy
- FromComponentSibling / FromComponentsSibling
- FromComponentInParents / FromComponentsInParents
- FromComponentInChildren / FromComponentsInChildren

So if you were previously using one of these methods to match multiple values you will have to change to use the plural version instead.

Another change worth mentioning is that the default value for the 'includeInactive' flag passed to the FromComponent methods was changed from false to true as discussed [here](https://github.com/modesttree/Zenject/issues/275#issuecomment-377619400)

There were also a few things that were renamed:

- `Factory<>` is now called `PlaceholderFactory<>` (in this case you should just get warnings about it however)
- BindFactoryContract is now called BindFactoryCustomInterface.  BindMemoryPool has a similar method called BindMemoryPoolCustomInterface
- Zenject.Lazy was renamed to Zenject.LazyInject to address a naming conflict with System.Lazy  (We'd like to use System.Lazy directly but this causes issues on IL2CPP)
- ByNewPrefab bind method was renamed to ByNewContextPrefab

## DiContainer Methods

In addition to the bind methods documented above, there are also some other methods you might want to occasionally use on DiContainer.  For example, if you are writing a custom factory, you might want to directly call one of the `DiContainer.Instantiate` methods.  Or you might have a situation where another library is creating instances of your classes (for example, a networking library) and you need to manually call DiContainer.Inject.

DiContainer is always added to itself, so you can always get it injected into any class.  However, note that injecting the DiContainer is usually a sign of bad practice, since there is almost always a better way to design your code such that you don't need to reference DiContainer directly (the exception being custom factories, but even in that case it's often better to [inject a factory into your custom factory](Documentation/Factories.md#custom-factories).  Once again, best practice with dependency injection is to only reference the DiContainer in the "composition root layer" which includes any custom factories you might have as well as the installers.  However there are exceptions to this rule.

### DiContainer.Instantiate

---

These instantiate methods might be useful for example inside a custom factory.  Note however that in most cases, you can probably get away with using a normal [Factory](#creating-objects-dynamically-using-factories) instead without needing to directly reference DiContainer.

When instantiating objects directly, you can either use DiContainer or you can use IInstantiator, which DiContainer inherits from.  IInstantiator exists because often, in custom factories, you are only interested in the instantiate operation so you don't need the Bind, Resolve, etc. methods

1. **Instantiate&lt;T&gt;** - Create the given class using the new operator and then inject it.  Note that this method should not be used for Components/MonoBehaviours.  However, it can be used for ScriptableObject derived classes (in which case Zenject will automatically call ScriptableObject.CreateInstance).

    ```csharp
    Foo foo = Container.Instantiate<Foo>();
    ```

    You can also pass extra arguments to it like this:

    ```csharp
    Foo foo = Container.Instantiate<Foo>(new object[] { "foo", 5 });
    ```

    There is also non-generic versions:

    ```csharp
    Foo foo = (Foo)Container.Instantiate(typeof(Foo));
    Foo foo = (Foo)Container.Instantiate(typeof(Foo), new object[] { "foo", 5 });
    ```

1. **InstantiatePrefab** - Instantiate the given prefab and then inject into any MonoBehaviour's that are on it.

    ```csharp
    GameObject gameObject = Container.InstantiatePrefab(MyPrefab);
    ```

    This method is equivalent to calling `var gameObject = GameObject.Instantiate(MyPrefab)` yourself and then calling `DiContainer.Inject(gameObject)`.  Note that MyPrefab above can either be a GameObject or it can be a direct reference to a component on the prefab.

    Similar to `GameObject.Instantiate`, you can also pass an initial parent transform to use:

    ```csharp
    GameObject gameObject = Container.InstantiatePrefab(MyPrefab, MyParentTransform);
    ```

1. **InstantiatePrefabResource** - Same as InstantiatePrefab except instead of passing a prefab, you pass a path within the unity Resources folder where the prefab exists.

    ```csharp
    GameObject gameObject = Container.InstantiatePrefabResource("path/to/myprefab");
    ```

    This method is simply a shortcut to calling `Container.InstantiatePrefab(Resources.Load("path/to/myprefab"));`

1. **InstantiatePrefabForComponent&lt;T&gt;** - Instantiates the given prefab, injects on the prefab, and then returns the given component which is assumed to exist somewhere in the hierarchy of the prefab.

    ```csharp
    var foo = Container.InstantiatePrefabForComponent<Foo>(FooPrefab)
    ```

    Unlike the InstantiatePrefab methods above, this method also allows passing parameters to the given component:

    ```csharp
    var foo = Container.InstantiatePrefabForComponent<Foo>(FooPrefab, new object[] { "asdf", 6.0f })
    ```

1. **InstantiatePrefabResourceForComponent&lt;T&gt;** - Same as InstantiatePrefabForComponent, except the prefab is provided as a resource path instead of as a direct reference

    ```csharp
    var foo = Container.InstantiatePrefabResourceForComponent<Foo>("path/to/fooprefab")
    var foo = Container.InstantiatePrefabResourceForComponent<Foo>("path/to/fooprefab", new object[] { "asdf", 6.0f })
    ```

1. **InstantiateComponent&lt;T&gt;** - Add the given component to a given game object.

    ```csharp
    var foo = Container.InstantiateComponent<Foo>(gameObject);
    var foo = Container.InstantiateComponent<Foo>(gameObject, new object[] { "asdf", 6.0f });
    ```

    Note that this is equivalent to calling GameObject.AddComponent yourself then immediately calling DiContainer.Inject on the new component instance.

1. **InstantiateComponentOnNewGameObject&lt;T&gt;** - Create a new empty game object then instantiate a new component of the given type on it

    ```csharp
    var foo = Container.InstantiateComponentOnNewGameObject<Foo>();
    var foo = Container.InstantiateComponentOnNewGameObject<Foo>(new object[] { "zxcv" });
    ```

    This is similar to calling `new GameObject()`, then call DiContainer.InstantiateComponent on the result.

1. **InstantiateScriptableObjectResource&lt;T&gt;** - Instantiate the given ScriptableObject type which is assumed to exist at the given resource path.  Note that if you want to create an entirely new ScriptableObject, you can just use DiContainer.Instantiate.

    ```csharp
    var foo = Container.InstantiateScriptableObjectResource<Foo>("path/to/fooscriptableobject")
    var foo = Container.InstantiateScriptableObjectResource<Foo>("path/to/fooscriptableobject", new object[] { "asdf", 6.0f })
    ```
### DiContainer.Bind

---

See [here](#binding)

### DiContainer.Resolve

---

1.  **DiContainer.Resolve** - Get instance to match the given type.  This may involve creating a new instance or it might return an existing instance, depending on how the given type was bound.

    ```csharp
    Container.Bind<Foo>().AsSingle();
    ...
    var foo = Container.Resolve<Foo>();
    ```

    An exception will be thrown if no bindings were found for the given type or if multiple bindings were found.  See TryResolve / ResolveAll for those cases.

1.  **DiContainer.ResolveId** - Same as resolve except includes an identifier

    ```csharp
    Container.Bind<Foo>().WithId("foo1").AsSingle();
    ...
    var foo = Container.ResolveId<Foo>("foo1");
    ```

1.  **DiContainer.TryResolve** - Same as DiContainer.Resolve except instead of throwing an exception when a match is not found, a null value is returned.

    ```csharp
    var foo = Container.TryResolve<Foo>();

    if (foo != null)
    {
        ...
    }
    ```

1.  **DiContainer.TryResolveId** - Same as DiContainer.TryResolve except also takes an identifier

1.  **DiContainer.ResolveAll** - Same as DiContainer.Resolve except it will return all matches instead of assuming just one.

    ```csharp
    List<Foo> foos = Container.ResolveAll<Foo>();
    ```

1.  **DiContainer.ResolveIdAll** - Same as DiContainer.ResolveAll except also takes an identifier

1.  **DiContainer.ResolveType** - Returns the type that would be retrieved/instantiated if Resolve is called with the same type/identifier.

    ```csharp
    if (Container.ResolveType<IFoo>() == typeof(Foo))
    {
        ...
    }
    ```

    This is safe to call during install phase since nothing will be instantiated by calling this.  Note also that if there are multiple matches found or zero matches then an exception will be thrown.

1.  **DiContainer.ResolveTypeAll** - Same as ResolveType except returns all matches instead of assuming a single match.

### DiContainer.Inject

1.  **DiContainer.Inject** - Inject on the given instance.

    ```csharp
    Container.Inject(foo);
    ```

    Note that you can also pass extra arguments to inject:

    ```csharp
    Container.Inject(foo, new object[] { "asdf", 6 });
    ```

    This will inject in the following order:

    1. Fields
    1. Properties
    1. Inject methods

1.  **DiContainer.InjectGameObject** - Inject into all MonoBehaviour's attached to the given game object as well as any children.

    ```csharp
    Container.InjectGameObject(gameObject);
    ```

    Note that it will inject all the components in their dependency order.  So if A is injected into B and B is injected into C, then A will be injected first, then B, then C, regardless of where in the hierarchy they are all placed

1.  **DiContainer.InjectGameObjectForComponent** - Same as InjectGameObject except it will return the given component after injection completes.

    ```csharp
    var foo = Container.InjectGameObjectForComponent<Foo>(gameObject);
    ```

    Also, unlike InjectGameObject, this method supports passing extra arguments to the given component:

    ```csharp
    var foo = Container.InjectGameObjectForComponent<Foo>(gameObject, new object[] { "asdf", 5.1f });
    ```

    Note however that it is assumed here that there is only one match for the given component.  Multiple matches (or zero matches) will generate an exception.

### DiContainer.QueueForInject

---

**DiContainer.QueueForInject** will queue the given instance for injection once the initial object graph is constructed.

Sometimes, there are instances that are not created by Zenject and which exist at startup, and which you want to be injected.  In these cases you will often add them to the container like this:

```csharp
var foo = new Foo();
...
Container.Bind<Foo>().FromInstance(foo);
```

Or, equivalently, using this shortcut:

```csharp
Container.BindInstance(foo);
```

However, using FromInstance will not cause the given instance to be injected.

One approach would be to inject on the instance immediately like this:

```csharp
Container.BindInstance(foo);
Container.Inject(foo);
```

However, this is bad practice.   You do not ever want to instantiate or inject during the install phase, because the objects that you are injecting could require bindings that have not yet been added.

Therefore the correct way to handle these cases is to use QueueForInject:

```csharp
Container.BindInstance(foo);
Container.QueueForInject(foo);
```

This way, our `foo` object will be injected at the same time the initial object graph is constructed, immediately after the install phase.

Another important advantage of using QueueForInject is that Zenject will guarantee that your instances are injected in the correct order.  In other words, if you have the following classes:

```csharp
class A
{
    [Inject]
    public void Init()
    {
        ...
    }
}

class B
{
    [Inject]
    public void Init(A a)
    {
        ...
    }
}
```

And they are added to the container this way:

```csharp
var a = new A();
var b = new B();

Container.BindInstance(a);
Container.BindInstance(b);

Container.QueueForInject(a);
Container.QueueForInject(b);
```

Then, you can assume that A will have its inject method called before B, regardless of the order that they were added to the container.  This is nice in the case where you have some initialization logic that occurs in the `[Inject]` method for B, and which requires that A has been initialized.

This is also precisely how the initial MonoBehaviour's in the scene are injected.  They are all simply added to the container with the QueueForInject method.

### DiContainer Unbind / Rebind

---

It is possible to remove or replace bindings that were added in a previous bind statement.  Note however that using methods are often a sign of bad practice.

1. **Unbind** - Remove all bindings matching the given type/id from container.

    ```csharp
    Container.Bind<IFoo>().To<Foo>();

    // This will nullify the above statement
    Container.Unbind<IFoo>();
    ```

1. **Rebind** - Override existing bindings with a new one.  This is equivalent to calling unbind with the given type and then immediately calling bind afterwards.

    ```csharp
    Container.Bind<IFoo>().To<Foo>();

    Container.Rebind<IFoo>().To<Bar>();
    ```

### Other DiContainer methods

---

1.  **DiContainer.ParentContainers** - The parent containers for the given DiContainer.  For example, for the DiContainer associated with SceneContext, this will usually be the DiContainer associated with the ProjectContext (unless you're using Scene Parenting in which case it will be another SceneContext)
1.  **DiContainer.IsValidating** - Returns true if the container is being run for validation.  This can be useful in some edge cases where special logic needs to be added during the validation step only.
1.  **DiContainer.CreateSubContainer** - Creates a new container as a child of the current container.  This might be useful for custom factories that involve creating objects with complex inter-dependencies.  For example:

    ```csharp
    var subContainer = Container.CreateSubContainer();

    subContainer.Bind<Foo>();
    subContainer.Bind<Bar>();
    subContainer.Bind<Qux>();

    var foo = subContainer.Resolve<Foo>();
    ```

    Although in most cases you should probably use the FromSubContainerResolve methods rather than doing it this way.

1.  **DiContainer.HasBinding** - Returns true if a binding that matches the given type/id has already been added.  This can be useful if you want to avoid adding a duplicate binding that may have been added in a previous installer

    ```csharp
    if (!Container.HasBinding<IFoo>())
    {
        Container.Bind<IFoo>().To<Foo>().AsSingle();
    }
    ```

1.  **DiContainer.GetDependencyContracts** - Returns a list of all the types that the given type depends on.  This might be useful, for example, if you wanted to do some static analysis of your project, or if you wanted to automatically generate a dependency diagram, etc.

## Frequently Asked Questions

### Isn't this overkill?  I mean, is using statically accessible singletons really that bad?

    For small enough projects, I would agree with you that using a global singleton might be easier and less complicated.  But as your project grows in size, using global singletons will make your code unwieldy.  Good code is basically synonymous with loosely coupled code, and to write loosely coupled code you need to (A) actually be aware of the dependencies between classes and (B) code to interfaces (however I don't literally mean to use interfaces everywhere, as explained [here](#overusinginterfaces))

    In terms of (A), using global singletons, it's not obvious at all what depends on what, and over time your code will become really convoluted, as everything will tend towards depending on everything.  There could always be some method somewhere deep in a call stack that does some hail mary request to some other class anywhere in your code base.  In terms of (B), you can't really code to interfaces with global singletons because you're always referring to a concrete class

    With a DI framework, in terms of (A), it's a bit more work to declare the dependencies you need up-front in your constructor, but this can be a good thing too because it forces you to be aware of the dependencies between classes.

    And in terms of (B), it also forces you to code to interfaces.  By declaring all your dependencies as constructor parameters, you are basically saying "in order for me to do X, I need these contracts fulfilled".  These constructor parameters might not actually be interfaces or abstract classes, but it doesn't matter - in an abstract sense, they are still contracts, which isn't the case when you are creating them within the class or using global singletons.

    Then the result will be more loosely coupled code, which will make it 100x easier to refactor, maintain, test, understand, re-use, etc.

### Is there a way to integrate with the upcoming Unity ECS?

    Currently there does not appear to be an official way to do custom injections into Unity ECS systems, however, there are [some workarounds](https://forum.unity.com/threads/request-for-world-addmanager.539271/#post-3558224) until Unity hopefully addresses this.

### Does this work on AOT platforms such as iOS and WebGL?

    Yes.  However, there are a few things that you should be aware of.  One of the things that Unity's IL2CPP compiler does is strip out any code that is not used.  It calculates what code is used by statically analyzing the code to find usage.  This is great, except that this will sometimes strip out methods/types that we don't refer to explicitly (and instead access via reflection instead).

    In some versions of Unity, or with some settings applied (eg. a higher level of code stripping), IL2CPP can sometimes strip out the constructors of classes, resulting in errors on IL2CPP platforms.  The recommended fix in these cases is to either edit `link.xml` to force your types to not be stripped (see Unity docs) or to add an `[Inject]` attribute above the constructor.  Adding this attribute signals to IL2CPP to not strip out this method.

    If you do want to use a higher level of code stripping, and decide to go the route of adding `[Inject]` attributes to all constructors, then you might also want to change the `ConstructorChoiceStrategy` value inside the settings found on `ProjectContext` to `InjectAttribute`.  This can be useful because it will force you to explicitly add `[Inject]` attributes to all constructors while testing in Unity Editor, rather than having to test on IL2CPP platforms to discover these problems.  Note however that this would probably be unnecessary with 'Low' code stripping.

    Sometimes, another issue that can occur is with classes that have generic arguments and which are instantiated with a "value type" generic argument (eg. int, float, enums, anything deriving from struct, etc.).  In this case, compiling on AOT platforms will sometimes strip out the constructor, so Zenject will not be able to create the class and you will get a runtime error.  For example:

    ```csharp
        public class Foo<T1>
        {
            public Foo()
            {
                Debug.Log("Successfully created Foo!");
            }
        }

        public class Runner2 : MonoBehaviour
        {
            public void OnGUI()
            {
                if (GUI.Button(new Rect(100, 100, 500, 100), "Attempt to Create Foo"))
                {
                    var container = new DiContainer();

                    // This will throw exceptions on AOT platforms because the constructor for Foo<int> is stripped out of the build
                    container.Instantiate<Foo<int>>();

                    // This will run fine however, because string is not value type
                    //container.Instantiate<Foo<string>>();
                }
            }

            static void _AotWorkaround()
            {
                // As a workaround, we can explicitly reference the constructor here to force the AOT
                // compiler to leave it in the build
                // new Foo<int>();
            }
        }
    ```

    Normally, in a case like above where a constructor is being stripped out, we can force-include it by adding the `[Inject]` attribute on the Foo constructor, however this does not work for classes with generic types that include a value type.  Therefore, the recommended workarounds here are to either explicitly reference the constructor similar to what you see in the _AotWorkaround, or avoid using value type generic arguments.  One easy way to avoid using value types is to wrap it in a reference type (for example, by using something like [this](https://gist.github.com/svermeulen/a6929e6e26f2de2cc697d24f108c5f85))

### How is performance?

    See [here](#optimization_notes)

* **<a id="faq-multiple-threads"></a>Does Zenject support multithreading?**

    Yes, with a few caveats:
    - You need to enable the define `ZEN_MULTITHREADING` in player settings
    - Only the resolve and instantiate operations support multithreading.  The bindings that occur during the install phase must cannot be executed concurrently on multiple threads.  In other words, everything after the initial install should support multithreading.
    - Circular reference errors are handled less gracefully.  Instead of an exception with an error message that details the object graph with the circular reference, a StackOverflowException might be thrown
    - If you make heavy use of zenject from multiple threads at the same time, you might also want to enable the define `ZEN_INTERNAL_NO_POOLS` which will cause zenject to not use memory pools for internal operations.  This will cause more memory allocations however can be much faster in some cases because it will avoid hitting locks the memory pools uses to control access to the shared data across threads

### How do I use Unity style Coroutines in normal C# classes?

    With Zenject, there is less of a need to make every class a `MonoBehaviour`.  But it is often still desirable to be able to call `StartCoroutine` to add asynchronous methods.

    One solution here is to use a dedicated class and just call `StartCoroutine` on that instead.  For example:

    ```csharp
    public class AsyncProcessor : MonoBehaviour
    {
        // Purposely left empty
    }

    public class Foo : IInitializable
    {
        AsyncProcessor _asyncProcessor;

        public Foo(AsyncProcessor asyncProcessor)
        {
            _asyncProcessor = asyncProcessor;
        }

        public void Initialize()
        {
            _asyncProcessor.StartCoroutine(RunAsync());
        }

        public IEnumerator RunAsync()
        {
            Debug.Log("Foo started");
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Foo finished");
        }
    }

    public class TestInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInitializable>().To<Foo>().AsSingle();
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
    ```

    Another solution to this problem which I highly recommend is [UniRx](https://github.com/neuecc/UniRx).

    Yet another option is to use a coroutine library that implements similar functionality to what Unity provides.  See [here](https://github.com/svermeulen/UnityCoroutinesWithoutMonoBehaviours) for one example that we've used in the past at Modest Tree

### Are there any more sample projects to look at?

    Complete examples (with source) using zenject:

    * [Zenject Hero](https://github.com/Mathijs-Bakker/Zenject-Hero) - Remake of the classic Atari game H.E.R.O.   Based on Zenject 6
    * [Quick Golf](https://assetstore.unity.com/packages/templates/packs/quick-golf-67900) - Mini-golf game
    * [EcsRx Roguelike 2D](https://github.com/grofit/ecsrx.roguelike2d) - An example of a Roguelike 2d game using EcsRx and Zenject
    * [Push The Squares](https://assetstore.unity.com/packages/templates/packs/push-the-squares-69780) - This is the puzzle game in which you have to find the proper way to connect squares with stars of the same color. 
    * [Submarine](https://github.com/shiwano/submarine) - A mobile game that is made with Unity3D, RoR, and WebSocket server written in Go.
    * [Craberoid](https://github.com/Crabar/Craberoid-3.0) - Arkanoid clone

### What games/applications/libraries are using Zenject?

You can find a comprehensive list of solutions that have Zenject under the hood [here](Documentation/GamesThatUseZenject.md).

### I keep getting errors complaining about circular reference!  How to address this?

If two classes are injected into each other and both classes use contructor injection, then obviously it is not possible for zenject to create both of these classes.  However, what you can do instead is switch to use method injection or field/property injection instead.  Or, alternatively, you could use the [LazyInject<>](#just-in-time-resolve) construct.

## Cheat Sheet

See [here](Documentation/CheatSheet.md).

## Further Help

For general troubleshooting / support, please use the [zenject subreddit](http://www.reddit.com/r/zenject) or the [zenject google group](https://groups.google.com/forum/#!forum/zenject/).  If you have found a bug, you are also welcome to create an issue on the [github page](https://github.com/svermeulen/Extenject), or a pull request if you have a fix / extension.  Finally, you can also email me directly at sfvermeulen@gmail.com or follow me on twitter at [@steve_verm](https://twitter.com/steve_verm)

## Release Notes

See [here](Documentation/ReleaseNotes.md).

## License

    The MIT License (MIT)

    Copyright (c) 2010-2015 Modest Tree Media  http://www.modesttree.com

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
