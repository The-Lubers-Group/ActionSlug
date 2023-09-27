using LabLuby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Collections.Shaders.CircleTransition
{
    public class CircleTransition : MonoBehaviour
    {
        public static CircleTransition main;
        private UnitController _player;

        private Canvas _canvas;
        private Image _blackScreen;

        private Vector2 _playerCanvasPos;

        private static readonly int RADIUS = Shader.PropertyToID("_Radius");
        private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
        private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

        private void Awake()
        {
            if(main == null)
            {
                main = this;
            }
            
            _canvas = GetComponent<Canvas>();
            _blackScreen = GetComponentInChildren<Image>();
            _player = GameObject.FindAnyObjectByType<UnitController>();
            //_player = UnitController.main
        }

        public void OpenBlackScreen()
        {
            DrawBlackScreen();
            StartCoroutine(Transition(2, 0, 1));
        }

        public void CloseBlackScreen()
        {
            DrawBlackScreen();
            StartCoroutine(Transition(2, 1, 0));
        }

        private void DrawBlackScreen()
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
         
            var playerScreenPos = Camera.main.WorldToScreenPoint(_player.transform.position);

            var canvasRect = _canvas.GetComponent<RectTransform>().rect;
            var canvasWidth = canvasRect.width;
            var canvasHeight = canvasRect.height;

            _playerCanvasPos = new Vector2
            {
                x = (playerScreenPos.x / screenWidth) * canvasWidth,
                y = (playerScreenPos.y / screenHeight) * canvasHeight,
            };

            var squareValue = 0f;
            if (canvasWidth > canvasHeight)
            {
                squareValue = canvasWidth;
                _playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
            }
            else
            {
                squareValue = canvasHeight;
                _playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
            }

            _playerCanvasPos /= squareValue;

            var mat = _blackScreen.material;
            mat.SetFloat(CENTER_X, _playerCanvasPos.x);
            mat.SetFloat(CENTER_Y, _playerCanvasPos.y);

            _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
        }

        private IEnumerator Transition(float duration, float beginRadius, float endRadius)
        {
            var mat = _blackScreen.material;
            var time = 0f;
            while (time <= duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
                var radius = Mathf.Lerp(beginRadius, endRadius, t);

                mat.SetFloat(RADIUS, radius);

                yield return null;
            }
        }
    }
}