using DG.Tweening;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class BossHit : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    [Space(5)]
    [SerializeField] private Animator _animator;

    private GruzMother _mother;
    private Vector2 _position;

    private bool _aux = false;

    private void Awake()
    {
        _mother = GameObject.FindAnyObjectByType<GruzMother>();
    }
    private void Update()
    {
        if(_aux == true)
        {
            _position = _mother.transform.position;
            _obj.transform.DOMove(_position, 1f).SetEase(Ease.Flash);
            //_obj.transform.DOMove(_mother.transform.position, 1f).SetEase(Ease.InBounce);
        }
       
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GruzMother" && _aux == true)
        {
            _animator.SetTrigger("Hit");
        }


        if (collision.gameObject.tag == "Player" && _aux == false)
        {
            //_obj.transform.DOMove(_position, 1f);
            _aux = true;
        }

    }

    public void OnHit()
    {
        _mother.GetDamage();
        Destroy(this.gameObject);
    }


    IEnumerator Wait(float _time)
    {
        yield return new WaitForSeconds(_time);
        //Components();

    }
}
