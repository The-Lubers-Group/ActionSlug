using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : BaseEnemies
{
    //private int maxHealth = 100;
    //int currentHealth;
    [SerializeField] private float speed;
    //[SerializeField] private GameObject player;

    
    [SerializeField] private Transform startingPoint;
    public bool chase = false;

    /*
    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = maxHealth;
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    */

    /*
    public void TakeDamage(int damage)
    {
        currentHealth -= maxHealth;
        Debug.Log(currentHealth);
        if(currentHealth <= 0 )
        {
            Die();
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }
        if(chase == true)
        {
            Chase();
        }
        else
        {
            StartingPoint();
        }
        Flip();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            //Attack
        }
    }

    private void StartingPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);

    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        if (transform.position.x > player.transform.position.x)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        else
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        
    }


    /*
    void Die()
    {
        Debug.Log("Morreu");
        GameObject.Destroy(gameObject);
    }
    */
}
