using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player");
            collision.gameObject.GetComponent<UnitController>().PlayerHit(damage);
        }
    }
}
