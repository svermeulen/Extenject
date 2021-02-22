using System;
using ModestTree;
using UnityEngine;

namespace Zenject.Asteroids
{
    public enum GameStates
    {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable
    {
        readonly SignalBus _signalBus;
        readonly Ship _ship;
        readonly AsteroidManager _asteroidSpawner;

        GameStates _state = GameStates.WaitingToStart;
        float _elapsedTime;

        public GameController(
            Ship ship, AsteroidManager asteroidSpawner,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _asteroidSpawner = asteroidSpawner;
            _ship = ship;
        }

        public float ElapsedTime
        {
            get { return _elapsedTime; }
        }

        public GameStates State
        {
            get { return _state; }
        }

        public void Initialize()
        {
            Physics.gravity = Vector3.zero;

            Cursor.visible = false;

            _signalBus.Subscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        public void Tick()
        {
            switch (_state)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
                case GameStates.GameOver:
                {
                    UpdateGameOver();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        void UpdateGameOver()
        {
            Assert.That(_state == GameStates.GameOver);

            if (Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }

        void OnShipCrashed()
        {
            Assert.That(_state == GameStates.Playing);
            _state = GameStates.GameOver;
            _asteroidSpawner.Stop();
        }

        void UpdatePlaying()
        {
            Assert.That(_state == GameStates.Playing);
            _elapsedTime += Time.deltaTime;
        }

        void UpdateStarting()
        {
            Assert.That(_state == GameStates.WaitingToStart);

            if (Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }

        void StartGame()
        {
            Assert.That(_state == GameStates.WaitingToStart || _state == GameStates.GameOver);

            _ship.Position = Vector3.zero;
            _elapsedTime = 0;
            _asteroidSpawner.Start();
            _ship.ChangeState(ShipStates.Moving);
            _state = GameStates.Playing;
        }
    }
}
