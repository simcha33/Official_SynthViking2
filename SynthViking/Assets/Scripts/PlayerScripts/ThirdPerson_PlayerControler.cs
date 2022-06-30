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
    HeavyAxeHit,
    LightPunchHit,
    GroundSlam,
    NormalePunch,
    BlockStun,
    SprintAttack,
    AirLaunchAttack,

    Reset,
}


public class ThirdPerson_PlayerControler : MonoBehaviour
{
    public Vector3 inputDir;
    private bool holsterWeapon;

    [Header("GROUND MOVEMENT")] //GROUND MOVEMENT
    #region
    //Moving
    public float maxMoveSpeed;
    public float sprintMoveSpeed;
    public float minWalkingMoveSpeed;
    public float minRunningMoveSpeed;
    private float currentMinMoveSpeed;
    private bool topSpeedReached;
    public float currentRotateSpeed;
    public float defaultRotateSpeed;
    private Vector3 startingRotation;


    public float currentMoveSpeed;
    private float moveVelocity = 0.0f;
    private float groundStopDrag = 20f;

    public Sequence moveSequence;

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
    public float minJumpForce = 450f;
    public float maxChargedJumpForce;
    public float currentJumpForce;
    private bool isChargedJump;


    public float maxJumpChargeDuration;
    private float maxJumpChargeTimer;
    private bool wantsToJump;
    //  public float fallTimer;
    public float freefallWaitTime = 1.1f;


    //  public float fullFullWaitTime;   
    public float maxJumps;
    private int jumpCount;
    private bool hasJumped = false;

    // private float jumpDelayTimer;
    //private float randomJumpVar = 2;

    //Movement
    public float airMoveSpeed;
    public float freeFallMoveSpeed;
    public float chargedJumpMoveSpeed;
    public float airDrag = 2f;
    public float fallMultiplier = 5f;
    public float lowJumpGravity;

    [HideInInspector] public float inAirTime = 0f;

    //Landing
    public float landingDelayTimer;
    private float landingDelayDuration;
    private float groundCheckHeight = .35f;
    private float landCheckHeight = 3.2f;
    private float groundCheckJumpDelay = .5f; //Temporaly turn of the groundcheck after jumping 
    public float groundCheckTimer;
    #endregion

    [Header("GENERAL ATTACK")]
    #region   
    public float maxTimeBetweenAttacks;
    private float nextAttackDuration;
    private float nextAttackTimer;

    public float attackTransitionOffset;
    public float attackingRotateSpeed;

    private float attackMoveLerpT;
    private Vector3 attackStartPos;


    [HideInInspector] public Vector3 attackDirection;
    public float attackMoveSpeed = 1f;
    [HideInInspector] public bool attackTargetInRange;
    [HideInInspector] public bool canDamageTarget;
    [HideInInspector] public Transform currentAttackTarget;
    [HideInInspector] public float currentAttackDamage;
    [HideInInspector] public float currentAttackForwardForce;
    // public float basicAttackForwardForce;
    // public float punchAttackDamage = 5f;
    public List<BasicEnemyScript> sprintHitList = new List<BasicEnemyScript>();
    #endregion

    [Header("PUNCH ATTACK")]
    #region 
    public float punchAttackTransitionOffset;
    private bool wantsToLightAttack;
    private float lightAttackHoldTimer;
    public float punchPauseLength;

    private int punchComboLength;
    private int totalPunchComboLength;
    public float punchAttackSpeed;
    public float basicLightAttackDamage = 10f;
    public float punchAttackForwardForce;

    #endregion

    [Header("AXE ATTACK")]
    #region 
    public float axeAttackTransitionOffset;
    private bool wantsToHeayAttack;
    private float heavyAttackHoldTimer;
    public float axeHitPauseLength;

    private int axeComboLength;
    private int totalAxeComboLength;
    public float axeAttackSpeed;
    public float basicHeavyAttackDamage = 20f;
    public float axeAttackForwardForce;
    #endregion

    [Header("AiR ATTACKING")]
    #region 

    public float airLaunchHoldDuration;
    public float airLaunchHeight;
    public float airLaunchDamage;
    public bool canairLaunch;
    //public bool isairLaunchting; 
    //public bool isAirAttacking; 
    public float closestTargetAngle;
    public Transform airAttackTarget;
    private bool wantsToAirLaunchCut;
    private Vector3 airLaunchPoint;
    #endregion

    [Header("RAGE")]
    public bool canRage;
    public bool isRaging;

    public float rageDuration;
    public float rageTimer;

    public float rageDamageGivenMultiplier;
    public float rageDamageTakenReduction;

    public GameObject rageLightning;
    public Material[] rageSkinMat;
    public GameObject rageElectricity;
    public MMFeedbacks rageStartFeedback;
    public MMFeedbacks rageEndFeedback;


    [Header("DASH ATTACK")]
    #region
    public float dashAttackForce;
    public float dashAttackDamage = 5f;
    public bool dashAirAttack;
    [HideInInspector] public GameObject dashAttackTarget;
    #endregion


    [Header("SPRINT ATTACK")]
    #region
    public float sprintAttackForwardForce;
    public Sequence sprintAttackTween;
    public float basicSprintAttackDamage = 20f;
    #endregion

    [Header("BLOCKING")]
    #region
    public bool isBlocking;
    public bool canBlock;
    [HideInInspector] public bool canBlockStun = true;
    public bool hasParriedAttack;
    // public float blockResetDuration;

    public float blockRechargeTimer;
    public float blockRechargeDuration;
    public float blockTimer;
    public float blockDuration;
    public float blockStunRadius;
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
    public int dashTargetCount;

    [HideInInspector] public float dashCooldownTimer;
    public float dashCooldownDuration;
    [HideInInspector] public bool solidDashObjectReached;
    [HideInInspector] public bool enemyDashObjectReached;
    [HideInInspector] public bool fullyChargedDash;
    private bool dashEndMove;
    [HideInInspector] public Vector3 dashDirection;
    private Vector3 dashOffset = Vector3.up * -1f;
    private Vector3 originalIndicatorSize;
    public Transform dashChecker;
    // public Image dashChargeIndicator;
    // public Color dashChargeColor; 
    // public Image dashChargeBackground;

    //Attack types
    //private string currentAttackType;
    private string attackType_Sprint = "SprintAttackTrigger";

    [HideInInspector] public string attackState;

    [HideInInspector]


    public enum currentAttackType
    {
        BasicLightAttack,
        BasicHeavyAttack,
        SprintAttack,
    }


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
    public float climbWallCheckDist;
    public float sideWallCheckDist;
    public float exitJumpDistance;
    public Transform frontWallChecker;
    public bool wallRunCooldown;
    private float wallRunCooldownTimer;
    private float wallRunCooldownDuration = 1f;

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
    public GameManager mainGameManager;
    public ComboManager _styleManager;
    public HitPauses hitPauseScript;
    #endregion

    [Header("VISUALS")] //COMPONENTS
    #region
    public TrailRenderer attackTrail;
    public GameObject dashAttackEffect;
    //public TrailRenderer sprintTrailLeft;
    // public TrailRenderer sprintTrailRight;
    public GameObject landVFX;
    public GameObject jumpVFX;
    public GameObject groundSlamVFX;
    public GameObject airPunchEffect;
    public GameObject dashStartEffect;
    public GameObject dashTrailEffect;

    public TrailRenderer[] footTrail;
    private float sprintTrailTimer;
    private float sprintTrailDuration;
    private float attackTrailDuration;
    private float attackTrailTimer;
    private bool enableTrail = false;
    public LineRenderer dashLine;


    #endregion

    [Header("Components")] //COMPONENTS
    #region
    public Animator playerAnim;
    public Transform playerModel;
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
    public Collider mainCollider;
    public Transform dashpoint;
    public Transform enemyLaunchPoint;
    #endregion

    [Header("FEEDBACK")]
    #region
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks chargedJumpFeedback;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks DashStartFeeback;
    public MMFeedbacks DashEndFeeback;
    public MMFeedbacks DashAttackFeedback;
    public MMFeedbacks AttackFeedback;
    public MMFeedbacks BasicLightAttackFeedback;
    public MMFeedbacks BasicHeavyAttackFeedback;
    public MMFeedbacks SprintAttackFeedback;
    public MMFeedbacks AirSlamStartFeeback;
    public MMFeedbacks AirSlamEndFeedback;
    public MMFeedbacks PlayerDamagedFeedback;
    public MMFeedbacks sprintFeedback;
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
    public bool canFall = true;
    public bool canStartAirSmash;
    public bool canEndAirSmash = true;
    public bool canEndDash = false;
    public bool canStartSprintFeedback = false;

    public bool isUpperCutting;
    public bool isMoving;
    public bool isAttacking;
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
    public bool isFreeFalling;
    public bool eventPausing;

    public bool alllowForwardForce;

    public bool wasSprintingBeforeJump;
    #endregion



    private void Start()
    {
        //Set components
        velocityHash = Animator.StringToHash("MoveVelocity");
        raycastCheckMeshr = raycastCheck.GetComponent<MeshRenderer>();
        raycastCheck.gameObject.SetActive(false);


        //Set default values
        defaultSkinMat = meshR.materials;
        currentMoveSpeed = maxMoveSpeed;
        currentDashDistance = minDashDistance;
        dashDelayTimer = dashDelayDuration;
        currentRotateSpeed = defaultRotateSpeed;
        sprintTrailDuration = footTrail[1].time;
        attackTrailDuration = attackTrail.time;
        attackTrail.time = -1f;
        foreach (TrailRenderer trail in footTrail) trail.time = -.1f;
        airSmashDelayTimer = 0f;
        airSmashCooldownTimer = airSmashCooldownDuration;
        dashCooldownTimer = 0f;
        playerRb.drag = groundStopDrag;
        startingRotation = playerModel.transform.eulerAngles;


        //Set default states
        ResetStates();
        fixedControllerState = (int)currentState.MOVING;
        controllerState = (int)currentState.MOVING;


    }



    private void Update()
    {
        if (!mainGameManager.gameIsPaused && !eventPausing)
        {
            switch (controllerState)
            {
                case (int)currentState.MOVING:

                    GroundCheck();
                    CheckForRage();
                    CheckForMoveInput();
                    CheckForAttack();

                    HandleMoveSpeed();
                    HandleRotation();
                    HandleDrag();
                    CheckForJump();
                    CheckForWall();

                    CheckForDash();
                    CheckForAirSmash();
                    CheckForBlock();
                    HandleSprinting();
                    break;

                case (int)currentState.WALLRUNNING:
                    CheckForRage();
                    CheckForJump();
                    CheckForWall();
                    CheckForAirSmash();
                    CheckForDash();
                    HandleMoveSpeed();
                    CheckForMoveInput();
                    HandleWallRunning();
                    HandleDrag();
                    CheckForBlock();
                    CheckForAirSmash();
                    break;

                case (int)currentState.ATTACKING:
                    CheckForRage();
                    HandleMoveSpeed();
                    CheckForAttack();
                    CheckForAirSmash();
                    CheckForDash();
                    CheckForMoveInput();
                    HandleRotation();
                    CheckForBlock();
                    CheckForJump();
                    CheckForAirLaunch();
                    GroundCheck();
                    CheckForWall();
                    break;

                case (int)currentState.DASHING:
                    CheckForDash();
                    CheckForAirSmash();
                    CheckForBlock();
                    CheckForWall();
                    break;

                case (int)currentState.AIRSMASHING:
                    CheckForDash();
                    CheckForMoveInput();
                    CheckForAirSmash();
                    HandleRotation();
                    CheckForWall();
                    break;


                case (int)currentState.BLOCKING:
                    DoBlock(previousState);
                    //       GroundCheck();
                    break;

                case (int)currentState.STUNNED:
                    break;
            }

            HandleSprinting();
            HandleAnimations();
            HolsterWeapon();
        }

    }


    private void FixedUpdate()
    {
        if (!mainGameManager.gameIsPaused && !eventPausing)
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
                    MovePlayer();
                    // MovePlayer();
                    break;
                case (int)currentState.AIRSMASHING:
                    MovePlayer();
                    DoAirSmash();
                    break;
                case (int)currentState.STUNNED:
                    break;



            }
            SetTrailRender();
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
        playerAnim.SetBool("IsAirSmashing", isAirSmashing);
        playerAnim.SetBool("IsStunned", isStunned);
        playerAnim.SetBool("IsBlocking", isBlocking);
        playerAnim.SetBool("IsFreeFalling", isFreeFalling);

        //Checks
        playerAnim.SetBool("CanJump", canJump);
        playerAnim.SetBool("CanFall", canFall);
        playerAnim.SetBool("CanMove", canMove);

    }

    void HolsterWeapon()
    {

        if ((isSprinting || isInAir || isAirSmashing || isLanding) && canStartNewAttack || (attackState == playerAttackType.LightPunchHit.ToString() || attackState == playerAttackType.AirLaunchAttack.ToString()) && !isBlocking)
        {
            drawnWeapon.SetActive(false);
            holsteredWeapon.SetActive(true);
            holsterWeapon = true;
        }
        else
        {
            drawnWeapon.SetActive(true);
            holsteredWeapon.SetActive(false);
            holsterWeapon = false;
        }


        /*
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
        */
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
        if (input.sprintButtonPressed && canSprint && (isMoving || isWallRunning) && !isUpperCutting)
        {
            isSprinting = true;
            if (!isWallRunning) canStartWallrun = true;
        }
        else
        {
            canStartWallrun = false;
            isSprinting = false;
        }


        //Set min movespeed 
        if (isRunning) currentMinMoveSpeed = minRunningMoveSpeed;
        else currentMinMoveSpeed = minWalkingMoveSpeed;


        //Set move speed 
        if (isGrounded)  //Handle ground move speed changes
        {

            if (isAttacking) currentMoveSpeed = attackMoveSpeed;
            if (isSprinting) currentMoveSpeed = sprintMoveSpeed;  //Sprinting 
            else if (isMoving && !isSprinting)
            {

                if (!isRunning) currentMoveSpeed = maxMoveSpeed * moveVelocity; //Walking
                else if (isRunning) currentMoveSpeed = maxMoveSpeed; //Running 
                if (currentMoveSpeed < currentMinMoveSpeed) currentMoveSpeed = currentMinMoveSpeed; //Check for min move speed 
            }

        }

        //Special cases 

        //      else if (attackState == playerAttackType.AirLaunchAttack.ToString()) currentMoveSpeed = 0;
        else if (isWallRunning) currentMoveSpeed = wallRunMoveSpeed; //Wallrunning
                                                                     //  else if (wasSprintingBeforeJump) currentMoveSpeed = currentMoveSpeed * 1.2f; //Jumping after sprinting 
        else if (isInAir) //Normale air move speed 
        {
            //wasSprintingBeforeJump = false; 
            if (isFreeFalling) currentMoveSpeed = freeFallMoveSpeed;
            else if (isChargedJump) currentMoveSpeed = chargedJumpMoveSpeed;
            else if (!isAttacking) currentMoveSpeed = airMoveSpeed;
            //   else if (isAttacking) currentMoveSpeed = attackMoveSpeed;
        }

    }

    void HandleDrag()
    {
        if (!isGrounded) playerRb.drag = airDrag;
        else if (isGrounded && isMoving || isWallRunning) playerRb.drag = 5f;
        else if (!isMoving) playerRb.drag = groundStopDrag;
    }

    void HandleSprinting()
    {
        if (input.sprintButtonPressed && canSprint && (isMoving && isGrounded || isWallRunning) && !isLanding)
        {
            if (canStartSprintFeedback)
            {
                sprintFeedback?.PlayFeedbacks();
                canStartSprintFeedback = false;
            }
        }

        if (!isGrounded && !isWallRunning || !isSprinting)
        {
            sprintFeedback?.StopFeedbacks();
            canStartSprintFeedback = true;
        }


    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * currentDashDistance, Color.green);
        forward.y = 0;


        //Check for airborne dashTargets
        // ray, out hit, groundCheckHeight, envorinmentLayer
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward.normalized);
        RaycastHit upHitOut;
        if (Physics.Raycast(ray, out upHitOut, currentDashDistance, 1 << LayerMask.NameToLayer("DashPoint")) && fullyChargedDash)
        {
            //dashpoint = upHitOut.collider.transform;

            if (upHitOut.collider.transform != dashpoint)
            {
                dashTargetCount++;
            }

            if (dashTargetCount == 1)
            {
                dashpoint = upHitOut.collider.transform;
                DashPoint dashScript = dashpoint.GetComponent<DashPoint>();
                dashScript.isTargeted = true;
            }
            else if (dashTargetCount > 1)
            {
                DashPoint oldDashScript = dashpoint.GetComponent<DashPoint>();
                oldDashScript.isTargeted = false;
                dashpoint = null;
                dashTargetCount--;

                dashpoint = upHitOut.collider.transform;
                //    DashPoint dashScript = dashpoint.GetComponent<DashPoint>();
                //    dashScript.isTargeted = true;

            }
        }
        else if (dashpoint != null && !isDashing && !isDashAttacking || !fullyChargedDash && dashpoint != null)
        {
            dashpoint.GetComponent<DashPoint>().isTargeted = false;
            dashpoint = null;
            dashTargetCount--;
        }



        //Check for charged dash obstacles
        if (input.dashButtonPressed && !isDashing && canDash && dashpoint == null)
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
        else if (dashpoint != null)
        {
            //  raycastCheck.gameObject.SetActive(false); 
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
                if (isAttacking) currentRotateSpeed = attackingRotateSpeed;
                else currentRotateSpeed = defaultRotateSpeed;
                float singleStep = currentRotateSpeed * Time.deltaTime;

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
                mainGameManager.DoHaptics(.1f, .1f, .2f);
                fullyChargedDash = true;
                currentDashDistance = maxDashDistance;
            }
        }

        //Dash button is released
        if (!input.dashButtonPressed && isChargingDash)
        {
            //Add small dash delay for the animation charge up 
            if (dashDelayTimer == dashDelayDuration)
            {

                if (dashpoint != null)
                {
                    DashPoint dashTargetScript = dashpoint.GetComponent<DashPoint>();
                    BasicEnemyScript enemyScript = dashTargetScript.enemyScript;
                    dashTargetScript.freezeEnemy = true;
                    dashTargetScript.stayPos = enemyScript.transform.position;
                    //enemyScript.enemyRb.isKinematic = true;
                    enemyScript.canMove = false;
                    enemyScript.enemyAgent.enabled = false;
                    enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);
                    dashDirection = enemyScript.transform.position;
                    transform.LookAt(dashDirection);
                    dashDirection += (transform.forward * 10f);
                    raycastCheck.position = dashDirection;
                    dashDelayTimer = dashDelayDuration;
                }
                else
                {
                    transform.LookAt(dashDirection);
                }

                dashLine.enabled = true;
                dashLine.SetPosition(0, transform.position);
                dashLine.SetPosition(1, dashDirection);



                playerState.canBeHit = false;
                canMove = false;
                canLand = false;
                ResetAnimator();
                transform.LookAt(dashDirection);
                playerAnim.SetTrigger("DashStartTrigger");
            }
            dashDelayTimer -= Time.deltaTime;


            if (dashDelayTimer <= 0)
            {
                dashDelayTimer = dashDelayDuration;
                isDashing = true;
                canDash = false;
                isChargingDash = false;
                mainCollider.isTrigger = true;
                Physics.IgnoreLayerCollision(enemyLayer, this.gameObject.layer, true);

                mainGameManager.DoHaptics(.2f, .5f, .5f);
                DashStartFeeback?.PlayFeedbacks();
                meshR.materials = holoSkinMat;

                playerRb.drag = airDrag;
                playerRb.velocity = new Vector3(0, 0, 0);
                playerRb.constraints = airConstraints.constraints;

                controllerState = (int)currentState.DASHING;
                fixedControllerState = (int)currentState.DASHING;

                GameObject dashEffect = Instantiate(dashTrailEffect, playerCamera.transform.position, transform.rotation);
                // dashEffect.transform.LookAt(transform.position);
                dashEffect.transform.parent = playerCamera.transform;
                dashEffect.AddComponent<CleanUpScript>();
            }
        }

        //The player is dashing 
        if (isDashing && !solidDashObjectReached && !enemyDashObjectReached && !canEndDash) //Maintain dash
        {
            if (dashpoint == null && currentDashDistance <= 6f || dashpoint != null && currentDashDistance <= 1f)
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
        else if (isDashing && canEndDash && dashpoint == null) //End dash
        {
            ResetDash();
        }
        else if (enemyDashObjectReached)
        {
            //   dashChecker.gameObject.SetActive(false);
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
            dashLine.SetPosition(0, transform.position + transform.forward * 2f + new Vector3(0, 2, 0));
            dashLine.SetPosition(1, dashDirection);
            playerRb.AddForce((dashDirection - transform.position) * dashForwardForce, ForceMode.VelocityChange);

        }
    }

    public void DoDashAttack()
    {
        // camHandeler.ResetSlowmoCompensation();
        BasicEnemyScript script = dashAttackTarget.transform.GetComponentInParent<BasicEnemyScript>();
        if (!script.isDead)
        {

            script.TakeDamage(dashAttackDamage, playerAttackType.PowerPunch.ToString());
            script.transform.position += new Vector3(0, 2, 0);
            GameObject dashAttackEffect = Instantiate(airPunchEffect, script.transform.position, transform.rotation);
            dashAttackEffect.transform.LookAt(-aimPoint.forward + new Vector3(0, .5f, 0));
            dashAttackEffect.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            dashAttackEffect.AddComponent<CleanUpScript>();

            if (script.canBeStunned)
            {
                script.LaunchEnemy(aimPoint.forward, dashAttackForce, 5f);
                mainGameManager.DoHaptics(.2f, .4f, .6f);
            }
        }





    }

    public void ResetDash()
    {

        dashLine.enabled = false;
        playerRb.velocity = new Vector3(0, 0, 0);
        DashEndFeeback?.PlayFeedbacks();
        mainGameManager.DoHaptics(.15f, .1f, .2f);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        solidDashObjectReached = false;
        enemyDashObjectReached = false;
        // dashChecker.gameObject.SetActive(false);
        raycastCheck.gameObject.SetActive(false);
        if (!isRaging) meshR.materials = defaultSkinMat;
        if (jumpCount > 1) jumpCount = 1;

        isDashing = false;
        // if(dashpoint != null) dashpoint = null; 
        canEndDash = false;
        canMove = true;
        dashEndMove = false;
        playerRb.useGravity = true;
        playerState.canBeHit = true;
        fullyChargedDash = false;
        mainCollider.isTrigger = false;
        Physics.IgnoreLayerCollision(enemyLayer, this.gameObject.layer, false);

        currentDashDistance = minDashDistance;
        dashDelayTimer = dashDelayDuration;
        controllerState = (int)currentState.MOVING;
        fixedControllerState = (int)currentState.MOVING;
    }



    //Jumping
    void CheckForJump()
    {

        //Check for jump charge 
        if (input.jumpButtonPressed)
        {
            wantsToJump = true;
            maxJumpChargeTimer += Time.deltaTime;

            if (currentJumpForce < maxChargedJumpForce && maxJumpChargeTimer > .3f) //Build up charge force
            {
                currentJumpForce = ((maxJumpChargeTimer - .3f) / maxJumpChargeDuration) * (maxChargedJumpForce + minJumpForce);
                mainGameManager.DoHaptics(.2f, .1f * (currentJumpForce / maxChargedJumpForce), .2f * (currentJumpForce / maxChargedJumpForce));
            }
            else if (currentJumpForce >= maxChargedJumpForce)
            {
                currentJumpForce = maxChargedJumpForce + 100f;
                mainGameManager.DoHaptics(.2f, .3f, .5f);
            }
        }

        if (jumpCount < maxJumps)
        {
            //Player presses the jump key
            if (wantsToJump && !input.jumpButtonPressed && canJump)
            {
                jumpCount++;
                playerAnim.SetFloat("JumpCount", jumpCount);
                //if (jumpCount > 1) inAirTime = .2f;
                //   if (isChargedJump) inAirTime = .2f; 

                if (isSprinting && jumpCount == 1) wasSprintingBeforeJump = true; //check if we we're running before the jump

                if (isWallRunning) ResetStates();
                controllerState = (int)currentState.MOVING;
                fixedControllerState = (int)currentState.MOVING;

                groundCheckTimer = groundCheckJumpDelay;

                isGrounded = false;
                isLanding = false;
                hasJumped = true;
                canJump = false;
                wantsToJump = false;


            }
            else if (!input.jumpButtonPressed && !hasJumped) canJump = true;

            //Do the jump
            if (hasJumped)
            {
                //Check from where we are jumping
                GroundCheck();
                if (input.sprintButtonPressed && canSprint) isSprinting = true;
                CheckForWall();

                if (isWallRunning)
                {
                    wallRunCooldown = true;
                    ExitWallRun();
                }


                //Set and trigger correct animation
                ResetAnimator();
                if (currentJumpForce < minJumpForce || inAirTime >= .2f)
                {

                    currentJumpForce = minJumpForce;
                    if (!isChargedJump) isChargedJump = false;
                }
                else
                {
                    chargedJumpFeedback?.PlayFeedbacks();
                    isChargedJump = true;
                }

                if (currentJumpForce > minJumpForce) playerAnim.SetFloat("RandomJumpVar", 5);
                else if (jumpCount <= 1) playerAnim.SetFloat("RandomJumpVar", 0);
                else playerAnim.SetFloat("RandomJumpVar", Random.Range(1, 5)); //Randomly select a second jump animation 
                playerAnim.SetTrigger("JumpTrigger");


                DoJump(currentJumpForce);

                //Reset values
                currentJumpForce = 0;
                maxJumpChargeTimer = 0;
                hasJumped = false;
            }
        }
        else canJump = false;


        //Add extra falling gravity 
        if (playerRb.velocity.y < 1)
        {
            playerRb.velocity -= Vector3.down * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //Low and high jumps
        else if (playerRb.velocity.y > 0) playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravity - 1) * Time.deltaTime;

        HandleFreeFall();
    }

    void HandleFreeFall()
    {

        //Trigger free fall
        if (playerRb.velocity.y <= -20f && !isLanding && !isGrounded && inAirTime >= freefallWaitTime && !isAttacking && !isWallRunning)
        {
            if (!isFreeFalling)
            {
                playerAnim.SetTrigger("FreeFallTrigger");
                playerModel.localEulerAngles = new Vector3(playerModel.localEulerAngles.x + 72, 0, 0);
            }
            isFreeFalling = true;

        }
        else
        {
            playerModel.localEulerAngles = new Vector3(0, 0, 0);
            isFreeFalling = false;

        }
    }

    public void DoJump(float jumpHeight)
    {

        DOTween.Kill(this.transform);
        playerRb.isKinematic = false;
        playerRb.constraints = airConstraints.constraints;
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        playerRb.drag = airDrag;

        mainGameManager.DoHaptics(.1f, .04f, .07f);
        JumpFeedback?.PlayFeedbacks();
        GameObject jumpEffect = Instantiate(jumpVFX, transform.position + new Vector3(0f, .3f, 0f), transform.rotation);
        jumpEffect.transform.eulerAngles = new Vector3(-90, jumpEffect.transform.eulerAngles.y, jumpEffect.transform.eulerAngles.z);
        jumpEffect.AddComponent<CleanUpScript>().SetCleanUp(3f);

        playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        if (jumpHeight > minJumpForce && isRunning)
        {


        }
        else if (jumpHeight > minJumpForce && isSprinting)
        {

        }

        if (isWallRunning)
        {
            playerRb.AddForce(-transform.forward * 1000, ForceMode.Impulse); //Move the player away from the wall so he doesn't get stuck
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, Vector3.down);

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
                    if (!isGrounded && inAirTime >= .2f)
                    {
                        mainGameManager.DoHaptics(.1f, .03f, .05f);
                        GameObject landEffect = Instantiate(landVFX, transform.position, transform.rotation);
                        landEffect.AddComponent<CleanUpScript>();
                    }

                    isGrounded = true;
                    inAirTime = 0f;
                    playerRb.useGravity = false;
                    playerRb.constraints = groundConstraints.constraints;
                    canJump = true;
                    jumpCount = 0;
                }

            }
            //Player isn't grounded 
            else if (!isUpperCutting)
            {
                playerRb.constraints = airConstraints.constraints;
                if (!isLanding) canLand = true;
                isGrounded = false;
                // isLanding = false; 
                isInAir = true;
                if (!isAttacking) inAirTime += Time.deltaTime;
                else inAirTime = 0;
                playerRb.useGravity = true;

            }

            //Player can land 
            if ((Physics.Raycast(ray, out hit, landCheckHeight) && canLand && isInAir && !isUpperCutting))
            {
                if (hit.collider.gameObject.layer == EnvorinmentLayer)
                {
                    canLand = false;
                    isInAir = false;
                    // isFreeFalling = false; 
                    wantsToJump = false;
                    isLanding = true;
                    landingDelayTimer = 0f;
                    isChargedJump = false;
                    playerAnim.SetTrigger("LandingTrigger");

                }

                //if(Vector3.Distance(hit.point, transform.position) < .1f)
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
        if (moveVelocity > .3f) landingDelayDuration = (moveLandingClip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed);
        else if (moveVelocity <= .3f) landingDelayDuration = (noMoveLandingClip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed);

        landingDelayTimer += Time.deltaTime;

        if (landingDelayTimer >= landingDelayDuration - .15f) isLanding = false;


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
        Debug.DrawLine(frontWallChecker.position + new Vector3(0, 0.1f, 0), frontWallChecker.position + frontWallChecker.forward * climbWallCheckDist, Color.green);


        if (Physics.Raycast(frontRay, out hit, frontWallCheckDist) && isSprinting)
        {
            //Start wallrun
            if (hit.collider.gameObject.layer == EnvorinmentLayer && !wallRunCooldown) //Environment
            {
                if (canStartWallrun) StartWallRun();
                isWallRunning = true;
            }
        }
        else if (isWallRunning && isSprinting || isWallRunning && !input.sprintButtonPressed || isWallRunning && !input.jumpButtonPressed && wantsToJump)
        {
            //Close and predictive raycast can both not find a target
            if (Physics.Raycast(frontRay, out hit, climbWallCheckDist))
            {
                if (hit.collider.gameObject.layer != EnvorinmentLayer)
                {
                    ExitWallRun();
                }
            }
            else
            {
                ExitWallRun();
            }

            if (!input.jumpButtonPressed && wantsToJump || !input.sprintButtonPressed)
            {
                jumpOffPoint = transform.position - transform.forward * exitJumpDistance;
                wallRunExitWithJump = true;
                ExitWallRun();
            }
        }

        if (wallRunCooldown)
        {
            wallRunCooldownTimer += Time.deltaTime;
            if (wallRunCooldownTimer >= wallRunCooldownDuration)
            {
                wallRunCooldownTimer = 0f;
                wallRunCooldown = false;
            }
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

        if (wallRunExitWithJump)
        {
            transform.DOMove(jumpOffPoint, .25f);
            // transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z), .5f);
            if (!input.jumpButtonPressed && wantsToJump) DoJump(currentJumpForce); //Jump off by pressing A
            else transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z), .5f); //Stop sprinting mid object
            jumpCount = 1;
            //  print("This one"); 


        }

        else if (!input.jumpButtonPressed && !!wantsToJump) //Reach top of object
        {
            //  DoJump(minJumpForce * .05f); 
            //  playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y / 5, playerRb.velocity.z); 



        }
        else
        {
            //  print("FUCK OFF");
            //    playerRb.velocity = new Vector3(0, 0, 0);

            // playerRb.velocity = new Vector3(0,0,0); 
        }

        print("This one");
        wallRunExitWithJump = false;
        isWallRunning = false;
        canStartWallrun = true;

        controllerState = (int)currentState.MOVING;
    }

    void HandleWallRunning()
    {
        float forwardForce = 1f;
        playerRb.AddForce(transform.forward * forwardForce, ForceMode.VelocityChange);
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
                controllerState = (int)currentState.AIRSMASHING;
            }


            airSmashDelayDuration = (playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed) - .1f;
            airSmashDelayTimer += Time.deltaTime;

            if (airSmashDelayTimer >= airSmashDelayDuration)
            {


                AirSlamStartFeeback?.PlayFeedbacks();
                mainGameManager.DoHaptics(.2f, .2f, .3f);
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
                if (!isRaging) meshR.materials = defaultSkinMat;
                airSmashDelayTimer = 0f;
                playerAnim.SetTrigger("AirSmashEndTrigger");
            }

            airSmashDelayDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;
            airSmashDelayTimer += Time.deltaTime;

            EndAirSmash();
            fixedControllerState = (int)currentState.MOVING;

        }
        else if (isAirSmashing && airSmashDelayTimer >= airSmashDelayDuration)
        {
            //DoAirSmash();
            //DoAirSmash();
            fixedControllerState = (int)currentState.AIRSMASHING;
            //  mainCollider.isTrigger = true; 
        }


        //Air smash Cooldown
        if (airSmashCooldownTimer > 0) airSmashCooldownTimer -= Time.deltaTime;
        if (airSmashCooldownTimer <= 0 && !input.airSmashButtonPressed && !isAirSmashing) canStartAirSmash = true;


    }

    void DoAirSmash()
    {
        playerRb.AddForce(-Vector3.up * airSmashDownSpeed, ForceMode.VelocityChange);
        playerState.canBeHit = false;
        dashChecker.gameObject.SetActive(true);

        //Dash has reached groundF
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -transform.up);
        if ((Physics.Raycast(downRay, out hit, airSmashEndHeight)))
        {
            if (hit.transform.gameObject.layer == EnvorinmentLayer || hit.transform.gameObject.layer == LayerMask.NameToLayer("DamagePlane"))
            {

                playerRb.velocity = new Vector3(0, 0, 0);
                AirSlamEndFeedback?.PlayFeedbacks();
                isGroundSmash = true;
                playerState.canBeHit = false;
                DoJump(minJumpForce / 1.2f);


                Collider[] colls = Physics.OverlapSphere(transform.position, groundSlamRadius);
                foreach (Collider slamTarget in colls)
                {
                    if (slamTarget.gameObject.layer == enemyLayer)
                    {
                        if (slamTarget.GetComponent<BasicEnemyScript>() != null)
                        {
                            BasicEnemyScript enemyScript = slamTarget.GetComponent<BasicEnemyScript>();
                            if (enemyScript.canBeStunned)
                            {
                                enemyScript.TakeDamage(currentGroundSlamDamage, playerAttackType.GroundSlam.ToString());
                                enemyScript.LaunchEnemy((slamTarget.transform.position - transform.position), slamImpactForwardForce, Random.Range(slamImpactUpForce / 1.1f, slamImpactUpForce * 1.1f));
                            }
                        }
                        else if (slamTarget.GetComponent<EyeBall>() != null)
                        {
                            EyeBall eyeBallScript = slamTarget.GetComponent<EyeBall>();
                            eyeBallScript.TakeDamage(500, "AirSlam");
                        }
                    }
                }

                mainGameManager.DoHaptics(.37f, 1f, 1.8f);

                GameObject slamEffect = Instantiate(groundSlamVFX, hit.point, transform.rotation);
                slamEffect.AddComponent<CleanUpScript>();
            }
        }

    }

    public void EndAirSmash()
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
            jumpCount = 1;
            playerState.canBeHit = true;

            playerRb.useGravity = true;
            airSmashDelayTimer = 0f;
            airSmashCooldownTimer = airSmashCooldownDuration;
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;
            //  mainCollider.isTrigger = false;
        }
    }

    void DoAirSlamAttack()
    {

    }

    //Blockiong

    void CheckForBlock()
    {
        if (input.blockButtonPressed && canBlock) //Check if the player is blocking
        {
            ResetStates();
            isBlocking = true;
            canBlock = false;
            //HolsterWeapon(false); 
            previousState = controllerState;
            playerAnim.SetBool("IsAttacking", false);
            isAttacking = false;
            canStartNewAttack = true;
            nextAttackTimer = 10f;
            playerAnim.SetTrigger("BlockTrigger");
            controllerState = (int)currentState.BLOCKING;
        }

        if (!canBlock && !isBlocking) //Recharge the block after use 
        {
            blockRechargeTimer += Time.deltaTime;
            if (blockRechargeTimer >= blockRechargeDuration && !input.blockButtonPressed)
            {
                blockRechargeTimer = 0f;
                hasParriedAttack = false;
                canBlock = true;
            }
        }
    }

    void DoBlock(int previousState)
    {
        blockTimer += Time.deltaTime;
        playerState.canBeHit = false;

        if (blockTimer >= blockDuration)
        {
            canBlockStun = true;
            isBlocking = false;
            playerState.canBeHit = true;
            blockTimer = 0f;
            controllerState = previousState;
            //  controllerState = (int)currentState.MOVING; 
        }

    }


    //Attacking
    void CheckForAttack()
    {
        //Check for player pressing an attack button 
        if ((input.heavyAttackButtonPressed || input.lightAttackButtonPressed) && nextAttackTimer >= nextAttackDuration - (attackTransitionOffset * 2.2f))
        {
            if (input.heavyAttackButtonPressed) //We want to do a heavy attack 
            {
                wantsToHeayAttack = true;
                wantsToLightAttack = false;
            }
            if (input.lightAttackButtonPressed) // We want to do a light attack 
            {
                wantsToHeayAttack = false;
                wantsToLightAttack = true;
            }
        }

        //Check for player holding an attack button
        if (input.heavyAttackButtonPressed) heavyAttackHoldTimer += Time.deltaTime;
        else heavyAttackHoldTimer = 0f;

        if (input.lightAttackButtonPressed)
        {
            lightAttackHoldTimer += Time.deltaTime;
            if (lightAttackHoldTimer >= airLaunchHoldDuration) mainGameManager.DoHaptics(.1f, .1f, .1f);

        }
        else if (lightAttackHoldTimer >= airLaunchHoldDuration)
        {
            wantsToAirLaunchCut = true;
        }
        else lightAttackHoldTimer = 0f;



        //Start attacking state once the player has released the attack button 
        if ((!input.heavyAttackButtonPressed && wantsToHeayAttack || !input.lightAttackButtonPressed && wantsToLightAttack) && canStartNewAttack)
        {
            if (!isAttacking)
            {
                axeComboLength = 0;
                punchComboLength = 0;
                ResetAnimator();
            }

            SetAttackType();
            ResetStates();
            isAttacking = true;
            canStartNewAttack = false;
            playerRb.isKinematic = false;
            // playerAnim.speed = axeAttackSpeed;
            nextAttackTimer = nextAttackDuration = 10f;

            controllerState = (int)currentState.ATTACKING;
            fixedControllerState = (int)currentState.ATTACKING;

        }

        //Attack loop 
        if (isAttacking) HandleAttack();
        else
        {
            nextAttackDuration = 0;
            nextAttackTimer = 10f;
        }
    }


    void SetAttackType()
    {
        int totalAttackTrees = 5;
        if (isSprinting && !wantsToAirLaunchCut)
        {
            attackState = playerAttackType.SprintAttack.ToString();
            axeComboLength = 2;
            // AttackFeedback = SprintAttackFeedback;
            currentAttackDamage = 0;
            currentAttackForwardForce = sprintAttackForwardForce;
            playerAnim.SetFloat("SprintAttackType", 1);
            playerRb.velocity = new Vector3(0, 0, 0);


        }
        else if (wantsToAirLaunchCut)
        {
            totalAttackTrees = 3;
            attackState = playerAttackType.AirLaunchAttack.ToString();

            // int originalReaction = playerAnim.GetInteger("AirLaunchAttackType");
            int randomType = Random.Range(1, totalAttackTrees + 1);
            playerAnim.SetFloat("AirLaunchAttackType", randomType);
            lightAttackHoldTimer = 0;
        }
        else if (wantsToHeayAttack)
        {
            totalAttackTrees = 6;
            attackState = playerAttackType.HeavyAxeHit.ToString();
            attackTransitionOffset = axeAttackTransitionOffset;
            AttackFeedback = BasicHeavyAttackFeedback;
            currentAttackDamage = basicHeavyAttackDamage;
            currentAttackForwardForce = axeAttackForwardForce;



            //Set new combo 
            if (axeComboLength == 0)
            {
                //No double hit animations
                int originalReaction = playerAnim.GetInteger("AxeAttackType");
                int newRandomNumber = Random.Range(1, totalAttackTrees + 1);
                if (newRandomNumber == originalReaction)
                {
                    // if (newRandomNumber - 1 <= 0) newRandomNumber += 1;
                    //else newRandomNumber -= 1;
                }
                playerAnim.SetInteger("AxeAttackType", newRandomNumber);  //Decide which combo tree to go into (only once per full combo tree)
            }

            //Set current attack tree length
            if (playerAnim.GetInteger("AxeAttackType") == 1) totalAxeComboLength = 3;
            else if (playerAnim.GetInteger("AxeAttackType") == 2) totalAxeComboLength = 4;
            else if (playerAnim.GetInteger("AxeAttackType") == 3) totalAxeComboLength = 4;
            else if (playerAnim.GetInteger("AxeAttackType") == 4) totalAxeComboLength = 4;
            else if (playerAnim.GetInteger("AxeAttackType") == 5) totalAxeComboLength = 6;
            else if (playerAnim.GetInteger("AxeAttackType") == 6) totalAxeComboLength = 4;
            //  else if (playerAnim.GetInteger("AxeAttackType") == 7) totalAxeComboLength = 4;

        }
        else if (wantsToLightAttack)
        {
            totalAttackTrees = 5;
            attackState = playerAttackType.LightPunchHit.ToString();
            attackTransitionOffset = punchAttackTransitionOffset;
            AttackFeedback = BasicLightAttackFeedback;
            currentAttackDamage = basicLightAttackDamage;
            currentAttackForwardForce = punchAttackForwardForce;



            if (punchComboLength == 0)
            {
                //No double hit animations
                int originalReaction = playerAnim.GetInteger("PunchAttackType");
                int newRandomNumber = Random.Range(2, totalAttackTrees + 1);

                if (newRandomNumber == originalReaction)
                {
                    if (newRandomNumber - 1 <= 0) newRandomNumber += 1;
                    else newRandomNumber -= 1;
                }

                playerAnim.SetInteger("PunchAttackType", newRandomNumber);  //Decide which combo tree to go into (only once per full combo tree)
            }

            //Set current punch-attack tree length 
            if (playerAnim.GetInteger("PunchAttackType") == 1) totalPunchComboLength = 4;
            else if (playerAnim.GetInteger("PunchAttackType") == 2) totalPunchComboLength = 4;
            else if (playerAnim.GetInteger("PunchAttackType") == 3) totalPunchComboLength = 4;
            else if (playerAnim.GetInteger("PunchAttackType") == 4) totalPunchComboLength = 4;
            else if (playerAnim.GetInteger("PunchAttackType") == 5) totalPunchComboLength = 4;

        }
    }

    void HandleAttack()
    {
        //Set attack duration equel to current animation clip length and speed
        if (isAttacking) nextAttackDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / (playerAnim.GetCurrentAnimatorStateInfo(0).speed * playerAnim.speed);


        //Check if we can continue to the next attack
        if (nextAttackTimer >= nextAttackDuration - attackTransitionOffset && (!input.heavyAttackButtonPressed && wantsToHeayAttack || !input.lightAttackButtonPressed && wantsToLightAttack || attackState == playerAttackType.LightPunchHit.ToString() && wantsToHeayAttack) || attackState == playerAttackType.HeavyAxeHit.ToString() && wantsToLightAttack)
        {
            if (attackState == playerAttackType.SprintAttack.ToString())
            {

            }
            else if (wantsToAirLaunchCut)
            {
                wantsToHeayAttack = false;
                wantsToLightAttack = false;
                wantsToAirLaunchCut = false;


            }
            else if (wantsToHeayAttack) //We are doing an axe attack 
            {
                wantsToLightAttack = false;
                wantsToAirLaunchCut = false;
                if (axeComboLength >= totalAxeComboLength) axeComboLength = 0; //Reset combo tree
                SetAttackType();
                //  CheckForMoveInput(); //Set attack direction 
                axeComboLength++;
                playerAnim.speed = axeAttackSpeed;
                playerAnim.SetInteger("AxeComboLength", axeComboLength);
                wantsToHeayAttack = false;
            }
            else if (wantsToLightAttack) //We are doing an punch attack
            {
                wantsToHeayAttack = false;
                wantsToAirLaunchCut = false;
                if (punchComboLength >= totalPunchComboLength) punchComboLength = 0; //Reset combo tree      
                SetAttackType();
                //  CheckForMoveInput(); //Set attack direction 
                punchComboLength++;
                playerAnim.speed = punchAttackSpeed;
                playerAnim.SetInteger("PunchComboLength", punchComboLength);
                wantsToLightAttack = false;
            }

            CheckForMoveInput(); //Set attack direction 

            //Trigger correct animation stuff 
            AttackFeedback?.PlayFeedbacks();
            playerAnim.SetBool("IsAttacking", true);
            playerAnim.SetTrigger(attackState + "Trigger");


            nextAttackTimer = 0f;
            attackMoveLerpT = 0f;
            canRotate = true;

            attackStartPos = transform.position;
        }



        //Check if the combo is broken 
        if (nextAttackTimer >= nextAttackDuration)
        {
            playerAnim.SetBool("IsAttacking", false);
            alllowForwardForce = false;

            canStartNewAttack = true;
            playerAnim.speed = 1f;
            attackTargetInRange = false;
            canRotate = true;
            playerRb.isKinematic = false;
            if (!isRaging) meshR.materials = defaultSkinMat;
            currentMoveSpeed = minWalkingMoveSpeed;

            if (attackState == playerAttackType.SprintAttack.ToString()) sprintHitList.Clear();
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;
            nextAttackTimer = 10f;
            //CheckForAttack();            
        }
        else if (nextAttackTimer < nextAttackDuration - attackTransitionOffset / 2)
        {
            if (attackState != playerAttackType.SprintAttack.ToString() && attackState != playerAttackType.AirLaunchAttack.ToString() && !input.jumpButtonPressed) playerRb.isKinematic = true; //Freeze enemy mid air if attacking
            else if (alllowForwardForce && attackState == playerAttackType.SprintAttack.ToString()) AllowAttackDamage();


            //currentMoveSpeed = attackMoveSpeed;
            isAttacking = true;
        }
        else
        {
            //   currentMoveSpeed = minWalkingMoveSpeed; 
            playerRb.isKinematic = false;
        }

        //Exit attack state
        if (nextAttackTimer >= nextAttackDuration + maxTimeBetweenAttacks || input.jumpButtonPressed && canJump)
        {
            axeComboLength = 0;
            punchComboLength = 0;
            playerAnim.speed = 1f;
            isAttacking = false;
            attackTargetInRange = false;
            canRotate = true;
            alllowForwardForce = false;
            playerRb.isKinematic = false;
            canStartNewAttack = true;
            playerAnim.SetBool("IsAttacking", false);
            limbCheckerScript.hitLimbs.Clear();
            limbCheckerScript.hitInsides.Clear();
            nextAttackTimer = 10f;
            if (!isRaging) meshR.materials = defaultSkinMat;

            if (attackState == playerAttackType.SprintAttack.ToString()) sprintHitList.Clear();
            fixedControllerState = (int)currentState.MOVING;
            controllerState = (int)currentState.MOVING;
        }

        nextAttackTimer += Time.deltaTime;
    }


    //Resets
    public void ResetStates()
    {
        canRotate = true;
        canStartNewAttack = true;
        isStunned = false;
        isMoving = false;
        isWalking = false;
        isSprinting = false;
        isBlocking = false;
        hasParriedAttack = false;

        // isairLaunchting = false; 
        isRunning = false;
        isFreeFalling = false;
        isInAir = false;
        isLanding = false;
        isGrounded = false;
        isWallRunning = false;
        isFreeFalling = false;
        isAttacking = false;
        isDashing = false;
        isChargingDash = false;
        isDashAttacking = false;
        isAirSmashing = false;
        isGroundSmash = false;
        isChargedJump = false;


        alllowForwardForce = false;

        playerRb.isKinematic = false;
        if (!isRaging) meshR.materials = defaultSkinMat;
        playerAnim.speed = 1f;
        mainCollider.isTrigger = false;
        nextAttackTimer = 0f;

        playerModel.localEulerAngles = new Vector3(0, 0, 0);
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
        attackTargetScript.TargetDamageCheck();
        attackDirection = transform.position + inputDir;

        // if (attackState != currentAttackType.SprintAttack.ToString()) transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .35f).SetUpdate(UpdateType.Fixed);
        //else meshR.materials = defaultSkinMat; 
        //     meshR.materials = defaultSkinMat;
    }

    void AllowAttackForwardForce()
    {
        if (attackState == playerAttackType.AirLaunchAttack.ToString())
        {
            //DoJump(500f);   
            airLaunchPoint = transform.position + transform.up * airLaunchHeight;
            isUpperCutting = true;
            print("uppercut");
            AllowAttackDamage();
            moveSequence.Append(transform.DOMove(airLaunchPoint, .3f).SetUpdate(UpdateType.Fixed));
            GameObject jumpEffect = Instantiate(jumpVFX, transform.position + new Vector3(0f, .3f, 0f), transform.rotation);
            jumpEffect.transform.eulerAngles = new Vector3(-90, jumpEffect.transform.eulerAngles.y, jumpEffect.transform.eulerAngles.z);
            jumpEffect.AddComponent<CleanUpScript>().SetCleanUp(3f);

        }
        else if (attackState == playerAttackType.HeavyAxeHit.ToString())
        {
            // playerAnim.speed = animAttackSpeed * 1.55f; 
            attackDirection = transform.position + inputDir;
            mainGameManager.DoHaptics(.1f, .09f, .09f);
            attackTargetScript.BackCheck();
            //playerRb.AddForce(transform.position + transform.forward * currentAttackForwardForce);
            moveSequence.Append(transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .35f).SetUpdate(UpdateType.Fixed));

        }
        else if (attackState == playerAttackType.LightPunchHit.ToString())
        {
            // playerAnim.speed = animAttackSpeed * 1.55f; 
            attackDirection = transform.position + inputDir;
            mainGameManager.DoHaptics(.1f, .05f, .05f);
            attackTargetScript.BackCheck();
            moveSequence.Append(transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .22f).SetUpdate(UpdateType.Fixed));
        }
        else if (attackState == playerAttackType.SprintAttack.ToString())
        {
            alllowForwardForce = true;
            meshR.materials = holoSkinMat;
            moveSequence.Append(transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .5f).SetUpdate(UpdateType.Fixed));
            SprintAttackFeedback?.PlayFeedbacks();
            mainGameManager.DoHaptics(.2f, .3f, .5f);
        }



    }

    void AllowNextAttack()
    {
        if (attackState == playerAttackType.HeavyAxeHit.ToString() || attackState == playerAttackType.LightPunchHit.ToString())
        {
            nextAttackTimer = nextAttackDuration;
        }
        else if (attackState == playerAttackType.SprintAttack.ToString())
        {
            nextAttackTimer = 10f;
            wantsToHeayAttack = false;
            wantsToLightAttack = false;
            isSprinting = false;
        }
    }

    void SetTrailRender()
    {
        float disableTrailDuration = .75f;

        if (isSprinting || jumpCount >= 1 || isDashing || isAirSmashing || isFreeFalling || attackState == playerAttackType.AirLaunchAttack.ToString()) enableTrail = true;
        else enableTrail = false;

        ///  Debug.Log(attackTrailTimer); 

        //Attack trail 
        if (isAttacking) //Enable the trail 
        {

            if (attackTrail.time <= 0)
            {
                attackTrail.time = attackTrailDuration * .05f;
            }

            if (attackTrail.time < attackTrailDuration)
            {
                attackTrail.time += Time.deltaTime * disableTrailDuration;
            }
            else
            {
                attackTrail.time = attackTrailDuration;
            }
            attackTrailTimer = attackTrailDuration;


        }
        else if (!!isAttacking && attackTrailTimer > -.1) //Eetract the trail      
        {

            attackTrail.time = attackTrailTimer;
        }

        if (!isAttacking && attackTrailTimer > -.1) //Disable the trail        
        {
            attackTrail.time -= Time.fixedDeltaTime * disableTrailDuration;
        }


        //Run trail
        foreach (TrailRenderer trail in footTrail)
        {
            if (enableTrail) //Enable the trail 
            {
                if (trail.time <= 0)
                {
                    trail.time = sprintTrailDuration * .05f;
                }

                if (trail.time < sprintTrailDuration)
                {
                    trail.time += Time.deltaTime * disableTrailDuration;

                }
                else
                {
                    trail.time = sprintTrailDuration;
                }
                sprintTrailTimer = sprintTrailDuration;


            }
            else if (!enableTrail && sprintTrailTimer > -.1) //Eetract the trail      
            {
                trail.time = sprintTrailTimer;
            }
        }

        if (!enableTrail && sprintTrailTimer > -.1) //Disable the trail        
        {
            sprintTrailTimer -= Time.fixedDeltaTime * disableTrailDuration;
        }
    }

    void CheckForAirLaunch()
    {
        if (isUpperCutting)
        {
            if (Vector3.Distance(airLaunchPoint, transform.position) < .5f || attackState != playerAttackType.AirLaunchAttack.ToString())
            {
                isUpperCutting = false;
                nextAttackTimer = nextAttackDuration;


                //

                foreach (BasicEnemyScript enemy in attackTargetScript.airLaunchTargets)
                {

                    enemy.isUpperCutted = false;
                    DOTween.Kill(enemy.transform);
                    enemy.enemyRb.velocity = new Vector3(0, 0, 0);

                    if (!enemy.isDead) enemy.transform.parent = null;  //transform.parent = enemy.transform.parent = enemy.enemySpawnManagerScript.aliveEnemyParent.transform;                                                                      //  enemy.isLaunched = true; 
                    enemy.transform.parent = enemy.enemySpawnManagerScript.aliveEnemyParent;
                }

                attackTargetScript.airLaunchTargets.Clear();
                wantsToAirLaunchCut = false;
                attackState = playerAttackType.Reset.ToString();
                nextAttackTimer = nextAttackDuration;
                // isAttacking = false;  



                playerRb.velocity = new Vector3(0, 0, 0);
            }
        }

        //Thanks voor het helpen opbouwen van dit conecpt corne hope you like it 
    }

    void CheckForRage()
    {
        if (canRage)
        {
            if (input.sprintButtonPressed && input.rightStickPressed && !isRaging)
            {
                DoRage();
                print("Do Rage");
            }
        }

        if (isRaging)
        {
            DoRage();
        }
    }

    void DoRage()
    {
        if (!isRaging)
        {
            GameObject rageEffect1 = Instantiate(rageLightning, transform.position, transform.rotation);
            rageEffect1.transform.parent = transform; 
            rageEffect1.AddComponent<CleanUpScript>();
            rageStartFeedback?.PlayFeedbacks();
            isRaging = true;
            rageElectricity.SetActive(true);
            canRage = false;

            basicLightAttackDamage *= 3f;
            basicHeavyAttackDamage *= 3f;
            rageTimer = 0f;
            mainGameManager.DoHaptics(.4f, 1.1f, 1.1f);
            _styleManager.rageButtonImage.SetActive(false);


            GameObject slamEffect = Instantiate(groundSlamVFX, transform.position, transform.rotation);
            slamEffect.AddComponent<CleanUpScript>();

            Collider[] colls = Physics.OverlapSphere(transform.position, blockStunRadius * 2f );

            foreach (Collider enemy in colls)
            {
                if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //Debug.Log(enemy.name);
                    BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
                    //    AttackTargetScript attackScript = 
                    if (!enemyScript.isDead)
                    {
                        canBlockStun = false;

                        enemyScript.TakeDamage(enemyScript.chainHitScript.blockChainDamage, playerAttackType.BlockStun.ToString());
                        enemyScript.isLaunched = true;
                        enemyScript.isBlockStunned = true;

                        //Feedback
                        GameObject lightningVFX = Instantiate(enemyScript.attackTargetScript.parryLightningVFX, enemy.transform.position - new Vector3(0, 0, 0), transform.rotation);
                        //  lightningVFX.transform.eulerAngles = new Vector3(-90, lightningVFX.transform.eulerAngles.y, lightningVFX.transform.eulerAngles.z);  
                        lightningVFX.AddComponent<CleanUpScript>();
                        enemyScript.attackTargetScript.parriedFeedback?.PlayFeedbacks();
                        mainGameManager.DoHaptics(.2f, .4f, .7f);
                        enemy.transform.LookAt(transform);
                    }
                }
                hasParriedAttack = true;
                canBlockStun = true;

            }
        }

        rageTimer += Time.deltaTime;
        meshR.materials = rageSkinMat;
        _styleManager.currentStyleAmount = _styleManager.levelMaxStyleAmount / (rageTimer / rageDuration);

        if (rageTimer >= rageDuration)
        {
            EndRage();
        }



    }

    void EndRage()
    {
        rageEndFeedback?.PlayFeedbacks();
        rageStartFeedback?.StopFeedbacks();

        print("endRage");
        rageElectricity.SetActive(false);
        meshR.materials = defaultSkinMat;
        _styleManager.currentStyleAmount = 0f;
        _styleManager.styleMeterFUll = false;
        isRaging = false;

        basicLightAttackDamage /= 3f;
        basicHeavyAttackDamage /= 3f;
    }

}