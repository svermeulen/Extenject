# Release Notes

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Version 9.1.0 (October 13, 2019)](#version-910-october-13-2019)
- [Version 9.0.0 (April 21, 2019)](#version-900-april-21-2019)
- [Version 6.6.1 (April 21, 2019)](#version-661-april-21-2019)
- [Version 8.0.1 (April 21, 2019)](#version-801-april-21-2019)
- [Version 8.0.0 (February 5, 2019)](#version-800-february-5-2019)
- [Version 6.6.0 (February 5, 2019)](#version-660-february-5-2019)
- [Version 7.3.1 (October 20, 2018)](#version-731-october-20-2018)
- [Version 7.3.0 (October 6, 2018)](#version-730-october-6-2018)
- [Version 6.5.0 (October 6, 2018)](#version-650-october-6-2018)
- [Version 7.2.0 (August 27, 2018)](#version-720-august-27-2018)
- [Version 6.4.0 (August 27, 2018)](#version-640-august-27-2018)
- [Version 7.1.0 (August 6, 2018)](#version-710-august-6-2018)
- [Version 6.3.0 (August 6, 2018)](#version-630-august-6-2018)
- [Version 7.0.0 (July 19, 2018)](#version-700-july-19-2018)
- [Version 6.2.1 (July 19, 2018)](#version-621-july-19-2018)
- [Version 6.2.0 (July 18, 2018)](#version-620-july-18-2018)
- [Version 6.1.1 (June 18, 2018)](#version-611-june-18-2018)
- [Version 6.1.0 (June 17, 2018)](#version-610-june-17-2018)
- [Version 5.5.1 (March 12, 2017)](#version-551-march-12-2017)
- [Version 5.5.0 (March 7, 2017)](#version-550-march-7-2017)
- [Version 5.4.0 (October 2, 2017)](#version-540-october-2-2017)
- [Version 5.3.0 (September 18, 2017)](#version-530-september-18-2017)
- [Version 5.2.0 (April 30, 2017)](#version-520-april-30-2017)
- [Version 5.1.0 (April 3, 2017)](#version-510-april-3-2017)
- [Version 5.0.2 (March 5, 2017)](#version-502-march-5-2017)
- [Version 5.0.1 (February 15, 2017)](#version-501-february-15-2017)
- [Version 5.0 (February 13, 2017)](#version-50-february-13-2017)
- [Version 4.7 (November 6, 2016)](#version-47-november-6-2016)
- [Version 4.6 (October 23, 2016)](#version-46-october-23-2016)
- [Version 4.5 (September 1, 2016)](#version-45-september-1-2016)
- [Version 4.4 (July 23, 2016)](#version-44-july-23-2016)
- [Version 4.3 (June 4, 2016)](#version-43-june-4-2016)
- [Version 4.2 (May 30, 2016)](#version-42-may-30-2016)
- [Version 4.1 (May 15, 2016)](#version-41-may-15-2016)
- [Version 4.0 (April 30, 2016)](#version-40-april-30-2016)
- [Version 3.11 (May 15, 2016)](#version-311-may-15-2016)
- [Version 3.10 (March 26, 2016)](#version-310-march-26-2016)
- [Version 3.9 (Feb 7, 2016)](#version-39-feb-7-2016)
- [Version 3.8 (Feb 4, 2016)](#version-38-feb-4-2016)
- [Version 3.7 (Jan 31, 2016)](#version-37-jan-31-2016)
- [Version 3.6 (Jan 24, 2016)](#version-36-jan-24-2016)
- [Version 3.5 (Jan 17, 2016)](#version-35-jan-17-2016)
- [Version 3.4 (Jan 7, 2016)](#version-34-jan-7-2016)
- [Version 3.2 (December 20, 2015)](#version-32-december-20-2015)
- [Version 3.1](#version-31)
- [Version 3.0](#version-30)
- [Version 2.8](#version-28)
- [Version 2.7](#version-27)
- [Version 2.6](#version-26)
- [Version 2.5](#version-25)
- [Version 2.4](#version-24)
- [Version 2.3](#version-23)
- [Version 2.2](#version-22)
- [Version 2.1](#version-21)
- [Version 2.0](#version-20)
- [Version 1.19](#version-119)
- [Version 1.18](#version-118)
- [Version 1.17](#version-117)
- [Version 1.16](#version-116)
- [Version 1.15](#version-115)
- [Version 1.14](#version-114)
- [Version 1.13](#version-113)
- [Version 1.12](#version-112)
- [Version 1.11](#version-111)
- [Version 1.10](#version-110)
- [Version 1.09](#version-109)
- [Version 1.08](#version-108)
- [Version 1.07](#version-107)
- [Version 1.06](#version-106)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->



## Version 9.1.0 (October 13, 2019)

New Features
* Changed SceneLoader methods to use optional arguments instead of overloads to allow only specifying parameters of interest
* Added CustomPoolableManager class to allow passing arguments to IPoolable<T> derived classes
* Added non generic variant of DeclareSignal method
* Added support for NSubstitute usage with auto mocking

Bug fixes
* Fixed exception that would sometimes occur in multi-threaded applications
* Fixed some issues related to game object subcontainers and circular dependencies
* Changed the PreInstall/PostInstall/PreResolve/PostResolve methods in ProjectContext to be static so they can be used before initialization occurs
* Fixed IL2CPP issue related to default values
* Other minor bug fixes

## Version 9.0.0 (April 21, 2019)

This is the beginning of zenject support for Unity 2019.  For Unity 2018.4.x use 8.0.x versions of zenject.  For Unity 2017.4.x use 6.6.X versions of zenject.

No changes in this version except compatibility fixes for Unity 2019.

## Version 6.6.1 (April 21, 2019)

- Fixed WindowsStoreApps platform to build properly when using .NET 4.6 backend

## Version 8.0.1 (April 21, 2019)

Minor bug fixes:
- Fixed issue with UniRx signals integration
- Fixed to allow reflection baking to work inside unity editor for 2018.1+

## Version 8.0.0 (February 5, 2019)

Minor bug fixes, a few extra bind methods, and one minor breaking api change in SignalBus.

Notable
- Added new bind methods FromScriptableObject and FromNewScriptableObject bind methods for cases where you already have the ScriptableObject reference at install time
- Added new subcontainer bind method ByInstanceGetter
- Bug fix - the inject order was wrong in some rare edge cases (ie. sometimes objects would receive injected parameters that have not themsevles been injected yet)
- Bug fix - readonly properties were not injectable
- Added ability to declare signals at runtime after install
- Fixed playmode tests to work inside builds instead of just in editor
- Changed SignalBus class to have separate method names for the methods that take an identifier parameter.  Also added the ability to specify the signal type explicitly in Fire and TryFire methods.

Minor
- Fixed Pool Monitor window to support unity dark theme
- Changed the visual order of installer types on context to match their execution order
- Bug fix - classes in the System namespace (ie. Stopwatch) were not able to be created by zenject
- Fixed minor bugs with validation
- Fixed warnings in unity 2018 and unity 2019
- Fixed OnInstantiate method to work properly in cases where you bind multiple types to a new prefab
- Added UnityEvent versions of the events to SceneContext to allow hooking into it from inspector directly
- Fixed compile warnings specific to Rider IDE

## Version 6.6.0 (February 5, 2019)

Minor bug fixes and a few minor extra features

- Changed the visual order of installer types on context to match their execution order
- Bug fix - readonly properties were not injectable
- Fixed OnInstantiate method to work properly in cases where you bind multiple types to a new prefab
- Bug fix - classes in the System namespace (ie. Stopwatch) were not able to be created by zenject
- Added the ability to specify the signal type explicitly in Fire and TryFire methods
- Fixed some compiler warnings
- Added new subcontainer bind method ByInstanceGetter

## Version 7.3.1 (October 20, 2018)

Minor bug fixes

- Fixed compiler error related to the test framework asmdef
- Fixed issue with reflection baking on Unity 2018.3
- Changed the visual order of installers in contexts to match the actual order they are executed in (eg. scriptable object installers first)

## Version 7.3.0 (October 6, 2018)

Merged in changes from LTS version 6.5.0

## Version 6.5.0 (October 6, 2018)

Mostly optimizations, some minor bug fixes, and a few minor new features.

Notable:
- Added support for [Reflection Baking](https://github.com/svermeulen/Zenject#optimization_notes) to automatically eliminate costs associated with reflection from your zenject application.
- General optimizations to memory usage and processing time
- Added non-generic versions of all the FromComponentX methods
- Fixed multi-threading issues
- Added new bind methods ByNewGameObjectInstaller and ByNewGameObjectMethod
- Added ZEN_INTERNAL_PROFILING define to allow users to easily see how much cpu time is devoted to zenject versus custom game code
- Added an optional identifier for signals

Minor:
- Added ability to use custom attributes in place of Zenject.InjectAttribute
- Changed to use Expression.New when possible for inject methods, fields, properties, and constructors for extra speed
- Added ArrayPool class
- Improved readability of error messages
- Fixed rare bug where instantiated prefabs would get offset slightly by the scene context position
- Renamed ByNewPrefabResource to ByNewContextPrefabResource properly and made the previous name obsolete
- Added documentation for WithKernel, and WithDefaultGameObjectParent bind methods

## Version 7.2.0 (August 27, 2018)

Merged in changes from LTS version 6.4.0

## Version 6.4.0 (August 27, 2018)

A few new minor features and some bug fixes

Notable:
- Added new bind method WithDefaultGameObjectParent when using FromSubContainerResolve and ByInstaller or ByMethod to ensure instantiated game objects get destroyed with the subcontainer
- Added WithKernel bind method to avoid the need to make Facades always derive from Kernel
- Added ability to specify transient scope with BindFactory and BindMemoryPool methods
- Added new bind method OnInstantiated to run custom code when the object is created

Minor:
- Fixed decorators to properly be inherited into subcontainers
- Fixed bug with validation + decorators
- Added PrefabFactory and PrefabResourceFactory helper classes
- Fixed issue with destruction order of signals in some edge cases
- Added FromSubContainerResolve.ByInstance bind method to explicitly supply container to use
- Bug fix to support factories with 6 arguments
- Added ParentBus property to SignalBus

## Version 7.1.0 (August 6, 2018)

Merged in changes from LTS version 6.3.0

## Version 6.3.0 (August 6, 2018)

Bug fixes and some minor extensions

- Fixed struct type signals to work properly on AOT platforms
- Fixed issue with ZenjectIntegrationTestFixture where exceptions were being thrown during setup
- Added support for testing multiple scenes at once when using SceneTestFixture
- Added TryFire method on SignalBus for cases where you don't care if it's declared or not
- Fixed zenject integration tests to play nicely with asmdef files

## Version 7.0.0 (July 19, 2018)

Upgraded project to 2018.1.  Created an LTS branch of zenject to maintain support for Unity 2017.x

- Fixed IL2CPP issue with 2018.2
- Fixed issue with the asmdef files failing to generate a valid solution

## Version 6.2.1 (July 19, 2018)

Hotfix release for issue with testframework

- Changed to have all the test helper classes in one place underneath OptionalExtras/TestFramework and also fixed to not place it in a zip
- Fixed rare issue when instantiating prefabs in ZenjectUnitTestFixture (#506)

## Version 6.2.0 (July 18, 2018)

Bug fixes and some minor extensions

- Added back IInstantiator interface to be used as an alternative to directly injecting DiContainer
- Added unity project management asmdef files
- Fixed compiler warning about missing assignment for Inject fields when using Rider IDE (#483)
- Fixed to support signals defined as structs instead of classes
- Added optional signals support to the non unity zenject dll build
- Fixed BindSignal to support mapping to multiple bindings at once
- Fixed support for UWP platform with .NET scripting backend

## Version 6.1.1 (June 18, 2018)

Hotfix for exception in SceneContext inspector editor

## Version 6.1.0 (June 17, 2018)

Large release with lots of new features, bug fixes, and performance improvements.  Some API changes to be aware of before upgrading.  See the <a href="../README.md#upgrading-from-zenject5">upgrade guide</a> for details

This will also be the beginning of a Zenject LTS stream that will follow Unity LTS

Significant:
- Replaced the signals system with a very different 'event bus' like approach.  Also fully decoupled signals from zenject core so it can be unchecked from OptionalExtras when importing
- Removed ability to use AsSingle with the same type across multiple bind statements.  If you want to map multiple contracts to the same AsSingle, you need to include multiple types in the Bind(..) or use FromResolve
- Renamed Factory<> to PlaceholderFactory<>
- Changed behaviour during OnApplicationQuit to not forcefully destroy all scenes and their contents.  This was added previously to force a reasonable destruction order, however it breaks things on android so was turned off by default.  However it can be re-enabled via ZenjectSettings for people that need a predictable destruction order
- Changed validate keyboard shortcut from CTRL+SHIFT+V to CTRL+ALT+V to avoid conflict with Vuforia

Notable:
- Performance improvements - in some cases doubling the startup speed.  Also allocates a lot less garbage now by using memory pools internally
- Added support for automatically loading parent scene contracts and decorated scenes by including a config file in resources that specifies default scenes for certain contract names
- Added support for "decorator" bindings by calling Container.Decorate (see docs for usage)
- Added support for much more complex custom factory configuration using FromIFactory in addition to just FromFactory
- Added new type of Test Fixture called SceneTestFixture to run tests on a given production scene
- Added ability to use [Inject], IInitializable, etc. from within custom user DLLs by referencing Zenject-Usage.dll
- Added ability to add user supplied validation logic by deriving from IValidatable
- Also added a way to set global zenject settings to control things like validation behaviour, error output, etc. through the ProjectContext inspector
- Renamed Zenject.Lazy class to Zenject.LazyInject to avoid name conflict with System.Lazy on .NET 4.6  (we cannot use System.Lazy directly because of issues with IL2CPP)
- Fixed to automatically inject StateMachineBehaviour derived classes attached to Animator components
- Changed the default value for includeInactive parameter to FromComponentX methods to be true, since this is very important when instantiating prefabs and therefore makes more sense as a default
- Fixed some issues related to binding open generic types
- Added documentation for ZenAutoInjecter to allow injection to occur when using GameObject.Instantiate
- Added debugging window to monitor all active memory pools (aka Memory Pool Monitor Window)
- Changed MonoMemoryPool to automatically revert to the original parents during despawn
- Changed to automatically add profiling information for ITickable.Tick, IInitializable.Initialize, IDisposable.Dispose methods, when inside the unity editor and when the unity profiler is open.  This now functions similar to MonoBehaviours in that these methods will automatically be listed in Profiler.
- Bind methods that involve a lookup now have a plural and non-plural versions (this includes FromComponentInChildren, FromComponentInParents, FromComponentSibling, FromComponentInNewPrefab, FromResource, FromResolveAll, and FromSubContainerResolveAll)
- Misc. bug fixes

Minor:
- Updated the sample projects to use more modern techniques
- Added missing bind methods to Container.Bind such as ByNewPrefabMethod, ByNewPrefabInstaller, ByNewPrefaResourceInstaller, and ByNewPrefabResourceMethod
- Added events on SceneContext, GameObjectContext, and ProjectContext to hook into post-install / pre-install events
- Standardized the naming of the bind methods for custom interfaces for factories and memory pools.  BindFactoryContract was renamed to BindFactoryCustomInterface and the overload of BindMemoryPool taking a custom interface was renamed BindMemoryPoolCustomInterface
- Added optional InjectSources argument for FromResolve and FromResolveGetter bind methods
- Changed ZenjectEditorWindow to handle failures better
- Added DisposeBlock class to make it easier to avoid unnecessary per frame memory allocations
- Added StaticMemoryPool class for cases where you want to store a pool statically instead of inside a container
- Added ability to combine Dispose pattern with PlaceholderFactory<> to pool objects without needing to explicitly create a MemoryPool class (see memory pool docs for details)
- Added methods to expand and shrink memory pools.  Also added optional OnDestroy user method to handle shrink operations
- Added ability to more easily pool facade/subcontainers
- Added Resize() method to Memory Pool classes
- Added FromComponentOnRoot bind method for game object contexts
- Added ZenjectStreams class for easier integration with UniRx
- Added FromMethodMultipleUntyped bind method
- Renamed ByNewPrefab to ByNewContextPrefab
- Added FromComponentOn/FromComponentsOn bind methods
- Added FromComponentOnRoot/FromComponentsOnRoot bind methods

## Version 5.5.1 (March 12, 2017)

Fixed compatibility issue with UniRx + the different scripting runtimes

## Version 5.5.0 (March 7, 2017)

Minor release, mostly bug fixes

Bug fixes
- Fixed some bugs with NonLazy bind method
- Changed validation to output only the errors relevant to the user
- Changed to automatically instantiate new game objects in the correct scene rather than the active scene
- Minor optimizations to memory allocations
- Fixed to have [inject] attribute inherited from abstract or virtual properties
- Fixed errors related to Unity 2018

New features
- Added ZenAutoInjector MonoBehaviour for cases where prefabs are instantiated from outside zenject
- Added LazyInject method for rare cases where you need to ensure something is injected before using it
- Added optional build define ZEN_STRIP_ASSERTS_IN_BUILDS as a trade off of error messages for speed

API changes
- Removed support for TickableManager.IsPaused

## Version 5.4.0 (October 2, 2017)

Big change to the way integration tests work, and some better error output.

- Completely changed how ZenjectIntegrationTestFixture works, to use Unity's support for playmode tests
- Improved validation error output.  Now outputs multiple errors at once, and also is more readable.
- Fixed signals that have value-type parameters to work properly with IL2CPP without a need for a reference wrapper class or anything
- Added back .NET 3.5 Moq dll as an alternative to the newer Moq dll

## Version 5.3.0 (September 18, 2017)

Some optimizations, bug fixes, and a few new bind methods

Notable
- Added optimizations to speed up startup time for scenes with many transforms.  In some cases it should be 5x faster to scan the initial scene hierarchy.
- Added optimizations to minimize memory allocations
- Added new bind methods FromNewComponentOnNewPrefab and FromNewComponentOnNewPrefabResource
- Added new bind methods for FromSubContainerResolve() bindings: ByNewPrefabMethod, ByNewPrefabResourceMethod, ByNewPrefabInstaller, and ByNewPrefabResourceInstaller
- Added support for having multiple "parent contract names" for the same scene
- Fixed rare exception that was occurring sometimes with circular dependencies
- Added better support for binding open generic types to open generic derived classes
- Bug fix - FromNewComponentOnNewGameObject was causing [Inject] methods to be executed after awake
- Added support for declaring collections of dependencies using IList<> instead of just List<>
- Bug fix - validation wasn't working when using FromResolveGetter with identifiers
- Changed to skip analyzing unity types (ie. those inside UnityEngine namespace) to minimize reflection costs
- Fixed AutoMocking to work by upgrading to newer Moq dll

Minor
- Added back Container.Install method as an alternative to FooInstaller.Install
- Added ability to lazily initialize GameObjectContext
- Added Position and Rotation to GameObjectCreationParameters
- Changed execution order extension methods to be proper methods to avoid namespace issues
- Added non-generic DeclareSignal overload
- Added BindFactoryContract and FromIFactoryResolve methods to make it easier to have custom factory interfaces
- Added support for custom memory pool interfaces
- Added UnbindInterfacesTo method
- Added support for loading asset bundles with FromComponentInNewPrefab instead of prefabs
- Added ability to pass 'late bindings' to next scene dynamically using  ZenjectSceneLoader
- Added support for injecting into C# 4.6 get-only properties
- Added more attributes to play more nicely with resharper
- Changed to disable support for profiling Zenject methods by default (need to define ZEN_PROFILING_ENABLED)

## Version 5.2.0 (April 30, 2017)

Minor release with just a few fixes.

- Fixed to compile again on WSA
- Fixed to be backwards compatible with Unity 5.5
- Removed MemoryPool DespawnAll() method because of its reliance on GetHashCode (issue #241)
- Added support for late install in decorator contexts
- Fixed to always trigger injection before the Awake event for MonoBehaviours attached to ProjectContext
- Added new bind method FromScriptableObjectResource which doesn't instantiate the scriptable object (issue #218)

## Version 5.1.0 (April 3, 2017)

Notable
- Fixes related to upgrading to Unity 5.6
- Moved Zenject folder to plugins directory
- Changed to trigger injection before Awake / OnEnable / Start for dynamically instantaited prefabs. This is nice because you can treat [Inject] methods more like constructors, and then use Awake/Start for initialization logic, for both dynamically created objects and objects in the scene at the start
- Marked the [Inject] attribute with MeansImplicitUseAttribute to improve integration with JetBrains Resharper
- Fixed bug that was happening when using ZenjectSceneLoader with LoadSceneMode.Single (it was destroying ProjectContext)
- Added support for declaring [Inject] methods with return type IEnumerator similar to Start
- Changed UnderTransform bind method overload to accept InjectContext instead of just DiContainer to be consistent with the other action overloads
- Added new bind method FromComponentOn that takes an Action<> instead of a specific game object
- Changed to just always include SignalManager in the project context since this is where it should always be declared anyway
- Changed to require that all signal parameters be reference types when on IL2CPP platforms.  See docs for why this is necessary.
- Added new signal bind method that gets both parameters and a handler class (so you can perform an operation on the parameters before forwarding to the handler for example)

Minor
- Fixed bug where some fields marked as InjectOptional were still producing errors
- Changed to allow doing Bind<Transform>().FromNewComponentOnNewGameObject()
- [Memory pools] Minor change to allow specifying an explicit interface for the memory pool itself
- [Memory pools] Fixed bug where validation was failing
- [Memory pools] Added Listen and Unlisten methods to ISignal
- Changed Pause/Resume methods on TickableManager to be inherited from parents
- Added an option to exclude self (current object) in FromComponentInChildren and FromComponentInParents
- Fixed minor issue when using FromSubContainerResolve with factories

## Version 5.0.2 (March 5, 2017)

- Fixed to allow parameterized tests using double parameters in ZenjectIntegrationTestFixtures
- Added another overload to BindMemoryPool to allow creating them directly without creating an empty subclass
- Changed memory pools to take an IFactory<> instead of a provider so that they can be instantiated directly by anyone that wants to do some custom stuff with it without needing to use BindMemoryPool
- Bug fix to validation for game object contexts
- Fixed script execution order to ensure that tickables, initializables, etc. are executed before MonoBehaviours in the scene (this is how it was in older versions)
- Changed to call IInitializable.Initialize immediately for GameObjextContext prefabs that are created dynamically. This is nice because otherwise, when you create a GameObjectContext via a factory, you can't use it immediately Unity waits until the end of the frame to call Start() to trigger Initialize
- Changed to use a runtime check inside profiler blocks to allow creating unit tests outside unity
- Fixed signals to validate properly
- Renamed FromScriptableObjectResource to FromNewScriptableObjectResource for consistency.
- Added a few missing factory bindings (FromComponentInHierarchy and FromNewScriptableObjectResource)
- Fixed signal installer bindings to work properly with AsTransient and multi-bindings

## Version 5.0.1 (February 15, 2017)

- Hotfix.  Signal UniRx integration was completely broken

## Version 5.0 (February 13, 2017)

Summary

Notable parts of this release includes the long awaited support for Memory Pools, a re-design of Commands/Signals, and support for late resolve via Lazy<> construct.  It also includes some API breaking changes to make it easier for new users.  Some of the bind methods were renamed to better represent what they mean, and in some cases the scope is now required to be made explicit, to avoid accidentally using transient scope.  Finally, there was also some significant performance improvements for when using Zenject in scenes with many transforms.

New Features
- Significant changes to commands and signals.  The functionality of commands was merged into Signals, and some more features were added to it to support subcontainers (see docs)
- Added Lazy<> construct so that you can have the resolve occur upon first usage
- Added menu option "Validate All Active Scenes"
- Added support for memory pools.  This includes a fluent interface similar to how factories work
- Added DiContainer.QueueForInject method to support adding pre-made instances to the initial inject list
- Added new construction methods
    - FromMethodMultiple
    - FromComponentInHierarchy
    - FromComponentSibling
    - FromComponentInParents
    - FromComponentInChildren 
    - FromScriptableObjectResource

Changes
- Updated sample projects to be easier to understand
- Improved error messages to include full type names
- Changed list bindings to default to optional so that you don't have to do this explicitly constantly
- Changed to require that the scope be explicitly set for some of the bind methods to avoid extremely common errors of accidentally leaving it as transient.  Bind methods that are more like "look ups" (eg. FromMethod, FromComponentInParents, etc.) have it as optional, however bind methods that create new instances require that it be set explicitly
- Renamed BindAllInterfaces to BindInterfacesTo and BindAllInterfacesAndSelf to BindInterfacesAndSelfTo to avoid the extremely common mistake of forgetting the To
- Removed support for passing arguments to InjectGameObject and InstantiatePrefab methods (issue #125)
- Removed UnityEventManager since it isn't core to keep things lightweight
- Renamed the Resolve overload that included an ID to ResolveId to avoid the ambiguity with the non generic version of Resolve
- Signals package received significant changes
    - The order of generic arguments to the Signal<> base class was changed to have parameters first to be consistent with everything else
    - The functionality of commands was merged into signals
- Renamed the following construction methods.  This was motivated by the fact that with the new construction methods it's unclear which ones are "look ups" versus creating new instances
    - FromComponent => FromNewComponentOn
    - FromSiblingComponent => FromNewComponentSibling 
    - FromGameObject => FromNewComponentOnNewGameObject 
    - FromPrefab => FromComponentInNewPrefab
    - FromPrefabResource => FromComponentInNewPrefabResource
    - FromSubContainerResolve.ByPrefab => FromSubContainerResolve.ByNewPrefab

Bug fixes
- (optimization) Fixed major performance issue for scenes that have a lot of transforms Re issue #188.
- (optimization) Fixed to avoid the extra performance costs of calling SetParent by directly passing the parent to the GameObject.Instantiate method issue #188
- Fixed extremely rare bug that would cause an infinite loop when using complex subcontainer setups
- Fixed to work with nunit test case attributes
- Fixed to instantiate prefabs without always changing them to be active
- Fixed WithArguments bind method to support passing null values
- Fixed context menu to work properly when creating installers etc. issue #200
- Fixed issue with ZenUtilInternal.InjectGameObjectForComponent method to support looking up non-monobehaviours.
- Fixed NonLazy() bind method to work properly wtihin sub containers

---------

## Version 4.7 (November 6, 2016)
- Removed the concept of triggers in favour of just directly acting on the Signal to both subscribe and fire, since using Trigger was too much overhead for not enough gain
- Fixed issue for Windows Store platform where zenject was not properly stripping out the WSA generated constructors
- Changed to automatically choose the public constructor if faced with a choice between public and private
- Fix to IL2CPP builds to work again
- Added support for using the WithArguments bind method combined with FromFactory
- Improved validation of multi-scene setups using Contract Names to output better error messages

---------

## Version 4.6 (October 23, 2016)
- Changed Validation to run at edit time rather than requiring that we enter play mode.  This is significantly faster.  Also added a hotkey to "validate then run" since it's fast enough to use as a pre-run check
- Added InstantiateComponentOnNewGameObject method
- Changed to install ScriptableObjectInstallers before MonoInstallers since it is common to include settings in ScriptableObjectInstallers (including settings for MonoInstallers)
- Added new option to ZenjectBinding BindType parameter to bind from the base class
- Changed to allow specifying singleton identifiers as object rather than just string
- Added design-time support to Scene Parenting by using Contract Names (see docs for details)
- Changed Scene Decorators to use Contract Names as well (see docs for details)
- Fixed to ensure that the order that initial instances on the container are injected in follows their dependency order #161
- Added LoadSceneAsync method to ZenjectSceneLoader class.  Also removed the option to pass in postBindings since nobody uses this and it's kind of bad practice anyway.  Also renamed LoadSceneContainerMode to LoadSceneRelationship
- Added AutoRun field on SceneContext for cases where you want to start it manually
- Removed the IBinder and IResolver interfaces since they weren't really used and were a maintenance headache
- Renamed WithGameObjectGroup to UnderTransformGroupX and also added UnderTransform method
- Added helper classes to make writing integration tests or unit tests with Unity's EditorTestRunner easier
- Added documentation on ZenjectEditorWindow, Unit Testing, and Integration Testing
- Misc. bug fixes

---------

## Version 4.5 (September 1, 2016)
- Fixed DiContainer.ResolveTypeAll() method to properly search in parent containers
- Fixed exception that was occuring with Factories when using derived parameter types
- Fixed FromResolve to properly search in parent containers
- Fixed exception that was occuring with FromMethod when using derived parameter types

---------

## Version 4.4 (July 23, 2016)
- Changed the way installers are called from other installers, to allow strongly typed parameter passing
- Added untyped version of FromMethod
- Added FromSiblingComponent bind method
- Added non-generic FromFactory bind method
- Minor bug fix to command binding to work with InheritInSubcontainers() method
- Bug fix - NonLazy method was not working properly when used with ByInstaller or ByMethod

---------

## Version 4.3 (June 4, 2016)
- Changed to disallow using null with BindInstance by default, to catch these errors earlier
- Changed to use UnityEngine.Object when referring to prefabs to allow people to get some stronger type checking of prefabs at edit time
- (bug fix) for Hololens with Unity 5.4
- (bug fix) Scene decorator property was not being serialized correctly
- (bug fix) Custom factories were not validating in some cases

---------

## Version 4.2 (May 30, 2016)
- Finally updated the documentation
- Renamed FromGetter to FromGetterResolve
- Added some optimizations to convention binding
- Renamed InstallPrefab to InstallPrefabResource
- (bug) Fixed PrefabFactory to work with abstract types
- (bug) Fixed some bugs related to convention binding
- (bug) Fixed bug with Unity 5.3.5 where the list of installers would not serialize properly
- (but) Fixed minor bug with validation

---------

## Version 4.1 (May 15, 2016)
- Changed ResolveAll method to be optional by default, so it can return the empty list
- Removed Zenject.Commands namespace in favour of just Zenject
- Added convention based binding (eg. Container.Bind().To(x => x.AllTypes().DerivingFrom()))
- Fixed GameObjectCompositionRoot to expose an optional facade property in its inspector
- Renamed CompositionRoot to Context.
- Changed to just re-use the InjectAttribute instead of PostInjectAttribute
- Better support for making custom Unity EditorWindow implementations that use Zenject
- Added right click Menu items in project pane to create templates of common zenject C# files
- Renamed TestWithContainer to ZenjectUnitTestFixture
- Added simple test framework for both unit tests and integration tests
- Changed Identifier to be type object so it can be used with enums (or other types)
- Added InjectLocal attribute
- Changed to guarantee that any component that is injected into another component has itself been injected already
- Fixed an issue where calling Resolve<> or Instantiate<> inside an installer would cause objects to be injected twice
- Fixes to WSA platform
- Changed to automatically call ScriptableObject.CreateInstance when creating types that derive from ScriptableObject
- Fix to non-unity build

---------

## Version 4.0 (April 30, 2016)
- Added another property to CompositionRoot to specify installers as prefabs re #96
- Changed global composition root to be a prefab instead of assembling together a bunch of ScriptableObject assets re #98
- Changed to lookup Zenject Auto Binding components by default, without the need for AutoBindInstaller. Also added new properties such as CompositionRoot, identifier, and made Component a list. Also works now when put underneath GameObjectCompositionRoot's.
- Added ability to pass in multiple types to the Bind() method. This opens up a lot of possibilities including convention-based binding. This also deprecated the use of BindAllInterfacesToSingle in favour of just BindAllInterfaces<>
- Added "WithArguments" bind method, to allow passing arguments directly to the type instead of always using WhenInjectedInto
- Added concept of EditorWindowCompositionRoot to make it easier to use Zenject with editor plugins
- Added "InheritInSubContainers" bind method, to allow having bindings automatically forwarded to sub containers
- Removed the different Exception classes in favour of just one (ZenjectException)
- Added 'AsCached' method as an alternative to 'AsSingle' and 'AsTransient'. AsCached will function like AsTransient except it will only create the object once and thereafter return that value
- Changed some methods that previously used 'params' to explicitly take a list, to avoid common errors
- Cleaned up InjectContext to be easier to work with
- Made significant change to how Factories work. Now there is just one definitive Factory class, and you can change how that factory constructs the object in your installers
- Changed the fluent interface to specify whether the binding is single or transient as a separate method, to avoid the explosion of ToSinglePrefab, ToTransientPrefab, etc. (now it's just ToPrefab)
- Renamed GlobalCompositionRoot to ProjectCompositionRoot and FacadeCompositionRoot to GameObjectCompositionRoot.
- Added more intuitive bindings for creating subcontainers. eg: Container.Bind().ToSubContainerResolve().ByPrefab()
- Added WithGameObjectName and WithGroupName bind methods to prefab or game object related bindings
- Made another big chagne to the fluent interface to avoid having duplicate methods for with Self and Concrete. Now you choose between Container.Bind().ToSelf() and Container.Bind().To(). ToSelf is assumed if unspecified
- Changed Triggers to directly expose the signal event so they can be used as if they are signals
- Added concept of ScriptableObjectInstaller - especially useful for use with settings
- Added ZenjectSceneLoader class to allow additively loading other scenes as children or siblings of existing scene
- Changed scene decorators to work more intuitively with the multi-scene editting features of Unity 5.3+. You can now drag in multiple scenes together, and as long as you use DecoratorCompositionRoot in scenes above a main scene, they will be loaded together.
- Removed IncludeInactive flag. Now always injects into inactive game objects. This was kinda necessary because validation needs to control the active flag
- Removed the concept of one single DependencyRoot in favour of having any number of them, using binding with identifier. Also added NonLazy() bind method to make this very easy
- Added new attribute ZenjectAllowDuringValidation for use with installer setting objects that you need during validation
- Changed validation to occur at runtime to be more robust and less hacky. Now works by adding dummy values to mark which dependencies have successfully been found
- Renamed BindPriority to BindExecutionOrder
- Removed support for binary version of Zenject. This was necessary since Zenject now needs to use some unity defines (eg. UNITY_EDITOR) which doesn't work in DLLs

---------

## Version 3.11 (May 15, 2016)
- Bug fix - Calling Resolve<> or Instantiate<> inside an installer was causing the object to be injected twice
- Added StaticCompositionRoot as an even higher level container than ProjectCompositionRoot, for cases where you want to add dependencies directly to the Zenject assembly before Unity even starts up
- Bug fix - loading the same scene multiple times with LoadSceneAdditive was not working
- Fixed compiler errors with Unity 5.4

---------

## Version 3.10 (March 26, 2016)
- Fixed to actually support Windows Store platform
- Added pause/resume methods to TickableManager
- Bug fix - OnlyInjectWhenActive flag did not work on root inactive game objects 

---------

## Version 3.9 (Feb 7, 2016)
- Added a lot more error checking when using the ToSingle bindings. It will no longer allow mixing different ToSingle types
- Fixed ToSingleGameObject and ToSingleMonoBehaviour to allow multiple bindings to the same result
- Made it easier to construct SceneCompositionRoot objects dynamically
- Added untyped versions of BindIFactory.ToFactory method
- Removed the ability warn on missing ITickable/IInitializable bindings
- Added a bunch of integration tests
- Reorganized folder structure

---------

## Version 3.8 (Feb 4, 2016)
- Changed back to only initializing the ProjectCompositionRoot when starting a scene with a SceneCompositionRoot rather than always starting it in every scene

---------

## Version 3.7 (Jan 31, 2016)
- Changed to not bother parenting transforms to the CompositionRoot object by default (This is still optional with a checkbox however)
- Added string parameter to BindMonoBehaviourFactory method to allow specifying the name of an empty GameObject to use for organization
- Changed FacadeFactory to inherit from IFactory
- Changed ProjectCompositionRoot to initialize using Unity's new [RuntimeInitializeOnLoadMethod] attribute
- Added easier ability to validate specific scenes from the command line outside of Unity
- Added AutoBindInstaller class and ZenjectBinding attribute to make it easier to add MonoBehaviours that start in the scene to the container
- Added optional parameter to the [Inject] attribute to specify which container to retrieve from in the case of nested containers
- Fixed some unity-specific bind commands to play more nicely with interfaces

---------

## Version 3.6 (Jan 24, 2016)
- Another change to signals to not require parameter types to the bind methods

---------

## Version 3.5 (Jan 17, 2016)
- Made breaking change to require separate bind commands for signals and triggers, to allow adding different conditionals on each.

---------

## Version 3.4 (Jan 7, 2016)
- Cleaned up directory structure
- Fixed bug with Global bindings not getting their Tick() called in the correct order
- Fixes to the releases automation scripts

---------

## Version 3.2 (December 20, 2015)
- Added the concept of "Commands" and "Signals".  See documentation for details.
- Fixed validation for decorator scenes that open decorator scenes.
- Changed to be more strict when using a combination of differents kinds of ToSingle<>, since there should only be one way to create the singleton.
- Added ToSingleFactory bind method, for cases where you have complex construction requirements and don't want to use ToSingleMethod
- Removed the InjectFullScene flag on SceneCompositionRoot.  Now always injects on the full scene.
- Renamed AllowNullBindings to IsValidating so it can be used for other kinds of validation-only logic
- Renamed BinderUntyped to UntypedBinder and BinderGeneric to GenericBinder
- Added the ability to install MonoInstaller's directly from inside other installers by calling Container.Install<MyCustomMonoInstaller>().  In this case it tries to load a prefab from Resources/Installers/MyCustomMonoInstaller.prefab before giving up.  This can be helpful to keep scenes incredibly small instead of having many installer prefabs.
- Added the ability to install MonoInstaller's directly from inside other installers.  In this case it tries to load a prefab from the resources directory before giving up.
- Added some better error output in a few places
- Fixed some iOS AOT issues
- Added BindFacade<> method to DiContainer, to allow creating nested containers without needing to use a factory.
- Added an Open button in scene decorator comp root for easily jumping to the decorated scene
- Removed support for object graph visualization since I hadn't bothered maintaining it
- Got the optional Moq extension method ToMock() working again
- Fixed scene decorators to play more nicely with Unity's own way of handling LoadLevelAdditive.  Decorated scenes are now organized in the scene heirarchy under scene headings just like when calling LoadLevelAdditive normally

---------

## Version 3.1
- Changes related to upgrading to Unity 5.3
- Fixed again to make zero heap allocations per frame

---------

## Version 3.0
- Added much better support for nested containers.  It now works more closely to what you might expect:  Any parent dependencies are always inherited in sub-containers, even for optional injectables.  Also removed BindScope and FallbackContainer since these were really just workarounds for this feature missing.  Also added [InjectLocal] attribute for cases where you want to inject dependencies only from the local container.
- Changed the way execution order is specified in the installers.  Now the order for Initialize / Tick / Dispose are all given by one property similar to how unity does it, using ExecutionOrderInstaller
- Added ability to pass arguments to Container.Install<>
- Added support for using Facade pattern in combination with nested containers to allow easily created distinct 'islands' of dependencies.  See documentation for details
- Changed validation to be executed on DiContainer instead of through BindingValidator for ease of use
- Added automatic support for WebGL by marking constructors as [Inject]

---------

## Version 2.8
* Fixed to properly use explicit default parameter values in Constructor/PostInject methods.  For eg: public Foo(int bar = 5) should consider bar to be optional and use 5 if not resolved.

---------

## Version 2.7
* Bug fix to ensure global composition root always gets initialized before the scene composition root
* Changed scene decorators to use LoadLevelAdditive instead of LoadLevel to allow more complex setups involving potentially several decorators within decorators

---------

## Version 2.6
* Added new bind methods: ToResource, ToTransientPrefabResource, ToSinglePrefabResource
* Added ability to have multiple sets of global installers
* Fixed support for using zenject with .NET 4.5
* Created abstract base class CompositionRoot for both SceneCompositionRoot and ProjectCompositionRoot
* Better support for using the same DiContainer from multiple threads
* Added back custom list inspector handler to make it easier to re-arrange etc.
* Removed the extension methods on DiContainer to avoid a gotcha that occurs when not including 'using Zenject
* Changed to allow having a null root transform given to DiContainer
* Changed to assume any parameters with hard coded default values (eg: int x = 5) are InjectOptional
* Fixed bug with asteroids project which was causing exceptions to be thrown on the second run due to the use of tags

---------

## Version 2.5
* Added support for circular dependencies in the PostInject method or as fields (just not constructor parameters)
* Fixed issue with identifiers that was occurring when having both [Inject] and [InjectOptional] attributes on a field/constructor parameter.  Now requires that only one be set
* Removed BindValue in favour of just using Bind for both reference and value types for simplicity
* Removed GameObjectInstantiator class since it was pretty awkward and confusing.  Moved methods directly into IInstantiator/DiContainer.  See IInstantiator class.
* Extracted IResolver and IBinder interfaces from DiContainer

---------

## Version 2.4
* Refactored the way IFactory is used to be a lot cleaner. It now uses a kind of fluent syntax through its own bind method BindIFactory<>

---------

## Version 2.3
* Added "ParentContexts" property to InjectContext, to allow very complex conditional bindings that involve potentially several identifiers, etc.
* Removed InjectionHelper class and moved methods into DiContainer to simplify API and also to be more discoverable
* Added ability to build dlls for use in outside unity from the assembly build solution

---------

## Version 2.2
* Changed the way installers invoke other installers.  Previously you would Bind them to IInstaller and now you call Container.Install<MyInstaller> instead.  This is better because it allows you to immediately call Rebind<> afterwards

---------

## Version 2.1
* Simplified interface a bit more by moving more methods into DiContainer such as Inject and Instantiate.  Moved all helper methods into extension methods for readability. Deleted FieldsInjector and Instantiator classes as part of this
* Renamed DiContainer.To() method to ToInstance since I had witnessed some confusion with it for new users.  Did the same with ToSingleInstance
* Added support for using Zenject outside of Unity by building with the ZEN_NOT_UNITY3D define set
* Bug fix - Validation was not working in some cases for prefabs.
* Renamed some of the parameters in InjectContext for better understandability.
* Renamed DiContainer.ResolveMany to DiContainer.ResolveAll
* Added 'InjectFullScene' flag to CompositionRoot to allow injecting across the entire unity scene instead of just objects underneath the CompositionRoot

---------

## Version 2.0
* Added ability to inject dependencies via parameters to the [PostInject] method just like it does with constructors.  Especially useful for MonoBehaviours.
* Fixed the order that [PostInject] methods are called in for prefabs
* Changed singletons created via ToSinglePrefab to identify based on identifier and prefab and not component type. This allows things like ToSingle<Foo>(prefab1) and ToSingle<Bar>(prefab1) to use the same prefab, so you can map singletons to multiple components on the same prefab. This also works with interfaces.
* Removed '.As()' method in favour of specifying the identifier in the first Bind() statement
* Changed identifiers to be strings instead of object to avoid accidental usage
* Renamed ToSingle(obj) to ToSingleInstance to avoid conflict with specifying an identifier
* Fixed validation to work properly for ToSinglePrefab
* Changed to allow using conditions to override a default binding. When multiple providers are found it will now try and use the one with conditions.  So for example you can define a default with `Container.Bind<IFoo>().ToSingle<Foo1>()` and then override for specific classes with `Container.Bind<IFoo>().ToSingle<Foo2>().WhenInjectedInto<Bar>()`, etc.

---------

## Version 1.19

* Upgraded to Unity 5
* Added an optional identifier to InjectOptional attribute
* Changed the way priorities are interpreted for tickables, disposables, etc. Zero is now used as default for any unspecified priorities.  This is helpful because it allows you to choose priorities that occur either before or after the unspecified priorities.
* Added some helper methods to ZenEditorUtil for use by CI servers to validate all scenes

---------

## Version 1.18

* Added minor optimizations to reduce per-frame allocation to zero
* Fixed unit tests to be compatible with unity test tools
* Minor bug fix with scene decorators, GameObjectInstantiator.

---------

## Version 1.17

* Bug fix.  Was not forwarding parameters correctly when instantiating objects from prefabs

---------

## Version 1.16

* Removed the word 'ModestTree' from namespaces since Zenject is open source and not proprietary to the company ModestTree.

---------

## Version 1.15

* Fixed bug with ToSinglePrefab which was causing it to create multiple instances when used in different bindings.

---------

## Version 1.14

* Added flag to CompositionRoot for whether to inject into inactive game objects or ignore them completely
* Added BindAllInterfacesToSingle method to DiContainer
* Changed to call PostInject[] on children first when instantiating from prefab
* Added ILateTickable interface, which works just like ITickable or IFixedTickable for unity's LateUpdate event
* Added support for 'decorators', which can be used to add dependencies to another scene

---------

## Version 1.13

* Minor bug fix to global composition root.  Also fixed a few compiler warnings.

---------

## Version 1.12

* Added Rebind<> method
* Changed Factories to use strongly typed parameters by default.  Also added ability to pass in null values as arguments as well as multiple instances of the same type
* Renamed _container to Container in the installers
* Added support for Global Composition Root to allow project-wide installers/bindings
* Added DiContainer.ToSingleMonoBehaviour method
* Changed to always include the StandardUnityInstaller in the CompositionRoot class.
* Changed TickableManager to not be a monobehaviour and receive its update from the UnityDependencyRoot instead
* Added IFixedTickable class to support unity FixedUpdate method

---------

## Version 1.11

* Removed Fasterflect library to keep Zenject nice and lightweight (it was also causing issues on WP8)
* Fixed bug related to singletons + object graph validation. Changed the way IDisposables are handled to be closer to the way IInitializable and ITickable are handled. Added method to BinderUntyped.

---------

## Version 1.10

* Added custom editor for the Installers property of CompositionRoot to make re-ordering easier

---------

## Version 1.09

* Added support for nested containers
* Added ability to execute bind commands using Type objects rather than a generic type
* Changed the way IDisposable bindings work to be similar to how ITickable and IInitializable work
* Bug fixes

---------

## Version 1.08

* Order of magnitude speed improvement by using more caching
* Minor change to API to use the As() method to specify identifiers
* Bug fixes

---------

## Version 1.07

* Simplified API by removing the concept of modules in favour of just having installers instead (and add support for installers installing other installers)
* Bug fixes

---------

## Version 1.06

* Introduced concept of scene installer, renamed installers 'modules'
* Bug fixes
