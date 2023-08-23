using LubyAdventure;
using System.Collections.Generic;
using Unity.VisualScripting;

using System.Collections;
using UnityEngine;

using UnityEngine.EventSystems;

namespace LubyAdventure
{

    public class UnitController : MonoBehaviour
    {
        [Header("Data")]
        //Scriptable object which holds all the player's movement parameters. If you don't want to use it
        //just paste in all the parameters, though you will need to manuly change all references in this script
        public UnitInfoData Data;

        [Header("Movement")]
        [SerializeField] private GameInput gameInput;
        public Rigidbody2D RB;
        public HingeJoint2D HJ;

        [Header("Health Settings")]
        public UnitHealthBehaviour healthBehaviour;
        private bool unitIsAlive;

        [Header("Ability Settings")]

        //Climb
        //[SerializeField] private float climbSpeed = 4f;
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

        //private float vertical;
        //private HashSet<GameObject> ladders = new HashSet<GameObject>();

        //STATE PARAMETERS
        //Variables control the various actions the player can perform at any time.
        //These are fields which can are public allowing for other sctipts to read them
        //but can only be privately written to.
        public bool IsFacingRight { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsWallJumping { get; private set; }
        public bool IsDashing { get; private set; }
        public bool IsSliding { get; private set; }


        public bool IsTouchingLedge { get; private set; }


        //Timers (also all fields, could be private and a method returning a bool could be used)
        public float LastOnGroundTime { get; private set; }
        public float LastOnWallTime { get; private set; }
        public float LastOnWallRightTime { get; private set; }
        public float LastOnWallLeftTime { get; private set; }
        
        
        public bool IsSwimming { get; private set; }
        public float swimmingTime { get; private set; }

        //[SerializeField] private float forceSwimming = 4f;

        //private bool IsSwimming = false;

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

        public Vector2 moveInput;

        public float LastPressedJumpTime { get; private set; }
        public float LastPressedDashTime { get; private set; }


        [Header("Checks")]
        [SerializeField] private Transform groundCheckPoint;
        //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
        [Space(5)]
        [SerializeField] private Transform frontWallCheckPoint;
        [SerializeField] private Transform backWallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);

        /// <summary>
        /// Ledge Climb
        /// </summary>
        /// 
        [Space(5)]
        [Header("Checks - Ledge Climb")]
        [SerializeField] private Transform ledgeCheckPoint;
        [SerializeField] private Vector2 ledgeClimbSize = new Vector2(1.3f, 0.5f);

        private bool canClibLedge = false;
        private bool ledgeDetected;

        private Vector2 ledgePosBot;
        private Vector2 ledgePos1;
        private Vector2 ledgePos2;

        [SerializeField] private float ledgeClimbBoXOffset1 = 0f;
        [SerializeField] private float ledgeClimbBoYOffset1 = 0f;
        [SerializeField] private float ledgeClimbBoXOffset2 = 0f;
        [SerializeField] private float ledgeClimbBoYOffset2 = 0f;


        // LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Debug")]
        public bool initializeSelf;

        private void Start()
        {

            if (initializeSelf)
            {
                //SetAlive();

                //groundLayer = LayerMask.GetMask("Ground");
                //waterLayer = LayerMask.GetMask("Water");

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
            LastPressedDashTime -= Time.deltaTime;

            //Debug.Log(" moveInput.x: " + moveInput.x + " moveInput.y: " + moveInput.y);
           
            
            IsSwimming = Physics2D.OverlapBox(RB.position, RB.transform.localScale, 0, 1 << 4);
            //Debug.Log(Physics2D.gravity);



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
            else if (!IsSwimming){
                characterAnimationBehaviour.SwimmingAnim(false);

                RB.drag = Data.linerDrag;

                Physics2D.gravity = new Vector2(0, -9.81f);
            }



            

            //Debug.Log(Physics2D.gravity);


            moveInput = gameInput.getMovementVectorNormalized();

            if (moveInput.x != 0)
                CheckDirectionToFace(moveInput.x > 0);

            /*
            if(ladders.Count > 0 && moveInput.y > 0)
            {
                IsClimbing = true;
            } else if(ladders.Count <= 0)
            {
                IsClimbing = false;
            }
            */

            if(moveInput.x < 0 && attached)
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
                if(IsSwimming)
                {
                  ForceSwimming();
                }

            }

            if (gameInput.IsJumpingReleases())
            {
               if(!IsSwimming)
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
                //Ground Check.GetMask("Player","Ground")
                if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)) //checks if set box overlaps with ground
                {
                   // Debug.Log("sadadasdsadasd");
                    if (LastOnGroundTime < -0.1f)
                    {
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

            //SET Anim Wall sider
            if (!IsSwimming && Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && !CanJump() && !canClibLedge)
            {
                characterAnimationBehaviour.SetWallSliderAnim(true);
            }
            else
            {
                characterAnimationBehaviour.SetWallSliderAnim(false);
            }

            if (!IsSwimming && !Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && !CanJump())
            {
                characterAnimationBehaviour.SetWallSliderAnim(false);
            }

            LedgeClimb();



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
                    //Debug.Log("(CanWallJump() && LastPressedJumpTime > 0): " + (CanWallJump() && LastPressedJumpTime > 0));
                    //Debug.Log("(CanWallJump()): " + (CanWallJump()));
                    //Debug.Log("(LastPressedJumpTime > 0): " + (LastPressedJumpTime > 0));

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

        }

        private void FixedUpdate()
        {

            //Handle Run
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

            //Handle Slide
            if (IsSliding)
                Slide();

            /*
            if(IsClimbing)
            {
                //RB.gravityScale = 0f;
                SetGravityScale(0);
                RB.velocity = new Vector2(RB.velocity.x, moveInput.y * climbSpeed);

            }
            else
            {
                SetGravityScale(Data.gravityScale);
            }
            */
        }


        //Methods which whandle input detected in Update()
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
            //Debug.Log("RB.gravityScale: " + RB.gravityScale);
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
            //Vector3 moveDir = new Vector3(moveInput.x,  moveInput.y, 0f);
            Vector3 moveDir = new Vector3(moveInput.x,  0f, 0f);


            //Debug.Log(moveInput.x);
            isWalking = moveDir != Vector3.zero;
            //isWalking = moveDir != moveInput.x;



            /*
             * For those interested here is what AddForce() will do
             * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
             * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
            */
        }


        private void Turn()
        {
            //stores scale and flips the player along the x axis, 

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

            /*
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
            */
        }

        private void Jump()
        {
            //Ensures we can't call Jump multiple times from one press
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            //We increase the force applied if we are falling
            //This means we'll always feel like we jump the same amount 
            //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
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



            //RB.AddForce(new Vector2(0, Data.forceSwimming), ForceMode2D.Force);
            //RB.AddForce(Vector2.up * force, ForceMode2D.Force);
            RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            swimmingTime = 0.3f;
            //LastPressedJumpTime = 0;
            //LastOnGroundTime = 0;
            //float force = Data.jumpForce;
            //if (RB.velocity.y < 0)
            //    force -= RB.velocity.y;
            //RB.AddForce(new Vector2(RB.velocity.y, forceSwimming), ForceMode2D.Impulse);
        }

        private void WallJump(int dir)
        {
            //Debug.Log("private void WallJump(int dir)");
            

            //Ensures we can't call Wall Jump multiple times from one press
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;
            LastOnWallRightTime = 0;
            LastOnWallLeftTime = 0;

            Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
            force.x *= dir; //apply force in opposite direction of wall

            if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
                force.x -= RB.velocity.x;

            if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
                force.y -= RB.velocity.y;

            
            //Unlike in the run we want to use the Impulse mode.
            //The default mode will apply are force instantly ignoring masss
            RB.AddForce(force, ForceMode2D.Impulse);
        }


        private IEnumerator StartDash(Vector2 dir)
        {
            //Overall this method of dashing aims to mimic Celeste, if you're looking for
            // a more physics-based approach try a method similar to that used in the jump

            LastOnGroundTime = 0;
            LastPressedDashTime = 0;

            float startTime = Time.time;

            dashesLeft--;
            isDashAttacking = true;

            SetGravityScale(0);

            //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
            while (Time.time - startTime <= Data.dashAttackTime)
            {
                RB.velocity = dir.normalized * Data.dashSpeed;
                //Pauses the loop until the next frame, creating something of a Update loop. 
                //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
                yield return null;
            }

            startTime = Time.time;

            isDashAttacking = false;

            //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
            SetGravityScale(Data.gravityScale);
            RB.velocity = Data.dashEndSpeed * dir.normalized;

            while (Time.time - startTime <= Data.dashEndTime)
            {
                yield return null;
            }

            //Dash over
            IsDashing = false;
        }
        private IEnumerator RefillDash(int amount)
        {
            //SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
            dashRefilling = true;
            yield return new WaitForSeconds(Data.dashRefillTime);
            dashRefilling = false;
            dashesLeft = Mathf.Min(Data.dashAmount, dashesLeft + 1);
        }

        private void Slide()
        {
            //We remove the remaining upwards Impulse to prevent upwards sliding
            if (RB.velocity.y > 0)
            {
                RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
            }

            //Works the same as the Run but only in the y-axis
            //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
            float speedDif = Data.slideSpeed - RB.velocity.y;
            float movement = speedDif * Data.slideAccel;
            //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
            //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
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
            //Debug.Log(" ========== private bool CanJump() ========== ");
            //Debug.Log("(LastOnGroundTime > 0): " + (LastOnGroundTime > 0));
            //Debug.Log("(!IsJumping): " + (!IsJumping));
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
            /*
            if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
            {
                Debug.Log("(public bool CanSlide()): " + (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0));
                characterAnimationBehaviour.SetWallSliderAnim(true);
            }
            else
            {
                characterAnimationBehaviour.SetWallSliderAnim(false);
            }
            */

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

            Debug.Log("Attach: " + attached);
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
            if(direction > 0)
            {
                if(myConnection.connectAbove != null)
                {
                    if(myConnection.connectAbove.gameObject.GetComponent<RopeSegment>() != null)
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
            if(newSeg != null) 
            {
                transform.position = newSeg.transform.position;
                myConnection.isPlayerAttached = false;
                newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
                HJ.connectedBody = newSeg.GetComponent<Rigidbody2D>();
            }
        }

        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!attached)
            {
                //if (GameObject.FindWithTag("Rope"))
               
                if (collision.CompareTag("Rope"))
                {
                    if(attachedTo != collision.gameObject.transform.parent)
                    {
                        if(disregard == null || collision.gameObject.transform.parent.gameObject != disregard)
                        {
                            Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                        }
                    }
                }
                
            }
        }

        private void CheckLedgeClimb()
        {
            if(!IsSwimming && ledgeDetected && !canClibLedge)
            {
                canClibLedge = true;

                if(IsFacingRight)
                {
                    ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + 0) - ledgeClimbBoXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset1);
                    ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + 0) + ledgeClimbBoXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset2);
                }
                else
                {
                    ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x + 0) + ledgeClimbBoXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset1);
                    ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x + 0) - ledgeClimbBoXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset2);
                }
            }
            if(canClibLedge)
            {
                transform.position = ledgePos1;
            }
        }

        public void FinishLedgeClimb()
        {
            canClibLedge = false;
            transform.position = ledgePos2;
            ledgeDetected = false; 
        }

        private void LedgeClimb()
        {

            /*
            IsTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, 0, groundLayer);
            if (Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !IsTouchingLedge && !ledgeDetected)
            {
                ledgeDetected = true;
                ledgePosBot = frontWallCheckPoint.position;
            }
            */

            if (!IsSwimming)
            {
                if ((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer))
                &&
                (!Physics2D.OverlapBox(ledgeCheckPoint.position, wallCheckSize, 0, groundLayer))
                &&
                (!Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)))
                {
                    canClibLedge = true;
                    Debug.Log("Pode Subir na quina");
                    ledgePosBot = frontWallCheckPoint.position;

                    ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + .5f) - ledgeClimbBoXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset1); ;
                    ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + .5f) + ledgeClimbBoXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbBoYOffset2);
                }


                if (canClibLedge)
                {
                    transform.position = ledgePos1;
                }






            }

            


            //CheckLedgeClimb();
        }










        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(frontWallCheckPoint.position, wallCheckSize);
            Gizmos.DrawWireCube(backWallCheckPoint.position, wallCheckSize);
            
            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(ledgeCheckPoint.position, ledgeClimbSize);
        }

    }

}
