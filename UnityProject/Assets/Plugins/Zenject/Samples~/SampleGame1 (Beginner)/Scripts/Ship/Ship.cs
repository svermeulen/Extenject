using UnityEngine;

#pragma warning disable 649
#pragma warning disable 618

namespace Zenject.Asteroids
{
    public class Ship : MonoBehaviour
    {
        [SerializeField]
        MeshRenderer _meshRenderer;

#if UNITY_2018_1_OR_NEWER
        [SerializeField]
        ParticleSystem _particleSystem;
#else
        [SerializeField]
        ParticleEmitter _particleEmitter;
#endif

        ShipStateFactory _stateFactory;
        ShipState _state;

        [Inject]
        public void Construct(ShipStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public MeshRenderer MeshRenderer
        {
            get { return _meshRenderer; }
        }

#if UNITY_2018_1_OR_NEWER
        public ParticleSystem ParticleEmitter
        {
            get { return _particleSystem; }
        }
#else
        public ParticleEmitter ParticleEmitter
        {
            get { return _particleEmitter; }
        }
#endif

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        public void Start()
        {
            ChangeState(ShipStates.WaitingToStart);
        }

        public void Update()
        {
            _state.Update();
        }

        public void OnTriggerEnter(Collider other)
        {
            _state.OnTriggerEnter(other);
        }

        public void ChangeState(ShipStates state)
        {
            if (_state != null)
            {
                _state.Dispose();
                _state = null;
            }

            _state = _stateFactory.CreateState(state);
            _state.Start();
        }
    }
}

