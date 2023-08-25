using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private UnitController unitController;

    [Space]

    public bool onGround;
    //public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onLedge;
    public bool onSpaceGround;

    [HideInInspector] public int wallSide;

    [Space]

    [Header("Collision")]

    [SerializeField] private float collisionRadius = 0.25f;
    [SerializeField] private float groundCheckDistance;

    [Header("Checks")]
    
    public Transform bottomOffset;
    public Transform leftOffset;
    public Transform rightOffset;
    
    [Space]
    public Transform ledgeOffset;

    private void Start()
    {
        unitController = GetComponent<UnitController>();
    }

    private void Update()
    {
        onGround = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);
        //onWall = Physics2D.OverlapCircle(rightOffset.position, collisionRadius, groundLayer)
        //    || Physics2D.OverlapCircle(leftOffset.position, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle(rightOffset.position, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle(leftOffset.position, collisionRadius, groundLayer);
        onLedge = Physics2D.OverlapCircle(ledgeOffset.position, collisionRadius, groundLayer);


        onSpaceGround = Physics2D.Raycast(bottomOffset.position, Vector2.down, groundCheckDistance, groundLayer);

        wallSide = onRightWall ? -1 : 1;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomOffset.position, collisionRadius);
        Gizmos.DrawWireSphere(rightOffset.position, collisionRadius);
        Gizmos.DrawWireSphere(leftOffset.position, collisionRadius);
        
        Gizmos.DrawWireSphere(ledgeOffset.position, collisionRadius);

        Gizmos.DrawLine(bottomOffset.position, new Vector3(bottomOffset.position.x, bottomOffset.position.y - groundCheckDistance, ledgeOffset.position.z));
    }
}



/*
  if(unitController.IsFacingRight)
  {
      onLedge = Physics2D.Raycast(ledgeOffset.position, ledgeOffset.transform.right, wallCheckDistance, groundLayer);
  }
  else
  {
      onLedge = Physics2D.Raycast(ledgeOffset.position, ledgeOffset.transform.right * 1, wallCheckDistance, groundLayer);
  }
  */

//onLedge = Physics2D.Raycast(ledgeOffset.position, ledgeOffset.transform.right, wallCheckDistance, groundLayer);