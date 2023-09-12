using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject[] _fireBalls;

    private float _cooldownTimer;

    private void Attack()
    {
        _cooldownTimer = 0;
        _fireBalls[FindFireball()].transform.position = _firePoint.position;
        _fireBalls[FindFireball()].GetComponent<EnemyProjectile>().SetDirection();
    }

    private int FindFireball()
    {
        for(int i = 0; i < _fireBalls.Length; i++)
        {
            if (!_fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _attackCooldown)
        {
            Attack();
        }
    }
   

}
