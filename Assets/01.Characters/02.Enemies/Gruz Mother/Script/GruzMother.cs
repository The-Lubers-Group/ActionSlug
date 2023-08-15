using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    [Header("idle")]
    [SerializeField] float idleMovementSpeed;
    [SerializeField] Vector2 idleMovementDirection;

    [Header("AttackUpDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;
    private Vector2 playerPosition;

    [Header("Other")]
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    private bool isTouchingUp;
    private bool isTouchingDown;  
    private bool isTouchingWall;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;

    private void Start()
    {

        idleMovementDirection.Normalize(); 
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
        //IdleState();
         //FlipTowardsPlayer();

    }

    public void IdleState()
    {
        if(isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if(isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if(isTouchingWall)
        {
            if(facingLeft)
            {
                Flip();
            }
            else if(!facingLeft)
            {
                Flip();
            }
        }

        enemyRB.velocity = idleMovementSpeed * idleMovementDirection;
    }

    public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }

        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayer()
    {
        playerPosition = player.position - transform.position;
        playerPosition.Normalize();
        enemyRB.velocity = playerPosition * attackPlayerSpeed;
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if(playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void ChangeDirection()
    {
        goingUp = !goingUp;
        idleMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        idleMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    }
}
