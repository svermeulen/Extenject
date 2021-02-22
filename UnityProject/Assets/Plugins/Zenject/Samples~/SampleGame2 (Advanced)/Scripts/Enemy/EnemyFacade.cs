using System;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    // Here we can add some high-level methods to give some info to other
    // parts of the codebase outside of our enemy facade
    public class EnemyFacade : MonoBehaviour, IPoolable<float, float, IMemoryPool>, IDisposable
    {
        EnemyView _view;
        EnemyTunables _tunables;
        EnemyDeathHandler _deathHandler;
        EnemyStateManager _stateManager;
        EnemyRegistry _registry;
        IMemoryPool _pool;

        [Inject]
        public void Construct(
            EnemyView view,
            EnemyTunables tunables,
            EnemyDeathHandler deathHandler,
            EnemyStateManager stateManager,
            EnemyRegistry registry)
        {
            _view = view;
            _tunables = tunables;
            _deathHandler = deathHandler;
            _stateManager = stateManager;
            _registry = registry;
        }

        public EnemyStates State
        {
            get { return _stateManager.CurrentState; }
        }

        public float Accuracy
        {
            get { return _tunables.Accuracy; }
        }

        public float Speed
        {
            get { return _tunables.Speed; }
        }

        public Vector3 Position
        {
            get { return _view.Position; }
            set { _view.Position = value; }
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public void Die()
        {
            _deathHandler.Die();
        }

        public void OnDespawned()
        {
            _registry.RemoveEnemy(this);
            _pool = null;
        }

        public void OnSpawned(float accuracy, float speed, IMemoryPool pool)
        {
            _pool = pool;
            _tunables.Accuracy = accuracy;
            _tunables.Speed = speed;

            _registry.AddEnemy(this);
        }

        public class Factory : PlaceholderFactory<float, float, EnemyFacade>
        {
        }
    }
}
