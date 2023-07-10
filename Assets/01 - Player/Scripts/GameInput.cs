using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerMap.Enable();

    }

    // Character movement
    public Vector2 getMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.PlayerMap.Move.ReadValue<Vector2>();


        inputVector = inputVector.normalized;
        return inputVector;
    }
}
