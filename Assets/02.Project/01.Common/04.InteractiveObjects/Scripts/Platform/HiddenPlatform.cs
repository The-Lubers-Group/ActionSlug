using DG.Tweening.Core.Easing;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class HiddenPlatform : MonoBehaviour
{
    //[SerializeField] private float _wait = 5f;
    public AnimationCurve timeCurve;
    
    private float _time = 1f;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;

    public float _platformTimer { get; private set; }

    private void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _platformTimer -= Time.deltaTime;
        //print(timeCurve.Evaluate(_platformTimer));

        if (_platformTimer < -timeCurve.Evaluate(_platformTimer))
        {
            if (_spriteRenderer.enabled)
            {
                transform.DOScale(0, _time).SetEase(Ease.OutBounce);
                StartCoroutine(Wait(_time));
            }
            else
            {
                Components();
                transform.DOScale(1, _time).SetEase(Ease.InBounce);
            }
            _platformTimer = 0;
        }
    }

    private void Components()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _boxCollider.enabled = !_boxCollider.enabled;
    }

    IEnumerator Wait(float _time)
    {
        yield return new WaitForSeconds(_time);
        Components();
       
    }
}
