using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;
    
    private bool isShoot;
    private bool isJumping;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerMap.Enable();

    }

    private void Update()
    {
        // Jump
        if (playerControls.PlayerMap.Jump.WasReleasedThisFrame())
        {
            //Debug.Log("---------> PULO <---------");
            isJumping = true;
        }
        else isJumping = false;

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

    public bool IsJumping()
    {
        return isJumping;
    }
}
