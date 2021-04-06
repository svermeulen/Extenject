using System;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    public class EnemyDeathHandler
    {
        readonly EnemyFacade _facade;
        readonly SignalBus _signalBus;
        readonly Settings _settings;
        readonly Explosion.Factory _explosionFactory;
        readonly AudioPlayer _audioPlayer;
        readonly EnemyView _view;

        public EnemyDeathHandler(
            EnemyView view,
            AudioPlayer audioPlayer,
            Explosion.Factory explosionFactory,
            Settings settings,
            SignalBus signalBus,
            EnemyFacade facade)
        {
            _facade = facade;
            _signalBus = signalBus;
            _settings = settings;
            _explosionFactory = explosionFactory;
            _audioPlayer = audioPlayer;
            _view = view;
        }

        public void Die()
        {
            var explosion = _explosionFactory.Create();
            explosion.transform.position = _view.Position;

            _audioPlayer.Play(_settings.DeathSound, _settings.DeathSoundVolume);

            _signalBus.Fire<EnemyKilledSignal>();

            _facade.Dispose();
        }

        [Serializable]
        public class Settings
        {
            public AudioClip DeathSound;
            public float DeathSoundVolume = 1.0f;
        }
    }
}
