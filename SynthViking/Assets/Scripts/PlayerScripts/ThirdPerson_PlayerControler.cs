using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using MoreMountains.Feedbacks; 
using System;

public class ThirdPerson_PlayerControler : MonoBehaviour
{
    [Header("GROUND MOVEMENT")] //GROUND MOVEMENT
    #region

    //Moving
    public float maxMoveSpeed;
    public float sprintMoveSpeed;
    public float minWalkingMoveSpeed;
    public float minRunningMoveSpeed;
    private float currentMinMoveSpeed;
    private bool topSpeedReached;

    public float currentMoveSpeed;
    private float moveVelocity = 0.0f;
    private float groundStopDrag = 8f;

    [HideInInspector] public Vector3 moveInput;
    private Vector3 moveDirection;


    //Rotation
    public float turnLerpValue = .3f;
    private float turnLerpTimer;
    [HideInInspector] public Vector2 targetTurnRotation;

    #endregion

    [Header("AIR MOVEMENT")]  //AIR MOVEMENT
    #region
    //Jumping 
    public float jumpForce;
    public float maxJumps;
    private int jumpCount;
    private float randomJumpVar = 2;
    public bool hasJumped = false;

    private float jumpDelayTimer;
    private float landingDelayTimer;
    private float gravity = 1f;
    public float fallMultiplier = 5f;
    public float lowJumpGravity;
    //[Range(0, 1)] public float airMSpeedReduction;
    public float airMoveSpeed;
    public float airDrag = 2f;


    private float groundCheckHeight = .4f;
    private float landCheckHeight = 2.5f;
    private float groundCheckJumpDelay = .5f; //Temporaly turn of the groundcheck after jumping 
    private float groundCheckTimer;


    #endregion

    [Header("DASH MOVEMENT")]
    #region

    public float dashForwardForce;
    public float dashDelayDuration;
    public float minDashTime;
    public float maxDashTime;
    private float dashDelayTimer;
    public float dashBuildUpSpeed;
    private float currentDashTime;

    private float dashCooldownTimer;
    public float dashCooldownDuration;
    public bool solidDashObjectReached;
    public bool enemyDashObjectReached;
    private bool dashEndMove;
    private Vector3 dashDirection;
    public Transform dashChecker;
    public Image dashChargeIndicator;
    public Image dashChargeBackground;
    public Vector3 originalIndicatorSize; 

    #endregion

    [Header("DASH ATTACK")]
    #region
    public float dashAttackForce;
    [HideInInspector] public GameObject dashAttackTarget;
    #endregion

    [Header("MELEE ATTACK")]
    #region   
    public float nextAttackDuration;
    public float nextAttackTimer;
    public float maxTimeBetweenAttacks; 
    public int currentComboLength; 
    public int totalComboLength;
    public float attackMoveSpeed;
    public bool attackTargetNearby;
     
    #endregion

    [Header("WALLRUNNING")] //Wallrunning; 
    #region
    public float wallRunMoveSpeed;
    public float wallRunUpSpeed;
    public float frontWallCheckDist;
    public float sideWallCheckDist;

    public Transform frontWallChecker;
    private LayerMask EnvorinmentLayer = 6;

    #endregion

    [Header("Animation")]  //ANIMATION
    #region  


    public AnimationClip jumpStartClip;
    public AnimationClip moveLandingClip;
    public AnimationClip noMoveLandingClip;
    public AnimationClip VerticalWallRunClip;
    public AnimationClip VerticalWallRunEndClip;
    int velocityHash;
    #endregion

    [Header("Camera")] //CAMERA
    #region
    public Camera playerCamera;
    public Transform aimPoint; 
    #endregion

    [Header("Scripts")] //SCRIPTS
    #region
    public PlayerInputCheck input;
    public PlayerState playerState;
    public AttackTargetScript attackTargetScript; 
    #endregion

    [Header("Components")] //COMPONENTS
    #region
    public Animator playerAnim;
    public Rigidbody playerRb;
    public SkinnedMeshRenderer meshR;
    public Material[] holoSkinMat;
    public Material[] defaultSkinMat;
    #endregion

    [Header("FEEDBACK")]
    #region
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks DashStartFeeback;
    public MMFeedbacks DashAttackFeeback;
    public MMFeedbacks LightAttackFeedback; 
    #endregion

    [Header("Switch Case")] //SWITCH CASE
    #region
    private int controllerState;
    private int fixedControllerState;
    [HideInInspector]
    public enum currentState
    {
        NOTHING,
        MOVING,
        WALLRUNNING,
        ATTACKING,
        DASHING,
    }
    #endregion

    [Header("States")]  //STATES
    #region
    public bool canMove = true;
    public bool canRotate = true;
    public bool canJump = true;
    public bool canSprint = true;
    public bool canLand = false;
    public bool canStartWallrun = true;
    public bool canDash = true;
    public bool canDashAttack = true;
    public bool canStartNewAttack; 
    //public bool canPauseWallrun = true;
    public bool canEndDash = false;

    public bool isMoving;
    public bool isAttacking; 
   // public bool isPausingWallrun;
    public bool isWallRunning;
    public bool isLanding;
    public bool isDashing;
    public bool isChargingDash;
    public bool isDashAttacking;
    public bool isWalking;
    public bool isSprinting;
    public bool isRunning;
    public bool isInAir;
    public bool isGrounded = true;

    public bool wasSprintingBeforeJump;
    /*
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canRotate = true;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool canLand = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool canStartWallrun = true;

    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isWallRunning; 
    [HideInInspector] public bool isLanding;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isInAir;
    [HideInInspector] public bool isGrounded = true;

   
    */
    #endregion



    private void Start()
    {
        velocityHash = Animator.StringToHash("MoveVelocity");
        //dashChecker.position, dashChecker.position + dashChecker.forward * 2f

        defaultSkinMat = meshR.materials; 
        //meshR.materials = holoSkinMat; 

        //Set default values
        currentMoveSpeed = maxMoveSpeed;
        currentDashTime = minDashTime;
        dashCooldownTimer = dashCooldownDuration;
        originalIndicatorSize = dashChargeIndicator.transform.localScale; 

        fixedControllerState = (int)currentState.NOTHING;
        controllerState = (int)currentState.MOVING;

        //Set default states
        ResetStates();

        playerRb.drag = groundStopDrag;
        dashDelayTimer = dashDelayDuration;
    }


    private void Update()
    {

        switch (controllerState)
        {
            case (int)currentState.MOVING:
                GroundCheck();
                HandleMoveSpeed();
                CheckForMoveInput();
                CheckForJump();
                CheckForWall();
                CheckForAttack(); 
                CheckForDamage();
                HandleDrag();
                CheckForDash();
                break;

            case (int)currentState.WALLRUNNING:
                CheckForJump();
                CheckForWall();
                CheckForDamage();
                CheckForDash(); 
                HandleMoveSpeed();
                CheckForMoveInput();
                HandleWallRunning();
                HandleDrag();
                break;

            case (int)currentState.ATTACKING:
                //HandleMoveSpeed();
                CheckForAttack();
               // CheckForMoveInput(); 
                GroundCheck();               
                HandleRotation();
                CheckForDamage();
                HandleMoveSpeed(); 
                break;

            case (int)currentState.DASHING:
                CheckForDash();
                break;
        }

        HandleAnimations();
    }


    private void FixedUpdate()
    {
        switch (fixedControllerState)
        {
            case (int)currentState.NOTHING:
                break;

            case (int)currentState.MOVING:
                MovePlayer();
                break;
            case (int)currentState.DASHING:
                DoDash();
                break;
            case (int)currentState.ATTACKING:
              //  MovePlayer(); 
                break;

        }
    }

    //Animations
    private bool CheckAnimationClip(string clipName)
    {
        if (playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == clipName) return true;
        else return false;
    }

    void HandleAnimations()
    {
        playerAnim.SetFloat(velocityHash, moveVelocity);
        playerAnim.SetFloat("JumpCount", jumpCount);

        //States
        playerAnim.SetBool("IsWallRunning", isWallRunning);
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetBool("IsInAir", isInAir);
        playerAnim.SetBool("IsLanding", isLanding);
        playerAnim.SetBool("IsSprinting", isSprinting);
        playerAnim.SetBool("IsDashing", isDashing);
       // if(isAttacking && nextAttackTimer < nextAttackDuration) playerAnim.SetBool("IsAttacking", isAttacking); 
       // else playerAnim.SetBool("IsAttacking", false);

        //Checks
        playerAnim.SetBool("CanJump", canJump);
        playerAnim.SetBool("CanMove", canMove);
        playerAnim.SetBool("CandDash", canDash);
        playerAnim.SetBool("canStartWallrun", canStartWallrun);
        playerAnim.SetBool("CanLand", canLand);

    }

    void CheckForDamage()
    {

    }





    //Base movement
    void CheckForMoveInput()
    {
        moveInput = input.moveInput;

        if (canMove)
        {
            //Save starting move velocity 
            float originalX = moveInput.x;
            float originalY = moveInput.y;

            //Make sure both input values are positive so we return the most dominant input value
            if (moveInput.x < 0) moveInput.x *= -1;
            if (moveInput.y < 0) moveInput.y *= -1f;


            if (isSprinting)
            {
                if (moveVelocity < 3) moveVelocity += Time.deltaTime * 1.3f;
            }

            //Set movevelocity to the highest input value 
            else if (moveInput.x > moveInput.y) moveVelocity = moveInput.x;
            else moveVelocity = moveInput.y;

            moveInput.x = originalX;
            moveInput.y = originalY;
        }

        //Handle move input 
        if ((moveVelocity > .1) && canMove)
        {
            isMoving = true;
            playerAnim.SetBool("IsMoving", true);

            if ((moveVelocity > .7f) && canMove)
            {
                isRunning = true;
                canRotate = true;
            }
            else isRunning = false;
        }
        else isMoving = false;


        //Move Player
        if (isMoving && canMove)
        {
            fixedControllerState = (int)currentState.MOVING;

            if (canRotate)
            {
                //turnLerpTimer = turnLerpValue;
                HandleRotation();
            }
        }
    }

    void MovePlayer()
    {
        moveDirection += moveInput.x * GetCameraRight(playerCamera) * currentMoveSpeed * Time.fixedDeltaTime;
        if (!isWallRunning) moveDirection += moveInput.y * GetCameraForward(playerCamera) * currentMoveSpeed * Time.fixedDeltaTime;

        playerRb.velocity = new Vector3(moveDirection.x, playerRb.velocity.y, moveDirection.z);
        moveDirection = Vector3.zero;

    }

    void HandleMoveSpeed()
    {

        //Check for sprinting 
        if (input.sprintButtonPressed && canSprint && (isMoving || isWallRunning))
        {
            isSprinting = true;
            if (!isWallRunning) canStartWallrun = true;
        }
        else
        {
            canStartWallrun = false;
            isSprinting = false;
        }

        //Fix weird rotation big
        if (!isMoving && !isWallRunning && isGrounded && !isAttacking) playerRb.isKinematic = true;
        else playerRb.isKinematic = false;


        //Set min movespeed 
        if (isRunning) currentMinMoveSpeed = minRunningMoveSpeed;
        else currentMinMoveSpeed = minWalkingMoveSpeed;


        //Set move speed 
        if (isGrounded)  //Handle ground move speed changes
        {
            if (isAttacking && nextAttackTimer < nextAttackDuration) currentMoveSpeed = attackMoveSpeed; 
            else if (isSprinting) currentMoveSpeed = sprintMoveSpeed;  //Sprinting 
            else if (isMoving && !isSprinting)
            {
                if (!isRunning) currentMoveSpeed = maxMoveSpeed * moveVelocity; //Walking
                else if (isRunning) currentMoveSpeed = maxMoveSpeed; //Running 
                if (currentMoveSpeed < currentMinMoveSpeed) currentMoveSpeed = currentMinMoveSpeed; //Check for min move speed 
            }
        }

        //Special cases 
        else if (isWallRunning) currentMoveSpeed = wallRunMoveSpeed; //Wallrunning
        else if (wasSprintingBeforeJump) currentMoveSpeed = airMoveSpeed * 1.5f; //Jumping after sprinting 
        else if (isInAir) //Normale air move speed 
        {
            //wasSprintingBeforeJump = false; 
            currentMoveSpeed = airMoveSpeed;
        }
    }

    void HandleDrag()
    {
        if (!isGrounded) playerRb.drag = airDrag;
        else if (isGrounded && isMoving || isWallRunning) playerRb.drag = 5f;
        else if (!isMoving) playerRb.drag = groundStopDrag;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 10, Color.green);       
        forward.y = 0;
        if (input.dashButtonPressed) dashDirection = playerCamera.transform.forward;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void HandleRotation()
    {
        if (canRotate)
        {
            //Player ground rotation 
            if (isMoving && !isWallRunning)
            {
                float speed = 10f;
                float singleStep = speed * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(playerRb.transform.forward, new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z), singleStep, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }



    //Dashing 

    void CheckForDash()
    {
        //Handle dash charge indicator UI
        float dashIndicatorSizeModifier = 1.3f; 
        dashChargeIndicator.fillAmount = (currentDashTime - minDashTime) / (maxDashTime - minDashTime);

        //Build up the dashduration on button hold
        if (input.dashButtonPressed && canDash)
        {
            isChargingDash = true;
            if (currentDashTime < maxDashTime) currentDashTime += dashBuildUpSpeed * Time.deltaTime;
            else
            {
                //Full charged dash UI effects
                if (dashChargeIndicator.transform.localScale == originalIndicatorSize)
                {
                    dashChargeIndicator.transform.localScale *= dashIndicatorSizeModifier;
                    dashChargeBackground.transform.localScale *= dashIndicatorSizeModifier; 
                }
                currentDashTime = maxDashTime;
            }
            //dashDirection = aimPoint.forward * currentDashTime; 
            //dashDirection = aimPoint.forward;
        }

        //Dash button is released
        if (!input.dashButtonPressed && isChargingDash)
        {
            //Add small dash delay for the animation charge up 
            if (dashDelayTimer == dashDelayDuration)
            {
                canMove = false;
                canLand = false; 
                ResetAnimator();
                transform.LookAt(aimPoint.position);
                //dashDirection = aimPoint.forward + Vector3.right;
                playerAnim.SetTrigger("DashStartTrigger");
            }
            dashDelayTimer -= Time.deltaTime;

            if (dashDelayTimer <= 0)
            {
                dashDelayTimer = dashDelayDuration;
                isDashing = true;
                canDash = false;
                isChargingDash = false;

                //dashDirection = aimPoint.forward;
                DashStartFeeback?.PlayFeedbacks();
                meshR.materials = holoSkinMat;

                playerRb.drag = airDrag;
                playerRb.velocity = new Vector3(0, 0, 0);

                controllerState = (int)currentState.DASHING;
                fixedControllerState = (int)currentState.DASHING;

                dashChargeIndicator.transform.localScale = originalIndicatorSize;
                dashChargeBackground.transform.localScale = originalIndicatorSize;
            }
        }

        //The player is dashing 
        if (isDashing && !solidDashObjectReached && !enemyDashObjectReached && currentDashTime > 0) //Maintain dash
        {
            playerRb.useGravity = false;
            playerRb.isKinematic = false;
            currentDashTime -= Time.deltaTime;
            

            if (currentDashTime <= 0) canEndDash = true;

            //Trigger stylish move at the end of the dash
            if (currentDashTime <= .25f && !dashEndMove)
            {
                //Delay and enable landing for ground dashes 
                canLand = true;
                dashEndMove = true;
                groundCheckTimer = .05f;
                if (jumpCount < 1) jumpCount += 1;              
                playerAnim.SetTrigger("DashEndTrigger");
                meshR.materials = defaultSkinMat;
            }            
        }
        else if (isDashing && currentDashTime < 0 && canEndDash) //End dash
        {
            ResetDash();
        }
        else if (enemyDashObjectReached)
        {
            dashChecker.gameObject.SetActive(false);
            canEndDash = false;
            canLand = true;
        }


        //Recharge dash 
        if (!canDash && !isDashing)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer < 0)
            {
                dashCooldownTimer = dashCooldownDuration;
                canDash = true;
            }
        }

        Debug.DrawLine(dashChecker.position, dashChecker.position + dashChecker.forward * 2f, Color.blue);
    }

    void DoDash()
    {
        if (!enemyDashObjectReached && !solidDashObjectReached)
        {
            playerRb.AddForce(dashDirection * dashForwardForce, ForceMode.Acceleration);
            //playerRb.AddForce(dashDirection - transform.position, ForceMode.Acceleration); 
           // transform.position = Vector3.MoveTowards(transform.position, dashDirection, 5f * Time.deltaTime); 
           
            dashChecker.gameObject.SetActive(true);
        }
    }

    

    public void DoDashAttack()
    {
        BasicEnemyScript script = dashAttackTarget.transform.GetComponentInParent<BasicEnemyScript>();
        script.LaunchEnemy(aimPoint.forward, dashAttackForce); 
    }

    public void ResetDash()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); 
        solidDashObjectReached = false;
        enemyDashObjectReached = false;
        dashChecker.gameObject.SetActive(false);
        meshR.materials = defaultSkinMat;

        isDashing = false;
        canEndDash = false;
        canMove = true;
        dashEndMove = false;
        playerRb.useGravity = true;
           
        currentDashTime = minDashTime;
        dashDelayTimer = dashDelayDuration;
        controllerState = (int)currentState.MOVING;
        fixedControllerState = (int)currentState.MOVING;
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(dashChecker.position, dashChecker.forward * m_MaxDistance);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(dashChecker.position + dashChecker.forward * m_MaxDistance, dashCheckerSize);

        Gizmos.color = Color.green;

        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(dashChecker.position, dashChecker.forward * m_MaxDistance2);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(dashChecker.position + dashChecker.forward * m_MaxDistance2, dashCheckerSize2);

    }
    */



    //Jumping
    void CheckForJump()
    {      
        if (jumpCount < maxJumps)
        {
            //Player presses the jump key
            if (input.jumpButtonPressed && canJump)
            {
                jumpCount++;
                groundCheckTimer = groundCheckJumpDelay;

                isGrounded = false;
                isLanding = false;
                hasJumped = true;
                canJump = false;

                //Animation handeling
                ResetAnimator();
                playerAnim.SetFloat("JumpCount", jumpCount);
                if (jumpCount == 1) playerAnim.SetFloat("RandomJumpVar", 0f); 
                else playerAnim.SetFloat("RandomJumpVar", Random.Range(1, 5)); //Randomly select a second jump animation 
                playerAnim.SetTrigger("JumpTrigger");

                if (isSprinting && jumpCount == 1) wasSprintingBeforeJump = true; //check if we we're running before the jump
            }
            else if (!input.jumpButtonPressed && !hasJumped) canJump = true;

            //Handle Jump animations
            if (hasJumped)
            {                          
                DoJump();
                hasJumped = false;                        
            }
        }
        else canJump = false;

   
        //Land player when near ground 
        if (isLanding)
        {
            if (canLand)
            {
                playerAnim.SetTrigger("LandingTrigger");
                landingDelayTimer = moveLandingClip.length * .7f;
                canLand = false; 
            }

            //Setup or interupt delay for next state when landing 
            landingDelayTimer -= Time.deltaTime;
            if (landingDelayTimer <= 0 && !CheckAnimationClip(noMoveLandingClip.name)) isLanding = false; //Player finishes the move landing animation 
            else if (isGrounded && CheckAnimationClip(noMoveLandingClip.name) && landingDelayTimer <= moveLandingClip.length * 1 - noMoveLandingClip.length * 1) isLanding = false; //Player exits the no move landing animation 

            //Player has landed 
            if (isGrounded && isInAir)
            {
                wasSprintingBeforeJump = false;
                isInAir = false; 
                canJump = true;

                groundCheckJumpDelay = .5f;
                jumpCount = 0;                   
            }
        }


        //Add extra falling gravity 
        if (playerRb.velocity.y < 1) playerRb.velocity -= Vector3.down * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        //Low and high jumps
        if (playerRb.velocity.y > 0 && !input.jumpButtonPressed) playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravity - 1) * Time.deltaTime;
    }

    void DoJump()
    {
        // playerAnim.SetTrigger("JumpTrigger");     
        playerRb.isKinematic = false;
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);

        if (!isWallRunning)
        {
            JumpFeedback?.PlayFeedbacks();       
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           
        }
        else if (isWallRunning)
        {
            playerRb.AddForce(transform.forward * -700, ForceMode.Impulse); //Move the player away from the wall so he doesn't get stuck
            playerRb.AddForce(Vector3.up * jumpForce /5, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position + Vector3.up * .25f, Vector3.down);

        //Shortly disable groundcheck so that it won't interupt the start of the jump 
        if (jumpCount > 0 && groundCheckTimer > 0)
        {
            groundCheckTimer -= Time.deltaTime;

            isGrounded = false;
            isLanding = false;
            isInAir = true;
        }
        else
        {
            //Player is grounded  
            if (Physics.Raycast(ray, out hit, groundCheckHeight))
            {
                playerRb.useGravity = false;
                isGrounded = true;     
                
            }
            //Player isn't grounded 
            else
            {
                canLand = true;
                isGrounded = false;
                isInAir = true;
                playerRb.useGravity = true;
            }

            //Player can land 
            if ((Physics.Raycast(ray, out hit, landCheckHeight)) && canLand) isLanding = true;                                       
        }
    }

   

    //Wallrunnning
    void CheckForWall()
    {
        RaycastHit hit;
        Ray frontRay = new Ray(frontWallChecker.position , frontWallChecker.forward);
        Debug.DrawLine(frontWallChecker.position, frontWallChecker.position + frontWallChecker.forward * frontWallCheckDist, Color.red);

        //Check if we can enter or exit the wallrun 
        if (Physics.Raycast(frontRay, out hit, frontWallCheckDist) && isSprinting)
        {
            //Start wallrun
            if (hit.collider.gameObject.layer == 6 && canStartWallrun) //Environment
            {
            
                StartWallRun();
            }
        }
        else if (isWallRunning || isWallRunning && input.jumpButtonPressed)
        {
            ExitWallRun(); 
        }
    }

    void StartWallRun()
    {
        playerAnim.SetTrigger("WallRunStartTrigger");
        ResetStates();
        isWallRunning = true;
        canStartWallrun = false;
        isSprinting = true; 
        canMove = true;

        playerRb.isKinematic = false;

        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        controllerState = (int)currentState.WALLRUNNING;
    }

    void ExitWallRun()
    {
        //Give the player a small boost after wallrunning 
        playerAnim.SetTrigger("WallRunEndTrigger");

        float jumpHeightDecrease = .6f;
        jumpForce *= jumpHeightDecrease;
        DoJump();
        jumpForce /= jumpHeightDecrease;

        isWallRunning = false;
        canStartWallrun = true;
        controllerState = (int)currentState.MOVING;
    }

    void HandleWallRunning()
    { 
        playerRb.velocity = new Vector3(playerRb.velocity.x, wallRunUpSpeed, playerRb.velocity.z);            
    }

   



    //Resets
    void ResetStates()
    {
        canLand =           false;
        canMove =           true;
        canRotate =         true;
        canJump =           true;
        canSprint =         true;
        canStartWallrun =   true;
        canDash =           true;  
        canDashAttack =     true;
        canEndDash =       false;
        canStartNewAttack =         true;

        isMoving =          false;    
        isWalking =         false;
        isSprinting =       false;
        isRunning =         false;
        isInAir =           false;
        isLanding =         false;
        isGrounded =        false;
        isWallRunning =     false;
        isAttacking =       false; 
        isDashing =         false;
        isChargingDash =    false;
        isDashAttacking =   false; 

        wasSprintingBeforeJump = false;
        playerRb.isKinematic = false;
        meshR.materials = defaultSkinMat;

        jumpCount = 0;      
    }

    void ResetAnimator()
    {
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            playerAnim.SetBool(parameter.name, false);
            playerAnim.ResetTrigger(parameter.name);
            playerAnim.SetInteger(parameter.name, 0); 
        }
    }

    void CheckForAttack()
    {
        int totalAttackTrees = 1; 

        if (input.meleeButtonPressed && canStartNewAttack && !isAttacking)
        {
            Debug.Log("ATTACK?");

            ResetAnimator();
            ResetStates();
            playerAnim.SetInteger("LComboType", Random.Range(1, totalAttackTrees));  //Decide which combo tree to go into
            isAttacking = true;
            canStartNewAttack = false;
            playerRb.isKinematic = false;
            controllerState = (int)currentState.ATTACKING;
            fixedControllerState = (int)currentState.ATTACKING;
            nextAttackTimer = 5f;
            currentComboLength = 0;
        }
        else if (input.meleeButtonPressed && canStartNewAttack && isAttacking)
        {
            //playerAnim.SetInteger("CurrentComboLength", currentComboLength);
            ResetStates();
            isAttacking = true;
            canStartNewAttack = false; 
            nextAttackTimer = 5f; 
            controllerState = (int)currentState.ATTACKING;
            fixedControllerState = (int)currentState.ATTACKING;
        }

        if (isAttacking) HandleAttack();
    }

    //Attacking

    void HandleAttack()
    {
        float stepSpeed = .1f;
        
        //Set attack duration equel to current animation clip length and speed
        nextAttackDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;

        //Check if we can continue to the next attack
        if (nextAttackTimer >= nextAttackDuration - (nextAttackDuration / 3f) && input.meleeButtonPressed)
        {
           
            //Set attack duration equel to current animation length    
            currentComboLength++;
            CheckForMoveInput(); 
            LightAttackFeedback?.PlayFeedbacks(); 
            playerAnim.SetBool("IsAttacking", true);
            playerAnim.SetInteger("CurrentComboLength", currentComboLength); 
            playerAnim.SetTrigger("LightAttackTrigger");          
            nextAttackTimer = 0f;

            //Check if there is a attack target within range 
            if (attackTargetScript.selectedTarget != null) attackTargetNearby = true;
            else attackTargetNearby = false;

            if (currentComboLength >= totalComboLength) currentComboLength = 0; //Reset combo tree
        }

        //Move towards attack target
        if (attackTargetNearby )
        {
            attackTargetScript.dontRemoveTarget = attackTargetNearby;

          //  if (attackTargetScript.selectedTarget != null)
           // {
                transform.position = Vector3.Lerp(transform.position, attackTargetScript.selectedTarget.position, stepSpeed); //Move towards attack target
               // transform.LookAt(new Vector3(attackTargetScript.selectedTarget.position.x, attackTargetScript.selectedTarget.position.y, transform.forward.z));
                float attackTargetDistance = Vector3.Distance(transform.position, attackTargetScript.selectedTarget.position); //Check for attackdistance 
                if (attackTargetDistance <= 1.5f) attackTargetNearby = false;
          //  }
          //  else attackTargetNearby = false; 

            
            
        }


        //Check if the combo is broken 
        if(nextAttackTimer >= nextAttackDuration)
        {
            playerAnim.SetBool("IsAttacking", false);
           // playerAnim.ResetTrigger("LightAttackTrigger"); 
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;
            canStartNewAttack = true;
            attackTargetNearby = false;
        }
        else if(nextAttackTimer < nextAttackDuration)
        {
          isAttacking = true;
        }

        //Exit attack state
        if(nextAttackTimer >= nextAttackDuration + maxTimeBetweenAttacks)
        {     
            isAttacking = false;
            attackTargetNearby = false;
            playerAnim.SetBool("IsAttacking", false);
        }

        nextAttackTimer += Time.deltaTime; 
    }

    

}