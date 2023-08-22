using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool issClimbing;

    public UnitInfoData Data;

    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private GameInput gameInput;
    private Vector2 moveInput;



    void Update()
    {
        moveInput = gameInput.getMovementVectorNormalized();
        vertical = Input.GetAxisRaw("Vertical");

        //Debug.Log(isLadder);

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            issClimbing = true;
        }
        else if(isLadder && moveInput.y > 0.0 )
        {
            //Debug.Log(" moveInput.y: " + moveInput.y );
            issClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (issClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
            Debug.Log("rb.velocity: " + rb.velocity);
        }
        else
        {
            rb.gravityScale = Data.gravityScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            //Debug.Log("Ladder");
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            issClimbing = false;
        }
    }
}
