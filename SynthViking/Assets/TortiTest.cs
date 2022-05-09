using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Feedbacks; 

public class TortiTest : MonoBehaviour
{
    //Components
    private NavMeshAgent agent;  
    private Animator tortiAnim;
    private Rigidbody tortiRb;

    //Animations 
    public float currentAnimationTimer;
    public float currentAnimationDuration; 

    [Header("Movement")]
    public float currentMoveSpeed;
    public float walkMoveSpeed;
    public bool canMove; 

    [Header("Target Selection")]
    public float targetDistance;
    public Transform target;

    [Header("Attack selection")]
    public float minNextAttackDuration;
    public float maxNextAttackDuration;
    private float nextAttackDuration;
    public float nextAttackTimer;
    public bool attackIsChoosen;

    [Header ("AirAttack")]
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

    [Header("Stunned")]
    public float stunTimer;
    public float stunDuration;
    public bool isStunned; 



    [Header("Charge Attack")]
    public float chargeMoveSpeed;
    public float chargeTimer;
    public float maxChargeDuration;
    public float minChargeDuration; 
    private float chargeDuration;
    public float chargeAttackDamage; 
    public bool isCharging;
    public bool chargeHitTarget; 
    public float chargeStartDelayTimer; 
    public float chargeStartDelayDuration;
    public float chargeEndStunDuration; 
    public MMFeedbacks chargeStartFeedback;
    public MMFeedbacks chargeLoopFeedback;
    public MMFeedback chargeEndFeedback; 

    

    private enum currentState
    {
        Follow,
        Charge,
        AirAttack,
        Dead,
        Stunned,
        Punch,
        Grab
    }

    private enum attackType
    {
        AirAttack,
        PunchAttack,
        ChargeAttack
    }

    private int choosenAttack; 

    private int tortiState; 

    void Start()
    {
  
        //Components 
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        tortiAnim = gameObject.GetComponent<Animator>(); 
        tortiRb = gameObject.GetComponent<Rigidbody>();

        //Set defeault states 
        tortiState = (int)currentState.Follow;
        currentMoveSpeed = walkMoveSpeed;
        ResetAttack(); 
    }

    // Update is called once per frame
    void Update()
    {
        switch (tortiState)
        {
            case (int)currentState.Follow:      
                WaitForNextAttack();
                CheckForTarget();
                FollowTarget();
                break;

            case (int)currentState.Charge:

            //    StartChargeAttack();
                DoChargeAttack(); 
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
        }

        agent.speed = currentMoveSpeed; 
        CheckForAnimation();
        Debug.Log(tortiState.ToString()); 
    }

    void SetTarget()
    {

    }

    void CheckForAnimation()
    {
        tortiAnim.SetBool("IsCharging", isCharging); 
    }

    void FollowTarget()
    {
        agent.destination = target.position - transform.forward;
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
        choosenAttack = (int)attackType.ChargeAttack; 


        if (choosenAttack == (int)attackType.AirAttack)
        {
            tortiState = (int)currentState.AirAttack;
        }

        if (choosenAttack == (int)attackType.ChargeAttack)
        {
            //StartChargeAttack();
            StartChargeAttack(); 
            tortiState = (int)currentState.Charge;
        }

        if (choosenAttack == (int)attackType.PunchAttack)
        {
            tortiState = (int)currentState.Punch;
        }
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

    void StartChargeAttack()
    {
        tortiAnim.SetTrigger("StartChargeTrigger");
        chargeStartDelayTimer = 0f;
        currentMoveSpeed = 0f;
        canMove = false;
        print("1. Start trigger");
        chargeStartFeedback?.PlayFeedbacks();
    }


    void DoChargeAttack()
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

            print("2. Start Loop trigger");
        }

        //Charge loop
        if (isCharging && chargeTimer < chargeDuration)
        {
            chargeTimer += Time.deltaTime; 
        }
        else if(isCharging)
        {
            print("3. End charge Trigger");
            
            EndChargeAttack(); 
        }
    }


    void EndChargeAttack()
    {
        ResetState();
        chargeLoopFeedback.StopFeedbacks();
        chargeTimer = 0;

        if (!chargeHitTarget)
        {
            print("3.1 End no colllision");
            stunDuration = chargeEndStunDuration; 
            tortiState = (int)currentState.Stunned;
        }
        else
        {
            print("3.2 End yes collision");
        }
    }

    void StunCoolDown()
    {
        stunTimer += Time.deltaTime;
        isStunned = true; 

        if(stunTimer >= stunDuration)
        {
            print("4. Exit cooldown state");
            ResetState(); 
            tortiState = (int)currentState.Follow;
            stunTimer = 0f;
       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCharging)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                chargeHitTarget = true;
                chargeTimer = chargeDuration; //End charge

                PlayerState playerstateScript = other.gameObject.GetComponent<PlayerState>();
                playerstateScript.TakeDamage(chargeAttackDamage, attackType.ChargeAttack.ToString());
                print("Player Was hit by charge attack");

            }
        }
    }

    void ResetState()
    {
        canMove = true;
        isCharging = false;
        isStunned = false; 
    }

   

    void DoAirSlamAttack()
    {

    }

    void DoPunchAttack()
    {

    }


}
