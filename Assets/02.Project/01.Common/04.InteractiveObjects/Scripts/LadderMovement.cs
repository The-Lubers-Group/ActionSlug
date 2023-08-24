using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    
    [Space(5)]
    [SerializeField] private float speed = 10f;

    private Rigidbody2D RB;
    private bool isLadder;
    private Vector2 moveInput;

    private void Start()
    {
        RB = this.transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        moveInput = unitController.moveInput;
        if (isLadder && moveInput.y > 0)
        {
            unitController.IsClimbing = true;
        }
    }

    private void FixedUpdate()
    {
       
        if(unitController.IsClimbing)
        {
            unitController.SetGravityScale(0);
            RB.velocity = new Vector2(RB.velocity.x, moveInput.y * speed);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            unitController.IsClimbing = false;
        }
    }
}
