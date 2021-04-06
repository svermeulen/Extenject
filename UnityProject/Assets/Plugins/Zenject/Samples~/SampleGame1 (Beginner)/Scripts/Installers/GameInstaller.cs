using System;
using UnityEngine;

namespace Zenject.Asteroids
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            // In this example there is only one 'installer' but in larger projects you
            // will likely end up with many different re-usable installers
            // that you'll want to use in several different scenes
            //
            // There are several ways to do this.  You can store your installer as a prefab,
            // a scriptable object, a component within the scene, etc.  Or, if you don't
            // need your installer to be a MonoBehaviour then you can just simply call
            // Container.Install
            //
            // See here for more details:
            // https://github.com/modesttree/zenject#installers
            //
            //Container.Install<MyOtherInstaller>();

            // Install the main game
            InstallAsteroids();
            InstallShip();
            InstallMisc();
            InstallSignals();
            InstallExecutionOrder();
        }

        void InstallAsteroids()
        {
            // ITickable, IFixedTickable, IInitializable and IDisposable are special Zenject interfaces.
            // Binding a class to any of these interfaces creates an instance of the class at startup.
            // Binding to any of these interfaces is also necessary to have the method defined in that interface be
            // called on the implementing class as follows:
            // Binding to ITickable or IFixedTickable will result in Tick() or FixedTick() being called like Update() or FixedUpdate().
            // Binding to IInitializable means that Initialize() will be called on startup during Unity's Start event.
            // Binding to IDisposable means that Dispose() will be called when the app closes or the scene changes

            // Any time you use To<Foo>().AsSingle, what that means is that the DiContainer will only ever instantiate
            // one instance of the type given inside the To<> (in this example, Foo). So in this case, any classes that take ITickable,
            // IFixedTickable, or AsteroidManager as inputs will receive the same instance of AsteroidManager.
            // We create multiple bindings for ITickable, so any dependencies that reference this type must be lists of ITickable.
            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();

            // Note that the above binding is equivalent to the following:
            //Container.Bind(typeof(ITickable), typeof(IFixedTickable), typeof(AsteroidManager)).To<AsteroidManager>.AsSingle();

            // Here, we're defining a generic factory to create asteroid objects using the given prefab
            // So any classes that want to create new asteroid objects can simply include an injected field
            // or constructor parameter of type Asteroid.Factory, then call Create() on that
            Container.BindFactory<Asteroid, Asteroid.Factory>()
                // This means that any time Asteroid.Factory.Create is called, it will instantiate
                // this prefab and then search it for the Asteroid component
                .FromComponentInNewPrefab(_settings.AsteroidPrefab)
                // We can also tell Zenject what to name the new gameobject here
                .WithGameObjectName("Asteroid")
                // GameObjectGroup's are just game objects used for organization
                // This is nice so that it doesn't clutter up our scene hierarchy
                .UnderTransformGroup("Asteroids");
        }

        void InstallMisc()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.Bind<LevelHelper>().AsSingle();

            Container.BindInterfacesTo<AudioHandler>().AsSingle();

            // FromComponentInNewPrefab matches the first transform only just like GetComponentsInChildren
            // So can be useful in cases where we don't need a custom MonoBehaviour attached
            Container.BindFactory<Transform, ExplosionFactory>()
                .FromComponentInNewPrefab(_settings.ExplosionPrefab);

            Container.BindFactory<Transform, BrokenShipFactory>()
                .FromComponentInNewPrefab(_settings.BrokenShipPrefab);
        }

        void InstallSignals()
        {
            // Every scene that uses signals needs to install the built-in installer SignalBusInstaller
            // Or alternatively it can be installed at the project context level (see docs for details)
            SignalBusInstaller.Install(Container);

            // Signals can be useful for game-wide events that could have many interested parties
            Container.DeclareSignal<ShipCrashedSignal>();
        }

        void InstallShip()
        {
            Container.Bind<ShipStateFactory>().AsSingle();

            // Note that the ship itself is bound using a ZenjectBinding component (see Ship
            // game object in scene heirarchy)

            Container.BindFactory<ShipStateWaitingToStart, ShipStateWaitingToStart.Factory>().WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateDead, ShipStateDead.Factory>().WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateMoving, ShipStateMoving.Factory>().WhenInjectedInto<ShipStateFactory>();
        }

        void InstallExecutionOrder()
        {
            // In many cases you don't need to worry about execution order,
            // however sometimes it can be important
            // If for example we wanted to ensure that AsteroidManager.Initialize
            // always gets called before GameController.Initialize (and similarly for Tick)
            // Then we could do the following:
            Container.BindExecutionOrder<AsteroidManager>(-20);
            Container.BindExecutionOrder<GameController>(-10);

            // Note that they will be disposed of in the reverse order given here
        }

        [Serializable]
        public class Settings
        {
            public GameObject ExplosionPrefab;
            public GameObject BrokenShipPrefab;
            public GameObject AsteroidPrefab;
            public GameObject ShipPrefab;
        }
    }
}

