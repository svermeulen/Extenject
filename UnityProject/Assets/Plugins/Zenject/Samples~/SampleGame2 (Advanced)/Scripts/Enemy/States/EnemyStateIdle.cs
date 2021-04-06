using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zenject.SpaceFighter
{
    public class EnemyStateIdle : IEnemyState
    {
        readonly EnemyRotationHandler _rotationHandler;
        readonly Settings _settings;
        readonly EnemyView _view;

        Vector3 _startPos;
        float _theta;
        Vector3 _startLookDir;

        public EnemyStateIdle(
            EnemyView view, Settings settings,
            EnemyRotationHandler rotationHandler)
        {
            _rotationHandler = rotationHandler;
            _settings = settings;
            _view = view;
        }

        public void EnterState()
        {
            _startPos = _view.Position;
            _theta = Random.Range(0, 2.0f * Mathf.PI);
            _startLookDir = _view.LookDir;
        }

        public void ExitState()
        {
        }

        // Just add a bit of subtle movement
        public void Update()
        {
            _theta += Time.deltaTime * _settings.Frequency;

            _view.Position = _startPos + _view.RightDir * _settings.Amplitude * Mathf.Sin(_theta);

            _rotationHandler.DesiredLookDir = _startLookDir;
        }

        public void FixedUpdate()
        {
        }

        [Serializable]
        public class Settings
        {
            public float Amplitude;
            public float Frequency;
        }
    }
}
