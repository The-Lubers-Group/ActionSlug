using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    private UnitController _player;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_player == null)
            {
                _player =  FindAnyObjectByType<UnitController>();
            }
            _player.PlayerHit(damage);
        }
    }
}
