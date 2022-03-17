using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI; 

public class BasicEnemyScript : MonoBehaviour
{

    public List <Rigidbody> ragdollRbs = new List<Rigidbody>(); 

    [Header("BASICS")]
    #region 
    public float currentHealth;
    public float maxHealth; 
    public float walkSpeed; 
    public float runSpeed;
    private float currentMoveSpeed; 
    public float lookRange; 

    public float attackDistance; 
    [HideInInspector] public Transform target; 
    #endregion


    [Header("ATTACKING")]
    #region 
    private float totalComboLength; 
    public float basicAttackDamage; 
    public float currentAttackDamage; 
    public float currentComboLength; 
    
    private float currentAttackForwardForce; 
    public float basicAttackForwardForce; 


    #endregion

    [Header("STUNNED")]
    #region 
    public float minStunnedImpactVelocity; //The minimum speed a launched enemy must have to be damaged by a impact
    public float currentVelocity; 
    public float stunDuration; 
    private float stunTimer; 
    private float revoveryTimer; 
    public float recoveryDuration;
   
 //    public bool isGettingUp; 
    #endregion

 

    [Header("FEEDBACKS")]
    #region
    public MMFeedbacks stunnedFeebacks; 
    public MMFeedbacks startAttackFeedback; 
    #endregion

    [Header("COMPONENTS")]
    #region 
    public Animator enemyAnim;
    [HideInInspector] public Rigidbody enemyRb;
    public Collider mainCollider; 
    public ThirdPerson_PlayerControler playerController; 
    public Transform rootTransform; 
    public Transform bloodSpawnPoint; 

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

  //  public bool canBeStunned;
    private bool canRecover = false;
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
        enemyAgent.speed = walkSpeed; 
        enemyRb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; 
        //canBeTargeted = true; 
        ResetState(); 
        target = playerController.transform; 

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if(rb != enemyRb) ragdollRbs.Add(rb); 
            rb.GetComponent<Collider>().isTrigger = true; 
            rb.isKinematic = true; 
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
                break; 

            case(int)currentState.ENGAGE:
                CheckForPlayer(); 
                CheckForMovement(); 
                CheckForAttack(); 
                MoveTowardsPlayer(); 
                break; 

            case(int)currentState.ATTACKING:
                CheckForPlayer(); 
                HandleAttack(); 
                CheckForPlayer(); 
                break; 

            case(int)currentState.STUNNED:
                CheckForRecovery(); 
                CheckForPlayer(); 
                CheckForImpactDamage(); 
   
                break; 

            case(int)currentState.DEAD:
                EnemyDies(); 
                break;

    
        }
    }

    public void HandleAnimation(){
        enemyAnim.SetBool("IsAttacking", isAttacking);
        enemyAnim.SetBool("CanMove", canMove); 
    }

    public void CheckForPlayer()
    {
        //Check if the enemy is close is close enough to attack
        if(Vector3.Distance(transform.position, target.position) <= attackDistance + .25f) 
        {
            isInAttackRange = true; 
        }
    }

    public void CheckForAttack()
    {
        if(playerLocationIsKnown && isInAttackRange && canAttack)
        {
            enemyState = (int)currentState.ATTACKING; 
        }
    }

    public void CheckForMovement()
    {
        if(playerLocationIsKnown && canFollow) 
        {
            isFollowing = true;
            enemyAgent.speed = runSpeed; 
            enemyState = (int)currentState.ENGAGE; 
        }
        else
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
        canFollow = false; 

        if(!isAttacking)
        { 
            isAttacking = true; 
            SetAttackType(); 
            DoAttack(); 
        }


    }

    void SetAttackType()
    {
        int totalAttackTrees = 1; 

        currentAttackDamage = basicAttackDamage; 
        currentAttackForwardForce = basicAttackForwardForce; 
        
        if(currentComboLength == 0) enemyAnim.SetInteger("AttackType", Random.Range(1, totalAttackTrees)); //Set combo attack tree
        if(enemyAnim.GetInteger("AttackType") == 1) totalComboLength = 3f; //Check length of combo attack tree
    }

    public void DoAttack()
    {
        isAttacking = true; 
        currentComboLength++; 

        enemyAnim.SetTrigger("AttackTrigger"); 
        if(currentComboLength >= totalComboLength) currentComboLength = 0f; 
        
        /*
        void SetAttackType()
    {
        int totalAttackTrees = 5;

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
             if(currentComboLength == 0) playerAnim.SetInteger("LightAttackType", Random.Range(1, totalAttackTrees + 1));  //Decide which combo tree to go into (only once per full combo tree)
         //   if (currentComboLength == 0) playerAnim.SetInteger("LightAttackType", 1); 

            //Set current attack tree length
            if      (playerAnim.GetInteger("LightAttackType") == 1) totalComboLength = 3; 
            else if (playerAnim.GetInteger("LightAttackType") == 2) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 3) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 4) totalComboLength = 4;
            else if (playerAnim.GetInteger("LightAttackType") == 5) totalComboLength = 6; //Done
          //  else if (playerAnim.GetInteger("LightAttackType") == 6) totalComboLength = 5;
        }
        */

        Debug.Log("Attack Player"); 
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
            enemyAnim.SetFloat("DamageReaction", Random.Range(0, 5)); 
            enemyAnim.SetTrigger("DamageTrigger");

            //Stun player
            ResetState();
            CheckForStun(1f); 
        }

        //Kill the enemy
        else
        {       
            isDead = true; 
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
       

        //Check if a stunned and launched enemy has stopped moving 
        if (isLaunched && currentVelocity < 2f)
        {
            revoveryTimer += Time.deltaTime;
            if (revoveryTimer >= recoveryDuration)
            {

                StandBackUp();
                isLaunched = false;
            }
        }
        else
        {
            canRecover = true;
            revoveryTimer = 0f;
        }
    
      
        if (stunTimer >= stunDuration && canRecover)
        {
            enemyAgent.enabled = true; 
            stunTimer = 0f;
            isStunned = false;
            enemyState = (int)currentState.ENGAGE;
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
        //transform.GetComponent<BasicEnemyScript>().enabled = false; 
    }
       
    
   

    void StandBackUp()
    {
        canRecover = true; 
        Debug.Log("Back on your feet soldier!"); 
    }

    public void EnableRagdoll(){

        mainCollider.isTrigger = true; 
        enemyAnim.enabled = false;

        foreach(Rigidbody rb in ragdollRbs)
        {        
            //rb.detectCollisions = true;  
            rb.GetComponent<Collider>().isTrigger = false;    
            rb.useGravity = true; 
            rb.isKinematic = false;         
            //CharacterJoint joint = rb.GetComponent<CharacterJoint>();
           // Destroy(joint);                
        }

        enemyRb.constraints = RigidbodyConstraints.None; 
        enemyRb.useGravity = false; 
        enemyRb.isKinematic = false;
        enemyAgent.enabled = false;

        /*
        foreach(Rigidbody rb in ragdollRbs)
        { 
            rb.AddForce(direction * Random.Range(force - 5, force + 5), ForceMode.VelocityChange);
            rb.AddForce(transform.up * 5f, ForceMode.VelocityChange);  
        }
        */

        isRagdolling = true; 
    }

    public void CutOffLimbs()
    {

    }

    public void DisableRagdoll()
    {
        
        foreach(Rigidbody rb in ragdollRbs)
        {
            rb.useGravity = false; 
            rb.isKinematic = true;   
          //  rb.detectCollisions = false;        
            rb.GetComponent<Collider>().isTrigger = true;    
        }
        
        enemyRb.constraints = RigidbodyConstraints.FreezePositionY; 
        mainCollider.isTrigger = false;  
        enemyAnim.enabled = true; 
        isRagdolling = false; 
        canAddImpactDamage = false; 
        isLaunched = false;
        enemyAgent.enabled = true;
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

        isFollowing = false;
        isStunned = false;
        isDead = false;
        isLaunched = false;
        isRagdolling = false;
        isAttacking = false;
        isInAttackRange = false;
        hasHitObject = false;
        isMeleeAttack = false;
        isProjectileAttack = false;
     //   playerLocationIsKnown = false;
        isFollowing = false;

        enemyAgent.enabled = true; 

}

    public void ResetAnimator()
    {

    }
    
}
