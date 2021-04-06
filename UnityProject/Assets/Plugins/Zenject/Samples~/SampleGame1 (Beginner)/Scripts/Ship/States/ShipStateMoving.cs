using System;
using ModestTree;
using UnityEngine;

namespace Zenject.Asteroids
{
    public class ShipStateMoving : ShipState
    {
        readonly Settings _settings;
        readonly Camera _mainCamera;
        readonly Ship _ship;

        Vector3 _lastPosition;
        float _oscillationTheta;

        public ShipStateMoving(
            Settings settings, Ship ship,
            [Inject(Id = "Main")]
            Camera mainCamera)
        {
            _ship = ship;
            _settings = settings;
            _mainCamera = mainCamera;
        }

        public override void Update()
        {
            UpdateThruster();
            Move();
            ApplyOscillation();
        }

        void ApplyOscillation()
        {
            var obj = _ship.MeshRenderer.gameObject;

            var cycleInterval = 1.0f / _settings.oscillationFrequency;
            var thetaMoveSpeed = 2 * Mathf.PI / cycleInterval;

            _oscillationTheta += thetaMoveSpeed * Time.deltaTime;

            obj.transform.position = obj.transform.parent.position + new Vector3(0, _settings.oscillationAmplitude * Mathf.Sin(_oscillationTheta), 0);
        }

        void UpdateThruster()
        {
            var speed = (_ship.Position - _lastPosition).magnitude / Time.deltaTime;
            var speedPx = Mathf.Clamp(speed / _settings.speedForMaxEmisssion, 0.0f, 1.0f);

#if UNITY_2018_1_OR_NEWER
            var emission = _ship.ParticleEmitter.emission;
            emission.rateOverTime = _settings.maxEmission * speedPx;
#else
            _ship.ParticleEmitter.maxEmission = _settings.maxEmission * speedPx;
#endif
        }

        void Move()
        {
            var mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var mousePos = mouseRay.origin;
            mousePos.z = 0;

            _lastPosition = _ship.Position;
            _ship.Position = Vector3.Lerp(_ship.Position, mousePos, Mathf.Min(1.0f, _settings.moveSpeed * Time.deltaTime));

            var moveDelta = _ship.Position - _lastPosition;
            var moveDistance = moveDelta.magnitude;

            if (moveDistance > 0.01f)
            {
                var moveDir = moveDelta / moveDistance;
                _ship.Rotation = Quaternion.LookRotation(-moveDir);
            }
        }

        public override void Start()
        {
            _lastPosition = _ship.Position;

            _ship.ParticleEmitter.gameObject.SetActive(true);
        }

        public override void Dispose()
        {
            _ship.ParticleEmitter.gameObject.SetActive(false);
        }

        public override void OnTriggerEnter(Collider other)
        {
            Assert.That(other.GetComponent<Asteroid>() != null);
            _ship.ChangeState(ShipStates.Dead);
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float rotateSpeed;

            public float speedForMaxEmisssion;
            public float maxEmission;

            public float oscillationFrequency;
            public float oscillationAmplitude;
        }

        public class Factory : PlaceholderFactory<ShipStateMoving>
        {
        }
    }
}
