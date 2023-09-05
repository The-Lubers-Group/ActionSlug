using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace IgnoreSolutions
{
    public class KeyholeAnimation : MonoBehaviour
    {
        [Serializable]
        public struct SizePos
        {
            public Vector2 Size;
            public Vector2 Pos;
        }

        public enum Sides
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [SerializeField] private bool _Enable = true;
        [SerializeField] private float _Speed = 0.4f;
        [SerializeField] private Image _KeyholeImage;
        [SerializeField] private SizePos PointA = new SizePos() { Size = new Vector2(12508, 14433), Pos = new Vector2(-5293.748f, -6725.214f) }, PointB = new SizePos() { Size = new Vector2(2, 2), Pos = new Vector2(959.5582f, 489.7585f) };
        private Coroutine _CurrentCoroutine = null;
        private RectTransform[] _SideRects = new RectTransform[(int)Sides.Bottom + 1];

        private void SetRectSizePosBySide(RectTransform sideRect, Sides side)
        {
            Vector2 MaskPos = _KeyholeImage.rectTransform.anchoredPosition;

            Vector2 MaskSize = _KeyholeImage.rectTransform.sizeDelta;

            switch (side)
            {
                case Sides.Left:
                    sideRect.anchoredPosition = Vector2.zero;
                    sideRect.sizeDelta = new Vector2(MaskPos.x, Screen.height);
                    break;
                case Sides.Right:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x + MaskSize.x, 0);
                    sideRect.sizeDelta = new Vector2(Screen.width - (MaskPos.x + MaskSize.x), Screen.height);
                    break;
                case Sides.Top:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x, MaskPos.y + MaskSize.y);
                    sideRect.sizeDelta = new Vector2(MaskSize.x, Screen.height - (MaskPos.y + MaskSize.y));
                    break;
                case Sides.Bottom:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x, 0);
                    sideRect.sizeDelta = new Vector2(MaskSize.x, MaskPos.y);
                    break;
            }
        }

        private void UpdateRects()
        {
            for (int i = 0; i < _SideRects.Length; i++)
            {
                if (_SideRects[i] != null)
                    SetRectSizePosBySide(_SideRects[i], (Sides)i);
            }
        }

        IEnumerator LerpSizePos()
        {
            for (float f = 0; f <= 1.0f; f += _Speed * Time.fixedDeltaTime)
            {
                _KeyholeImage.rectTransform.sizeDelta = Vector2.Lerp(PointA.Size, PointB.Size, f);
                _KeyholeImage.rectTransform.anchoredPosition = Vector2.Lerp(PointA.Pos, PointB.Pos, f);
                UpdateRects();
                yield return null;
            }
            _KeyholeImage.rectTransform.sizeDelta = PointB.Size;
            _KeyholeImage.rectTransform.anchoredPosition = PointB.Pos;
            UpdateRects();

            yield return new WaitForSeconds(2f);

            for (float f = 0.0f; f <= 1.0f; f += _Speed * Time.fixedDeltaTime)
            {
                _KeyholeImage.rectTransform.sizeDelta = Vector2.Lerp(PointB.Size, PointA.Size, f);
                _KeyholeImage.rectTransform.anchoredPosition = Vector2.Lerp(PointB.Pos, PointA.Pos, f);
                UpdateRects();
                yield return null;
            }
            _KeyholeImage.rectTransform.sizeDelta = PointA.Size;
            _KeyholeImage.rectTransform.anchoredPosition = PointA.Pos;
            UpdateRects();

            yield return new WaitForSeconds(2f);
            _CurrentCoroutine = null;
        }


        private void OnEnable()
        {
            for (int i = 0; i < _SideRects.Length; i++)
            {
                if (_SideRects[i] != null) continue;

                GameObject newSideRect = new GameObject("RectSide_" + (Sides)i, typeof(Image));

                Image img = newSideRect.GetComponent<Image>();
                img.color = Color.black;
                _SideRects[i] = img.rectTransform;
                _SideRects[i].parent = transform.parent;
                _SideRects[i].anchorMin = Vector2.zero;
                _SideRects[i].anchorMax = Vector2.zero;
                _SideRects[i].pivot = Vector2.zero;

                SetRectSizePosBySide(_SideRects[i], (Sides)i);
            }
        }

        public void Update()
        {
            if (_Enable)
                if (_CurrentCoroutine == null)
                    _CurrentCoroutine = StartCoroutine(LerpSizePos());
        }
    }
}