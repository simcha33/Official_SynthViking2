using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
using DG.Tweening;

public class BasicEnemyScript : MonoBehaviour
{

    public OffMeshLink offLink;
    public NavMeshLink navLink;
    public Vector3 targetPos;

    [Header("BASICS")]
    #region 
    public float currentHealth;
    public float maxHealth;
    public float walkSpeed;
    private float originalWalkSpeed;
    private float originalRunSpeed;
    public float runSpeed;
    private float animSpeed;
    private float currentMoveSpeed;
    public float lookRange;
    public float groundCheckHeight = .6f;
    public LayerMask envorinmentLayer;
    public BasicEnemyScript thisScript;
    public bool canBeManaged;
    public bool isGrounded;
    public Transform target;
    public Sequence moveSequence;
    [HideInInspector] public float originalRbMass;
    [HideInInspector] public float stunnedMass = 400f;


    #endregion

    [Header("ATTACKING")]
    #region  
    public float attackDistance;
    public float attackMoveSpeed;
    public float currentRequiredDistance;
    public float circleDistance;
    public float basicMeleeAttackDamage;
    private int totalComboLength;
    [HideInInspector] public float currentAttackDamage;
    private int currentComboLength;


    public List<Animation> clipsToPause = new List<Animation>();
    private Animation pausedAttackClip;

    private bool attackHasBeenPaused;
    private float attackPauseDuration;
    private float attackPauseTimer;

    public string enemyAttackType;
    private float currentAttackForwardForce;
    public float basicMeleeAttackForwardForce;
    private float nextAttackTimer;
    private float nextAttackDuration;
    private Vector3 attackStartPos;
    private MeshRenderer weaponMeshr;

    [HideInInspector] public float targetDistance;

    #endregion

    [Header("STUNNED")]
    #region 
    // public bool canDoStunImpact; 
    public float minStunnedImpactVelocity; //The minimum speed a launched enemy must have to be damaged by a impact
    public float currentVelocity;
    public float stunDuration;
    public float stunTimer;
    public float recoveryTimer;
    public float recoveryDuration;
    public string stunType;
    public float getUpTimer;
    public float getUpDuration;
    public Vector3 launchDirection;

    public float chainHitStunDuration;
    public bool canBeChainHit;
    public bool canBeParried;
    public float canBeParriedTimer;
    public float canBeParriedDuration;
    private string StunType;
    public bool addSlowdownForce;
    [HideInInspector] public bool isDashTargeted;
    private float stopFallTimer;
    public float stopFallDuration = .7f;
    #endregion

    [Header("SOUL")]
    #region
    public SoulObject soulObject; 
    public bool hasSoul;
    public float soulValue = 1f; 
    #endregion



    [Header("VISUALS")]
    #region 
    public Material[] stunnedSkinMat;
    public Material[] defaultSkinMat;
    public Material[] hitSkinMat;
    public Material[] deadSkinMat;
    public Material attackIndicationMat;
    public Material originalWeaponMat;
    public TextMesh styleScoreText;
    public GameObject jumpEffect;
    public GameObject bloodSplash;

    public GameObject stunnedEffect;
    [HideInInspector] public List<Rigidbody> ragdollRbs = new List<Rigidbody>();
    public List<GameObject> limbInsides = new List<GameObject>();

    #endregion


    [Header("FEEDBACKS")]
    #region
    public MMFeedbacks stunnedFeeback;
    public MMFeedbacks startAttackFeedback;
    public MMFeedbacks parryIndictionFeedback;
    #endregion


    [Header("COMPONENTS")]
    #region 
    public ThirdPerson_PlayerControler playerController;
    public AttackTargetScript attackTargetScript;
    public ChainHitScript chainHitScript;
    public ComboManager comboManagerScript;
    //   public EnemyManager enemyBehaviourManagerScript;
    public EnemySpawnManager enemySpawnManagerScript;
    public Animator enemyAnim;
    [HideInInspector] public Rigidbody enemyRb;
    public Collider mainCollider;
    public Collider chainHitCollider;
    public Transform rootTransform;
    public Transform bloodSpawnPoint; //Cube in waist
    public Transform weapon; //Weapon in hand
    public CapsuleCollider dashPointCol;

    public GameObject holoShield;

    public Rigidbody groundCheckPoint;
    private Vector3 weaponPos;
    private Vector3 weaponAngle;
    private FixedJoint mainColJoint;
    public Rigidbody connectedJointRb; //Should be the pelvis
    public NavMeshAgent enemyAgent;
    public SkinnedMeshRenderer enemyMeshr;

    public GameObject lookAt; 
    #endregion


    [Header("STATES")]
    #region 
    public bool isStunned;
    public bool isDead;
    public bool isLaunched;
    public bool isRagdolling;
    public bool isAttacking;
    public bool isLinkJumping;
    public bool isInAttackRange;
    public bool isInCircleRange;
    public bool hasHitObject;
    public bool isMeleeAttack;
    public bool isProjectileAttack;
    public bool playerLocationIsKnown;
    public bool isFollowing;
    public bool isGettingUp;
    public bool isBlockStunned;
    public bool isUpperCutted;
    public bool isStoppingFall;

    public bool canBeStunned;
    public bool canRecover = false;
    public bool canFollow;
    public bool canDetectPlayer;
    public bool canMove;
    public bool canAttack;
    public bool canBeTargeted;
    public bool canAddImpactDamage;
    public bool canBeLaunched = true;
    public bool canStopFall;
    public bool canRotate = true;
    public bool inLaser;

    #endregion

    [Header("STATEMACHINE")]
    #region 
    public int enemyState;
    public enum currentState
    {
        IDLE,
        CHASING,
        ATTACKING,
        ENGAGE,
        STUNNED,
        DEAD,
        CIRCLETARGET,
        LINKJUMPING,
    }
    #endregion


    void Start()
    {
        //Components
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>();
        comboManagerScript = GameObject.Find("StyleManager").GetComponent<ComboManager>();
        //  enemyBehaviourManagerScript = GameObject.Find("EnemyBehaviourManager").GetComponent<EnemyManager>();
        enemySpawnManagerScript = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();
        thisScript = this.gameObject.GetComponent<BasicEnemyScript>();
        weaponMeshr = weapon.GetComponent<MeshRenderer>();
        enemyRb = GetComponent<Rigidbody>();
        int runType = Random.Range(1, 3);
        enemyAnim.SetFloat("RunType", runType);

        //Starting values
        chainHitScript.enabled = false;
        originalRunSpeed = runSpeed;
        originalWalkSpeed = walkSpeed;
        walkSpeed = Random.Range(walkSpeed - 1f, walkSpeed + 1f);
        runSpeed = Random.Range(runSpeed - 1f, runSpeed + 1f);
        enemyAgent.speed = walkSpeed;
        enemyState = (int)currentState.IDLE;

        defaultSkinMat = enemyMeshr.materials;
        originalRbMass = enemyRb.mass;
        currentHealth = maxHealth;
        ResetState();
        target = playerController.transform;
        weaponAngle = weapon.localEulerAngles;
        weaponPos = weapon.localPosition;
        originalWeaponMat = weaponMeshr.material;

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb != enemyRb && rb != groundCheckPoint)
            {
                ragdollRbs.Add(rb);
                rb.GetComponent<Collider>().isTrigger = true;
                rb.isKinematic = true;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //if(rb.name != "pelvis")rb.gameObject.AddComponent<SetEnemyInside>(); 
            }

            rb.useGravity = false;
            //rb.isKinematic = true; 
        }

        foreach (GameObject inside in limbInsides)
        {
            inside.GetComponent<MeshRenderer>().enabled = false;
        }

        enemyRb.detectCollisions = true;
        enemySpawnManagerScript.spawnedAliveEnemies.Add(thisScript.gameObject);
        transform.parent = enemySpawnManagerScript.aliveEnemyParent.transform;
        //   enemyBehaviourManagerScript.currentenemiesInScene.Add(thisScript);
        // transform.parent = enemyBehaviourManagerScript.aliveEnemyParent.transform;         
    }



    void Update()
    {
        // Debug.Log(enemyAgent.currentOffMeshLinkData.activated); 


        switch (enemyState)
        {
            case (int)currentState.IDLE:
                CheckForPlayer();
                CheckForMovement();
                HandleAnimation();
                GroundCheck();
                CheckForLinkJumping();
                CheckForAttack();
                break;

            case (int)currentState.ENGAGE:
                CheckForPlayer();
                CheckForMovement();
                CheckForAttack();
                MoveTowardsTarget();
                HandleAnimation();
                GroundCheck();
                CheckForLinkJumping();
                break;

            case (int)currentState.ATTACKING:
                CheckForPlayer();
                GroundCheck();
                HandleAttack();
                HandleAnimation();
                AllowAttackDamage();
                break;

            case (int)currentState.STUNNED:
                CheckForRecovery();
                CheckForPlayer();
                CheckForImpactDamage();
                HandleAnimation();
                GroundCheck();
                AddSlowDownForce();
                StopFall();
                // GroundCheck();

                break;

            case (int)currentState.DEAD:
                EnemyDies();
                break;

            case (int)currentState.LINKJUMPING:
                CheckForLinkJumping();
                // HandleAnimation(); 
                break;
        }


        animSpeed = enemyAnim.speed;


    }

    public void HandleAnimation()
    {
        enemyAnim.SetBool("IsAttacking", isAttacking);
        enemyAnim.SetBool("CanMove", canMove);
        enemyAnim.SetBool("IsStunned", isStunned);
        enemyAnim.SetBool("IsGettingUp", isGettingUp);
        enemyAnim.SetBool("IsFollowing", isFollowing);
        enemyAnim.SetBool("IsLinkJumping", isLinkJumping);
        // enemyAnim.SetBool("IsLaunched")
    }

    public void CheckForLinkJumping()
    {
        float jumpSpeedMulitplier = 3f;

        if (enemyAgent.currentOffMeshLinkData.activated && !isLinkJumping)
        {
            isLinkJumping = true;
            //enemyAgent.currentOffMeshLinkData.offMeshLink.




            HandleAnimation();
            targetPos = enemyAgent.currentOffMeshLinkData.endPos;
            enemyAgent.speed *= jumpSpeedMulitplier;
            enemyAnim.SetTrigger("LinkJumpStartTrigger");
            GameObject effect = Instantiate(jumpEffect, transform.position + new Vector3(0f, .3f, 0f), transform.rotation);
            effect.transform.eulerAngles = new Vector3(-90, effect.transform.eulerAngles.y, effect.transform.eulerAngles.z);
            effect.AddComponent<CleanUpScript>().SetCleanUp(3f);

            enemyState = (int)currentState.LINKJUMPING;
        }
        else if (isLinkJumping && !enemyAgent.currentOffMeshLinkData.activated)
        {
            isLinkJumping = false;
            enemyAgent.speed /= jumpSpeedMulitplier;
            enemyState = (int)currentState.ENGAGE;
        }
    }


    public void CheckForPlayer()
    {
        targetDistance = Vector3.Distance(transform.position, target.position);

        //Check if the enemy is close is close enough to attack
        if (targetDistance <= attackDistance + .25f) isInAttackRange = true;
        else isInAttackRange = false;


        /*
        //If we are engaged and can't attack check for circle distance
        if(targetDistance <= circleDistance && !canAttack)
        {
            isInCircleRange = true; 
        }
        else
        {
            isInCircleRange = false; 
        }
        */

        playerLocationIsKnown = true;
    } /*[MOVE]*/

    public void CheckForAttack()
    {
        if (playerLocationIsKnown && isInAttackRange)
        {
            currentMoveSpeed = attackMoveSpeed;
            enemyState = (int)currentState.ATTACKING;
        }
        else if (playerLocationIsKnown && isInCircleRange && !canAttack)
        {
            //Circle state
        }
    }

    public void CheckForMovement() /*[MOVE]*/
    {
        if (playerLocationIsKnown && canFollow && !isFollowing)
        {

            enemyAgent.speed = runSpeed;
            enemyAnim.speed = runSpeed / originalRunSpeed; //Slow down the animation based on the random move speed
                                                           //  transform.LookAt(target, Vector3.up); 
            isFollowing = true;
            enemyState = (int)currentState.ENGAGE;
            //  enemyManager.SetNewEngager(thisScript); 
        }
        else if (!playerLocationIsKnown || !canFollow)
        {
            //enemyManager.attackingEnemies.Remove(this.gameObject); 
            enemyState = (int)currentState.IDLE;
            enemyAgent.speed = walkSpeed;
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(groundCheckPoint.position, Vector3.down);

        //Check for ground 
        if (Physics.Raycast(ray, out hit, groundCheckHeight, envorinmentLayer))
        {
            isGrounded = true;
            //  dashPointCol.enabled = false;  

        }
        else
        {
            // dashPointCol.enabled = true; 

            isGrounded = false;
        }

        Debug.DrawRay(groundCheckPoint.position, Vector3.down * groundCheckHeight, Color.red);


        if (!isGrounded && isStunned && !isRagdolling && !isLaunched && !isGettingUp && !isUpperCutted && !isStoppingFall)
        {
            isLaunched = true;
            EnableRagdoll();
            CheckForStunType("PhysicsImpact");
        }
        else if (isStoppingFall && isRagdolling)
        {
            DisableRagdoll();
        }
    }

    public void MoveTowardsTarget()
    {
        if (canMove)
        {
            enemyAgent.destination = target.position - transform.forward * currentRequiredDistance;
        }
    }


    //Attacking
    void HandleAttack()
    {
        //Set a new attack
        if (!isAttacking && isInAttackRange)
        {
            ResetState();
            ResetAnimator();
            enemyRb.mass = stunnedMass;
            transform.LookAt(target, Vector3.up);
            nextAttackTimer = 0;
            nextAttackDuration = 40f; 
       
            //  nextAttackDuration = 10f; 

            SetAttackType();
        }

        //Reset attack state and return to enage state
        else if (!isAttacking)
        {
            enemyRb.mass = originalRbMass;
            currentComboLength = 0;
            enemyState = (int)currentState.ENGAGE;
        }

        DoAttack();



    }

    void SetAttackType()
    {
        int totalAttackTrees = 1;

        canAttack = true;

        //Choose next attack in the combo 
        if (gameObject.CompareTag("BasicEnemy"))
        {
            currentComboLength = 0;
            //  if (currentComboLength >= totalComboLength) currentComboLength = 0;
            //currentComboLength++;
            // if (currentComboLength == 1) enemyAnim.SetInteger("AttackType", 1); //Set combo attack tree   
            enemyAnim.SetInteger("AttackType", 1); //Set combo attack tree   
                                                   // enemyAnim.SetInteger("AttackType", 1);

            if (enemyAnim.GetInteger("AttackType") == 1) totalComboLength = 3; //Check length of combo attack tree 
                                                                               // if(currentComboLength > totalComboLength) currentComboLength = 0;

        }
        else if (gameObject.CompareTag("BigEnemy"))
        {
            enemyAnim.SetInteger("AttackType", 1);
            if (enemyAnim.GetInteger("AttackType") == 1) totalComboLength = 6; //Check length of combo attack tree 
            currentComboLength = Random.Range(1, totalComboLength + 1);
        }

        currentAttackDamage = basicMeleeAttackDamage;
        currentAttackForwardForce = basicMeleeAttackForwardForce;
        enemyAttackType = "BasicMeleeAttackDamage";
    }

    public void DoAttack()
    {

        if (isAttacking && nextAttackTimer > .3f) nextAttackDuration = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / enemyAnim.GetCurrentAnimatorStateInfo(0).speed * animSpeed;

        if (canAttack)
        {
            canAttack = false;
            isAttacking = true;
            canRotate = true;
            enemyRb.velocity = new Vector3(0, 0, 0);
            currentComboLength++;

            if (currentComboLength > totalComboLength) currentComboLength = 1;
            if (gameObject.CompareTag("BasicEnemy") && currentComboLength > 3 || currentComboLength  <= 0) print("goodbye"); 
            enemyAnim.SetInteger("CurrentComboLength", currentComboLength);
            enemyAnim.SetTrigger("AttackTrigger");

            nextAttackTimer = 0f;
            nextAttackDuration = 5f; //arbitrary value to not instantly complete the timer
            attackStartPos = transform.position;
            if (currentComboLength >= totalComboLength) currentComboLength = 0; //Reset combo tree

        }

        if (nextAttackTimer >= nextAttackDuration - .2f && isInAttackRange)
        {
            canAttack = true;
        }



        if (nextAttackTimer >= nextAttackDuration && !isInAttackRange)
        {
            isAttacking = false;
        }
        else if (canRotate)
        {
            float singleStep = 23f * Time.deltaTime;
            Vector3 targetDir = playerController.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        //If the attack is pause wait for the pause to end 
        nextAttackTimer += Time.deltaTime;

    


    }

    public void AttackEventTrigger() /*[MOVE]*/
    {
        canBeParried = true;
        parryIndictionFeedback?.PlayFeedbacks();
        if (gameObject.CompareTag("BigEnemy")) canRotate = false;

    }

    public void AllowAttackDamage() /*[MOVE]*/
    {
        if (canBeParried)
        {
            canBeParriedTimer += Time.deltaTime;
            weapon.gameObject.GetComponent<MeshRenderer>().material = attackIndicationMat;

            if (canBeParriedTimer > canBeParriedDuration)
            {
                canBeParriedTimer = 0f;
                canBeParried = false;
                weaponMeshr.material = originalWeaponMat;
                attackTargetScript.TargetDamageCheck();
            }
        }
    }



    public void PauseAttack()
    {
        /*
        enemyAnim.speed = 0; 
        attackPauseTimer = 0; 
        attackPauseDuration = Random.Range(2f,6f); 
        attackHasBeenPaused = true; 

        
        foreach(Animation clip in clipsToPause)
        {
            if(clip.name == enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name) clip[enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name].speed = 0;          
        }
        //AnimationClip clip = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip; 
      //  clip.


        attackPauseTimer = 0; 
       attackPauseDuration = 3f; 
      //  enemyAnim.StopPlayback(); 
      // enemyAnim.speed = 0; 
       //enemyAnim.enabled = false; 
       enemyAnim.speed = 0; 
       attackHasBeenPaused = true;

       */




    }





    public void LaunchEnemy(Vector3 direction, float forwardForce, float upForce)
    {

        if (canBeLaunched)
        {
            dashPointCol.enabled = true;
            canAddImpactDamage = true;
            isLaunched = true;
            isStunned = true;
            canRecover = false;
            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
            }

            EnableRagdoll();

            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.AddForce(direction * forwardForce, ForceMode.VelocityChange);
                rb.AddForce(Vector3.up * upForce, ForceMode.VelocityChange);
            }

            enemyState = (int)currentState.STUNNED;
        }
    }

    public void TakeDamage(float damageAmount, string DamageType)
    {

        currentHealth -= damageAmount;
        if (canBeStunned) ResetAnimator();

        if (currentHealth > 0 && !isDead)
        {
            launchDirection = playerController.transform.forward;
            enemyRb.velocity = new Vector3(0, 0, 0);
            //currentComboLength = 0; 


            if (DamageType == playerAttackType.PowerPunch.ToString() && canBeStunned) //Enemy is hit by a ch
            {
                ResetState();
                canBeChainHit = false;
            }

            if (DamageType == playerAttackType.GroundSlam.ToString() && canBeStunned) //Enemy is hit by power punch
            {
                ResetState();
                canBeChainHit = true;
            }

            if (DamageType == playerAttackType.SprintAttack.ToString() && canBeStunned) //Enemy is hit by sprint attack
            {
                ResetState();
                EnableRagdoll();
                // isLaunched = true; 
                canBeChainHit = true;
            }

            if (DamageType == playerAttackType.BlockStun.ToString()) //Enemy is hit by power punch
            {
                ResetState();
                comboManagerScript.AddStyle(DamageType, this.transform);
                canBeChainHit = true;
                enemyAnim.speed = Random.Range(.5f, 1f);
                enemyAnim.SetFloat("DamageReaction", 6f);
                enemyAnim.SetTrigger("DamageTrigger");
            }

            if (DamageType == playerAttackType.AirLaunchAttack.ToString() && !isRagdolling && canBeStunned) //Enemy is hit by axe
            {
                ResetState();
                isUpperCutted = true;
                enemyRb.mass = originalRbMass;
                enemyRb.constraints = RigidbodyConstraints.None;
                enemyRb.freezeRotation = true;

                //No double hit animations
                float originalReaction = enemyAnim.GetFloat("DamageReaction");
                float newRandomNumber = Random.Range(0, 6f);


                if (newRandomNumber == originalReaction)
                {
                    if (newRandomNumber - 1 < 0) newRandomNumber += 1f;
                    else newRandomNumber -= 1f;

                }

                transform.LookAt(playerController.transform);
                enemyAnim.SetFloat("DamageReaction", newRandomNumber);
                enemyAnim.SetTrigger("DamageTrigger");
            }

            if (DamageType == playerAttackType.HeavyAxeHit.ToString() && !isRagdolling && canBeStunned) //Enemy is hit by axe
            {

                ResetState();
                if (damageAmount >= playerController.basicLightAttackDamage)
                {
                    enemyRb.mass = stunnedMass;
                    canBeChainHit = false;
                }
                else
                {
                    canBeChainHit = true;
                    enemyRb.mass = originalRbMass;
                }

                enemyRb.freezeRotation = true;
                //enemyMeshr.materials = hitSkinMat;

                //No double hit animations
                float originalReaction = enemyAnim.GetFloat("DamageReaction");
                float newRandomNumber = Random.Range(0, 6f);


                if (newRandomNumber == originalReaction)
                {
                    if (newRandomNumber - 1 < 0) newRandomNumber += 1f;
                    else newRandomNumber -= 1f;

                }

                transform.LookAt(playerController.transform);
                enemyAnim.SetFloat("DamageReaction", newRandomNumber);
                enemyAnim.SetTrigger("DamageTrigger");
            }

            if (DamageType == playerAttackType.LightPunchHit.ToString() && !isRagdolling && canBeStunned) //Enemy is hit by axe
            {


                ResetState();
                if (damageAmount >= playerController.basicLightAttackDamage)
                {
                    enemyRb.mass = stunnedMass;
                    canBeChainHit = false;
                }
                else
                {
                    canBeChainHit = true;
                    enemyRb.mass = originalRbMass;
                }

                enemyRb.freezeRotation = true;
                //enemyMeshr.materials = hitSkinMat;

                //No double hit animations
                float originalReaction = enemyAnim.GetFloat("DamageReaction");
                float newRandomNumber = Random.Range(0, 6f);


                if (newRandomNumber == originalReaction)
                {
                    if (newRandomNumber - 1 < 0) newRandomNumber += 1f;
                    else newRandomNumber -= 1f;

                }

                transform.LookAt(playerController.transform);
                enemyAnim.SetFloat("DamageReaction", newRandomNumber);
                enemyAnim.SetTrigger("DamageTrigger");
            }

            if (DamageType == chainHitScript.chainHitString) //Enemy is hit by power punch
            {
                canBeChainHit = true;
            }

            if (DamageType == "ImpactDamage")
            {

            }

            if (DamageType == "SawTrap")
            {

            }

            CheckForStunType(DamageType);

        }

        else if (!isDead)//Kill the enemy
        {
            setStyleMeter(DamageType);
            KillEnemy(DamageType);
        }

        if (DamageType != playerAttackType.BlockStun.ToString())
        {
            SetComboMeter();
        }

    }

    public void SetComboMeter()
    {
        comboManagerScript.AddCombo();
    }

    public void setStyleMeter(string styleType)
    {
        //  comboManagerScript.AddStyle(styleType, this.transform); 
    }

    void KillEnemy(string DamageType)
    {
        ResetState();

        foreach (GameObject enemyInside in limbInsides)
        {
            // enemyInside.SetActive(true);
        }

        //Tell the enemymanager that the enemy is dead 
        if (enemySpawnManagerScript.spawnedAliveEnemies.Contains(thisScript.gameObject)) enemySpawnManagerScript.spawnedAliveEnemies.Remove(thisScript.gameObject);
        enemySpawnManagerScript.spawnedDeadEnemies.Add(thisScript);
        enemySpawnManagerScript.enemiesLeft--;
        if (DamageType != LayerMask.NameToLayer("Environment").ToString()) transform.parent = enemySpawnManagerScript.deadEnemyParent.transform;
        //enemySpawnManagerScript.enemyCount--; 

        //Soul sucking   
        soulObject.gameObject.SetActive(true); 
        soulObject.soulIsFree = true;

        
        //Enter death state 
        dashPointCol.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        enemyRb.mass = originalRbMass;
        enemyMeshr.materials = deadSkinMat;
        holoShield.SetActive(false);
        isDead = true;
        //canBeTargeted = false; 
        enemyAgent.enabled = false;
        chainHitScript.enabled = true;
        chainHitScript.SetChainHitType(DamageType);
        EnableRagdoll();
        stunnedEffect.SetActive(false);
        enemyState = (int)currentState.DEAD;
        playerController.attackTargetScript.limbCheckerScript.CheckForBloodDrip();
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
    }



    public void CheckForStunType(string damageType)
    {

        stunType = damageType;

        if (canBeStunned)
        {
            stunnedEffect.SetActive(true);
            enemyAgent.enabled = false;
            enemyMeshr.materials = stunnedSkinMat;
        }

        //stunnedFeeback?.PlayFeedbacks();


        if (stunType == playerAttackType.GroundSlam.ToString() && canBeStunned) //Enemy is hit by ground slam
        {
            stunDuration = 2f;
            enemyRb.mass = originalRbMass;

        }

        if (stunType == playerAttackType.PowerPunch.ToString() && canBeStunned) //Enemy is hit by ground slam
        {
            chainHitScript.isOrigin = true;
            chainHitScript.enabled = true;
            chainHitScript.SetChainHitType(stunType);
            stunDuration = 2f;
            enemyRb.mass = originalRbMass;

        }

        if (stunType == playerAttackType.HeavyAxeHit.ToString() && !isRagdolling && canBeStunned) //Enemy is hit with axe attack
        {
            stunnedEffect.SetActive(false);
            stunDuration = 1f;
            mainCollider.isTrigger = false;
            enemyMeshr.materials = hitSkinMat;
            if (!isUpperCutted) isLaunched = false;
        }

        if (stunType == playerAttackType.LightPunchHit.ToString() && !isRagdolling && canBeStunned) //Enemy is hit with axe attack
        {
            chainHitScript.enabled = true; //allow this enemy to cause chain hit impacts
                                               
            stunnedEffect.SetActive(false);
            chainHitScript.isOrigin = true;
            canBeChainHit = false;
            chainHitScript.SetChainHitType(stunType);
            stunDuration = .7f;
            mainCollider.isTrigger = false;
            //enemyRb.mass = stunnedMass; 
            enemyMeshr.materials = hitSkinMat;
            if (!isUpperCutted) isLaunched = false;

        }


        if (stunType == playerAttackType.SprintAttack.ToString() && canBeStunned)
        {
            chainHitScript.isOrigin = true;
            chainHitScript.enabled = true;
            chainHitScript.SetChainHitType(stunType);
            stunDuration = 1f;
            enemyRb.mass = originalRbMass;
            canBeChainHit = true;
            //LaunchEnemy(playerController.attackDirection, playerController.dashAttackForce, 2f);            
        }

        if (stunType == playerAttackType.BlockStun.ToString()) //Enemy attack is parried
        {
            chainHitScript.enabled = true;
            chainHitScript.isOrigin = true;
            chainHitScript.SetChainHitType(stunType);
            stunDuration = chainHitScript.currentStunDuration;
            enemyRb.mass = stunnedMass;
            enemyMeshr.materials = stunnedSkinMat;
            stunnedEffect.SetActive(true);
            //enemyRb.isKinematic = true; 
        }


        if (canBeStunned || stunType == playerAttackType.BlockStun.ToString())
        {
            enemyAgent.speed = 0f;
            stunTimer = 0f;
            enemyRb.isKinematic = false;
            enemyState = (int)currentState.STUNNED;
        }
    }

    private void CheckForRecovery()
    {
        stunTimer += Time.deltaTime;
        currentVelocity = enemyRb.velocity.magnitude;
        isStunned = true;
        float maxRecoveryVel = 3f;
        //  enemyRb.isKinematic = false;

        if (isLaunched && currentVelocity < maxRecoveryVel && !isBlockStunned) //Check if a launched enemy has slowed down enough to be allowed to start recovery
        {
            recoveryTimer += Time.deltaTime;
        }
        else if (isLaunched && currentVelocity >= maxRecoveryVel && !isGettingUp && !isBlockStunned || isDashTargeted || isStoppingFall) //If a launched enemy is moving to fast don't allow him to recover 
        {
            recoveryTimer = 0f;
            canRecover = false;
        }

        if (!isLaunched && isGrounded || recoveryTimer >= recoveryDuration || isBlockStunned && isGrounded) //Recovery is allowed
        {
            canRecover = true;
            recoveryTimer = 0f;
        }

        if (stunTimer >= stunDuration && canRecover && !isDead) //Let enemy recover from stunned state s
        {
            StandBackUp();
        }
    }


    void StandBackUp()
    {
        if (isBlockStunned)
        {
            getUpTimer = getUpDuration;
        }

        if (isLaunched) //Trigger logic for getting up once if the enemy has to get up first 
        {
            isGettingUp = true;
            isLaunched = false;
            enemyRb.isKinematic = true;
            DisableRagdoll();


            enemyRb.velocity = new Vector3(0, 0, 0);


            enemyAnim.SetFloat("GetUpType", Random.Range(1f, 4f));
            enemyAnim.SetTrigger("GetUpTrigger");
            transform.LookAt(playerController.transform);
            enemyAnim.speed = Random.Range(.6f, 1f);

            getUpDuration = 10f;
            getUpTimer = 0f;
        }
        else if (!isGettingUp) //if not exit the stunned state without a standing up cooldown 
        {
            getUpTimer = getUpDuration;
        }



        if (getUpTimer >= getUpDuration) //exit stunned satte 
        {

            ResetState();
            stunnedEffect.SetActive(false);
            if (isRagdolling) DisableRagdoll();
            enemyAgent.speed = currentMoveSpeed;
            // enemyAgent.enabled = true; 
            enemyRb.isKinematic = false;
            isBlockStunned = false;
            canRecover = false;
            canBeChainHit = true; //probally set some kind of enemy type check to this
            recoveryTimer = 0f;
            stunTimer = 0f;
            getUpTimer = 0f;
            enemyRb.mass = originalRbMass;
            enemyRb.velocity = new Vector3(0, 0, 0);
            enemyRb.freezeRotation = false;
            enemyState = (int)currentState.ENGAGE;
            mainCollider.isTrigger = false;
        }

        getUpTimer += Time.deltaTime;
        if (!isBlockStunned) getUpDuration = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / enemyAnim.GetCurrentAnimatorStateInfo(0).speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLaunched)
        {
            if (other.gameObject.layer == playerController.EnvorinmentLayer)
            {
                hasHitObject = true;
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (isLaunched)
        {
            if (!canAddImpactDamage && hasHitObject)
            {
                hasHitObject = false;
            }
        }
    }


    public void CheckForImpactDamage()
    {

        //Add damage to enemy on high speed launch collisions
        if (hasHitObject && currentVelocity > minStunnedImpactVelocity && canAddImpactDamage && isLaunched)
        {
            canAddImpactDamage = false;
            GameObject bloodvfx = Instantiate(bloodSplash, transform.position, transform.rotation);
            bloodvfx.AddComponent<CleanUpScript>().SetCleanUp(10f);
            TakeDamage(50f, "ImpactDamage");
        }
    }

    void AddSlowDownForce()
    {
        if (addSlowdownForce)
        {
            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.velocity *= (Time.deltaTime * .8f);
                if (rb.velocity.magnitude < 5) addSlowdownForce = false;
            }
        }
    }


    public void EnableRagdoll()
    {
        if (!isRagdolling)
        {
            mainColJoint = gameObject.AddComponent<FixedJoint>(); //Add joint to main collider so it doesn't leave the body 
            mainColJoint.connectedBody = connectedJointRb;
        }

        enemyAnim.enabled = false;
        mainCollider.isTrigger = true;

        //Enable ragdoll imbs
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.GetComponent<Collider>().isTrigger = false;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        /*
        if (weapon != null)
        {
            Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;
            weaponRb.useGravity = true; 
        }
        */

        //Turn of normal physics    
        enemyRb.constraints = RigidbodyConstraints.None;
        enemyRb.useGravity = true;
        enemyRb.isKinematic = false;
        enemyAgent.enabled = false;
        isRagdolling = true;

    }


    public void DisableRagdoll()
    {

        Destroy(mainColJoint);
        enemyRb.constraints = RigidbodyConstraints.FreezePositionY;
        enemyRb.constraints = RigidbodyConstraints.FreezeRotation;

        canAddImpactDamage = false;
        isRagdolling = false;
        if (!isStoppingFall) enemyAgent.enabled = true;

        if (!isGettingUp) isLaunched = false;

        enemyAnim.enabled = true;
        enemyRb.useGravity = false;
        mainCollider.isTrigger = false;
        // dashPointCol.enabled = false; 


        //disable ragdoll limbs
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.GetComponent<Collider>().isTrigger = true;
        }

        /*
        if (weapon != null)
        {
            Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;
            weaponRb.useGravity = true;
        }
        */

        weapon.localPosition = weaponPos;
        weapon.localEulerAngles = weaponAngle;
    }


    public void ResetState()
    {
        // canBeStunned = true;
        //   canRecover = true; 
        //    canDetectPlayer = true; 
        // canMove = true;  
        canAttack = true;
        canBeParried = false;
        canMove = true;
        //  canBeTargeted = true; 
        //   canAddImpactDamage = true; 
        //   canBeLaunched = true; 
        //   canFollow = true;

        //canDoStunImpact = false;
        //isUpperCutted = false; 
        isGettingUp = false;
        isFollowing = false;
        isStunned = false;
        //isDead = false;
        isBlockStunned = false;
        isLinkJumping = false;

        //   isLaunched = false;
        // isRagdolling = false;
        isAttacking = false;
        isInAttackRange = false;
        hasHitObject = false;
        isMeleeAttack = false;
        isProjectileAttack = false;
        //   playerLocationIsKnown = false;
        isFollowing = false;

        enemyAgent.enabled = true;
        enemyAnim.enabled = true;
        weapon.gameObject.GetComponent<MeshRenderer>().material = originalWeaponMat;
        enemyAnim.speed = 1f;
        canBeParriedTimer = 0f;
        enemyMeshr.materials = defaultSkinMat;
        transform.eulerAngles = new Vector3(0, 0, 0);

    }

    public void ResetAnimator()
    {
        foreach (AnimatorControllerParameter parameter in enemyAnim.parameters)
        {
            enemyAnim.SetBool(parameter.name, false);
            enemyAnim.ResetTrigger(parameter.name);
            //  enemyAnim.SetInteger(parameter.name, 0);

        }
    }

    public void EnemyDies()
    {
        canBeTargeted = false;
        transform.tag = "Dead";
        enemyAgent.enabled = false;
        //transform.GetComponent<BasicEnemyScript>().enabled = false; 
    }

    public void DestroySelf()
    {
        //if(enemySpawnManagerScript.spawnedDeadEnemies.Contains(thisScript)) enemySpawnManagerScript.spawnedDeadEnemies.Remove(thisScript);
        // if (enemySpawnManagerScript.spawnedAliveEnemies.Contains(thisScript)) enemySpawnManagerScript.spawnedAliveEnemies.Remove(thisScript); 
        if (playerController.attackTargetScript.targetsInRange.Contains(this.gameObject)) playerController.attackTargetScript.targetsInRange.Remove(this.gameObject);
        if (playerController.hitPauseScript.objectsToPause.Contains(enemyAnim)) playerController.hitPauseScript.objectsToPause.Remove(enemyAnim);
        if (enemySpawnManagerScript.spawnedDeadEnemies.Contains(thisScript)) enemySpawnManagerScript.spawnedDeadEnemies.Remove(thisScript);

        Destroy(weapon.gameObject);
        Destroy(this.gameObject);
        foreach (GameObject limb in limbInsides)
        {
            Destroy(limb);
        }
    }

    void StopFall()
    {

        if (canStopFall)
        {
            if (!isStoppingFall)
            {
                isStoppingFall = true;
                enemyRb.velocity = new Vector3(0, 0, 0);
                enemyRb.isKinematic = true;
                foreach (Rigidbody rb in ragdollRbs)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.isKinematic = true;

                }
            }

            stopFallTimer += Time.deltaTime;

            if (stopFallTimer >= stopFallDuration)
            {
                canStopFall = false;
                stopFallTimer = 0f;
            }
        }

        else if (isStoppingFall)
        {
            isStoppingFall = false;
            enemyRb.isKinematic = false;
            foreach (Rigidbody rb in ragdollRbs)
            {
                EnableRagdoll();
                rb.isKinematic = false;
            }

        }

    }
}
