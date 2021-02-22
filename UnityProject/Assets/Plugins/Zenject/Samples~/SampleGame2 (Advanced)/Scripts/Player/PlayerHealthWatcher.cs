using System;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    public class PlayerHealthWatcher : ITickable
    {
        readonly SignalBus _signalBus;
        readonly AudioPlayer _audioPlayer;
        readonly Settings _settings;
        readonly Explosion.Factory _explosionFactory;
        readonly Player _player;

        public PlayerHealthWatcher(
            Player player,
            Explosion.Factory explosionFactory,
            Settings settings,
            AudioPlayer audioPlayer,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _audioPlayer = audioPlayer;
            _settings = settings;
            _explosionFactory = explosionFactory;
            _player = player;
        }

        public void Tick()
        {
            if (_player.Health <= 0 && !_player.IsDead)
            {
                Die();
            }
        }

        void Die()
        {
            _player.IsDead = true;

            var explosion = _explosionFactory.Create();
            explosion.transform.position = _player.Position;

            _player.Renderer.enabled = false;

            _signalBus.Fire<PlayerDiedSignal>();

            _audioPlayer.Play(_settings.DeathSound, _settings.DeathSoundVolume);
        }

        [Serializable]
        public class Settings
        {
            public AudioClip DeathSound;
            public float DeathSoundVolume = 1.0f;
        }
    }
}
