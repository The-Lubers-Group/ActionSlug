using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    
    private bool isWalking;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = gameInput.getMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        
        isWalking = moveDir != Vector3.zero;

        if(inputVector.x == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        
        if(inputVector.x == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
