using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemies : MonoBehaviour
{

    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;
    [SerializeField] protected float speed = 2;
    protected GameObject player;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= maxHealth;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Morreu");
        GameObject.Destroy(gameObject);
    }
}
