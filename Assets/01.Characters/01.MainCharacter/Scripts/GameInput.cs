using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;
    
    private bool isShoot;
    private bool isJumpingPress;
    private bool isJumpingReleases;
    private bool isDash;

    private bool moveUpDown;
    private bool moveLeftRight;

    private Vector2 moveInput;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerMap.Enable();

    }

    private void Update()
    {
        moveInput = getMovementVectorNormalized();

        // Moviment in Y (UP and Down)
        if (moveInput.y < 0)
        {
            moveUpDown = true;
        }
        else
        {
            moveUpDown = false;
        }

        // Moviment in Y (Left and Right)
        if (moveInput.x != 0)
        {
            moveLeftRight = true;
        }
        else
        {
            moveLeftRight = false;
        }

        //Jump PRESS
        //if (playerControls.PlayerMap.Jump.WasPerformedThisFrame())
        if (playerControls.PlayerMap.Jump.WasPressedThisFrame())
            isJumpingPress = true;
        else isJumpingPress = false;

        //Jump Releases 
        if (playerControls.PlayerMap.Jump.WasReleasedThisFrame())
            isJumpingReleases = true;
        else isJumpingReleases = false;


        // Dash 
        //if (playerControls.PlayerMap.Dash.IsPressed())
        if (playerControls.PlayerMap.Dash.WasPressedThisFrame())
        {
            //Debug.Log("playerControls.PlayerMap.Dash.IsPressed()" + playerControls.PlayerMap.Dash.IsPressed());
            isDash = true;

        }
        else isDash = false;

        

        // Shoot
        if (playerControls.PlayerMap.Shoot.IsPressed()) isShoot = true;
        else isShoot = false;
        
    }

    // Character movement
    public Vector2 getMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.PlayerMap.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public bool IsShoot()
    {
        return isShoot;
    }

    public bool IsJumpingPress()
    {
        return isJumpingPress;
    }

    public bool IsJumpingReleases()
    {
        return isJumpingReleases;
    }

    public bool IsDash()
    {
        return isDash;
    }

    public bool GetMoveUpDown()
    {
        return moveUpDown;
    }

    public bool GetMoveLeftRight()
    {
        return moveLeftRight;
    }
}
