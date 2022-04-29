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

    [Header("FEEDBACKS")] //GROUND MOVEMENT
    #region
    public MMFeedbacks weaponHitFeedback;
    public MMFeedbacks weaponKillFeedback;
    public MMFeedbacks weapinFirstImpactFeedback;
    public MMFeedbacks playerBlockFeedback;
    public MMFeedbacks parriedFeedback; 
    #endregion


    [Header("COMPONENTS")] //GROUND MOVEMENT
    #region
    public BasicEnemyScript enemyController;

    [HideInInspector] public ThirdPerson_PlayerControler playerController;
   // public CleanUpScript cleanUpScript;
    public HitPauses hitpauseScript; 
    public CheckForLimbs limbCheckerScript;

    public Transform swordPoint;
    public Transform targetCube;
    public Transform selectedTarget;
    #endregion

    [Header("VALUES")] //GROUND MOVEMENT
    #region
    public float targetCheckRadius; 
    public float targetCheckRange;
    public bool canTarget;
    private float attackBackForce = 1.1f; 
    public List<GameObject> targetsInRange = new List<GameObject>();
    public List<GameObject> hitTargets = new List<GameObject>(); 
    #endregion

     [Header("VISUALS")] //GROUND MOVEMENT
     #region 
    public GameObject axeHitBloodVFX;
    public GameObject axeKillBloodVFX; 
    public GameObject parryLightningVFX; 
     #endregion

   




    void Start()
    {
        if (isPlayer) attackerType = (int)currentAttackerType.PLAYER;
        else if (isEnemy) attackerType = (int)currentAttackerType.BASICENEMY;
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        //comboManagerScript = GameObject.Find("StyleManager").GetComponent<ComboManager>(); 
    }

    void Update()
    {
        switch (attackerType)
        {
            case (int)currentAttackerType.BASICENEMY:
                CheckForParry(); 
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

     void CheckForParry()
    {
        foreach (GameObject obj in targetsInRange)
        {
            PlayerState targetScript = obj.GetComponent<PlayerState>();

            if (playerController.isBlocking && enemyController.canBeParried && enemyController.isAttacking) //Check if the player is blocking              
            {
                playerController.blockRechargeTimer = playerController.blockRechargeDuration;
                playerController.blockTimer = playerController.blockDuration - .1f; 

                if (playerController.canBlockStun) //Do a stun effect around the player 
                {
                    Collider[] colls = Physics.OverlapSphere(playerController.transform.position, playerController.blockStunRadius);
                    foreach (Collider enemy in colls)
                    {
                        if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                        {
                            //Debug.Log(enemy.name);
                            BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>(); 
                            if(!enemyScript.isDead)
                            {
                                playerController.canBlockStun = false;
                                enemyScript.TakeDamage(enemyController.chainHitScript.blockChainDamage, playerAttackType.BlockStun.ToString());
                                enemyScript.isLaunched = true;
                                enemyScript.isBlockStunned = true;
                                
                                //Feedback
                                GameObject lightningVFX =  Instantiate(parryLightningVFX, enemy.transform.position, transform.rotation);
                                lightningVFX.transform.eulerAngles = new Vector3(-90, lightningVFX.transform.eulerAngles.y, lightningVFX.transform.eulerAngles.z);  
                                lightningVFX.AddComponent<CleanUpScript>();
                                parriedFeedback?.PlayFeedbacks();
                                playerController.mainGameManager.DoHaptics(.2f, .4f, .7f); 
                            }
                        }

                    }
                    playerController.playerAnim.SetTrigger("HasParriedTrigger"); 
                    playerController.attackTargetScript.playerBlockFeedback?.PlayFeedbacks();
                    playerController.hasParriedAttack = true; 
              
                    print("block feedback");
                }
            }
           // else targetScript.TakeDamage(enemyController.currentAttackDamage, enemyController.enemyAttackType); //Damage the player
        }
    }

    public void BackCheck()
    {
        foreach (GameObject enemy in targetsInRange)
        {
            BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
            Vector3 backDirection = playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * attackBackForce;
            enemyScript.transform.DOMove(backDirection, .3f).SetUpdate(UpdateType.Fixed);  //Move enemy backwards enemyScript.enemyRb.AddForce(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce, ForceMode.VelocityChange);  
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
                if (!enemyScript.isDead)
                {
                    enemyScript.TakeDamage(playerController.currentAttackDamage, playerAttackType.LightAxeHit.ToString());
                    hitpauseScript.theHitPaused.Add(obj);
                }
                Vector3 backDirection = playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * attackBackForce;

                //Feedback
                GameObject blood = Instantiate(axeHitBloodVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                blood.AddComponent<CleanUpScript>();
                weapinFirstImpactFeedback?.PlayFeedbacks();
                playerController.mainGameManager.DoHaptics(.3f, .2f, .4f); 


                //Is the enemy dead after our hit?            
                if (enemyScript.isDead)
                {
                    //Collect and detach limbs
                    foreach (Rigidbody limb in limbCheckerScript.hitLimbs)
                    {
                        if (enemyScript.ragdollRbs.Contains(limb))
                        {
                            CharacterJoint joint = limb.GetComponent<CharacterJoint>();
                            GameObject blood2 = Instantiate(axeKillBloodVFX, limb.position, limb.rotation);
                            blood2.AddComponent<CleanUpScript>();
                            Destroy(joint);
                            limb.transform.parent = null;

                            limb.velocity = new Vector3(0, 0, 0);
                            limb.AddExplosionForce(10f, limb.position, 2f, 4f, ForceMode.Impulse);
                            if (enemyScript.canBeTargeted) limb.mass *= 3f;
                            // limb.GetComponent<SetEnemyInside>().ActivateLimbInside(); 
                        }
                    }

                    foreach (GameObject enemyInside in limbCheckerScript.hitInsides)
                    {
                        enemyInside.GetComponent<MeshRenderer>().enabled = true;
                    }

                    if (enemyScript.canBeTargeted)
                    {

                        //Unhand enemies weapon
                        if (enemyScript.weapon != null)
                        {
                            enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);
                            Destroy(enemyScript.weapon.GetComponent<FixedJoint>());
                            enemyScript.weapon.parent = null;
                            Rigidbody swordrb = enemyScript.weapon.GetComponent<Rigidbody>();
                            swordrb.useGravity = true;
                            swordrb.isKinematic = false;
                            swordrb.velocity = new Vector3(0, 0, 0);
                            swordrb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);                        
                        }

                        // targetsInRange.Remove(obj);
                        enemyScript.canBeTargeted = false;
                        playerController.mainGameManager.DoHaptics(.2f, .55f, .75f); 
                        weaponKillFeedback?.PlayFeedbacks();
                    }

                }
                else
                {
                    //  hitpauseScript.objectsToPause.Add(enemyScript.enemyAnim);
                    hitpauseScript.doHitPause = true;
                }
                enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);
                enemyScript.transform.DOMove(backDirection, .3f).SetUpdate(UpdateType.Fixed);  //Move enemy backwards enemyScript.enemyRb.AddForce(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce, ForceMode.VelocityChange);            
                if (enemyScript.isRagdolling ||  enemyScript.isDead) enemyScript.enemyRb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);


                limbCheckerScript.hitLimbs.Clear();
                limbCheckerScript.hitInsides.Clear();
            }
        }
        


        if (isEnemy)
        {

            foreach (GameObject obj in targetsInRange)
            {
                print("DamagePlayer"); 
                PlayerState targetScript = obj.GetComponent<PlayerState>();
                targetScript.TakeDamage(enemyController.currentAttackDamage, enemyController.enemyAttackType); //Damage the player
            }         
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(isPlayer){
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(playerController.transform.position,playerController.blockStunRadius);
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
