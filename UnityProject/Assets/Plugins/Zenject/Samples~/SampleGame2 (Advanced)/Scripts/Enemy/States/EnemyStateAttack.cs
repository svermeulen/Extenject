using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zenject.SpaceFighter
{
    public class EnemyStateAttack : IEnemyState
    {
        readonly EnemyRotationHandler _rotationHandler;
        readonly EnemyCommonSettings _commonSettings;
        readonly AudioPlayer _audioPlayer;
        readonly EnemyTunables _tunables;
        readonly EnemyStateManager _stateManager;
        readonly PlayerFacade _player;
        readonly Settings _settings;
        readonly EnemyView _view;
        readonly Bullet.Factory _bulletFactory;

        float _lastShootTime;
        bool _strafeRight;
        float _lastStrafeChangeTime;

        public EnemyStateAttack(
            Bullet.Factory bulletFactory,
            EnemyView view,
            Settings settings,
            PlayerFacade player,
            EnemyStateManager stateManager,
            EnemyTunables tunables,
            AudioPlayer audioPlayer,
            EnemyCommonSettings commonSettings,
            EnemyRotationHandler rotationHandler)
        {
            _rotationHandler = rotationHandler;
            _commonSettings = commonSettings;
            _audioPlayer = audioPlayer;
            _tunables = tunables;
            _stateManager = stateManager;
            _player = player;
            _settings = settings;
            _view = view;
            _bulletFactory = bulletFactory;
            _strafeRight = Random.Range(0.0f, 1.0f) < 0.5f;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void Update()
        {
            if (_player.IsDead)
            {
                _stateManager.ChangeState(EnemyStates.Idle);
                return;
            }

            _rotationHandler.DesiredLookDir = (_player.Position - _view.Position).normalized;

            // Strafe back and forth over the given interval
            if (Time.realtimeSinceStartup - _lastStrafeChangeTime > _settings.StrafeChangeInterval)
            {
                _lastStrafeChangeTime = Time.realtimeSinceStartup;
                _strafeRight = !_strafeRight;
            }

            // Shoot every X seconds
            if (Time.realtimeSinceStartup - _lastShootTime > _settings.ShootInterval)
            {
                _lastShootTime = Time.realtimeSinceStartup;
                Fire();
            }

            // If the player runs away then chase them
            if ((_player.Position - _view.Position).magnitude > _commonSettings.AttackDistance + _settings.AttackRangeBuffer)
            {
                _stateManager.ChangeState(EnemyStates.Follow);
            }
        }

        public void FixedUpdate()
        {
            // Strafe to avoid getting hit too easily
            if (_strafeRight)
            {
                _view.AddForce(_view.RightDir * _settings.StrafeMultiplier * _tunables.Speed);
            }
            else
            {
                _view.AddForce(-_view.RightDir * _settings.StrafeMultiplier * _tunables.Speed);
            }
        }

        void Fire()
        {
            var bullet = _bulletFactory.Create(
                _settings.BulletSpeed, _settings.BulletLifetime, BulletTypes.FromEnemy);

            // Randomize our aim a bit
            var accuracy = Mathf.Clamp(_tunables.Accuracy, 0, 1);
            var maxError = 1.0f - accuracy;
            var error = Random.Range(0, maxError);

            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                error *= -1;
            }

            var thetaError = error * _settings.ErrorRangeTheta;

            bullet.transform.position = _view.Position + _view.LookDir * _settings.BulletOffsetDistance;
            bullet.transform.rotation = Quaternion.AngleAxis(thetaError, Vector3.forward) * _view.Rotation;

            _audioPlayer.Play(_settings.ShootSound, _settings.ShootSoundVolume);
        }

        [Serializable]
        public class Settings
        {
            public AudioClip ShootSound;
            public float ShootSoundVolume = 1.0f;

            public float BulletLifetime;
            public float BulletSpeed;
            public float BulletOffsetDistance;
            public float ShootInterval;
            public float ErrorRangeTheta;
            public float AttackRangeBuffer;
            public float StrafeMultiplier;
            public float StrafeChangeInterval;
        }
    }
}
