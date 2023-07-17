using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData Data;

    // COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public PlayerAnimator AnimHandler { get; private set; }

    //STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }

    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    // Walk + Shoot
    private bool isWalking;
    private bool isShoot;

    //Jump
    private bool isJumpCut;
    private bool isJumpFalling;

    //Wall Jump
    private float wallJumpStartTime;
    private int lastWallJumpDir;

    //Dash
    private int dashesLeft;
    private bool dashRefilling;
    private Vector2 lastDashDir;
    private bool isDashAttacking;

    //INPUT PARAMETERS
    private Vector2 _moveInput;
    
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }

    //CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform frontWallCheckPoint;
    [SerializeField] private Transform backWallCheckPoint;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);

    // LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundLayer;

    // CAMERAA PARAMETERS
    [Header("Camera Stuff")]
    [SerializeField] private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowObjectGO;

    // ---------------------
    //[SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float maxFallSpeed = 17f;
    Vector2 inputVector;
    //private RaycastHit2D groundHit;
    // ---------------------



    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
    }

    void Update()
    {
        inputVector = gameInput.getMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDir * Data.maxFallSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        isShoot = gameInput.IsShoot();

    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput * moveSpeed, RenderBuffer.velocity.y);
       RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -maxFallSpeed, maxFallSpeed * 5));


        if(inputVector.x > 0 || inputVector.x < 0)
        {
            TurnCheck();
        }
    }

    private void TurnCheck()
    {
        if (inputVector.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (inputVector.x < 0 && IsFacingRight)
        {
            Turn();
        }

        
    }

    private void Turn()
    {
        if(IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
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
