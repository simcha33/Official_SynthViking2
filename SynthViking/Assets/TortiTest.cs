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

    [Header("Charge Attack")]
    public float chargeMoveSpeed;
    public float chargeTimer;
    public float chargeDuration;
    public bool isCharging;
    public float chargeStartDelayTimer; 
    public float chargeStartDelayDuration;
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
        }

        agent.speed = currentMoveSpeed; 
        CheckForAnimation(); 
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
        //currentAnimationDuration = tortiAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / tortiAnim.GetCurrentAnimatorStateInfo(0).speed;
        tortiAnim.SetTrigger("StartChargeTrigger");
        chargeStartDelayTimer = 0f;

       // agent.enabled = false; 
        isCharging = true; 
    }

    void DoChargeAttack()
    {
        chargeStartDelayTimer += Time.deltaTime; 
        if(chargeStartDelayTimer >= chargeStartDelayDuration)
        {
        //    agent.enabled = true; 
            currentMoveSpeed = chargeMoveSpeed;
            tortiAnim.SetTrigger("ChargeLoopTrigger");
        }
    }


    void EndChargeAttack()
    {

    }

   

    void DoAirSlamAttack()
    {

    }

    void DoPunchAttack()
    {

    }


}
