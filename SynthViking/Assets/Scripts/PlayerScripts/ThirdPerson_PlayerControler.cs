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
    /*
    public float m_MaxDistance = 300f;
    public Vector3 dashCheckerSize;
    bool m_HitDetect;
    RaycastHit m_Hit;
   

    public float m_MaxDistance2 = 300f;
    public Vector3 dashCheckerSize2;
    bool m_HitDetect2;
    RaycastHit m_Hit2;
    */

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

    #endregion

    [Header("DASH ATTACK")]
    #region
    public float dashAttackForce;
    public GameObject attackTarget; 
    #endregion

    [Header("WALLRUNNING")] //Wallrunning; 
    #region
    public float wallRunMoveSpeed;
    public float wallRunUpSpeed;
    public float frontWallCheckDist;
    public float sideWallCheckDist;
    //private float wallRunEndTimer;
   // private float wallRunEndDuration;

    public Transform frontWallChecker;
    public Transform leftWallChecker;
    public Transform RightWallChecker;

    private LayerMask EnvorinmentLayer = 6;

    #endregion


    [Header("Animation")]  //ANIMATION
    #region  


    public AnimationClip jumpStartClip;
    public AnimationClip moveLandingClip;
    public AnimationClip noMoveLandingClip;
    public AnimationClip VerticalWallRunClip;
    public AnimationClip VerticalWallRunEndClip;
    //public AnimationClip VerticalWall
    int velocityHash;
    #endregion


    [Header("Camera")] //CAMERA
    #region
    public Camera playerCamera;

    #endregion


    [Header("Scripts")] //SCRIPTS
    #region
    public PlayerInputCheck input;
    public PlayerState playerState;
    #endregion


    [Header("Components")] //COMPONENTS
    #region
    public Animator playerAnim;
    public Rigidbody playerRb;
    #endregion

    [Header("FEEDBACK")]
    #region
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks DashStartFeeback;
    public MMFeedbacks DashAttackFeeback;
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
    //public bool canPauseWallrun = true;
    public bool canEndDash = false;

    public bool isMoving;
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

        //Set default values
        currentMoveSpeed = maxMoveSpeed;
        currentDashTime = minDashTime;
        dashCooldownTimer = dashCooldownDuration;

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
                HandleDrag();
                CheckForDash();
                break;

            case (int)currentState.WALLRUNNING:
                CheckForJump();
                CheckForWall();
                HandleMoveSpeed();
                CheckForMoveInput();
                HandleWallRunning();
                HandleDrag();
                break;

            case (int)currentState.ATTACKING:
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


        //Ground Movement    

        //Jump
        // playerAnim.SetInteger("JumpCount", jumpCount);

        playerAnim.SetFloat("JumpCount", jumpCount);

        //States
        playerAnim.SetBool("IsWallRunning", isWallRunning);
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetBool("IsInAir", isInAir);
        playerAnim.SetBool("IsLanding", isLanding);
        playerAnim.SetBool("IsSprinting", isSprinting);
        playerAnim.SetBool("IsDashing", isDashing);

        //Checks
        playerAnim.SetBool("CanJump", canJump);
        playerAnim.SetBool("CanMove", canMove);
        playerAnim.SetBool("CandDash", canDash);
        playerAnim.SetBool("canStartWallrun", canStartWallrun);
        playerAnim.SetBool("CanLand", canLand);

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
            //MovePlayer(); 

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
        if (!isMoving && !isWallRunning && isGrounded) playerRb.isKinematic = true;
        else playerRb.isKinematic = false;


        //Set min movespeed 
        if (isRunning) currentMinMoveSpeed = minRunningMoveSpeed;
        else currentMinMoveSpeed = minWalkingMoveSpeed;

        //Set move speed 
        if (isGrounded)  //Handle ground move speed changes
        {
            if (isSprinting) currentMoveSpeed = sprintMoveSpeed;  //Sprinting 
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


                //if (turnLerpTimer > 0) turnLerpTimer -= Time.deltaTime;
                //targetTurnRotation = Vector3.Lerp(targetTurnRotation, moveInput, turnLerpTimer);
                float speed = 10f;
                float singleStep = speed * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(playerRb.transform.forward, new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z), singleStep, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            /*
            else if(isMoving && isWallRunning)
            {
                float speed = 10f;
                float singleStep = speed * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(playerRb.transform.forward, new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z), singleStep, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            */


        }
    }



    //Dashing 

    void CheckForDash()
    {

        //Build up the dashduration on button hold
        if (input.dashButtonPressed && canDash)
        {
            isChargingDash = true;
            if (currentDashTime < maxDashTime) currentDashTime += dashBuildUpSpeed * Time.deltaTime;
            else currentDashTime = maxDashTime;
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
                playerAnim.SetTrigger("DashStartTrigger");
            }
            dashDelayTimer -= Time.deltaTime;

            if (dashDelayTimer <= 0)
            {
                dashDelayTimer = dashDelayDuration;
                isDashing = true;
                canDash = false;
                isChargingDash = false;

                //dashDirection = playerCamera.transform.forward; //Set dash direction 
                dashDirection = transform.forward;
                DashStartFeeback?.PlayFeedbacks();

                playerRb.drag = airDrag;
                playerRb.velocity = new Vector3(0, 0, 0);

                controllerState = (int)currentState.DASHING;
                fixedControllerState = (int)currentState.DASHING;
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
            dashChecker.gameObject.SetActive(true);
            /*
            m_HitDetect = Physics.BoxCast(dashChecker.position, dashCheckerSize, dashChecker.forward, out m_Hit, dashChecker.rotation, m_MaxDistance);

            if (m_HitDetect)
            {

                //Dash collides with enemy
                if (m_Hit.collider.gameObject.CompareTag("Enemy"))
                {
                    enemyDashObjectReached = true;
                    playerRb.velocity = new Vector3(0, 0, 0);
                   // playerRb.transform.LookAt(new Vector3(m_Hit.transform.position.x, m_Hit.transform.position.y, transform.forward.z));  
                
                    playerAnim.SetTrigger("DashAttackTrigger");
                    DashAttackFeeback?.PlayFeedbacks();
                }
            }
            */
        }
    }

    

    public void DoDashAttack()
    {
        BasicEnemyScript script = attackTarget.transform.GetComponent<BasicEnemyScript>();
        script.LaunchEnemy(transform.forward, dashAttackForce);
    }

    public void ResetDash()
    {
        solidDashObjectReached = false;
        enemyDashObjectReached = false;
        dashChecker.gameObject.SetActive(false);

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
               // playerAnim.ResetTrigger("WallRunEndTrigger");
               // playerAnim.ResetTrigger("WallRunPauseTrigger");
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
            //Debug.Log("Groundcheck delay works");
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
                StartWallRun();
        }
        else if (isWallRunning || isWallRunning && input.jumpButtonPressed)
        {
            ExitWallRun(); 
        }
    }

    void StartWallRun()
    {
        playerAnim.SetTrigger("WallRunStartTrigger");

        Debug.Log("StartWallrun"); 

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
        canStartWallrun =        true;
        canDash =           true;  
     //   canPauseWallrun =   true;
        canDashAttack =     true;
        canEndDash = false; 

        isMoving =          false;    
        isWalking =         false;
        isSprinting =       false;
        isRunning =         false;
        isInAir =           false;
        isLanding =         false;
        isGrounded =        false;
        isWallRunning =     false;
       // isPausingWallrun =  false;
        isDashing =         false;
        isChargingDash =    false;
        isDashAttacking =   false; 

        wasSprintingBeforeJump = false;
        playerRb.isKinematic = false;

        jumpCount = 0;      
    }

    void ResetAnimator()
    {
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            playerAnim.SetBool(parameter.name, false);
            playerAnim.ResetTrigger(parameter.name);
        }
    }



    //Attacking
    void CheckForAttack()
    {

    }

    void HandleAttack()
    {

    }

}