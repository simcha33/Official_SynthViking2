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
    public float animSpeed;
    private float currentMoveSpeed; 
    public float lookRange;


    
    [HideInInspector] public Transform target; 
    #endregion

    [Header("ATTACKING")]
    #region  
    public float attackDistance;
    public float basicMeleeAttackDamage;
    private float totalComboLength;
    [HideInInspector] public float currentAttackDamage; 
    private int currentComboLength;

    public string attackType; 
    private float currentAttackForwardForce; 
    public float basicMeleeAttackForwardForce;
    private float nextAttackTimer;
    private float nextAttackDuration;
    private Vector3 attackStartPos; 
    [HideInInspector] public List<Rigidbody> ragdollRbs = new List<Rigidbody>();

    #endregion

    [Header("STUNNED")]
    #region 
    public float minStunnedImpactVelocity; //The minimum speed a launched enemy must have to be damaged by a impact
    private float currentVelocity; 
    public float stunDuration; 
    private float stunTimer; 
    public float recoveryTimer; 
    public float recoveryDuration;
    public string stunType;
    public float getUpTimer;
    public float getUpDuration;
   
 //    public bool isGettingUp; 
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
    public Animator enemyAnim;
    [HideInInspector] public Rigidbody enemyRb;
    public Collider mainCollider; 
    public Transform rootTransform; 
    public Transform bloodSpawnPoint; //Cube in waist
    public Transform weapon; //Weapon in hand
    private Vector3 weaponPos;
    private Vector3 weaponAngle;
    private FixedJoint mainColJoint;
    public Rigidbody connectedJointRb; //Should be the pelvis

    public NavMeshAgent enemyAgent; 

    
    #endregion

    [Header("STATES")]
    #region 
    public bool isStunned;
    public bool isDead; 
    public bool isLaunched;
    public bool isRagdolling; 
    public bool isAttacking; 
    public bool isInAttackRange; 
    public bool hasHitObject; 
    public bool isMeleeAttack; 
    public bool isProjectileAttack; 
    public bool playerLocationIsKnown;
    public bool isFollowing;
    public bool isGettingUp; 

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
    private int enemyState;
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
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        enemyState = (int)currentState.IDLE;
       // connectedJointRb = mainColJoint.connectedBody;
      


        //Add a bit of randomness to the walking speed
        originalRunSpeed = runSpeed;
        originalWalkSpeed = walkSpeed; 
        walkSpeed = Random.Range(walkSpeed - 1f, walkSpeed + 1f);
        runSpeed = Random.Range(runSpeed - 1f, runSpeed + 1f); 
        enemyAgent.speed = walkSpeed;

        enemyRb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; 
        //canBeTargeted = true; 
        ResetState(); 
        target = playerController.transform;
        weaponAngle = weapon.localEulerAngles; 
        weaponPos = weapon.localPosition;

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb != enemyRb)
            {
                ragdollRbs.Add(rb);
                rb.GetComponent<Collider>().isTrigger = true;
                rb.isKinematic = true;
            }


           
            rb.useGravity = false; 
            //rb.isKinematic = true; 
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
                break; 

            case(int)currentState.ENGAGE:
                CheckForPlayer(); 
                CheckForMovement(); 
                CheckForAttack(); 
                MoveTowardsPlayer();
                HandleAnimation();
                break; 

            case(int)currentState.ATTACKING:
                HandleAttack(); 
                CheckForPlayer();
                HandleAnimation();
                break; 

            case(int)currentState.STUNNED:
                CheckForRecovery(); 
                CheckForPlayer(); 
                CheckForImpactDamage();
                HandleAnimation();

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
    }

    public void CheckForPlayer()
    {
        //Check if the enemy is close is close enough to attack
        if(Vector3.Distance(transform.position, target.position) <= attackDistance + .25f) 
        {
            isInAttackRange = true;
        }
        else
        {
            isInAttackRange = false; 
        }

        playerLocationIsKnown = true; 
    }

    public void CheckForAttack()
    {
        if(playerLocationIsKnown && isInAttackRange)
        {
            enemyState = (int)currentState.ATTACKING; 
        }
    }

    public void AllowAttackDamage()
    {
        attackTargetScript.TargetDamageCheck(); 
       
        //if (attackState != currentAttackType.SprintAttack.ToString()) transform.DOMove(transform.position + transform.forward * currentAttackForwardForce, .4f).SetUpdate(UpdateType.Fixed);
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
        }
        else if(!playerLocationIsKnown || !canFollow)
        {
            enemyState = (int)currentState.IDLE;
            enemyAgent.speed = walkSpeed; 
        }
    }

    public void CheckForStun(float stunLength)
    {
        canRecover = true;
        enemyAgent.enabled = false; 
        enemyState = (int)currentState.STUNNED;
        stunTimer = 0f; 
        stunDuration = stunLength; 
    }


    public void MoveTowardsPlayer()
    {
        if(canMove)
        {
            enemyAgent.destination = target.position - transform.forward * attackDistance; 
        }   
    }

    

    void HandleAttack()
    {
        //Set a new attack
        if(!isAttacking && isInAttackRange)
        {
            ResetState();
            enemyRb.isKinematic = true; 
            transform.LookAt(target, Vector3.up);
            SetAttackType(); 
        }    

        //Reset attack state and return to enage state
        else if(!isAttacking)
        {
            enemyRb.isKinematic = false;
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
        attackType = "BasicMeleeAttackDamage";
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

    public void TakeDamage(float damgeAmount){
        
        currentHealth -= damgeAmount;

        if(currentHealth > 0)
        {

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

            //Stun player
            ResetState();
            CheckForStun(1f); 
        }

        //Kill the enemy
        else
        {       
            isDead = true;
            enemyAgent.enabled = false; 
            EnableRagdoll(); 
            enemyState = (int)currentState.DEAD; 
        }    
    }

    void OnTriggerEnter(Collider other) 
    {
        if(isLaunched)
        {
            if(other.gameObject.layer == playerController.EnvorinmentLayer)
            {
                hasHitObject = true; 
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //Add some backwards force to enemies hit by stunned enemies
        if(other.gameObject.layer == playerController.enemyLayer)
        {
            BasicEnemyScript otherScript = other.gameObject.GetComponent<BasicEnemyScript>();
            if (otherScript.isStunned)
            {
                enemyRb.AddForce(-other.transform.forward * 5f, ForceMode.Impulse);
            }
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(isLaunched)
        {
            if(!canAddImpactDamage && hasHitObject)
            {
                hasHitObject = false; 
            }
        }
    }

    private void CheckForRecovery()
    {
        stunTimer += Time.deltaTime;
        currentVelocity = enemyRb.velocity.magnitude;
        isStunned = true;
       // enemyRb.isKinematic = true;
        
       

        //Check if a stunned and launched enemy has stopped moving 
        if (isLaunched && currentVelocity < 2f)
        {
            recoveryTimer += Time.deltaTime;         
            canRecover = false; 

            if (recoveryTimer >= recoveryDuration)
            {
                StandBackUp();
            }
        }
        else if(isLaunched && currentVelocity >= 2f)
        {
            canRecover = false; 
        }
        else if(!isLaunched)
        {
            canRecover = true;
            recoveryTimer = 0f;
        }
      
        //Recover
        if (stunTimer >= stunDuration && canRecover && !isGettingUp)
        {
            isGettingUp = true;
            getUpTimer = 10f;
            DisableRagdoll();
            StandBackUp();
        }
    }


    public void CheckForImpactDamage()
    {

        //Add damage to enemy on high speed launch collisions
        if(hasHitObject && currentVelocity > minStunnedImpactVelocity && canAddImpactDamage && isLaunched)
        {
            canAddImpactDamage = false; 
            TakeDamage(50f); 
        }
        
    }
 

    public void EnemyDies()
    {
        //canBeTargeted = false; 
        transform.tag = "Dead";
        enemyAgent.enabled = false; 
        //transform.GetComponent<BasicEnemyScript>().enabled = false; 
    }
       
    
   

    void StandBackUp()
    {
        getUpTimer += Time.deltaTime; 

        if (!isGettingUp)
        {
            isGettingUp = true;
            DisableRagdoll();
         //   enemyAnim.applyRootMotion = true;
            enemyAnim.SetFloat("GetUpType", Random.Range(1f, 4f));
            enemyAnim.speed = Random.Range(.6f, 1f); 
            enemyAnim.SetTrigger("GetUpTrigger");      
            enemyRb.velocity = new Vector3(0, 0, 0);
            getUpDuration = 1f;
            

        }
     
        if(getUpTimer >= getUpDuration)
        {
           // enemyAnim.applyRootMotion = false; 
            enemyAnim.speed = 1f; 
            isLaunched = false;
            isGettingUp = false;
            isStunned = false; 
            canRecover = false; 
            recoveryTimer = 0f;
            stunTimer = 0f; 
            getUpTimer = 0f;
            //enemyAgent.enabled = true;
            enemyState = (int)currentState.ENGAGE;
        }

        getUpDuration = enemyAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / enemyAnim.GetCurrentAnimatorStateInfo(0).speed;
    }

    public void EnableRagdoll(){

        mainCollider.isTrigger = true;    
        enemyAnim.enabled = false;
        mainColJoint = gameObject.AddComponent<FixedJoint>();
        mainColJoint.connectedBody = connectedJointRb;

        //Enable ragdoll imbs
        foreach (Rigidbody rb in ragdollRbs)
        {        
            rb.GetComponent<Collider>().isTrigger = false;    
            rb.useGravity = true; 
            rb.isKinematic = false;                       
        }

        enemyRb.constraints = RigidbodyConstraints.None; 
        enemyRb.useGravity = false; 
        enemyRb.isKinematic = false;
        enemyAgent.enabled = false;
        isRagdolling = true; 
    }


    public void DisableRagdoll()
    {

        Destroy(mainColJoint);

     //   mainColJoint.connectedBody = null;
        enemyRb.constraints = RigidbodyConstraints.FreezePositionY;

        canAddImpactDamage = false;

        isRagdolling = false;
        enemyAgent.enabled = true;

        if (!isGettingUp) isLaunched = false;        
     

        enemyAnim.enabled = true;
       // enemyRb.isKinematic = true;
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
        //  canAttack = true; 
        //  canBeTargeted = true; 
        //   canAddImpactDamage = true; 
        //   canBeLaunched = true; 
        //   canFollow = true;

        isGettingUp = false; 
        isFollowing = false;
        isStunned = false;
        isDead = false;
     //   isLaunched = false;
        isRagdolling = false;
        isAttacking = false;
        isInAttackRange = false;
        hasHitObject = false;
        isMeleeAttack = false;
        isProjectileAttack = false;
     //   playerLocationIsKnown = false;
        isFollowing = false;

        enemyAgent.enabled = true;
        enemyAnim.speed = 1f; 

}

    public void ResetAnimator()
    {

    }
    
}
