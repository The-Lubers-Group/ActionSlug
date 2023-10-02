using Cinemachine;
using DG.Tweening;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossGameManager : GameManager
{
    [Space(10)]
    [SerializeField] private Transform _pointDoor;
    [SerializeField] private float _collisionRadius = 0.25f;
    [SerializeField] private Vector2 _collisionSize;

    [SerializeField] private PolygonCollider2D _confine;
    [SerializeField] private PolygonCollider2D _bossConfine;
    [SerializeField] private CinemachineConfiner _cinemachineConfiner;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _colliderLayer;

    [SerializeField] PlayableDirector _beginCutscene;
    [SerializeField] PlayableDirector _endCutscene;
    
    private bool aux = false;

    private void LateUpdate()
    {
        if (Physics2D.OverlapBox(_pointDoor.position, _collisionSize, _collisionRadius, _colliderLayer) && aux == false )
        {
            BeginCutscene();
        }
    }

    private void BeginCutscene()
    {
        MenuManager.main._canvasGroup.transform.DOScale(.0f, 0f);
        //print(_colliderLayer);
        _cinemachineConfiner.m_BoundingShape2D = _bossConfine;
        //_pointDoor.GetComponent<BoxCollider2D>().isTrigger = false;
        aux = true;

        _beginCutscene.Play();
        StartCoroutine(Show(9.5f));

    }

    public void EndCutscene()
    {
        _endCutscene.Play();
        StartCoroutine(Wait(9f));

    }

    IEnumerator Show(float _time)
    {
        yield return new WaitForSeconds(_time);
        MenuManager.main._canvasGroup.transform.DOScale(1f, .1f).SetEase(Ease.Linear);

    }


    IEnumerator Wait(float _time)
    {
        yield return new WaitForSeconds(_time);
        _cinemachineConfiner.m_BoundingShape2D = _confine;
        MenuManager.main._canvasGroup.transform.DOScale(1f, .1f).SetEase(Ease.Linear);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(_pointDoor.position, _collisionSize);
        Gizmos.DrawWireCube(_pointDoor.position, _collisionSize);
    }
}
