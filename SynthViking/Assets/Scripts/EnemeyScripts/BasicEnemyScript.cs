using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI; 

public class BasicEnemyScript : MonoBehaviour
{


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
    public float groundCheckHeight = .8f;
    public LayerMask envorinmentLayer; 
    public  BasicEnemyScript thisScript;
    public bool canBeManaged;
    public bool isGrounded;
    [HideInInspector] public Transform target;
    private float originalRbMass;
    private float stunnedMass = 400f; 
   
    #endregion

    [Header("ATTACKING")]
    #region  
    public float attackDistance;
    public float attackMoveSpeed; 
    public float currentRequiredDistance; 
    public float circleDistance; 
    public float basicMeleeAttackDamage;
    private float totalComboLength;
    [HideInInspector] public float currentAttackDamage; 
    private int currentComboLength;

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
   // public float chainhitBackForce;
   // public float chainHitActiveTime = .4f;
   // public float chainHitMaxDistance = 11f; 

      private string StunType;
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
    [HideInInspector] public List<Rigidbody> ragdollRbs = new List<Rigidbody>();
    public List<GameObject> limbInsides = new List<GameObject>();
    #endregion

    [Header("FEEDBACKS")]
    #region
    public MMFeedbacks stunnedFeebacks; 
    public MMFeedbacks startAttackFeedback; 
    #endregion

    [Header("COMPONENTS")]
    #region 
    public ThirdPerson_PlayerControler playerController;
    public AttackTargetScript attackTargetScript;
    public ChainHitScript chainHitScript;
    public ComboManager comboManagerScript; 
    //public SetEnemyInside enemyInsideScript; 
  
  //  public EnemyManager enemyManager; 
    public Animator enemyAnim;
    [HideInInspector] public Rigidbody enemyRb;
    public Collider mainCollider;
    public Collider chainHitCollider;
    public Transform rootTransform; 
    public Transform bloodSpawnPoint; //Cube in waist
    public Transform weapon; //Weapon in hand
 
    public Rigidbody groundCheckPoint;
    private Vector3 weaponPos;
    private Vector3 weaponAngle;
    private FixedJoint mainColJoint;
    public Rigidbody connectedJointRb; //Should be the pelvis
    public NavMeshAgent enemyAgent; 
    public SkinnedMeshRenderer enemyMeshr;
  

    #endregion

    [Header("STATES")]
    #region 
    public bool isStunned;
    public bool isDead; 
    public bool isLaunched;
    public bool isRagdolling; 
    public bool isAttacking; 
    public bool isInAttackRange; 
    public bool isInCircleRange; 
    public bool hasHitObject; 
    public bool isMeleeAttack; 
    public bool isProjectileAttack; 
    public bool playerLocationIsKnown;
    public bool isFollowing;
    public bool isGettingUp;
    public bool isBlockStunned; 

  //  public bool canBeStunned;
    public bool canRecover = false;
    public bool canFollow; 
    public bool canDetectPlayer; 
    public bool canMove; 
    public bool canAttack; 
    public bool canBeTargeted; 
    public bool canAddImpactDamage; 
    public bool canBeLaunched = true; 
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
    }
    #endregion


    void Start()
    {
        //Components
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>();
        comboManagerScript = GameObject.Find("StyleManager").GetComponent<ComboManager>(); 
        thisScript = this.gameObject.GetComponent<BasicEnemyScript>(); 
        weaponMeshr = weapon.GetComponent<MeshRenderer>();
        enemyRb = GetComponent<Rigidbody>();
        //styleScoreText = GetComponentInChildren<TextMesh>(); 

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

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb != enemyRb && rb != groundCheckPoint)
            {
                ragdollRbs.Add(rb);
                rb.GetComponent<Collider>().isTrigger = true;
                rb.isKinematic = true;
                //if(rb.name != "pelvis")rb.gameObject.AddComponent<SetEnemyInside>(); 
            }
       
            rb.useGravity = false; 
            //rb.isKinematic = true; 
        }

        foreach(GameObject inside in limbInsides)
        {
            inside.GetComponent<MeshRenderer>().enabled = false; 
        }

        enemyRb.detectCollisions = true; 
        
    }



    void Update()
    {
        switch(enemyState){
                case(int)currentState.IDLE:
                CheckForPlayer(); 
                CheckForMovement();
                HandleAnimation();
                GroundCheck(); 
                break; 

            case(int)currentState.ENGAGE:
                CheckForPlayer(); 
                CheckForMovement(); 
                CheckForAttack(); 
                MoveTowardsPlayer();
                HandleAnimation();
                GroundCheck();
                break; 

            case(int)currentState.ATTACKING:
                HandleAttack(); 
                CheckForPlayer();
                GroundCheck();
                HandleAnimation();
                AllowAttackDamage(); 
                break; 

            case(int)currentState.STUNNED:
                CheckForRecovery(); 
                CheckForPlayer(); 
                CheckForImpactDamage();
                HandleAnimation();
                if(StunType != "PhysicsImpact") GroundCheck(); 
               // GroundCheck();

                break; 

            case(int)currentState.DEAD:
                EnemyDies(); 
                break;  
        }

     
        animSpeed = enemyAnim.speed; 

        
    }

    public void HandleAnimation(){
        enemyAnim.SetBool("IsAttacking", isAttacking);
        enemyAnim.SetBool("CanMove", canMove);
        enemyAnim.SetBool("IsStunned", isStunned);
        enemyAnim.SetBool("IsGettingUp", isGettingUp);
        enemyAnim.SetBool("IsFollowing", isFollowing); 
       // enemyAnim.SetBool("IsLaunched")
    }

    public void CheckForPlayer()
    {
        targetDistance = Vector3.Distance(transform.position, target.position); 

        //Check if the enemy is close is close enough to attack
        if(targetDistance <= attackDistance + .25f) 
        {
            isInAttackRange = true;
        }
        else
        {
            isInAttackRange = false; 
        }

        //If we are engaged and can't attack check for circle distance
        if(targetDistance <= circleDistance && !canAttack)
        {
            isInCircleRange = true; 
        }
        else
        {
            isInCircleRange = false; 
        }

        playerLocationIsKnown = true; 
    }

    public void CheckForAttack()
    {
        if(playerLocationIsKnown && isInAttackRange)
        {
            currentMoveSpeed = attackMoveSpeed; 
            enemyState = (int)currentState.ATTACKING; 
        }
        else if(playerLocationIsKnown && isInCircleRange && !canAttack){
            //Circle state
        }
    }

    public void CheckForMovement()
    {
        if(playerLocationIsKnown && canFollow && !isFollowing) 
        {
        
            enemyAgent.speed = runSpeed;
            enemyAnim.speed = runSpeed / originalRunSpeed; //Slow down the animation based on the random move speed
            transform.LookAt(target, Vector3.up); 
            isFollowing = true;      
            enemyState = (int)currentState.ENGAGE; 
          //  enemyManager.SetNewEngager(thisScript); 
        }
        else if(!playerLocationIsKnown || !canFollow)
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

        }
        else isGrounded = false;

        Debug.DrawRay(groundCheckPoint.position, Vector3.down * groundCheckHeight, Color.red);

        
        if(!isGrounded && isStunned && !isRagdolling && !isLaunched && !isGettingUp)
        {
            isLaunched = true;
            EnableRagdoll();
            
            CheckForStunType("PhysicsImpact"); 
        }
        


    }

    public void MoveTowardsPlayer()
    {
        if(canMove)
        {
            enemyAgent.destination = target.position - transform.forward * currentRequiredDistance; 
        }   
    }

    
    //Attacking
    void HandleAttack()
    {
        //Set a new attack
        if(!isAttacking && isInAttackRange)
        {
            ResetState();
            //enemyRb.isKinematic = true;
            enemyRb.mass = stunnedMass; 
            transform.LookAt(target, Vector3.up);
            SetAttackType(); 
        }    

        //Reset attack state and return to enage state
        else if(!isAttacking)
        {
            //  enemyRb.isKinematic = false;
            enemyRb.mass = originalRbMass; 
            currentComboLength = 0; 
            enemyState = (int)currentState.ENGAGE; 
        }

        DoAttack();
    }

    void SetAttackType()
    {
        int totalAttackTrees = 1;

        //Reset some attack values 
        canAttack = true;
        currentComboLength++; 

        currentAttackDamage = basicMeleeAttackDamage; 
        currentAttackForwardForce = basicMeleeAttackForwardForce;
        enemyAttackType = "BasicMeleeAttackDamage";
       // BasicMeleeAttackDamageStunTrigger


        if (currentComboLength == 1) enemyAnim.SetInteger("AttackType", Random.Range(1, totalAttackTrees)); //Set combo attack tree
        if(enemyAnim.GetInteger("AttackType") == 1) totalComboLength = 3f; //Check length of combo attack tree 
    }

    public void DoAttack()
    {
        nextAttackDuration = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / enemyAnim.GetCurrentAnimatorStateInfo(0).speed;

        if (canAttack)
        {
            canAttack = false; 
            isAttacking = true;

            enemyRb.velocity = new Vector3(0, 0, 0); 
     
            enemyAnim.SetInteger("CurrentComboLength", currentComboLength);
            enemyAnim.SetTrigger("AttackTrigger");

            nextAttackTimer = 0f;
            nextAttackDuration = 5f; //arbitrary value to not instantly complete the timer
            attackStartPos = transform.position; 

            if (currentComboLength >= totalComboLength) currentComboLength = 0; //Reset combo tree
        }

        if(nextAttackTimer >= nextAttackDuration)
        {
            isAttacking = false; 
        }

        nextAttackTimer += Time.deltaTime; 
    }

    public void AttackEventTrigger()
    {
        canBeParried = true;
        // weapon.gameObject.GetComponent<MeshRenderer>().material = attackIndicationMat;
        //if (attackState != currentAttackType.SprintAttack.ToString()) transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .4f).SetUpdate(UpdateType.Fixed);
    }

    public void AllowAttackDamage()
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





    public void LaunchEnemy(Vector3 direction, float forwardForce, float upForce)
    {

        if(canBeLaunched)
        {
            canAddImpactDamage = true; 
            isLaunched = true; 
            isStunned = true;
            canRecover = false; 
            enemyRb.velocity = new Vector3(0,0,0); 
            EnableRagdoll(); 

            foreach(Rigidbody rb in ragdollRbs)
            { 
                rb.AddForce(direction * forwardForce, ForceMode.VelocityChange);
                rb.AddForce(Vector3.up * upForce, ForceMode.VelocityChange);  
            }
      
            enemyState = (int)currentState.STUNNED; 
        }
    }

    public void TakeDamage(float damageAmount, string DamageType){
        
        currentHealth -= damageAmount;

        if (currentHealth > 0 && !isDead)
        {
            launchDirection = playerController.transform.forward;


            enemyRb.velocity = new Vector3(0, 0, 0);


            if (DamageType == playerAttackType.PowerPunch.ToString()) //Enemy is hit by ground slam
            {
                ResetState();
                canBeChainHit = false;
                //  enemyMeshr.materials = stunnedSkinMat; 
            }

            if (DamageType == playerAttackType.GroundSlam.ToString()) //Enemy is hit by power punch
            {
                ResetState();
                canBeChainHit = true;
                // enemyMeshr.materials = stunnedSkinMat;
            }

            if (DamageType == playerAttackType.BlockStun.ToString()) //Enemy is hit by power punch
            {
                ResetState();
                //  canBeChainHit = true;
                comboManagerScript.AddStyle(DamageType, this.transform);
                canBeChainHit = true;
                enemyAnim.speed = Random.Range(.5f, 1f);
                enemyAnim.SetFloat("DamageReaction", 6f);
                enemyAnim.SetTrigger("DamageTrigger");
                // enemyMeshr.materials = stunnedSkinMat;
            }

            if (DamageType == chainHitScript.chainHitString) //Enemy is hit by power punch
            {
                canBeChainHit = true;
                //   enemyMeshr.materials = stunnedSkinMat;
            }

            if (DamageType == "ImpactDamage")
            {

            }


            if (DamageType == playerAttackType.LightAxeHit.ToString()) //Enemy is hit by axe
            {

                ResetState(); //** this might fuck up something to do with enemies not being able to attack after getting hit**
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

                enemyAnim.SetFloat("DamageReaction", newRandomNumber);
                enemyAnim.SetTrigger("DamageTrigger");
            }


            CheckForStunType(DamageType);
        }

        else if (!isDead)//Kill the enemy
        {
            setStyleMeter(DamageType);
            KillEnemy(DamageType);
        }

       if(DamageType != playerAttackType.BlockStun.ToString())
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
        print("SetStyleMeter"); 
        comboManagerScript.AddStyle(styleType, this.transform); 
    }

    void KillEnemy(string DamageType)
    {
        ResetState();

        foreach (GameObject enemyInside in limbInsides)
        {
           // enemyInside.SetActive(true);
        }

        enemyRb.mass = originalRbMass;
        enemyMeshr.materials = deadSkinMat;
        isDead = true;
        enemyAgent.enabled = false;
        chainHitScript.enabled = true;
        chainHitScript.SetChainHitType(DamageType);
        EnableRagdoll();
        enemyState = (int)currentState.DEAD;
    }


    
    public void CheckForStunType(string damageType)
    {
        stunType = damageType;
        enemyMeshr.materials = stunnedSkinMat;
        enemyAgent.enabled = false;

        if (stunType == playerAttackType.GroundSlam.ToString()) //Enemy is hit by ground slam
        {
            stunDuration = 2f;
            enemyRb.mass = originalRbMass;

        }

        if (stunType == playerAttackType.PowerPunch.ToString()) //Enemy is hit by ground slam
        {
            chainHitScript.enabled = true;
            chainHitScript.isOrigin = true; 
            chainHitScript.SetChainHitType(stunType);
            stunDuration = 2f;
            enemyRb.mass = originalRbMass; 

        }

        if (stunType == playerAttackType.LightAxeHit.ToString()) //Enemy is hit with axe attack
        {
            chainHitScript.enabled = true; //allow this enemy to cause chain hit impacts
            chainHitScript.isOrigin = true; 
            chainHitScript.SetChainHitType(stunType);
            stunDuration = 1f;
            //enemyRb.mass = stunnedMass; 
            enemyMeshr.materials = hitSkinMat;
        }

        if (stunType == playerAttackType.BlockStun.ToString()) //Enemy attack is parried
        {
            chainHitScript.enabled = true;
            chainHitScript.isOrigin = true;
            chainHitScript.SetChainHitType(stunType);
            stunDuration = chainHitScript.currentStunDuration;
            enemyRb.mass = stunnedMass; 
            //enemyRb.isKinematic = true; 
        }

        if (stunType == chainHitScript.chainHitString) //Enemy is hit by an already stunned enemy
        { 
            chainHitScript.enabled = true;
            stunDuration = chainHitScript.currentStunDuration;
            enemyRb.mass = originalRbMass; 
        }
       
        enemyAgent.speed = 0f;
        stunTimer = 0f;
        enemyRb.isKinematic = false;    
        enemyState = (int)currentState.STUNNED;        
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
        else if (isLaunched && currentVelocity >= maxRecoveryVel && !isGettingUp && !isBlockStunned) //If a launched enemy is moving to fast don't allow him to recover 
        {
            recoveryTimer = 0f;
            canRecover = false;
        }

        if(!isLaunched && isGrounded || recoveryTimer >= recoveryDuration || isBlockStunned  && isGrounded) //Recovery is allowed
        {
            canRecover = true;
            recoveryTimer = 0f;
        }

        if (stunTimer >= stunDuration && canRecover && !isDead) //Let enemy recover from stunned state 
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

        
    
        if(getUpTimer >= getUpDuration) //exit stunned satte 
        {
            ResetState();  
            if(isRagdolling) DisableRagdoll(); 
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
            enemyRb.velocity = new Vector3(0,0,0);
            enemyRb.freezeRotation = false;
            enemyState = (int)currentState.ENGAGE;   
        }

        getUpTimer += Time.deltaTime;
        if(!isBlockStunned) getUpDuration = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / enemyAnim.GetCurrentAnimatorStateInfo(0).speed;
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
        if(hasHitObject && currentVelocity > minStunnedImpactVelocity && canAddImpactDamage && isLaunched)
        {
            canAddImpactDamage = false; 
            TakeDamage(50f, "ImpactDamage"); 
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

        canAddImpactDamage = false;
        isRagdolling = false;
        enemyAgent.enabled = true;

        if (!isGettingUp) isLaunched = false;        
     
        enemyAnim.enabled = true;
        enemyRb.useGravity = false;
        mainCollider.isTrigger = false;


        //disable ragdoll limbs
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.GetComponent<Collider>().isTrigger = true;
        }

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
        //  canBeTargeted = true; 
        //   canAddImpactDamage = true; 
        //   canBeLaunched = true; 
        //   canFollow = true;

        //canDoStunImpact = false;


        isGettingUp = false; 
        isFollowing = false;
        isStunned = false;
        //isDead = false;
        isBlockStunned = false; 
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

    }

      public void EnemyDies()
    {
        canBeTargeted = false; 
        transform.tag = "Dead";
        enemyAgent.enabled = false; 
        //transform.GetComponent<BasicEnemyScript>().enabled = false; 
    }
       
    
    
}
