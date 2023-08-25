using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onLedge;

    public int wallSide;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;

    [Header("Checks")]
    
    [SerializeField] private Transform bottomOffset;
    [SerializeField] private Transform leftOffset;
    [SerializeField] private Transform rightOffset;
    
    [Space]
    [SerializeField] private Transform ledgeOffset;

    
    
    void Update()
    {
        onGround = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle(rightOffset.position, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle(leftOffset.position, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle(rightOffset.position, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle(leftOffset.position, collisionRadius, groundLayer);

        onLedge = Physics2D.OverlapCircle(ledgeOffset.position, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomOffset.position, collisionRadius);
        Gizmos.DrawWireSphere(rightOffset.position, collisionRadius);
        Gizmos.DrawWireSphere(leftOffset.position, collisionRadius);
        Gizmos.DrawWireSphere(ledgeOffset.position, collisionRadius);
    }
}
