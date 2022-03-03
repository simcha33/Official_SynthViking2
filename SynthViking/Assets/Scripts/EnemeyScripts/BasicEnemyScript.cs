using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class BasicEnemyScript : MonoBehaviour
{

    public List <Rigidbody> ragdollRbs = new List<Rigidbody>(); 

    [Header("BASICS")]
    public float currentHealth;
    public float maxHealth; 
    public float moveSpeed;


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

    [Header("STATEMACHINE")]
    #region 
    private int enemyState;
    public enum currentState
    {
        IDLE,
        CHASING,
        ATTACKING,
        STUNNED,
        DEAD, 
    }
    #endregion

    [Header("FEEDBACKS")]
    #region
    public MMFeedbacks stunnedFeebacks; 
    #endregion

      [Header("COMPONENTS")]
    public Animator enemyAnim;
    [HideInInspector] public Rigidbody enemyRb;
    public Collider mainCollider; 
    public ThirdPerson_PlayerControler playerController; 
    public Transform rootTransform; 

    [Header("STATES")]
    public bool isStunned;
    public bool canBeStunned;
    public bool isDead; 

    public bool canBeTargeted; 
    public bool canAddImpactDamage; 
    public bool hasHitObject; 
    public bool canBeLaunched = true; 
  
    public bool isLaunched;
    public bool isRagdolling; 


    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        enemyRb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; 
        canBeTargeted = true; 

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
            
            break; 

            case(int)currentState.STUNNED:
            CheckForImpactDamage(); 
            CheckForRecovery(); 
            break; 

            case(int)currentState.DEAD:
            EnemyDies(); 
            break;
        }
    }
    


    public void LaunchEnemy(Vector3 direction, float force)
    {
        if(canBeLaunched)
        {
            canAddImpactDamage = true; 
            isLaunched = true; 
            isStunned = true; 

            transform.position = playerController.transform.position + playerController.transform.forward + transform.up; 
            EnableRagdoll(); 

            foreach(Rigidbody rb in ragdollRbs)
            { 
                rb.AddForce(direction * Random.Range(force - 5, force + 5), ForceMode.VelocityChange);
                rb.AddForce(transform.up * 5f, ForceMode.VelocityChange);  
            }
      
            enemyState = (int)currentState.STUNNED; 
        }
    }

    public void TakeDamage(float damgeAmount){
        
        currentHealth -= damgeAmount;

        if(currentHealth > 0)
        {
            enemyAnim.SetTrigger("DamageTrigger"); 
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
            if(other.gameObject.layer == 6)
            {
                Debug.Log("We have hit an object with impact"); 
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
                Debug.Log("Reset hit impact state"); 
                hasHitObject = false; 
            }
        }
    }

      private void CheckForRecovery()
    {
        
        currentVelocity = enemyRb.velocity.magnitude; 

        //Check if a stunned and launched enemy has stopped moving 
        if(isLaunched && currentVelocity < 2f)
        {
            revoveryTimer += Time.deltaTime; 
            if(revoveryTimer >= recoveryDuration)
            {
                StandBackUp(); 
                Debug.Log("We can recover"); 
             //   isGettingUp = true; 
                isLaunched = false;
                isStunned = false; 
            }
        }
        else
        {
            revoveryTimer = 0f; 
        }
    }

    public void CheckForImpactDamage()
    {

        //Add damage to enemy on high speed launch collisions
        if(hasHitObject && currentVelocity > minStunnedImpactVelocity && canAddImpactDamage)
        {
            canAddImpactDamage = false; 
            TakeDamage(50f); 
            Debug.Log("Add like some damage"); 
        }
        
    }

  

    public void EnemyDies()
    {
        canBeTargeted = false; 
         transform.tag = "Dead"; 
        //transform.GetComponent<BasicEnemyScript>().enabled = false; 
    }
       
    
   

    void StandBackUp()
    {
        enemyState = (int)currentState.IDLE; 
    //    isGettingUp = false ;
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

        enemyRb.useGravity = false; 
        enemyRb.isKinematic = false; 

        /*
        foreach(Rigidbody rb in ragdollRbs)
        { 
            rb.AddForce(direction * Random.Range(force - 5, force + 5), ForceMode.VelocityChange);
            rb.AddForce(transform.up * 5f, ForceMode.VelocityChange);  
        }
        */
      
        isRagdolling = true; 
    }

    public void CutOffLimbs(){

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

        mainCollider.isTrigger = false;  
        enemyAnim.enabled = true; 
        isRagdolling = false; 
        canAddImpactDamage = false; 
        isLaunched = false; 
    }
    
}
