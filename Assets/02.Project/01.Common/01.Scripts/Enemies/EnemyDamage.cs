using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    private UnitController _player;


    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        print("collision.gameObject.tag: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<UnitController>().PlayerHit(damage);
            if (_player == null)
            {
                _player =  FindAnyObjectByType<UnitController>();
            }
            _player.PlayerHit(damage);
        }
    }
}
