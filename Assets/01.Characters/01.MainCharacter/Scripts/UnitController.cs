using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LubyAdventure
{

    public class UnitController : MonoBehaviour
    {
        [Header("Data")]
        public UnitInfoData Data;

        [Header("Movement")]
        [SerializeField] private GameInput gameInput;
        public Rigidbody2D RB;
        private Vector2 moveInput;

        [Header("Health Settings")]
        private bool unitIsAlive;

        [Header("Ability Settings")]

        [Header("Animation Settings")]
        public UnitCharacterAnimationBehaviour characterAnimationBehaviour;

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

        // LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask groundLayer;

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

        //Walk
        private bool isWalking;

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

        public float LastPressedJumpTime { get; private set; }
        public float LastPressedDashTime { get; private set; }

        private void Start()
        {
            if (initializeSelf)
            {
                SetAlive();

                cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
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
            LastPressedDashTime = Time.deltaTime;

            //Debug.Log(LastOnGroundTime);

            Debug.Log(LastPressedJumpTime);

            moveInput = gameInput.getMovementVectorNormalized();

            if (moveInput.x != 0)
            {
                CheckDirectionToFace(moveInput.x > 0);
            }

            //Jumps
            if (gameInput.IsJumpingPress())
            {
                OnJumpInput();
            }

            if (gameInput.IsJumpingReleases())
            {
                OnJumpUpInput();
            }

            //Dash
            if (gameInput.IsDash())
            {
                OnDashInput();
            }

            if (!IsDashing && !IsJumping)
            {
                //Ground Check
                if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)) //checks if set box overlaps with ground
                {
                    if (LastOnGroundTime < -0.1f)
                    {
                        Debug.Log("Physics2D.OverlapBox");
                        characterAnimationBehaviour.justLanded = true;
                    }

                    LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
                }

                //Right Wall Check
                if (((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && IsFacingRight)
                        || (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !IsFacingRight)) && !IsWallJumping)
                    LastOnWallRightTime = Data.coyoteTime;

                //Right Wall Check
                if (((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !IsFacingRight)
                    || (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && IsFacingRight)) && !IsWallJumping)
                    LastOnWallLeftTime = Data.coyoteTime;

                //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
                LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
            }

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
                //Debug.Log(CanJump());

                    
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
                //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
                Sleep(Data.dashSleepTime);

                //If not direction pressed, dash forward
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


            //characterAnimationBehaviour

            // SLIDE CHECKS
            if (CanSlide() && ((LastOnWallLeftTime > 0 && moveInput.x < 0) || (LastOnWallRightTime > 0 && moveInput.x > 0))) { IsSliding = true; }
            else { IsSliding = false; }

            // GRAVITY
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


        private void FixedUpdate()
        {
            if (!IsDashing)
            {
                if (IsWallJumping) { Run(Data.wallJumpRunLerp); }
                else { Run(1); }
            }
            else if (isDashAttacking) { Run(Data.dashEndRunLerp); }
            if (IsSliding) { Slide(); }
        }


        //INPUT CALLBACKS
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
            RB.gravityScale = scale;
        }

        private void Sleep(float duration)
        {
            //Method used so we don't need to call StartCoroutine everywhere
            //nameof() notation means we don't need to input a string directly.
            //Removes chance of spelling mistakes and will improve error messages if any
            StartCoroutine(nameof(PerformSleep), duration);
        }

        private IEnumerator PerformSleep(float duration)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
            Time.timeScale = 1;
        }

        public void SetAlive()
        {
            unitIsAlive = true;
        }

        private void Run(float lerpAmount)
        {
            //Calculate the direction we want to move in and our desired velocity
            float targetSpeed = moveInput.x * Data.runMaxSpeed;
            //We can reduce are control using Lerp() this smooths changes to are direction and speed
            targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

            #region Calculate AccelRate
            float accelRate;

            //Gets an acceleration value based on if we are accelerating (includes turning) 
            //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
            #endregion

            #region Add Bonus Jump Apex Acceleration
            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((IsJumping || IsWallJumping || isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }
            #endregion

            #region Conserve Momentum
            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                //Prevent any deceleration from happening, or in other words conserve are current momentum
                //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }
            #endregion

            //Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - RB.velocity.x;
            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelRate;

            //Convert this to a vector and apply to rigidbody
            RB.AddForce(movement * Vector2.right, ForceMode2D.Force);


            // Walk
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            isWalking = moveDir != Vector3.zero;


            /*
             * For those interested here is what AddForce() will do
             * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
             * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
            */
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

        

        public bool IsWalking()
        {
            return isWalking;
        }


        private bool CanJump()
        {
            //Debug.Log("+++++++++++ !IsJumping ==== ?= " + (!IsJumping));

            Debug.Log("+++++++++++ LastOnGroundTime > 0  ?= " + (LastOnGroundTime > 0));

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

    }

}
