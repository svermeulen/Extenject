using System;
using ModestTree;
using UnityEngine;

namespace Zenject.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        LevelHelper _level;
        Rigidbody _rigidBody;
        Settings _settings;

        // We could just add [Inject] to the field declarations but
        // it's often better practice to use PostInject methods
        // Note that we can't use Constructors here because this is
        // a MonoBehaviour
        [Inject]
        public void Construct(LevelHelper level, Settings settings)
        {
            _level = level;
            _settings = settings;
            _rigidBody = GetComponent<Rigidbody>();
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public float Mass
        {
            get { return _rigidBody.mass; }
            set { _rigidBody.mass = value; }
        }

        public float Scale
        {
            get
            {
                var scale = transform.localScale;
                // We assume scale is uniform
                Assert.That(scale[0] == scale[1] && scale[1] == scale[2]);

                return scale[0];
            }
            set
            {
                transform.localScale = new Vector3(value, value, value);
                _rigidBody.mass = value;
            }
        }

        public Vector3 Velocity
        {
            get { return _rigidBody.velocity; }
            set { _rigidBody.velocity = value; }
        }

        public void FixedTick()
        {
            // Limit speed to a maximum
            var speed = _rigidBody.velocity.magnitude;

            if (speed > _settings.maxSpeed)
            {
                var dir = _rigidBody.velocity / speed;
                _rigidBody.velocity = dir * _settings.maxSpeed;
            }
        }

        public void Tick()
        {
            CheckForTeleport();
        }

        void CheckForTeleport()
        {
            if (Position.x > _level.Right + Scale && IsMovingInDirection(Vector3.right))
            {
                transform.SetX(_level.Left - Scale);
            }
            else if (Position.x < _level.Left - Scale && IsMovingInDirection(-Vector3.right))
            {
                transform.SetX(_level.Right + Scale);
            }
            else if (Position.y < _level.Bottom - Scale && IsMovingInDirection(-Vector3.up))
            {
                transform.SetY(_level.Top + Scale);
            }
            else if (Position.y > _level.Top + Scale && IsMovingInDirection(Vector3.up))
            {
                transform.SetY(_level.Bottom - Scale);
            }

            transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
        }

        bool IsMovingInDirection(Vector3 dir)
        {
            return Vector3.Dot(dir, _rigidBody.velocity) > 0;
        }

        [Serializable]
        public class Settings
        {
            public float massScaleFactor;
            public float maxSpeed;
        }

        public class Factory : PlaceholderFactory<Asteroid>
        {
        }
    }
}
