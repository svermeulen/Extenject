using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zenject.SpaceFighter
{
    public class EnemyStateFollow : IEnemyState
    {
        readonly EnemyRotationHandler _rotationHandler;
        readonly EnemyCommonSettings _commonSettings;
        readonly Settings _settings;
        readonly EnemyTunables _tunables;
        readonly EnemyStateManager _stateManager;
        readonly EnemyView _view;
        readonly PlayerFacade _player;

        bool _strafeRight;
        float _lastStrafeChangeTime;

        public EnemyStateFollow(
            PlayerFacade player,
            EnemyView view,
            EnemyStateManager stateManager,
            EnemyTunables tunables,
            Settings settings,
            EnemyCommonSettings commonSettings,
            EnemyRotationHandler rotationHandler)
        {
            _rotationHandler = rotationHandler;
            _commonSettings = commonSettings;
            _settings = settings;
            _tunables = tunables;
            _stateManager = stateManager;
            _view = view;
            _player = player;
        }

        public void EnterState()
        {
            _strafeRight = Random.Range(0, 1) == 0;
            _lastStrafeChangeTime = Time.realtimeSinceStartup;
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

            var distanceToPlayer = (_player.Position - _view.Position).magnitude;

            // Always look towards the player
            _rotationHandler.DesiredLookDir = (_player.Position - _view.Position).normalized;

            // Strafe back and forth over the given interval
            // This helps avoiding being too easy a target
            if (Time.realtimeSinceStartup - _lastStrafeChangeTime > _settings.StrafeChangeInterval)
            {
                _lastStrafeChangeTime = Time.realtimeSinceStartup;
                _strafeRight = !_strafeRight;
            }

            if (distanceToPlayer < _commonSettings.AttackDistance)
            {
                _stateManager.ChangeState(EnemyStates.Attack);
            }
        }

        public void FixedUpdate()
        {
            MoveTowardsPlayer();
            Strafe();
        }

        void Strafe()
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

        void MoveTowardsPlayer()
        {
            var playerDir = (_player.Position - _view.Position).normalized;

            _view.AddForce(playerDir * _tunables.Speed);
        }

        [Serializable]
        public class Settings
        {
            public float StrafeMultiplier;
            public float StrafeChangeInterval;
            public float TeleportNewDistance;
        }
    }
}

