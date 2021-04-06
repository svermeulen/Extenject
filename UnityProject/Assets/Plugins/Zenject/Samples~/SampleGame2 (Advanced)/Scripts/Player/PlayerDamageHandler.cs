using System;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    public class PlayerDamageHandler
    {
        readonly AudioPlayer _audioPlayer;
        readonly Settings _settings;
        readonly Player _player;

        public PlayerDamageHandler(
            Player player,
            Settings settings,
            AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            _settings = settings;
            _player = player;
        }

        public void TakeDamage(Vector3 moveDirection)
        {
            _audioPlayer.Play(_settings.HitSound, _settings.HitSoundVolume);

            _player.AddForce(-moveDirection * _settings.HitForce);

            _player.TakeDamage(_settings.HealthLoss);
        }

        [Serializable]
        public class Settings
        {
            public float HealthLoss;
            public float HitForce;

            public AudioClip HitSound;
            public float HitSoundVolume = 1.0f;
        }
    }
}
