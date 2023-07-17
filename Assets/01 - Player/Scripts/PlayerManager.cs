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
    //private Vector2 inputVector;
    private Vector2 moveInput;

    //private Vector2 moveInput;


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
    //private RaycastHit2D groundHit;
    // ---------------------



    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AnimHandler = GetComponent<PlayerAnimator>();
    }
    private void Start()
    {
        cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }

    void Update()
    {
        // TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        // INPUT HANDLER
        moveInput = gameInput.getMovementVectorNormalized();

        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);

        //Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //transform.position += moveDir * Data.maxFallSpeed * Time.deltaTime;
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // Walking
        isWalking = moveDir != Vector3.zero;

        // Shoot
        isShoot = gameInput.IsShoot();

        // Jump
        IsJumping = gameInput.IsJumping();

        // JUMP CHECKS
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

            isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            isJumpCut = false;

            isJumpFalling = false;
        }

        
        if(!IsDashing)
        {
            //Debug.Log("+++ LastPressedJumpTime: " + LastPressedJumpTime);

            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                Debug.Log("+++ IsJumping: " + IsJumping + " +++ LastPressedJumpTime: " + LastPressedJumpTime);
                
                IsJumping = true;
                IsWallJumping = false;
                isJumpCut = false;
                isJumpFalling = false;

                Jump();

                AnimHandler.startedJumping = true;
            }
            //WALL JUMP
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                isJumpCut = false;
                isJumpFalling = false;

                wallJumpStartTime = Time.time;
                lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

                WallJump(lastWallJumpDir);
            }
        }

        if (CanDash() && LastPressedDashTime > 0)
        {
            Sleep(Data.dashSleepTime);

            if (moveInput != Vector2.zero)
                lastDashDir = moveInput;
            else
                lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            isJumpCut = false;

            StartCoroutine(nameof(StartDash), lastDashDir);
        }

        // SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && moveInput.x < 0) || (LastOnWallRightTime > 0 && moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;


    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput * moveSpeed, RenderBuffer.velocity.y);
       //RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -maxFallSpeed, maxFallSpeed * 5));

        /*
        if(inputVector.x > 0 || inputVector.x < 0)
        {
            TurnCheck();
        }
        */


        //Handle Run
        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                Run(Data.wallJumpRunLerp);
                //isWalking = false;
            }
            else
            {
                Run(1);
                //isWalking = false;
            }
        }
        else if (isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
            //isWalking = false;
        }

        //Handle Slide
        if (IsSliding)
            Slide();
    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void TurnCheck()
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            Turn();
        }
    }

    private void Run(float lerpAmount)
    {
        //Debug.Log("run");

        float targetSpeed = moveInput.x * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;

        if ((IsJumping || IsWallJumping || isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }

        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }

        float speedDif = targetSpeed - RB.velocity.x;
        float movement = speedDif * accelRate;

        //Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        //transform.position += moveDir * Data.maxFallSpeed * Time.deltaTime;

        //isWalking = moveDir != Vector3.zero;
        //isWalking = moveDir != Vector3.zero;
        //isWalking = true;

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        //isWalking = false;
    }

    private void Turn()
    {
        //Vector3 scale = transform.localScale;
        //scale.x *= -1;
        //transform.localScale = scale;

        //IsFacingRight = !IsFacingRight;
       
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

    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void WallJump(int dir)
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0)
            force.y -= RB.velocity.y;

        RB.AddForce(force, ForceMode2D.Impulse);
    }


    //Short period before the player is able to dash again
    private IEnumerator RefillDash(int amount)
    {
        dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        dashRefilling = false;
        dashesLeft = Mathf.Min(Data.dashAmount, dashesLeft + 1);
    }

    private void Slide()
    {
        if (RB.velocity.y > 0)
        {
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }

    private IEnumerator StartDash(Vector2 dir)
    {

        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        dashesLeft--;
        isDashAttacking = true;

        SetGravityScale(0);

        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            yield return null;
        }

        startTime = Time.time;

        isDashAttacking = false;

        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        IsDashing = false;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsShoot()
    {
        return isShoot;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        //Debug.Log("CanJump() +++ IsJumping --> " + IsJumping);
        //Debug.Log("+++ LastOnGroundTime: " + LastOnGroundTime + " +++ && IsJumping: " + IsJumping);
        return LastOnGroundTime > 0 && !IsJumping;
    }


    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }

    private bool CanDash()
    {
        if (!IsDashing && dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return dashesLeft > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }

    private void Sleep(float duration)
    {
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); 
        Time.timeScale = 1;
    }

}
