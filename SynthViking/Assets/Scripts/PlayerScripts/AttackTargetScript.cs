using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks; 

public class AttackTargetScript : MonoBehaviour
{
    [Header("Type")] //GROUND MOVEMENT
    #region
    public bool isEnemy;
    public bool isPlayer;
    private int attackerType;

    public enum currentAttackerType
    {
        BASICENEMY,
        PLAYER,
    }
    #endregion

    [Header("COMPONENTS")] //GROUND MOVEMENT
    #region
    public MMFeedbacks weaponHitFeedback;
    public MMFeedbacks weaponKillFeedback;
    public MMFeedbacks weapinFirstImpactFeedback;
    [HideInInspector] public ThirdPerson_PlayerControler playerController;
    public BasicEnemyScript enemyController; 

    public HitPauses hitpauseScript; 
    public CheckForLimbs limbCheckerScript;

    public GameObject bloodFx1;
    public GameObject bloodFx2;
    public Transform swordPoint;
    public Transform targetCube;
    public Transform selectedTarget;
    #endregion

    [Header("VALUES")] //GROUND MOVEMENT
    #region
    public float targetCheckRadius; 
    public float targetCheckRange;
    public bool canTarget; 
    public List<GameObject> targetsInRange = new List<GameObject>();
    #endregion

   




    void Start()
    {
        if (isPlayer) attackerType = (int)currentAttackerType.PLAYER;
        else if (isEnemy) attackerType = (int)currentAttackerType.BASICENEMY;
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
    }

    void Update()
    {
        switch (attackerType)
        {
            case (int)currentAttackerType.BASICENEMY:
                break;

            case (int)currentAttackerType.PLAYER:
                TargetLock();
                break; 
        }
              
    }
        
     

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == playerController.enemyLayer && !targetsInRange.Contains(other.gameObject) && isPlayer)
        {
            targetsInRange.Add(other.transform.gameObject);
        }
        else if(other.gameObject.CompareTag("Player") && !targetsInRange.Contains(other.gameObject) && isEnemy)
        {
            targetsInRange.Add(other.transform.gameObject);
        }
    }

    void TargetLock()
    {
        //Check for enemies within range 
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, targetCheckRadius, playerController.inputDir, out hit, targetCheckRange) && canTarget)
        {
            //Check for enemy layer
            if (hit.transform.gameObject.layer == playerController.enemyLayer && !hit.transform.CompareTag("Dead"))
            {
                BasicEnemyScript script = hit.transform.GetComponent<BasicEnemyScript>();
                if (script.canBeTargeted)
                {
                    selectedTarget = hit.transform;
                    targetCube.transform.position = selectedTarget.position + new Vector3(0, 2, 0);
                }
            }
        }
        else
        {
            targetCube.transform.position = new Vector3(5000, 5000, 500);
            selectedTarget = null;
        }
    }



    
    void OnTriggerExit(Collider other)
    {
        if (targetsInRange.Count > 0)
        {
            targetsInRange.Remove(other.gameObject); 
        }

    }
    


    public void TargetDamageCheck()
    {
        if(targetsInRange.Count <= 0) return;

        if (isPlayer)
        {
            foreach (GameObject obj in targetsInRange)
            {
                //Deal damage to hit target
                BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>();
                enemyScript.TakeDamage(playerController.currentAttackDamage, "LightAxeDamage");
                hitpauseScript.theHitPaused.Add(obj);
                Instantiate(bloodFx1, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                weapinFirstImpactFeedback?.PlayFeedbacks();


                //Is the enemy dead after our hit?            
                if (enemyScript.isDead)
                {

                    //Collect and detach limbs
                    foreach (Rigidbody limb in limbCheckerScript.hitLimbs)
                    {
                        if (enemyScript.ragdollRbs.Contains(limb))
                        {
                            CharacterJoint joint = limb.GetComponent<CharacterJoint>();
                            Instantiate(bloodFx1, limb.position, limb.rotation);
                            Destroy(joint);
                            limb.transform.parent = null;

                            limb.velocity = new Vector3(0, 0, 0);
                            //limb.AddForce((swordPoint.position - limb.transform.position) * 1f, ForceMode.Impulse);
                            //limb.AddForce(Vector3.up * Random.Range(7f, 10f), ForceMode.Impulse);
                            limb.AddExplosionForce(10f, limb.position, 2f, 4f, ForceMode.Impulse); 
                            //limb.AddTorque(transform.up * 200f * Time.deltaTime, ForceMode.Impulse);
                            //limb.AddTorque(transform.right * 200f * Time.deltaTime, ForceMode.Impulse); 
                            //Instantiate(bloodFx1, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);

                            if (enemyScript.canBeTargeted) limb.mass *= 3f;
                        }
                    }

                    if (enemyScript.canBeTargeted)
                    {
                        if (enemyScript.weapon != null)
                        {
                            enemyScript.enemyRb.velocity = new Vector3(0,0,0); 
                        
                            Destroy(enemyScript.weapon.GetComponent<FixedJoint>());
                            enemyScript.weapon.parent = null;
                            Rigidbody swordrb = enemyScript.weapon.GetComponent<Rigidbody>();
                            swordrb.useGravity = true;
                            swordrb.isKinematic = false;
                            swordrb.velocity = new Vector3(0, 0, 0);
                            swordrb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);

                        }

                        enemyScript.canBeTargeted = false;
                        weaponKillFeedback?.PlayFeedbacks();
                    }

                }
                else
                {
                    hitpauseScript.objectsToPause.Add(enemyScript.enemyAnim);
                    hitpauseScript.doHitPause = true;
                }

                // targetsInRange.Remove(obj); 
                if (!enemyScript.isRagdolling || enemyScript.isDead) enemyScript.transform.DOMove(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * 1.5f, .3f).SetUpdate(UpdateType.Fixed);  //Move enemy backwards enemyScript.enemyRb.AddForce(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce, ForceMode.VelocityChange); 
                else if(enemyScript.isRagdolling || enemyScript.isDead)enemyScript.enemyRb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange); 
                                                                                                                                                                                              // targetsInRange.Clear();

            }

            limbCheckerScript.hitLimbs.Clear();
            //targetsInRange.Clear();
        }
        if (isEnemy)
        {
            foreach (GameObject obj in targetsInRange)
            {
                //Deal damage to hit target
             
                PlayerState targetScript = obj.GetComponent<PlayerState>();               
                targetScript.TakeDamage(enemyController.currentAttackDamage, enemyController.attackType);
                //   hitpauseScript.theHitPaused.Add(obj);
                // targetsInRange.Remove(obj);

                //  Instantiate(bloodFx1, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                //  weapinFirstImpactFeedback?.PlayFeedbacks();
            }
        }

    }

   

    public void TargetDamageEffects()
    {
     
    
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
      //  Gizmos.DrawSphere(playerController.transform.position * playerController.moveInput,)

        /*
        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(dashChecker.position, dashChecker.forward * m_MaxDistance);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(dashChecker.position + dashChecker.forward * m_MaxDistance, dashCheckerSize);

        Gizmos.color = Color.green;

        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(dashChecker.position, dashChecker.forward * m_MaxDistance2);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(dashChecker.position + dashChecker.forward * m_MaxDistance2, dashCheckerSize2);
        */

    }
    /*

    // Update is called once per frame
    void Update()
    {
        CheckForAttackTarget(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackTargetCheck") && !dontRemoveTarget)
        {
            if (!nearbyTargets.Contains(other.transform))
            {
                nearbyTargets.Add(other.transform);
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AttackTargetCheck") && !dontRemoveTarget)
        {
            if (nearbyTargets.Contains(other.transform))
            {
                nearbyTargets.Remove(other.transform);
            } 
        }
    }

    void CheckForAttackTarget()
    {
        if (nearbyTargets.Count > 0)
        {

            //Find the closest target to attack
            foreach (Transform target in nearbyTargets)
            {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (targetDistance < closedDistance || nearbyTargets.Count >= 1 && closedDistance == 0)
                {
                    closedDistance = targetDistance;
                    selectedTarget = target;
                }
            }
        }

        
        {
            closedDistance = 0; 
            selectedTarget = null;
        }
    }
    */
}
