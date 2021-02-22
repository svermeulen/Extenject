using System;
using UnityEngine;

namespace Zenject.Asteroids
{
    public class ShipStateWaitingToStart : ShipState
    {
        readonly Settings _settings;
        readonly Ship _ship;

        float _theta;

        public ShipStateWaitingToStart(
            Ship ship,
            Settings settings)
        {
            _settings = settings;
            _ship = ship;
        }

        public override void Start()
        {
            _ship.Position = _settings.StartOffset;
            _ship.Rotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.right);
        }

        public override void Update()
        {
            _ship.Position = _settings.StartOffset + Vector3.up * _settings.Amplitude * Mathf.Sin(_theta);
            _theta += Time.deltaTime * _settings.Frequency;
        }

        [Serializable]
        public class Settings
        {
            public Vector3 StartOffset;
            public float Amplitude;
            public float Frequency;
        }

        public class Factory : PlaceholderFactory<ShipStateWaitingToStart>
        {
        }
    }
}
