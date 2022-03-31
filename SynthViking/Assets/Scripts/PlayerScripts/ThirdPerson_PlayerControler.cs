using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using MoreMountains.Feedbacks; 
using System;
using DG.Tweening;

public enum playerAttackType
{
    PowerPunch,
    PhysicsImpact,
    LightAxeHit,
    GroundSlam,
    NormalePunch,
}


public class ThirdPerson_PlayerControler : MonoBehaviour
{
    public Vector3 inputDir;

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
    [HideInInspector] public Vector3 moveDirection;


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
    public int jumpCount;
    private float randomJumpVar = 2;
    public bool hasJumped = false;

    private float jumpDelayTimer;
    public float landingDelayTimer;
    private float landingDelayDuration; 
    private float gravity = 1f;
    public float fallMultiplier = 5f;
    public float lowJumpGravity;
    //[Range(0, 1)] public float airMSpeedReduction;
    public float airMoveSpeed;
    public float airDrag = 2f;


    private float groundCheckHeight = .5f;
    private float landCheckHeight = 2.5f;
    private float groundCheckJumpDelay = .5f; //Temporaly turn of the groundcheck after jumping 
    public float groundCheckTimer;
    #endregion

     [Header("BLOCKING")]
    #region
    public bool isBlocking;
    public bool canBlock;
   // public bool isPerfectParry;
   // public float blockResetDuration;

    public float blockRechargeTimer;
    public float blockRechargeDuration;

    public float blockTimer;
    public float blockDuration; 
   // public float perfectParryWindow;
   /// public int perfectParryRestorationAmount; 
    //public int maxAvailableBlocks;
    //public int currentAvailableBlocks; 
   
    #endregion


    [Header("DASH MOVEMENT")]
    #region

    public float dashForwardForce;
    public float dashDelayDuration;
    public float minDashDistance;
    public float maxDashDistance;
    private float dashDelayTimer;
    public float dashBuildUpSpeed;
    public float currentDashDistance;


    [HideInInspector] public float dashCooldownTimer;
    public float dashCooldownDuration;
    [HideInInspector] public bool solidDashObjectReached;
    [HideInInspector] public bool enemyDashObjectReached;
    private bool dashEndMove;
    private Vector3 dashDirection;
    private Vector3 dashOffset = Vector3.up * -1f;
    private Vector3 originalIndicatorSize;
    public Transform dashChecker;
   // public Image dashChargeIndicator;
   // public Color dashChargeColor; 
   // public Image dashChargeBackground;

    //Attack types
    //private string currentAttackType;
    private string attackType_Sprint = "SprintAttackTrigger";

    private string attackState; 

    [HideInInspector]


    public enum currentAttackType
    {
        BasicLightAttack,
        BasicHeavyAttack,
        SprintAttack,
    }


    #endregion

    [Header("DASH ATTACK")]
    #region
    public float dashAttackForce;
    public float dashAttackDamage = 5f; 
    [HideInInspector] public GameObject dashAttackTarget;
    #endregion

    [Header("MELEE ATTACK")]
    #region   
    public float maxTimeBetweenAttacks;
    private float nextAttackDuration;
    private float nextAttackTimer; 
    public int currentComboLength;
    public int totalComboLength;
    public float animAttackSpeed; 
    private float attackMoveLerpT;
    private Vector3 attackStartPos;

    [HideInInspector] public Vector3 attackDirection; 

    private float attackMoveSpeed = 1f;
    [HideInInspector]public bool attackTargetInRange;
    [HideInInspector] public bool canDamageTarget;
    [HideInInspector]public Transform currentAttackTarget;
    [HideInInspector] public float currentAttackDamage;
    [HideInInspector] public float currentAttackForwardForce; 
    public float basicAttackForwardForce;
    public float sprintAttackForwardForce; 
    public float basicLightAttackDamage = 10f;
    public float basicHeavyAttackDamage = 20f;
    public float basicSprintAttackDamage = 20f; 
    public float punchAttackDamage = 5f;
  

    #endregion

    [Header("AIR SMASH")]
    #region 
    public float groundSlamMinDamage;
    public float groundSlamMaxDamage;
    private float currentGroundSlamDamage;
    public float groundSlamRadius;
    public float slamImpactForwardForce;
    public float slamImpactUpForce; 

    private float airSmashCurrentDamage;
    public float airSmashDownSpeed; 
    private float airSmashDelayDuration;
    private float airSmashDelayTimer;
    private Vector3 airSmashDirection;
    public float airSmashMinStartHeight;
    public float airSmashEndHeight;   
    private float airSmashCooldownTimer;
    public float airSmashCooldownDuration; 
    #endregion

    [Header("WALLRUNNING")] //Wallrunning; 
    #region
    public float wallRunMoveSpeed;
    public float wallRunUpSpeed;
    public float frontWallCheckDist;
    public float sideWallCheckDist;
    public Transform frontWallChecker;

    private Vector3 jumpOffPoint;
    //[HideInInspector] public LayerMask EnvorinmentLayer = 10;
    public int EnvorinmentLayer = 10;
  //  public int enemyLayer = 11; 
   [HideInInspector] public LayerMask enemyLayer = 11;
    public bool wallRunExitWithJump;

    #endregion

    [Header("Animation")]  //ANIMATION
    #region  

   // public bool holsterWeapon;
  //  public bool drawWeapon; 
    public AnimationClip jumpStartClip;
    public AnimationClip moveLandingClip;
    public AnimationClip noMoveLandingClip;
    public AnimationClip VerticalWallRunClip;
    public AnimationClip VerticalWallRunEndClip;
    public AnimationClip airSmashAttackStartClip;
    public AnimationClip airSmashAttackEndClip;
    int velocityHash;
    #endregion

    [Header("Camera")] //CAMERA
    #region
    public Camera playerCamera;
    public Transform aimPoint;
    [HideInInspector] public Vector3 camForward;
    #endregion

    [Header("Scripts")] //SCRIPTS
    #region
    public PlayerInputCheck input;
    public PlayerState playerState;
    public AttackTargetScript attackTargetScript;
    public CameraHandeler camHandeler;
    public SlowMoScript slowScript;
    public CheckForLimbs limbCheckerScript; 

    public HitPauses hitPauseScript; 
    #endregion

    [Header("Components")] //COMPONENTS
    #region
    public Animator playerAnim;
    public Rigidbody playerRb;
    public SkinnedMeshRenderer meshR;
    private MeshRenderer raycastCheckMeshr;
    public Material holoRed;
    public Material holoGreen;
    public Material[] holoSkinMat;
    public Material[] defaultSkinMat;
    public Transform raycastCheck;
    public Rigidbody groundConstraints;
    public Rigidbody airConstraints;
    public GameObject holsteredWeapon;
    public GameObject drawnWeapon; 
    #endregion

    [Header("FEEDBACK")]
    #region
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks DashStartFeeback;
    public MMFeedbacks DashEndFeeback;
    public MMFeedbacks DashAttackFeedback; 
    public MMFeedbacks AttackFeedback; 
    public MMFeedbacks BasicLightAttackFeedback;
    public MMFeedbacks SprintAttackFeedback;
    public MMFeedbacks AirSlamStartFeeback;
    public MMFeedbacks AirSlamEndFeedback;
    // public MMFeedbacks aimingFeedback;
    // public MMFeedbacks slowmoFeedback; 
    #endregion

    [Header("Switch Case")] //SWITCH CASE
    #region
    [HideInInspector] public int controllerState;
    private int previousState; 
    [HideInInspector] public int fixedControllerState;
    [HideInInspector]
    public enum currentState
    {
        NOTHING,
        MOVING,
        WALLRUNNING,
        ATTACKING,
        DASHING,
        AIRSMASHING,
        STUNNED,
        BLOCKING, 
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
    public bool canFall = true;
    public bool canStartAirSmash;
    private bool canEndAirSmash = true;
    public bool canEndDash = false;

    public bool isMoving;
    public bool isAttacking;
    // public bool isPausingWallrun;
    public bool isWallRunning;
    public bool isLanding;
    public bool isDashing;
    public bool isChargingDash;
    public bool isRechargingDash; 
    public bool isDashAttacking;
    public bool isWalking;
    public bool isSprinting;
    public bool isRunning;
    public bool isInAir;
    public bool isAirSmashing;
    public bool isGroundSmash;
    public bool isGrounded = true;
    public bool isStunned; 

    public bool wasSprintingBeforeJump;
    #endregion



    private void Start()
    {
        velocityHash = Animator.StringToHash("MoveVelocity");
        raycastCheckMeshr = raycastCheck.GetComponent<MeshRenderer>();
        raycastCheck.gameObject.SetActive(false);
        HolsterWeapon(true); 
        //dashChecker.position, dashChecker.position + dashChecker.forward * 2f

        defaultSkinMat = meshR.materials;
        //meshR.materials = holoSkinMat; 

        //Set default values
        currentMoveSpeed = maxMoveSpeed;
        currentDashDistance = minDashDistance;
        dashDelayTimer = dashDelayDuration;
        airSmashDelayTimer = 0f;
        airSmashCooldownTimer = airSmashCooldownDuration;
        dashCooldownTimer = 0f; 
      
       // originalIndicatorSize = dashChargeIndicator.transform.localScale;
       // dashChargeColor = dashChargeIndicator.color; 

        fixedControllerState = (int)currentState.MOVING;
        controllerState = (int)currentState.MOVING;

        //mmSlowmoTime = aimingFeedback.GetComponent<MMFeedbackTimescaleModifier>(); 

        //Set default states
        ResetStates();

        playerRb.drag = groundStopDrag;
    }


    private void Update()
    {

        switch (controllerState)
        {
            case (int)currentState.MOVING:
                GroundCheck();
                HandleMoveSpeed();
                HandleRotation();
                CheckForMoveInput();
                CheckForJump();
                CheckForWall();
                CheckForAttack();
                HandleDrag();
                CheckForDash();
                CheckForAirSmash();
                CheckForBlock(); 
               // HandleWeaponSwaping(); 
                break;

            case (int)currentState.WALLRUNNING:
                CheckForJump();
                CheckForWall();
                CheckForAirSmash(); 
                CheckForDash();
                HandleMoveSpeed();
                CheckForMoveInput();
                HandleWallRunning();
                HandleDrag();
                CheckForAirSmash();
               // HandleWeaponSwaping(); 
                break;

            case (int)currentState.ATTACKING:
                CheckForAttack();
                CheckForAirSmash();
                CheckForDash(); 
                CheckForMoveInput();
            //    GroundCheck();
                HandleRotation();
                CheckForBlock(); 
             //   HandleWeaponSwaping(); 
                break;

            case (int)currentState.DASHING:
                CheckForDash();
                CheckForAirSmash();
               // GroundCheck(); 
                break;

            case (int)currentState.AIRSMASHING:
                CheckForDash();
                CheckForMoveInput(); 
                CheckForAirSmash();
                HandleRotation(); 
                break;


            case(int)currentState.BLOCKING:
                DoBlock(previousState); 
                
                break; 

            case (int)currentState.STUNNED:
                break; 
        }

        HandleAnimations();
        //if(transform.eulerAngles.x != 0) transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z); 
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
                // MovePlayer();
                break;
            case (int)currentState.AIRSMASHING:
                MovePlayer(); 
                break;
            case (int)currentState.STUNNED:
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
        playerAnim.SetBool("IsAiming", isChargingDash);
        playerAnim.SetBool("IsAirSmashing", isAirSmashing);
        playerAnim.SetBool("IsStunned", isStunned);
        playerAnim.SetBool("IsBlocking", isBlocking);  

        //Checks
        playerAnim.SetBool("CanJump", canJump);
        playerAnim.SetBool("CanFall", canFall); 
        playerAnim.SetBool("canStartAirSmash", canStartAirSmash); 
        playerAnim.SetBool("CanMove", canMove);
        playerAnim.SetBool("CandDash", canDash);
        playerAnim.SetBool("canStartWallrun", canStartWallrun);
        playerAnim.SetBool("CanLand", canLand);
    }

    void HolsterWeapon(bool holsterWeapon)
    {
        if (holsterWeapon)
        {
            drawnWeapon.SetActive(false);
            holsteredWeapon.SetActive(true); 
           // hols 
        }

        if (!holsterWeapon)
        {
            drawnWeapon.SetActive(true);
            holsteredWeapon.SetActive(false);
           // drawWeapon = false;
        }   
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
                if (moveVelocity < 3) moveVelocity += Time.deltaTime * 1.4f;
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
                //canRotate = true;
            }
            else isRunning = false;
        }
        else isMoving = false;

        //For attack targeting
        inputDir = GetCameraForward(playerCamera) * moveInput.y + GetCameraRight(playerCamera) * moveInput.x;
        inputDir = inputDir.normalized;

    }

    void MovePlayer()
    {
        moveDirection += moveInput.x * GetCameraRight(playerCamera) * currentMoveSpeed * Time.fixedUnscaledDeltaTime;
        if (!isWallRunning) moveDirection += moveInput.y * GetCameraForward(playerCamera) * currentMoveSpeed * Time.fixedUnscaledDeltaTime;

        playerRb.velocity = new Vector3(moveDirection.x, playerRb.velocity.y, moveDirection.z);
        moveDirection = Vector3.zero;
    }

    void HandleMoveSpeed()
    {

        //Check for sprinting 
        if (input.sprintButtonPressed && canSprint && (isMoving || isWallRunning))
        {
            if (!isSprinting) HolsterWeapon(true); 
         
            isSprinting = true;
            if (!isWallRunning) canStartWallrun = true;
        }
        else
        {
            canStartWallrun = false;
            isSprinting = false;
        }

        //Fix weird rotation big
        //   if (!isMoving && !isWallRunning && isGrounded && !isAttacking) playerRb.isKinematic = true;
        //  else playerRb.isKinematic = false;


        //Set min movespeed 
        if (isRunning) currentMinMoveSpeed = minRunningMoveSpeed;
        else currentMinMoveSpeed = minWalkingMoveSpeed;


        //Set move speed 
        if (isGrounded)  //Handle ground move speed changes
        {
            //if (isAttacking && nextAttackTimer < nextAttackDuration) currentMoveSpeed = attackMoveSpeed;
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
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * currentDashDistance, Color.green);
        forward.y = 0;

        //dashDirection = playerCamera.transform.forward;

        if (input.dashButtonPressed && !isDashing && canDash)
        {
            raycastCheck.gameObject.SetActive(true);
            RaycastHit camForwardHit;

            //Check camera forward position 
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward.normalized, out camForwardHit, currentDashDistance) && canDash)
            {
                //if (canDash && (camForwardHit.transform.gameObject.layer == 6 || camForwardHit.transform.CompareTag("Enemy")))
                 raycastCheckMeshr.material = holoGreen;
                if ((camForwardHit.transform.gameObject.layer == EnvorinmentLayer))
                {
                    raycastCheck.transform.position = camForwardHit.point;
                    dashDirection = camForwardHit.point;          
                }
                else
                {             
                    raycastCheck.transform.position = playerCamera.transform.position + playerCamera.transform.forward * currentDashDistance;
                    dashDirection = raycastCheck.transform.position + dashOffset;
                }
            }
            else
            {
                raycastCheckMeshr.material = holoRed;
                raycastCheck.transform.position = playerCamera.transform.position + playerCamera.transform.forward * currentDashDistance;
                dashDirection = raycastCheck.transform.position + dashOffset;
            }
        }
        else
        {
            //raycastCheck.gameObject.SetActive(false);
        }
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
            {
                float speed = 10f;
                float singleStep = speed * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(playerRb.transform.forward, new Vector3(inputDir.x, 0, inputDir.z), singleStep, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }



    //Dashing 

    void CheckForDash()
    {

        //Build up the dashduration on button hold
        if (input.dashButtonPressed && canDash)
        {
            //slowScript.DoSlowmo(true); 

            isChargingDash = true;
            if (currentDashDistance < maxDashDistance) currentDashDistance += dashBuildUpSpeed * Time.unscaledDeltaTime;
            else
            {
                currentDashDistance = maxDashDistance;
            }
        }

        //Dash button is released
        if (!input.dashButtonPressed && isChargingDash)
        {
            //Add small dash delay for the animation charge up 
            if (dashDelayTimer == dashDelayDuration)
            {
                playerState.canBeHit = false; 
                canMove = false;
                canLand = false;
                ResetAnimator();
                transform.LookAt(dashDirection);
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
                playerRb.constraints = airConstraints.constraints; 

                controllerState = (int)currentState.DASHING;
                fixedControllerState = (int)currentState.DASHING;
            }
        }

        //The player is dashing 
        if (isDashing && !solidDashObjectReached && !enemyDashObjectReached && !canEndDash) //Maintain dash
        {
            if (currentDashDistance <= 6f)
            {
                canEndDash = true; //End Dash
                return;
            }

            playerRb.useGravity = false;
            playerRb.isKinematic = false;
            currentDashDistance = Vector3.Distance(transform.position, dashDirection);


            //Trigger stylish move at the end of the dash
            if (currentDashDistance <= 35 && !dashEndMove)
            {
                //Delay and enable landing for ground dashes 
                canLand = true;
                dashEndMove = true;
                groundCheckTimer = .05f;
                if (jumpCount < 1) jumpCount += 1;
                playerAnim.SetTrigger("DashEndTrigger");
            }
        }
        else if (isDashing && canEndDash) //End dash
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
            dashCooldownTimer += Time.deltaTime;
            isRechargingDash = true; 

            if (dashCooldownTimer >= dashCooldownDuration)
            {
                isRechargingDash = false;
                dashCooldownTimer = 0f; 
                canDash = true;
            }
        }

        Debug.DrawLine(dashChecker.position, dashChecker.position + dashChecker.forward * 2f, Color.blue);
    }

    public void DoDash()
    {
        
        if (!enemyDashObjectReached && !solidDashObjectReached)
        {
  
           // dashSequence.Append(transform.DOMove(dashDirection, 1f)); 
            //transform.DOMove(dashDirection, 1f); 
            playerRb.AddForce((dashDirection - transform.position) * dashForwardForce, ForceMode.VelocityChange);
            dashChecker.gameObject.SetActive(true);
        }
    }

    public void DoDashAttack()
    {
        BasicEnemyScript script = dashAttackTarget.transform.GetComponentInParent<BasicEnemyScript>();
        script.TakeDamage(dashAttackDamage, playerAttackType.PowerPunch.ToString()); 
        script.LaunchEnemy(aimPoint.forward, Random.Range( dashAttackForce -5, dashAttackForce + 5), 5f);    
        script.transform.position = transform.position + transform.forward + script.transform.up; 
        playerRb.AddForce(transform.up * 5, ForceMode.VelocityChange);
        
    }

    public void ResetDash()
    {
     
        playerRb.velocity = new Vector3(0, 0, 0); 
        DashEndFeeback?.PlayFeedbacks();
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        solidDashObjectReached = false;
        enemyDashObjectReached = false;
        dashChecker.gameObject.SetActive(false);
        raycastCheck.gameObject.SetActive(false);
        meshR.materials = defaultSkinMat;
        if (jumpCount > 1) jumpCount = 1;

        isDashing = false;
        canEndDash = false;
        canMove = true;
        dashEndMove = false;
        playerRb.useGravity = true;
        playerState.canBeHit = true;

        currentDashDistance = minDashDistance;
        dashDelayTimer = dashDelayDuration;
        controllerState = (int)currentState.MOVING;
        fixedControllerState = (int)currentState.MOVING;
     
    }



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
                GroundCheck(); 
                DoJump(jumpForce);
                hasJumped = false;
            }
        }
        else canJump = false;


        //Add extra falling gravity 
        if (playerRb.velocity.y < 1) playerRb.velocity -= Vector3.down * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        //Low and high jumps
        if (playerRb.velocity.y > 0 && !input.jumpButtonPressed) playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravity - 1) * Time.deltaTime;
    }

    void DoJump(float jumpHeight)
    {   
        playerRb.isKinematic = false;
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);

        JumpFeedback?.PlayFeedbacks();
        playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        
        if (isWallRunning)
        {
            playerRb.AddForce(-transform.forward * 1000, ForceMode.Impulse); //Move the player away from the wall so he doesn't get stuck
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
            playerRb.constraints = airConstraints.constraints; 
      
            playerRb.useGravity = true;
        }
        else
        {

            //Player is grounded  
            if (Physics.Raycast(ray, out hit, groundCheckHeight))
            {
                if (hit.collider.gameObject.layer == EnvorinmentLayer)
                {
                    playerRb.useGravity = false;
                    playerRb.constraints = groundConstraints.constraints;
                    isGrounded = true;
                    canJump = true;
                    jumpCount = 0;
                }

            }
            //Player isn't grounded 
            else
            {
                playerRb.constraints = airConstraints.constraints; 
                if (!isLanding) canLand = true; 
                isGrounded = false;
               // isLanding = false; 
                isInAir = true;
                playerRb.useGravity = true;
        
            }

            //Player can land 
            if ((Physics.Raycast(ray, out hit, landCheckHeight) && canLand && isInAir))
            {
                if (hit.collider.gameObject.layer == EnvorinmentLayer)
                { 
                    canLand = false;
                    isInAir = false;
                    isLanding = true;
                    landingDelayTimer = 0f;
                    playerAnim.SetTrigger("LandingTrigger");
                }
            }

            if (isLanding) LandPlayer();

        }

        //Player can airSmash 
        if ((!Physics.Raycast(ray, out hit, airSmashMinStartHeight)) && isInAir) canStartAirSmash = true;
        else canStartAirSmash = false;

      
       

    }

    void LandPlayer()
    {
        
        //Set animation based on
        if(moveVelocity > .3f) landingDelayDuration = (moveLandingClip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed);
        else if(moveVelocity <= .3f) landingDelayDuration = (noMoveLandingClip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed);
        
        landingDelayTimer += Time.deltaTime;
        if(landingDelayTimer >= landingDelayDuration - .15f) isLanding = false; 

        
        //Player has landed 
        if (isGrounded && isInAir && !isLanding)
        {
            wasSprintingBeforeJump = false;
            isInAir = false;
            canLand = true;
            groundCheckJumpDelay = .5f;
        }
        
    }



    //Wallrunnning
    void CheckForWall()
    {
        RaycastHit hit;
        Ray frontRay = new Ray(frontWallChecker.position, frontWallChecker.forward);
        Debug.DrawLine(frontWallChecker.position, frontWallChecker.position + frontWallChecker.forward * frontWallCheckDist, Color.red);

        //Check if we can enter or exit the wallrun 
        if (Physics.Raycast(frontRay, out hit, frontWallCheckDist) && isSprinting)
        {
            //Start wallrun
            if (hit.collider.gameObject.layer == EnvorinmentLayer && canStartWallrun) //Environment
            {

                StartWallRun();
            }
        }
        else if (isWallRunning || isWallRunning && (input.jumpButtonPressed || !input.sprintButtonPressed))
        {
            if(input.jumpButtonPressed || !input.sprintButtonPressed)
            { 
                jumpOffPoint = transform.position - transform.forward * 3f; 
                wallRunExitWithJump = true; 
            }
       
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

        playerRb.constraints = airConstraints.constraints; 
        playerRb.isKinematic = false;

        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        controllerState = (int)currentState.WALLRUNNING;
    }

    void ExitWallRun()
    {
        //Give the player a small boost after wallrunning 
  


        playerAnim.SetTrigger("WallRunEndTrigger");
        groundCheckTimer = .45f;
        jumpCount = 1;
        

      

        if(wallRunExitWithJump)
        {
            transform.DOMove(jumpOffPoint, .25f); 
            transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z), .5f); 
        }
        else
        {
            DoJump(jumpForce * .05f);
        }

        wallRunExitWithJump = false; 
        isWallRunning = false;
        canStartWallrun = true;
        
        controllerState = (int)currentState.MOVING;
    }

    void HandleWallRunning()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, wallRunUpSpeed, playerRb.velocity.z);
    }


    //Air smashing 
    void CheckForAirSmash()
    { 
       
        if (input.airSmashButtonPressed && canStartAirSmash)
        {
            //Air smash delay
            if (airSmashDelayTimer <= 0)
            {
                ResetAnimator();
                ResetStates(); 
                playerRb.velocity = new Vector3(0, 0, 0);            
                
                canFall = false; 
                playerAnim.SetTrigger("AirSmashStartTrigger");
                playerRb.useGravity = false;
                isAirSmashing = true;
                AirSlamStartFeeback?.PlayFeedbacks(); 
                controllerState = (int)currentState.AIRSMASHING;
                fixedControllerState = (int)currentState.AIRSMASHING; 
            }


            airSmashDelayDuration = (playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed) -.1f;
            airSmashDelayTimer += Time.deltaTime;

            if (airSmashDelayTimer >= airSmashDelayDuration)
            {
                //isAirSmashing = true;
                meshR.materials = holoSkinMat;
                canStartAirSmash = false;
            }
        }

        //Handle air smash start and stop 
        if (isAirSmashing && !input.airSmashButtonPressed || isGroundSmash)
        {
          
            //Only do this once at the start of the end
            if (canEndAirSmash)  
            {
                meshR.materials = defaultSkinMat;
                airSmashDelayTimer = 0f;
                playerAnim.SetTrigger("AirSmashEndTrigger");
            }
           
            airSmashDelayDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;
            airSmashDelayTimer += Time.deltaTime;

            EndAirSmash();

        }
        else if (isAirSmashing && airSmashDelayTimer >= airSmashDelayDuration)
        {
            
            DoAirSmash();
        }
        

        //Air smash Cooldown
        if (airSmashCooldownTimer > 0) airSmashCooldownTimer -= Time.deltaTime;
        if(airSmashCooldownTimer <= 0 && !input.airSmashButtonPressed && !isAirSmashing) canStartAirSmash = true; 
        
        
    }

    void DoAirSmash()
    {      
        playerRb.AddForce(-Vector3.up * airSmashDownSpeed, ForceMode.VelocityChange);
        dashChecker.gameObject.SetActive(true);

        //Dash has reached ground
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -transform.up);
        if ((Physics.Raycast(downRay, out hit, airSmashEndHeight)))
        {
            if (hit.transform.gameObject.layer == EnvorinmentLayer)
            {
             
                playerRb.velocity = new Vector3(0, 0, 0);
                AirSlamEndFeedback?.PlayFeedbacks(); 
                isGroundSmash = true;
                playerState.canBeHit = false; 
                DoJump(jumpForce / 1.2f); 


                Collider[] colls = Physics.OverlapSphere(transform.position, groundSlamRadius);
                foreach (Collider slamTarget in colls)
                {
                    if (slamTarget.gameObject.layer == enemyLayer)
                    {
                        BasicEnemyScript enemyScript = slamTarget.GetComponent<BasicEnemyScript>();
                        enemyScript.TakeDamage(currentGroundSlamDamage, playerAttackType.GroundSlam.ToString());
                        enemyScript.LaunchEnemy((slamTarget.transform.position - transform.position), slamImpactForwardForce, Random.Range(slamImpactUpForce / 1.3f, slamImpactUpForce * 1.3f));
                    }
                }
            }
        }

    }

    void EndAirSmash()
    {
        //Have we hit the ground with our slam?
        if (isGroundSmash)
        {

            playerState.canBeHit = true; 

        }

        canEndAirSmash = false; 
     

        if (airSmashDelayTimer >= airSmashDelayDuration - .09f || !isGroundSmash)
        {
            isAirSmashing = false;
            isGroundSmash = false;
            canEndAirSmash = true; 
            canLand = true;
            canFall = true;
            canMove = true; 

            playerRb.useGravity = true;
            airSmashDelayTimer = 0f;      
            airSmashCooldownTimer = airSmashCooldownDuration;
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;              
        }     
    }

    void DoAirSlamAttack()
    {

    }

    //Blockiong

    void CheckForBlock()
    {
        if(input.blockButtonPressed && canBlock) //Check if the player is and can block 
        {
            isBlocking = true;
            canBlock = false;  
            HolsterWeapon(false); 
            previousState = controllerState; 
            playerAnim.SetTrigger("BlockTrigger"); 
            controllerState = (int)currentState.BLOCKING; 
        }

        if(!canBlock && !isBlocking) //Recharge the block after use 
        {
            blockRechargeTimer += Time.deltaTime; 
            if(blockRechargeTimer >= blockRechargeDuration && !input.blockButtonPressed)
            {
                blockRechargeTimer = 0f; 
                canBlock = true; 
            }
        }
    }

    void DoBlock(int previousState)
    {
        blockTimer+= Time.deltaTime; 
        playerState.canBeHit = false; 

        if(blockTimer >= blockDuration)
        {
            isBlocking = false;  
            playerState.canBeHit = true; 
            blockTimer = 0f;
            controllerState = previousState; 
        }
    }


    //Attacking

    void CheckForAttack()
    {

        if (input.meleeButtonPressed && canStartNewAttack && !isAttacking)
        {
            currentComboLength = 0;
            ResetAnimator(); 
            SetAttackType();           
            ResetStates();
            isAttacking = true;
            canStartNewAttack = false;
            playerRb.isKinematic = false;
            //HandleWeaponSwaping(false);     
            playerAnim.speed = animAttackSpeed;  
            nextAttackTimer = 10f;
            
           
            controllerState = (int)currentState.ATTACKING;
            fixedControllerState = (int)currentState.ATTACKING;
        }
        else if (input.meleeButtonPressed && canStartNewAttack && isAttacking)
        {
            playerAnim.speed = animAttackSpeed;
            //playerAnim.SetInteger("CurrentComboLength", currentComboLength);
            ResetStates();
            isAttacking = true;
            canStartNewAttack = false;
            nextAttackTimer = 10f;

            controllerState = (int)currentState.ATTACKING;
            fixedControllerState = (int)currentState.ATTACKING;
        }

        if (isAttacking) HandleAttack();
    }

    void SetAttackType()
    {
        int totalAttackTrees = 5;
        HolsterWeapon(false);

        if (isSprinting)
        {
            totalComboLength = 2;
            AttackFeedback = SprintAttackFeedback;
            currentAttackDamage = basicSprintAttackDamage;
            currentAttackForwardForce = sprintAttackForwardForce;


            attackState = currentAttackType.SprintAttack.ToString();
            playerAnim.SetFloat("SprintAttackType", Random.Range(1, 4));

        }
        else
        {
            AttackFeedback = BasicLightAttackFeedback;
            currentAttackDamage = basicLightAttackDamage;
            currentAttackForwardForce = basicAttackForwardForce;

            attackState = currentAttackType.BasicLightAttack.ToString();

            //Set new combo 
            if (currentComboLength == 0)
            {
                //No double hit animations
                int originalReaction = playerAnim.GetInteger("LightAttackType");
                int newRandomNumber = Random.Range(1, totalAttackTrees + 1);
                if (newRandomNumber == originalReaction)
                {
                    if (newRandomNumber - 1 <= 0) newRandomNumber += 1;
                    else newRandomNumber -= 1;
                }
                playerAnim.SetInteger("LightAttackType", newRandomNumber);  //Decide which combo tree to go into (only once per full combo tree)
            }

            //Set current attack tree length
            if      (playerAnim.GetInteger("LightAttackType") == 1) totalComboLength = 3; 
            else if (playerAnim.GetInteger("LightAttackType") == 2) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 3) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 4) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 5) totalComboLength = 6; //Done
          //  else if (playerAnim.GetInteger("LightAttackType") == 6) totalComboLength = 5;
        }


    }


    void HandleAttack()
    {
        //Set attack duration equel to current animation clip length and speed
        nextAttackDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;

        //Check if we can continue to the next attack
        if (nextAttackTimer >= nextAttackDuration - .21f && input.meleeButtonPressed)
        {      
            
            CheckForMoveInput(); //Set attack direction 
            if(currentComboLength > 0) SetAttackType(); // Set attack type (skip first attack so we can do conditional start attacks)
            currentComboLength++;

            //Trigger correct animation stuff
            AttackFeedback?.PlayFeedbacks();
            playerAnim.SetBool("IsAttacking", true);
            playerAnim.SetInteger("CurrentComboLength", currentComboLength);
            playerAnim.SetTrigger(attackState + "Trigger");

            nextAttackTimer = 0f;
            attackMoveLerpT = 0f;
            canRotate = true;

            attackStartPos = transform.position;
          //  playerRb.velocity = new Vector3(0, 0, 0);


            //Check if there is a attack target within range 
            if (attackTargetScript.selectedTarget != null)
            {
                attackTargetInRange = true;
                currentAttackTarget = attackTargetScript.selectedTarget;
            }
            else attackTargetInRange = false;

            if (currentComboLength >= totalComboLength)
            {
                currentComboLength = 0; //Reset combo tree
                SetAttackType(); 
            }
        }

        /*
        //Move towards attack target
        if (attackTargetInRange && attackState != currentAttackType.SprintAttack.ToString()) 
        {
            float animStartDistance = 5;
            //Slightly delay attacks animations
            if (Vector3.Distance(transform.position, currentAttackTarget.position) > animStartDistance) playerAnim.enabled = false;
            else playerAnim.enabled = true; 

            float timeToReach = .15f;
            float timeToReach = .15f;
            attackMoveLerpT = Time.deltaTime / timeToReach;
            transform.position = Vector3.Lerp(transform.position, currentAttackTarget.position, attackMoveLerpT);
            transform.LookAt(currentAttackTarget.position);
        }
        else
        {
            playerAnim.enabled = true; 
        }
        */


        //Check if the combo is broken 
        if (nextAttackTimer >= nextAttackDuration - .2f)
        {
            playerAnim.SetBool("IsAttacking", false);
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;
            canStartNewAttack = true;
            playerAnim.speed = 1f; 
            attackTargetInRange = false;
            canRotate = true;
            playerRb.isKinematic = false;
           // canMove = true; 

        }
        else if (nextAttackTimer < nextAttackDuration)
        {
            //HandleRotation();
            playerRb.isKinematic = true;
            isAttacking = true;
        }

        //Exit attack state
        if (nextAttackTimer >= nextAttackDuration + maxTimeBetweenAttacks)
        {
            currentComboLength = 0;
            isAttacking = false;
            attackTargetInRange = false;
            playerAnim.SetBool("IsAttacking", false);
        }

        nextAttackTimer += Time.deltaTime;
    }

    //Resets
    public void ResetStates()
    {
        canRotate = true;
        canStartNewAttack = true; 
       // canMove = true; 
        /*
                canLand = false;
                canMove = true;
                canRotate = true;
                canJump = true;
                canSprint = true;
                canStartWallrun = true;
              //  canDash = true;
                canDashAttack = true;
                canEndDash = false;
                canStartNewAttack = true;
                canStartAirSmash = true;
                canEndAirSmash = true;
                canFall = true;
                */

        isStunned = false; 
        isMoving = false;
        isWalking = false;
        isSprinting = false;

        isRunning = false;
        isInAir = false;
        isLanding = false;
        isGrounded = false;
        isWallRunning = false;
        isAttacking = false;
        isDashing = false;
        isChargingDash = false;
        isDashAttacking = false;
        isAirSmashing = false;
        isGroundSmash = false;

        wasSprintingBeforeJump = false;
        playerRb.isKinematic = false;
        meshR.materials = defaultSkinMat;
        playerAnim.speed = 1f;

        jumpCount = 0;

        //ResetDash(); 
    }

    public void ResetAnimator()
    {
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            playerAnim.SetBool(parameter.name, false);
            playerAnim.ResetTrigger(parameter.name);
            playerAnim.SetInteger(parameter.name, 0);
        }
    }

    void AllowAttackDamage()
    {

        //canRotate = false;
        attackTargetScript.TargetDamageCheck();  
        attackDirection = transform.position + inputDir;
        if (attackState != currentAttackType.SprintAttack.ToString()) transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .35f).SetUpdate(UpdateType.Fixed);
        else meshR.materials = defaultSkinMat; 
        //nextAttackTimer = nextAttackDuration; 

    }

    void AllowAttackForwardForce()
    {
        meshR.materials = holoSkinMat;
        transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .7f).SetUpdate(UpdateType.Fixed);
    }

    void AllowNextAttack()
    {
        nextAttackTimer = nextAttackDuration;
    }


    
}