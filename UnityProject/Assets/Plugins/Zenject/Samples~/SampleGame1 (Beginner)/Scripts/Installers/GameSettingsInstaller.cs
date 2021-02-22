using System;

namespace Zenject.Asteroids
{
    // We prefer to use ScriptableObjectInstaller for installers that contain game settings
    // There's no reason why you couldn't use a MonoInstaller here instead, however
    // using ScriptableObjectInstaller has advantages here that make it nice for settings:
    //
    // 1) You can change these values at runtime and have those changes persist across play
    //    sessions.  If it was a MonoInstaller then any changes would be lost when you hit stop
    // 2) You can easily create multiple ScriptableObject instances of this installer to test
    //    different customizations to settings.  For example, you might have different instances
    //    for each difficulty mode of your game, such as "Easy", "Hard", etc.
    // 3) If your settings are associated with a game object composition root, then using
    //    ScriptableObjectInstaller can be easier since there will only ever be one definitive
    //    instance for each setting.  Otherwise, you'd have to change the settings for each game
    //    object composition root separately at runtime
    //
    // Uncomment if you want to add alternative game settings
    //[CreateAssetMenu(menuName = "Asteroids/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public ShipSettings Ship;
        public AsteroidSettings Asteroid;
        public AudioHandler.Settings AudioHandler;
        public GameInstaller.Settings GameInstaller;

        // We use nested classes here to group related settings together
        [Serializable]
        public class ShipSettings
        {
            public ShipStateMoving.Settings StateMoving;
            public ShipStateDead.Settings StateDead;
            public ShipStateWaitingToStart.Settings StateStarting;
        }

        [Serializable]
        public class AsteroidSettings
        {
            public AsteroidManager.Settings Spawner;
            public Asteroid.Settings General;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(Ship.StateMoving);
            Container.BindInstance(Ship.StateDead);
            Container.BindInstance(Ship.StateStarting);
            Container.BindInstance(Asteroid.Spawner);
            Container.BindInstance(Asteroid.General);
            Container.BindInstance(AudioHandler);
            Container.BindInstance(GameInstaller);
        }
    }
}

