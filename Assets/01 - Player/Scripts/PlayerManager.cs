using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public PlayerData Data;

    // COMPONENTS
    public Rigidbody2D rb { get; private set; }
    public PlayerAnimator AnimHandler { get; private set; }













    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float maxFallSpeed = 17f;
    

    [Header("Camera Stuff")]
    [SerializeField] private GameObject cameraFollowObjectGO;


    Vector2 inputVector;
    //public bool isFacingRight;

    
    private RaycastHit2D groundHit;
    private CameraFollowObject cameraFollowObject;


    //public PlayerRunData Data;

   

    #region STATE PARAMETERS
    //Variables control the varioues action the player can perform at any time.
    private bool isWalking;
    private bool isShoot;
    private bool isJumping;
    private float jumpTimeCounter;
    
    public bool isFacingRight { get; private set; }
    public float LastOnGroundTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
    private Vector2 moveInput;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
    }

    void Update()
    {
        inputVector = gameInput.getMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        isShoot = gameInput.IsShoot();

    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput * moveSpeed, RenderBuffer.velocity.y);
       rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpeed, maxFallSpeed * 5));


        if(inputVector.x > 0 || inputVector.x < 0)
        {
            TurnCheck();
        }
    }

    private void TurnCheck()
    {
        if (inputVector.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (inputVector.x < 0 && isFacingRight)
        {
            Turn();
        }

        
    }

    private void Turn()
    {
        if(isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
            cameraFollowObject.CallTurn();
        }
    }


    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsShoot()
    {
        return isShoot;
    }
}
