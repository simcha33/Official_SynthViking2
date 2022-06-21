using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Feedbacks; 

public class TortiTest : MonoBehaviour
{
    //Components
    private NavMeshAgent tortiAgent;  
    private Animator tortiAnim;
    private Rigidbody tortiRb;
    public ThirdPerson_PlayerControler playerController;

    //Animations 
    public float currentAnimationTimer;
    public float currentAnimationDuration; 

    [Header("Movement")]
    #region
    public float currentMoveSpeed;
    public float walkMoveSpeed;
    public bool canMove; 
    #endregion

    [Header("Target Selection")]
    #region
    public float targetDistance;
    public Transform target;
    #endregion
    
    [Header("Attack selection")]
    #region
    public float minNextAttackDuration;
    public float maxNextAttackDuration;
    private float nextAttackDuration;
    public float nextAttackTimer;
    public bool attackIsChoosen;
    #endregion

    [Header ("AirAttack")]
    #region
    public float airAttackMinDistance;
    public float airAttackMaxDistance;
    public float inAirMoveSpeed;
    public float maxInAirTime;
    public float airAttackHeight;
    public GameObject groundTargetObj;

    private float inAirTimer;
    public float inAirDuration;
    private float airAttackImpactTimer;
    public float airAttackImpactDuration;

    public bool canAirAttack;
    public bool isInAir;
    public bool hasLanded;
    #endregion

    [Header("Stunned")]
    #region
    public float stunTimer;
    public float stunDuration;
    public bool isStunned;
    #endregion

    public bool hasExploded; 

    [Header("Charge Attack")]
    #region
    public float chargeMoveSpeed;
    public float chargeAttackDamage; 
    private float chargeTimer;
    public float maxChargeDuration, minChargeDuration; 
    private float chargeDuration;
    private bool isCharging;
    private bool isChargeAttacking; 
    private bool chargeHitTarget; 
    private float chargeStartDelayTimer; 
    public float chargeStartDelayDuration;
   // public float chargeEndStunDuration; 
    public MMFeedbacks chargeStartFeedback;
    public MMFeedbacks chargeLoopFeedback;
    public MMFeedback chargeEndFeedback; 

    //Grab attack
    public float grabAttackTimer;
    public bool canEndGrabAttack; 
    public float grabAttackDuration; 

    private float grabAirSpeed; 
    public Vector3 jumpPoint;
    #endregion

    [Header("Visuals")]
    #region
    public SkinnedMeshRenderer backOrbMeshr; 

    public List <SkinnedMeshRenderer> litMeshes = new List <SkinnedMeshRenderer>(); 
    public Material backOrbDefaultMat; 
    public Material backOrbStunnedMat;
    public Material backOrbDeadMat;

    public GameObject explosionEffect;
    public GameObject explosionRadius;
    public GameObject stunnedEffect;

    public GameObject hatFire;
    #endregion

    

    private enum currentState
    {
        Follow,
        Charge,
        ChargeAttack,
        AirAttack,
        Dead,
        Stunned,
        Punch,
        Grab,
        Explode
    }

    private enum attackType
    {
        AirAttack,
        PunchAttack,
        ChargingForward,
        ChargeAttack,
    }

    private int choosenAttack; 

    private int tortiState; 

    void Start()
    {
  
        //Components 
        target = GameObject.Find("Player").transform;
        tortiAgent = GetComponent<NavMeshAgent>();
        tortiAnim = gameObject.GetComponent<Animator>(); 
        tortiRb = gameObject.GetComponent<Rigidbody>();
        // litMeshes.Add(backOrbMeshr); 

        //Set defeault states 
        //tortiState = (int)currentState.Follow;
        tortiState = (int)currentState.Follow;
        currentMoveSpeed = walkMoveSpeed; 
        ResetState(); 
        ResetAttack();
      //  isCharging = true;
        StartChargeAttack();
        tortiState = (int)currentState.Charge;
        ResetAttack(); 

    }

    // Update is called once per frame
    void Update()
    {
        switch (tortiState)
        {
            case (int)currentState.Follow:      
              //  WaitForNextAttack();
               // CheckForTarget();
               // FollowTarget();
                break;

            case (int)currentState.Charge:

            //    StartChargeAttack();
                DoChargeForward(); 
                CheckForTarget(); 
                FollowTarget();
                break;

            case (int)currentState.Punch:
    
                break;

            case (int)currentState.AirAttack:        
                FollowTarget();
                CheckForTarget();
                break;

            case (int)currentState.Stunned:
                StunCoolDown(); 
                break;
            case(int)currentState.ChargeAttack:
                DoChargeAttack(); 
                break;
            case (int)currentState.Explode:
                if(!hasExploded)DoExplode(); 
                break;
        }

        tortiAgent.speed = currentMoveSpeed; 
        CheckForAnimation();
    }

    void SetTarget()
    {

    }

    void DoExplode()
    {
        hasExploded = true; 
        GameObject explodeEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        explodeEffect.AddComponent<CleanUpScript>();
    }

    void CheckForAnimation()
    {
        tortiAnim.SetBool("IsCharging", isCharging); 
        tortiAnim.SetBool("IsStunned", isStunned); 
        tortiAnim.SetBool("IsChargeAttacking", isChargeAttacking); 
    }

    void FollowTarget()
    {
        tortiAgent.destination = target.position - transform.forward;
        transform.LookAt(target);
    }



    void CheckForTarget()
    {
        targetDistance = Vector3.Distance(transform.position, target.position);
    }

    void WaitForNextAttack()
    {
        nextAttackTimer += Time.deltaTime;
        if(nextAttackTimer >= maxNextAttackDuration)
        {
            ChooseNewAttackType(); 
        }
    }

    void ChooseNewAttackType()
    {
        attackIsChoosen = true;
        choosenAttack = (int)attackType.ChargingForward; 


        if (choosenAttack == (int)attackType.AirAttack)
        {
            tortiState = (int)currentState.AirAttack;
        }

        if (choosenAttack == (int)attackType.ChargingForward)
        {
            //StartChargeAttack();
            StartChargeAttack(); 
            tortiState = (int)currentState.Charge;
        }

        if (choosenAttack == (int)attackType.PunchAttack)
        {
            tortiState = (int)currentState.Punch;
        }

        ResetAttack(); 

        
    }

    void ResetAttack()
    {
        attackIsChoosen = false;
        nextAttackTimer = 0f;
        nextAttackDuration = Random.Range(minNextAttackDuration, maxNextAttackDuration); 
    }
      

    void SetAttackType()
    {
    //    choosenAttack = (int)Random.Range(0, 2); 

    }


    //CHARGING

    void StartChargeAttack()
    {
        tortiAnim.SetTrigger("StartChargeTrigger");
        chargeStartDelayTimer = 0f;
        currentMoveSpeed = 0f;
        canMove = false;
        isCharging = true; 
        chargeStartFeedback?.PlayFeedbacks();
    }


    void DoChargeForward()
    {
       chargeStartDelayTimer += Time.deltaTime;

        //Charge start
        if (!isCharging && chargeStartDelayTimer >= chargeStartDelayDuration)
        {
            isCharging = true;
            chargeStartDelayTimer = 0f;
            chargeDuration = Random.Range(minChargeDuration, maxChargeDuration);
            currentMoveSpeed = chargeMoveSpeed;
            chargeLoopFeedback.PlayFeedbacks();

            tortiAnim.SetTrigger("ChargeLoopTrigger");
            tortiState = (int)currentState.Charge;
        }

        //Charge loop
        if (isCharging && chargeTimer < chargeDuration)
        {
            chargeTimer += Time.deltaTime; 
        }
        else if(isCharging)
        {     
            EndChargeForward(); 
        }
    }

    void EndChargeForward()
    {
        ResetState();
        chargeLoopFeedback.StopFeedbacks();
        chargeTimer = 0;

        if (!chargeHitTarget) //Charge has ended without hitting target 
        {
           // stunDuration = chargeEndStunDuration; 
            tortiState = (int)currentState.Follow; 

        }   
        else //Charge has hit target
        {
            tortiState = (int)currentState.ChargeAttack; 
            ParentPlayerToFist(); 

        }
    }

    void ParentPlayerToFist()
    {

    }

    void DoChargeAttack()
    {
        float jumpHeight = 7.5f; 
         
        if(!isChargeAttacking)        
        {
            jumpPoint = transform.position + new Vector3(0,jumpHeight,0); 
            tortiAnim.SetTrigger("GrabAttackStartTrigger");   
            isChargeAttacking = true; 
            grabAttackTimer = 0f; 
            tortiAgent.enabled = false;     
            grabAirSpeed = 15f;  
        }

           float UpDistance = Vector3.Distance(transform.position, jumpPoint); 
    


        if(UpDistance < .5f || canEndGrabAttack)
        {       
            if(!canEndGrabAttack)
            {
                jumpPoint = transform.position - new Vector3(0,jumpHeight,0); 
                tortiAnim.SetTrigger("GrabAttackEndTrigger");
                canEndGrabAttack = true; 
                grabAttackTimer = 0f; 
                grabAirSpeed = 30f; 
        
            }               
     
            if(UpDistance <.5f && grabAttackTimer > 0)
            {
                /*
                print("Return To follow state" ); 
                ResetState(); 
                tortiState = (int)currentState.Follow; 
                */
                tortiState = (int)currentState.Explode;
                GameObject explodeEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
                explosionEffect.AddComponent<CleanUpScript>();
                print("boooom");
            }

        }
        
        
        if(grabAttackTimer > .45f || canEndGrabAttack && grabAttackTimer > .5f)
        {
            print("Move torti up" ); 
            transform.position = Vector3.MoveTowards(transform.position, jumpPoint, grabAirSpeed * Time.deltaTime); 
        }



        grabAttackTimer += Time.deltaTime; 
        
    }




    void StunCoolDown()
    {
        

        if(!isStunned)
        {                   
            isStunned = true; 
            tortiAgent.enabled = false;
            stunnedEffect.SetActive(true); 
            hatFire.SetActive(false); 
            tortiAnim.SetTrigger("StunTrigger");         
            foreach(SkinnedMeshRenderer meshR in litMeshes) meshR.material = backOrbStunnedMat;   
        }

        if(stunTimer >= stunDuration)
        {
            ResetState(); 
            tortiAgent.enabled = true; 
            stunnedEffect.SetActive(false); 
            hatFire.SetActive(true); 
            tortiState = (int)currentState.Follow;
            stunTimer = 0f;     
        }

        stunTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCharging)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                chargeHitTarget = true;
                EndChargeForward();
            //    chargeTimer = chargeDuration; //End charge
                PlayerState playerstateScript = other.gameObject.GetComponent<PlayerState>();
                playerstateScript.TakeDamage(chargeAttackDamage, attackType.ChargeAttack.ToString());

            }
        }
    }

    void ResetState()
    {
        canMove = true;
        isCharging = false;
        isStunned = false; 
        isChargeAttacking = false;
        canEndGrabAttack = false;  
        foreach(SkinnedMeshRenderer meshR in litMeshes) meshR.material = backOrbDefaultMat; 
    }

   

    void DoAirSlamAttack()
    {

    }

    void DoPunchAttack()
    {

    }


}
