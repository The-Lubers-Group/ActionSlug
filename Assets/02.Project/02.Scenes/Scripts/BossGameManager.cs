using Cinemachine;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        //print(_colliderLayer);
        _cinemachineConfiner.m_BoundingShape2D = _bossConfine;
        //_pointDoor.GetComponent<BoxCollider2D>().isTrigger = false;
        aux = true;

        _beginCutscene.Play();
    }

    public void EndCutscene()
    {
        _endCutscene.Play();
        _cinemachineConfiner.m_BoundingShape2D = _confine;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(_pointDoor.position, _collisionSize);
        Gizmos.DrawWireCube(_pointDoor.position, _collisionSize);
    }
}
