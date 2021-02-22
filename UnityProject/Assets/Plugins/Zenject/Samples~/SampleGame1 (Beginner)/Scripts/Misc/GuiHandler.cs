using System;
using ModestTree;
using UnityEngine;

#pragma warning disable 649

namespace Zenject.Asteroids
{
    public class GuiHandler : MonoBehaviour, IDisposable, IInitializable
    {
        GameController _gameController;

        [SerializeField]
        GUIStyle _titleStyle;

        [SerializeField]
        GUIStyle _instructionsStyle;

        [SerializeField]
        GUIStyle _timeStyle;

        [SerializeField]
        float _gameOverFadeInTime;

        [SerializeField]
        float _gameOverStartFadeTime;

        [SerializeField]
        float _restartTextStartFadeTime;

        [SerializeField]
        float _restartTextFadeInTime;

        float _gameOverElapsed;
        SignalBus _signalBus;

        [Inject]
        public void Construct(
            GameController gameController, SignalBus signalBus)
        {
            _gameController = gameController;
            _signalBus = signalBus;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                switch (_gameController.State)
                {
                    case GameStates.WaitingToStart:
                    {
                        StartGui();
                        break;
                    }
                    case GameStates.Playing:
                    {
                        PlayingGui();
                        break;
                    }
                    case GameStates.GameOver:
                    {
                        PlayingGui();
                        GameOverGui();
                        break;
                    }
                    default:
                    {
                        Assert.That(false);
                        break;
                    }
                }
            }
            GUILayout.EndArea();
        }

        void GameOverGui()
        {
            _gameOverElapsed += Time.deltaTime;

            if (_gameOverElapsed > _gameOverStartFadeTime)
            {
                var px = Mathf.Min(1.0f, (_gameOverElapsed - _gameOverStartFadeTime) / _gameOverFadeInTime);
                _titleStyle.normal.textColor = new Color(1, 1, 1, px);
            }
            else
            {
                _titleStyle.normal.textColor = new Color(1, 1, 1, 0);
            }

            if (_gameOverElapsed > _restartTextStartFadeTime)
            {
                var px = Mathf.Min(1.0f, (_gameOverElapsed - _restartTextStartFadeTime) / _restartTextFadeInTime);
                _instructionsStyle.normal.textColor = new Color(1, 1, 1, px);
            }
            else
            {
                _instructionsStyle.normal.textColor = new Color(1, 1, 1, 0);
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("GAME OVER", _titleStyle);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(60);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();

                            GUILayout.Label("Click to restart", _instructionsStyle);

                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        void PlayingGui()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(30);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(30);
                    GUILayout.Label("Time: " + _gameController.ElapsedTime.ToString("0.##"), _timeStyle);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        void StartGui()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(100);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("ASTEROIDS", _titleStyle);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(60);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();

                            GUILayout.Label("Click to start", _instructionsStyle);

                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        void OnShipCrashed()
        {
            _gameOverElapsed = 0;
        }
    }
}

