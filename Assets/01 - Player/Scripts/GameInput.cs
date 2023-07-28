using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;
    
    private bool isShoot;
    private bool isJumpingPress;
    private bool isJumpingReleases;
    private bool isDash;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerMap.Enable();

    }

    private void Update()
    {
        //Jump PRESS
        if (playerControls.PlayerMap.Jump.WasPerformedThisFrame())
            isJumpingPress = true;
        else isJumpingPress = false;

        //Jump Releases 
        if (playerControls.PlayerMap.Jump.WasReleasedThisFrame())
            isJumpingReleases = true;
        else isJumpingReleases = false;


        // Dash 
        if (playerControls.PlayerMap.Dash.IsPressed())
            isDash = true;
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
}
