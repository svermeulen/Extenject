using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject.SpaceFighter
{
    public interface IEnemyState
    {
        void EnterState();
        void ExitState();
        void Update();
        void FixedUpdate();
    }

    public enum EnemyStates
    {
        Idle,
        Attack,
        Follow,
        None
    }

    // This class controls the basic "AI" of our enemy
    // Which works as a finite state machine, between three states:
    // - Attack
    // - Follow/Chase
    // - Idle
    public class EnemyStateManager : ITickable, IFixedTickable, IInitializable
    {
        IEnemyState _currentStateHandler;
        EnemyStates _currentState = EnemyStates.None;
        EnemyView _view;

        List<IEnemyState> _states;

        // We can't use a constructor due to a circular dependency issue
        [Inject]
        public void Construct(
            EnemyView view,
            EnemyStateIdle idle, EnemyStateAttack attack, EnemyStateFollow follow)
        {
            _view = view;
            _states = new List<IEnemyState>
            {
                // This needs to follow the enum order
                idle, attack, follow
            };
        }

        public EnemyStates CurrentState
        {
            get { return _currentState; }
        }

        public void Initialize()
        {
            Assert.IsEqual(_currentState, EnemyStates.None);
            Assert.IsNull(_currentStateHandler);

            ChangeState(EnemyStates.Follow);
        }

        public void ChangeState(EnemyStates state)
        {
            if (_currentState == state)
            {
                // Already in state
                return;
            }

            //Log.Trace("View Changing state from {0} to {1}", _currentState, state);

            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }

        public void Tick()
        {
            // Always ensure we are on the main plane
            _view.Position = new Vector3(_view.Position.x, _view.Position.y, 0);

            _currentStateHandler.Update();
        }

        public void FixedTick()
        {
            _currentStateHandler.FixedUpdate();
        }
    }
}
