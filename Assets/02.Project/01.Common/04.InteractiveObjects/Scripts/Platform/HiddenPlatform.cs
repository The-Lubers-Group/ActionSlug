using DG.Tweening.Core.Easing;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlatform : MonoBehaviour
{
    [SerializeField] private float _timer = 5;
    
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

        if (_platformTimer < -_timer)
        {
            Components(_spriteRenderer.enabled);
            _platformTimer = 0;
        }
    }

    private void Components(bool state)
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _boxCollider.enabled = !_boxCollider.enabled;

    }
}
