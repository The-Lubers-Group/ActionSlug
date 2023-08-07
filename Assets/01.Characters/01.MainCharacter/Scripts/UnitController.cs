using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    [Header("Data")]
    public UnitInfoData data;
    
    [Header("Movement")]
    [SerializeField] private GameInput gameInput;
    public Rigidbody2D RB { get; private set; }
    public PlayerAnimator AnimHandler { get; private set; }
    private Vector2 moveInput;
    
    [Header("Health Settings")]
    private bool unitIsAlive;

    [Header("Ability Settings")]

    [Header("Animation Settings")]

    [Header("Audio Settings")]

    [Header("Camera Stuff")]
    [SerializeField] private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowObjectGO;

    [Header("Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform frontWallCheckPoint;
    [SerializeField] private Transform backWallCheckPoint;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);

    [Header("Debug")]
    public bool initializeSelf;



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



    //Dash
    private int dashesLeft;
    private bool dashRefilling;
    private Vector2 lastDashDir;
    private bool isDashAttacking;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AnimHandler = GetComponent<PlayerAnimator>();
    }

    private void Start()
    {
        cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
        if (initializeSelf)
        {
            SetAlive();
            IsFacingRight = true;
        }
    }

    private void Update()
    {
        moveInput = gameInput.getMovementVectorNormalized();
        if (moveInput.x != 0)
        {
            CheckDirectionToFace(moveInput.x > 0);
        }
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                Run(data.wallJumpRunLerp);
            }
            else
            {
                Run(1);
            }
        }
        else if (isDashAttacking)
        {
            Run(data.dashEndRunLerp);
        }
    }

    public void SetAlive()
    {
        unitIsAlive = true;
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = moveInput.x * data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        if (LastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;
        }

        if (data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }

        float speedDif = targetSpeed - RB.velocity.x;
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight) { Turn(); }
    }

    private void Turn()
    {
        if (IsFacingRight)
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
}