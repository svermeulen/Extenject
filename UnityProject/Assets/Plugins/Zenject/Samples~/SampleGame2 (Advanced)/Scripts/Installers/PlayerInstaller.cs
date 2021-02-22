using System;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings _settings = null;

        public override void InstallBindings()
        {
            Container.Bind<Player>().AsSingle()
                .WithArguments(_settings.Rigidbody, _settings.MeshRenderer);

            Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerDamageHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerDirectionHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerShootHandler>().AsSingle();

            Container.Bind<PlayerInputState>().AsSingle();

            Container.BindInterfacesTo<PlayerHealthWatcher>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public Rigidbody Rigidbody;
            public MeshRenderer MeshRenderer;
        }
    }
}
