using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : BaseEnemies
{
    
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    //[SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("IsRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position; 
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2 (speed,0);
        }
        else
        {
            rb.velocity = new Vector2 (-speed,0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
