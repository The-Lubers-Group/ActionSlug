using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
   
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private GameObject[] _fireBalls;

    [Space(5)]
    [SerializeField] private AudioClip _soundFX;

    private float _cooldownTimer;
    private AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _cooldownTimer = 0;
        _fireBalls[FindFireball()].transform.position = _firePoint.position;
        _fireBalls[FindFireball()].GetComponent<EnemyProjectile>().SetDirection();
        _audioSource.PlayOneShot(_soundFX);

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
}
