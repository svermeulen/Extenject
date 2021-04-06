using UnityEngine;

namespace Zenject.SpaceFighter
{
    public enum BulletTypes
    {
        FromEnemy,
        FromPlayer
    }

    public class Bullet : MonoBehaviour, IPoolable<float, float, BulletTypes, IMemoryPool>
    {
        float _startTime;
        BulletTypes _type;
        float _speed;
        float _lifeTime;

        [SerializeField]
        MeshRenderer _renderer = null;

        [SerializeField]
        Material _playerMaterial = null;

        [SerializeField]
        Material _enemyMaterial = null;

        IMemoryPool _pool;

        public BulletTypes Type
        {
            get { return _type; }
        }

        public Vector3 MoveDirection
        {
            get { return transform.right; }
        }

        public void OnTriggerEnter(Collider other)
        {
            var enemyView = other.GetComponent<EnemyView>();

            if (enemyView != null && _type == BulletTypes.FromPlayer)
            {
                enemyView.Facade.Die();
                _pool.Despawn(this);
            }
            else
            {
                var player = other.GetComponent<PlayerFacade>();

                if (player != null && _type == BulletTypes.FromEnemy)
                {
                    player.TakeDamage(MoveDirection);
                    _pool.Despawn(this);
                }
            }
        }

        public void Update()
        {
            transform.position -= transform.right * _speed * Time.deltaTime;

            if (Time.realtimeSinceStartup - _startTime > _lifeTime)
            {
                _pool.Despawn(this);
            }
        }

        public void OnSpawned(float speed, float lifeTime, BulletTypes type, IMemoryPool pool)
        {
            _pool = pool;
            _type = type;
            _speed = speed;
            _lifeTime = lifeTime;

            _renderer.material = type == BulletTypes.FromEnemy ? _enemyMaterial : _playerMaterial;

            _startTime = Time.realtimeSinceStartup;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public class Factory : PlaceholderFactory<float, float, BulletTypes, Bullet>
        {
        }
    }
}
