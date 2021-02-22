using UnityEngine;

namespace Zenject.SpaceFighter
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField]
        MeshRenderer _renderer = null;

        [SerializeField]
        Collider _collider = null;

        [SerializeField]
        Rigidbody _rigidBody = null;

        [Inject]
        public EnemyFacade Facade
        {
            get; set;
        }

        public MeshRenderer Renderer
        {
            get { return _renderer; }
        }

        public Collider Collider
        {
            get { return _collider; }
        }

        public Rigidbody Rigidbody
        {
            get { return _rigidBody; }
        }

        public Vector3 LookDir
        {
            get { return -_rigidBody.transform.right; }
        }

        public Vector3 RightDir
        {
            get { return _rigidBody.transform.up; }
        }

        public Vector3 ForwardDir
        {
            get { return _rigidBody.transform.right; }
        }

        public Vector3 Position
        {
            get { return _rigidBody.transform.position; }
            set { _rigidBody.transform.position = value; }
        }

        public Quaternion Rotation
        {
            get { return _rigidBody.rotation; }
            set { _rigidBody.rotation = value; }
        }

        public Vector3 Velocity
        {
            get { return _rigidBody.velocity; }
        }

        public Vector3 AngularVelocity
        {
            get { return _rigidBody.angularVelocity; }
            set { _rigidBody.angularVelocity = value; }
        }

        public void AddForce(Vector3 force)
        {
            _rigidBody.AddForce(force);
        }

        public void AddTorque(float value)
        {
            _rigidBody.AddTorque(Vector3.forward * value);
        }
    }
}
