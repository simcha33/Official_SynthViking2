using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;

public class Torti2 : MonoBehaviour
{


    public float targetDistance;
    public float maxHealth;
    public float currentHealth;

    [Header("MOVING")]
    public float currentMoveSpeed;
  //  public bool canCharge;
  //  public bool hasReachedTarget;
    public bool chargeLoop;
    public bool chargeStart;
    public float chargeMoveSpeed;
    public float waitForChargeLoopTimer;
    public float waitForChargeLoopDuration;

    [Header("ATTACKING")]
    public float attackDistance;
    public bool isAttacking;
  //  public bool canAttack;

    [Header("Explosion")]
    public float explosionRadius;

  
    public float explosionDamage;
    public GameObject explosionEffect;
    public GameObject stunnedEffect;
    public GameObject hatFire;

    [Header("Components")]
    private Transform target;
    private NavMeshAgent tortiAgent;
    private Animator tortiAnim;
    private Rigidbody tortiRb;
    public MMFeedbacks chargeStartFeedback;
    public MMFeedbacks chargeLoopFeedback;
    public MMFeedback chargeEndFeedback;

    public List<Rigidbody> tortiRbs = new List<Rigidbody>();

    [Header("Visuals")]
    public SkinnedMeshRenderer backOrbMeshr;

    public List<SkinnedMeshRenderer> litMeshes = new List<SkinnedMeshRenderer>();
    public Material backOrbDefaultMat;
    public Material backOrbStunnedMat;
    public Material backOrbDeadMat;


    private int tortiState;

    private enum currentState
    {
        Charge,
        Attacking,

    }


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        tortiAgent = GetComponent<NavMeshAgent>();
        tortiAnim = gameObject.GetComponent<Animator>();
        tortiRb = gameObject.GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        currentMoveSpeed = 0f;
        tortiState = (int)currentState.Charge; 
    }

    // Update is called once per frame
    void Update()
    {
        switch (tortiState)
        {
            case (int)currentState.Charge:
                CheckForTarget();
                MoveTowardsPlayer(); 
                break;

            case (int)currentState.Attacking:
                CheckForTarget(); 
                break; 
              
        }

        CheckForTarget();
        tortiAgent.speed = currentMoveSpeed;
    }

    void CheckForTarget()
    {
        targetDistance = Vector3.Distance(transform.position, target.position);

        if(targetDistance > attackDistance)
        {
            
            tortiState = (int)currentState.Charge; 
        }
        else if(targetDistance <= attackDistance)
        {
            tortiState = (int)currentState.Attacking; 
        }
    }

    void MoveTowardsPlayer()
    {
        if (!chargeStart && !chargeLoop)
        {
            ResetStates();
            chargeStart = true;
            tortiAnim.SetTrigger("ChargeAttackStartTrigger");
            chargeStartFeedback?.PlayFeedbacks();
            currentMoveSpeed = 0f; 

        }

        if (chargeStart)
        {
            waitForChargeLoopTimer += Time.deltaTime;
            if(waitForChargeLoopTimer >= waitForChargeLoopDuration)
            {
                waitForChargeLoopTimer = 0f;
                chargeLoop = true;
                chargeStart = false;
           
                tortiAnim.SetTrigger("ChargeLoopTrigger");
                chargeLoopFeedback?.PlayFeedbacks(); 
                currentMoveSpeed = chargeMoveSpeed;
            }
        }

        if (targetDistance > attackDistance)
        {
            tortiAgent.destination = target.position - transform.forward;
        }

    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            //trigger come kind of attack animation 
        }
    }

    void TriggerCharge()
    {

    }

    void ResetStates()
    {
        chargeStart = false;
        chargeLoop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(other.gameObject.GetComponents<BasicEnemyScript>() != null)
            {
                BasicEnemyScript otherScript = other.gameObject.GetComponent<BasicEnemyScript>();
                if (!otherScript.isStunned)
                {
                    otherScript.LaunchEnemy(other.transform.position - transform.position, 1.3f, 10f);
                }

            }
        }
    }
}
