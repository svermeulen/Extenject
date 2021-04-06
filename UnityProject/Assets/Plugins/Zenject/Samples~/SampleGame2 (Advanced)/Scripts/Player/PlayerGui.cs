using ModestTree;
using UnityEngine;

#pragma warning disable 649

namespace Zenject.SpaceFighter
{
    public class PlayerGui : MonoBehaviour
    {
        [SerializeField]
        float _leftPadding;

        [SerializeField]
        float _bottomPadding;

        [SerializeField]
        float _labelWidth;

        [SerializeField]
        float _labelHeight;

        [SerializeField]
        float _textureWidth;

        [SerializeField]
        float _textureHeight;

        [SerializeField]
        float _killCountOffset;

        [SerializeField]
        Color _foregroundColor;

        [SerializeField]
        Color _backgroundColor;

        Player _player;
        Texture2D _textureForeground;
        Texture2D _textureBackground;
        int _killCount;

        [Inject]
        public void Construct(Player player, SignalBus signalBus)
        {
            _player = player;

            _textureForeground = CreateColorTexture(_foregroundColor);
            _textureBackground = CreateColorTexture(_backgroundColor);

            signalBus.Subscribe<EnemyKilledSignal>(OnEnemyKilled);
        }

        void OnEnemyKilled()
        {
            _killCount++;
        }

        Texture2D CreateColorTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(1, 1, color);
            texture.Apply();
            return texture;
        }

        public void OnGUI()
        {
            var healthLabelBounds = new Rect(_leftPadding, Screen.height - _bottomPadding, _labelWidth, _labelHeight);
            GUI.Label(healthLabelBounds, "Health: {0:0}".Fmt(_player.Health));

            var killLabelBounds = new Rect(healthLabelBounds.xMin, healthLabelBounds.yMin - _killCountOffset, _labelWidth, _labelHeight);
            GUI.Label(killLabelBounds, "Kill Count: {0}".Fmt(_killCount));

            var boundsBackground = new Rect(healthLabelBounds.xMax, healthLabelBounds.yMin, _textureWidth, _textureHeight);
            GUI.DrawTexture(boundsBackground, _textureBackground);

            var boundsForeground = new Rect(boundsBackground.xMin, boundsBackground.yMin, (_player.Health / 100.0f) * _textureWidth, _textureHeight);
            GUI.DrawTexture(boundsForeground, _textureForeground);
        }
    }
}
