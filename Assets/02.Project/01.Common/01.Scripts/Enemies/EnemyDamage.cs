using LabLuby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    private UnitController _player;
    private static string TAG = "Player";
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TAG)
        {
            if (_player == null)
            {
                _player =  FindAnyObjectByType<UnitController>();
            }
            _player.PlayerHit(damage);
        }

        CollisonIgnore(collision);
    }

    public virtual void CollisonIgnore(Collider2D collision) { }

}
