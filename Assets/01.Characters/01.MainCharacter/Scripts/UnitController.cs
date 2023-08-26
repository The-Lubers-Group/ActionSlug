using LubyAdventure;
using System.Collections.Generic;
using Unity.VisualScripting;

using System.Collections;
using UnityEngine;

using UnityEngine.EventSystems;
using DG.Tweening;

namespace LubyAdventure
{

    public class UnitController : MonoBehaviour
    {
        [Header("Data")]
        public UnitInfoData Data;
        
        private CollisionManager coll;
        
        [Header("Movement")]
        [HideInInspector] public GameInput gameInput;
        [HideInInspector] public Rigidbody2D RB;
        [HideInInspector] public HingeJoint2D HJ;

        [Header("Health Settings")]
        public UnitHealthBehaviour healthBehaviour;
        private bool unitIsAlive;

        [SerializeField] private float PushForce = 10f;
        [SerializeField] private bool attached = false;

        [SerializeField] private Transform attachedTo;
        [SerializeField] private GameObject disregard;

        [Header("Animation Settings")]
        public UnitCharacterAnimationBehaviour characterAnimationBehaviour;

        [Header("Audio Settings")]

        [Header("Camera Stuff")]
        [SerializeField] private CameraFollowObject cameraFollowObject;
        [SerializeField] private GameObject cameraFollowObjectGO;

        public bool IsFacingRight { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsWallJumping { get; private set; }
        public bool IsDashing { get; private set; }
        public bool IsSliding { get; private set; }
        public bool IsSwimming { get; private set; }
        
        public bool IsClimbing;


        public bool IsTouchingLedge { get; private set; }


        public float LastOnGroundTime { get; private set; }
        public float LastOnWallTime { get; private set; }
        public float LastOnWallRightTime { get; private set; }
        public float LastOnWallLeftTime { get; private set; }

        public float swimmingTime { get; private set; }

        private bool isWalking;

        private bool isJumpCut;
        private bool isJumpFalling;

        private float wallJumpStartTime;
        private int lastWallJumpDir;

        private int dashesLeft;
        private bool dashRefilling;
        private Vector2 lastDashDir;
        private bool isDashAttacking;

        public Vector2 moveInput;

        public float LastPressedJumpTime { get; private set; }
        public float LastPressedDashTime { get; private set; }

        [Space(5)]
        [Header("Checks - Ledge Climb")]
        [SerializeField] private Transform ledgeCheckPoint;
        [SerializeField] private Vector2 ledgeClimbSize = new Vector2(1.3f, 0.5f);

        private bool canClibLedge = false;
        private bool ledgeDetected;

        private Vector2 ledgePosBot;
        private Vector2 ledgePos1;
        private Vector2 ledgePos2;

        [SerializeField] private float ledgeClimbBoXOffset1 = 0.3f;
        [SerializeField] private float ledgeClimbBoYOffset1 = 0f;
        [SerializeField] private float ledgeClimbBoXOffset2 = 0.5f;
        [SerializeField] private float ledgeClimbBoYOffset2 = 0.5f;


        // LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask groundLayer;

        [Header("Debug")]
        public bool initializeSelf;

        private void Start()
        {
            coll = GetComponent<CollisionManager>();
            RB = GetComponent<Rigidbody2D>();
            HJ = GetComponent<HingeJoint2D>();
            gameInput = FindAnyObjectByType<GameInput>();
            cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();

            if (initializeSelf)
            {
                //SetAlive();
                SetGravityScale(Data.gravityScale);
                IsFacingRight = true;
            }

        }

        private void Update()
        {
            LastOnGroundTime -= Time.deltaTime;
            LastOnWallTime -= Time.deltaTime;
            LastOnWallRightTime -= Time.deltaTime;
            LastOnWallLeftTime -= Time.deltaTime;

            LastPressedJumpTime -= Time.deltaTime;
            LastPressedDashTime -= Time.deltaTime;

            IsSwimming = Physics2D.OverlapBox(RB.position, RB.transform.localScale, 0, 1 << 4);

            CanSwim();

            moveInput = gameInput.getMovementVectorNormalized();

            if (moveInput.x != 0)
                CheckDirectionToFace(moveInput.x > 0);

            if (moveInput.x < 0 && attached)
            {
                RB.AddRelativeForce(new Vector3(-1, 0, 0) * PushForce);
            }

            if (moveInput.x > 0 && attached)
            {
                RB.AddRelativeForce(new Vector3(1, 0, 0) * PushForce);
            }

            // Climb Up
            if (moveInput.y > 0 && attached)
            {
                SlideRope(1);
            }

            if (moveInput.y < 0 && attached)
            {
                SlideRope(-1);
            }

            //Jumps
            if (gameInput.IsJumpingPress())
            {
                //Jump();
                OnJumpInput();
                if (IsSwimming)
                {
                    ForceSwimming();
                }

            }

            if (gameInput.IsJumpingReleases())
            {
                if (!IsSwimming)
                {
                    OnJumpUpInput();

                    if (attached)
                    {
                        Detach();
                        //attached = false;
                    }
                }
            }

            //Dash
          
            if (gameInput.IsDash())
            {
                OnDashInput();
            }
          

            if (!IsDashing && !IsJumping)
            {
                if (coll.onGround) 
                {
                    if (LastOnGroundTime < -0.1f)
                    {
                        characterAnimationBehaviour.justLanded = true;
                    }

                    LastOnGroundTime = Data.coyoteTime;
                }

                //Right Wall Check
                if( (coll.onLeftWall && IsFacingRight || coll.onRightWall && !IsFacingRight) && !IsWallJumping)
                {
                    LastOnWallRightTime = Data.coyoteTime;
                }

                if((coll.onLeftWall && !IsFacingRight || coll.onRightWall && IsFacingRight) && !IsWallJumping)
                {
                    LastOnWallLeftTime = Data.coyoteTime;
                }


                //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
                LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
            }
            
            CanSlider();
            CanLedgeClimb();

            if (!IsSwimming)
            {

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

                if (!IsDashing)
                {
                    //Jump
                    if (CanJump() && LastPressedJumpTime > 0)
                    {
                        IsJumping = true;
                        IsWallJumping = false;
                        isJumpCut = false;
                        isJumpFalling = false;
                        Jump();

                        characterAnimationBehaviour.startedJumping = true;
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
                    Camera.main.transform.DOComplete();
                    Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
                    print("dasd");

                    Sleep(Data.dashSleepTime);

                    if (moveInput != Vector2.zero)
                        lastDashDir = moveInput;
                    else
                        lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

                    IsDashing = true;
                    IsJumping = false;
                    IsWallJumping = false;
                    isJumpCut = false;

                    characterAnimationBehaviour.OnIsDashingAnim(IsDashing);
                    StartCoroutine(nameof(StartDash), lastDashDir);
                }

                if (CanSlide() && ((LastOnWallLeftTime > 0 && moveInput.x < 0) || (LastOnWallRightTime > 0 && moveInput.x > 0)))
                {
                    IsSliding = true;
                }
                else
                {
                    IsSliding = false;
                }

                if (!isDashAttacking)
                {
                    if (IsSliding)
                    {
                        SetGravityScale(0);
                    }
                    else if (RB.velocity.y < 0 && moveInput.y < 0)
                    {
                        SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                        RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
                    }
                    else if (isJumpCut)
                    {
                        SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                        RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
                    }
                    else if ((IsJumping || IsWallJumping || isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
                    {
                        SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
                    }
                    else if (RB.velocity.y < 0)
                    {
                        SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                        RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
                    }
                    else if (!IsClimbing)
                    {
                        SetGravityScale(Data.gravityScale);
                    }
                    else
                    {
                        SetGravityScale(Data.gravityScale);
                    }
                }
                else
                {
                    SetGravityScale(0);
                }

            }

        }

        private void FixedUpdate()
        {
            if (!IsDashing)
            {
                if (IsWallJumping)
                    Run(Data.wallJumpRunLerp);
                else
                    Run(1);
            }
            else if (isDashAttacking)
            {
                Run(Data.dashEndRunLerp);
            }

            if (IsSliding)
                Slide();
        }
        public void OnJumpInput()
        {
            LastPressedJumpTime = Data.jumpInputBufferTime;
        }

        public void OnJumpUpInput()
        {
            if (CanJumpCut() || CanWallJumpCut())
                isJumpCut = true;
        }

        public void OnDashInput()
        {
            LastPressedDashTime = Data.dashInputBufferTime;
        }

        public void SetGravityScale(float scale)
        {
            if (IsSwimming)
            {
                RB.gravityScale = Data.gravitySwimming;
            }
            else
            {
                RB.gravityScale = scale;
            }
        }

        private void Sleep(float duration)
        {
            StartCoroutine(nameof(PerformSleep), duration);
        }
        private IEnumerator PerformSleep(float duration)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
            Time.timeScale = 1;
        }

        private void Run(float lerpAmount)
        {
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
            RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
            Vector3 moveDir = new Vector3(moveInput.x, 0f, 0f);
            isWalking = moveDir != Vector3.zero;
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

        private void Jump()
        {
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;
          
            float force = Data.jumpForce;
            if (RB.velocity.y < 0)
                force -= RB.velocity.y;

            RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        private void ForceSwimming()
        {
            characterAnimationBehaviour.SwimmingAnim(false);
            RB.velocity = new Vector2(RB.velocity.x, 0);

            float force = Data.forceSwimming;

            RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            swimmingTime = 0.3f;
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

      
        private IEnumerator StartDash(Vector2 dir)
        {
            //FindObjectOfType<GhostTrail>().ShowGhost();

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

            DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

            startTime = Time.time;

            isDashAttacking = false;

            SetGravityScale(Data.gravityScale);
            RB.velocity = Data.dashEndSpeed * dir.normalized;

            while (Time.time - startTime <= Data.dashEndTime)
            {
                yield return null;
            }

            IsDashing = false;
            characterAnimationBehaviour.OnIsDashingAnim(IsDashing);


        }
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


        public void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
            {
                Turn();
            }

        }

        public bool IsWalking()
        {
            return isWalking;
        }

        private bool CanJump()
        {
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

        public void Attach(Rigidbody2D ropeBone)
        {
            ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
            HJ.connectedBody = ropeBone;
            HJ.enabled = true;
            attached = true;
            attachedTo = ropeBone.gameObject.transform.parent;
        }

        void Detach()
        {
            HJ.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
            attached = false;
            HJ.enabled = false;
            HJ.connectedBody = null;
        }

        void SlideRope(int direction)
        {
            RopeSegment myConnection = HJ.connectedBody.gameObject.GetComponent<RopeSegment>();
            GameObject newSeg = null;
            if (direction > 0)
            {
                if (myConnection.connectAbove != null)
                {
                    if (myConnection.connectAbove.gameObject.GetComponent<RopeSegment>() != null)
                    {
                        newSeg = myConnection.connectAbove;
                    }
                }
            }
            else
            {
                if (myConnection.connectBelow != null)
                {
                    newSeg = myConnection.connectBelow;
                }
            }
            if (newSeg != null)
            {
                transform.position = newSeg.transform.position;
                myConnection.isPlayerAttached = false;
                newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
                HJ.connectedBody = newSeg.GetComponent<Rigidbody2D>();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!attached)
            {
                if (collision.CompareTag("Rope"))
                {
                    if (attachedTo != collision.gameObject.transform.parent)
                    {
                        if (disregard == null || collision.gameObject.transform.parent.gameObject != disregard)
                        {
                            Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                        }
                    }
                }

            }
        }

        private void CanSwim()
        {
            if (IsSwimming)
            {
                characterAnimationBehaviour.SwimmingAnim(true);
                RB.drag = Data.linerDragSwimming;

                // Movemnt swimming
                if (moveInput.x < 0)
                {
                    Physics2D.gravity = new Vector2(0, Data.gravitySwimming);
                }
                else if (moveInput.x > 0)
                {
                    Physics2D.gravity = new Vector2(0, -Data.gravitySwimming);
                }
                else if (moveInput.y < 0)
                {
                    Physics2D.gravity = new Vector2(-Data.gravitySwimming, 0);
                }
                else if (moveInput.y > 0)
                {
                    Physics2D.gravity = new Vector2(Data.gravitySwimming, 0);
                }
            }
            else if (!IsSwimming)
            {
                characterAnimationBehaviour.SwimmingAnim(false);

                RB.drag = Data.linerDrag;

                Physics2D.gravity = new Vector2(0, -9.81f);
            }
        }

        private void CanSlider()
        {
            if (!IsSwimming && coll.onLeftWall && !coll.onGround && !CanJump() && !canClibLedge && LastOnGroundTime < -.3f)
            {
                characterAnimationBehaviour.SetWallSliderAnim(true);
            }
            else
            {
                characterAnimationBehaviour.SetWallSliderAnim(false);
            }

            if (!IsSwimming && !coll.onLeftWall && !coll.onGround && !CanJump())
            {
                characterAnimationBehaviour.SetWallSliderAnim(false);
            }
        }
        private void CanLedgeClimb()
        {
            if (!coll.onSpaceGround && !coll.onLedge && coll.onLeftWall && !IsSwimming)
            {
                canClibLedge = true;
                ledgePosBot = coll.leftOffset.position;

                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + 1f) - ledgeClimbBoXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset1); ;
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + 1f) + ledgeClimbBoXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset2);

                if (canClibLedge)
                {
                   // characterAnimationBehaviour.OnLedgeClimbAnim(canClibLedge);
                    transform.position = ledgePos1;
                    canClibLedge = false;

                }
            }
        }

        public void FinishLedgeClimb()
        {
            transform.position = ledgePos1;
            canClibLedge = false;
            transform.position = ledgePos2;
            ledgeDetected = false;
            characterAnimationBehaviour.OnLedgeClimbAnim(canClibLedge);
        }
        void RigidbodyDrag(float x)
        {
            RB.drag = x;
        }
    }

}