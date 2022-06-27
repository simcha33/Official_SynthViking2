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
    public MMFeedbacks airLaunchHitFeedback;


    public MMFeedbacks punchFirstImpactFeedback;
    public MMFeedbacks punchHitFeedback;
    public MMFeedbacks punchKillFeedback;

    public MMFeedbacks playerBlockFeedback;
    public MMFeedbacks parriedFeedback; 
    public MMFeedbacks sprintAttackHitFeedback; 
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

    [Header("AIR LAUNCH ATTACK")] //SPRINT ATTACK
    #region
    [HideInInspector] public List<BasicEnemyScript> airLaunchTargets = new List<BasicEnemyScript>();
    #endregion

    [Header("SPRINT ATTACK")] //SPRINT ATTACK
    #region
    public float sprintAttackBackForce = 50f;
    public float backPointDistance = 8f;
    public Transform backPointCheck;
    private int sprintTargetCount;
    private List<BasicEnemyScript> sprintAttackTargets = new List<BasicEnemyScript>();
    #endregion

    [Header("SPRINT ATTACK")]
    private List<BasicEnemyScript> punchKillList = new List<BasicEnemyScript>(); 

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
    public GameObject axeHitBloodVFX2;
    public GameObject axeHitBloodVFX3;
    public GameObject axeKillBloodVFX;
    public GameObject bloodDrip;
    public GameObject bloodPool;
    public GameObject punchDustVFX;
    public GameObject punchSparksVFX;
    public GameObject bloodSparksVFX;
    public GameObject punchForceVFX2; 
    public GameObject axeDustVFX; 
    public GameObject punchForceVFX; 
    public GameObject parryLightningVFX;
    public GameObject limbBloodVFX; 
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
                Vector3 backPoint = transform.position + transform.forward * backPointDistance;
                backPointCheck.transform.position = backPoint;
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
                            if (enemy.gameObject.GetComponent<BasicEnemyScript>() != null)
                            {
                                BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
                                if (!enemyScript.isDead)
                                {
                                    playerController.canBlockStun = false;

                                    enemyScript.TakeDamage(enemyController.chainHitScript.blockChainDamage, playerAttackType.BlockStun.ToString());
                                    enemyScript.isLaunched = true;
                                    enemyScript.isBlockStunned = true;

                                    //Feedback
                                    GameObject lightningVFX = Instantiate(parryLightningVFX, enemy.transform.position - new Vector3(0, 0, 0), transform.rotation);
                                    //  lightningVFX.transform.eulerAngles = new Vector3(-90, lightningVFX.transform.eulerAngles.y, lightningVFX.transform.eulerAngles.z);  
                                    lightningVFX.AddComponent<CleanUpScript>();
                                    parriedFeedback?.PlayFeedbacks();
                                    playerController.mainGameManager.DoHaptics(.2f, .4f, .7f);
                                    enemy.transform.LookAt(playerController.transform);
                                }

                            }
                        }

                    }
                    playerController.playerAnim.SetTrigger("HasParriedTrigger");
                    playerController.playerAnim.SetInteger("BlockType", 1); 
                    playerController.attackTargetScript.playerBlockFeedback?.PlayFeedbacks();
                    playerController.hasParriedAttack = true; 
              
                }
            }
           // else targetScript.TakeDamage(enemyController.currentAttackDamage, enemyController.enemyAttackType); //Damage the player
        }
    }

    public void BackCheck()
    {
        foreach (GameObject enemy in targetsInRange)
        {
            if (enemy.gameObject.GetComponent<BasicEnemyScript>() != null)
            {
                BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
                Vector3 backDirection = playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * attackBackForce;
                enemyScript.transform.DOMove(backDirection, .3f).SetUpdate(UpdateType.Fixed);  //Move enemy backwards enemyScript.enemyRb.AddForce(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce, ForceMode.VelocityChange);  
            }
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
                if (obj.GetComponent<BasicEnemyScript>() != null)
                {
                    BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>();
                    if (!enemyScript.isDead)
                    {
                        if (playerController.attackState == playerAttackType.AirLaunchAttack.ToString())
                        {
                            enemyScript.TakeDamage(0, playerController.attackState);
                            if (enemyScript.canBeStunned) hitpauseScript.objectsToPause.Add(obj.GetComponent<Animator>());

                        }
                        if (playerController.attackState == playerAttackType.HeavyAxeHit.ToString())
                        {
                            enemyScript.TakeDamage(playerController.currentAttackDamage, playerController.attackState);
                            hitpauseScript.objectsToPause.Add(obj.GetComponent<Animator>());
                        }
                        else if (playerController.attackState == playerAttackType.LightPunchHit.ToString())
                        {
                            enemyScript.TakeDamage(playerController.currentAttackDamage, playerController.attackState);
                            hitpauseScript.objectsToPause.Add(obj.GetComponent<Animator>());
                        }
                        else if (playerController.attackState == playerAttackType.SprintAttack.ToString() && !playerController.sprintHitList.Contains(enemyScript))
                        {
                            enemyScript.TakeDamage(playerController.currentAttackDamage, playerController.attackState);
                            if (enemyScript.canBeStunned) sprintAttackTargets.Add(enemyScript);
                        }

                    }

                    Vector3 backDirection = new Vector3(0, 0, 0);

                    enemyScript.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    if (!playerController.isGrounded || playerController.attackState == playerAttackType.AirLaunchAttack.ToString()) backDirection = playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * attackBackForce;
                    else if (playerController.isGrounded) backDirection = enemyScript.transform.position + enemyScript.transform.forward * (playerController.currentAttackForwardForce * .8f) * attackBackForce;

                    enemyScript.transform.LookAt(playerController.transform);

                    if (enemyScript.isDead)
                    {
                        KillEnemy(enemyScript); //The enemy is dead
                                                //   enemyScript.canBeTargeted = false;
                    }
                    else AddhitFeedback(enemyScript, backDirection); //The enemy is not dead


                    //   if (enemyScript.isRagdolling ||  enemyScript.isDead) enemyScript.enemyRb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
                    limbCheckerScript.hitInsides.Clear();
                }
                else if(obj.GetComponent<EyeBall>() != null)
                {
                    print("Damage the eyeball");
                    EyeBall eyeballScript = obj.GetComponent<EyeBall>();
                    eyeballScript.TakeDamage(playerController.currentAttackDamage, playerController.attackState);

                    hitpauseScript.hitPauseDuration = playerController.axeHitPauseLength;
                    hitpauseScript.doHitPause = true;
                    hitpauseScript.DoHitPause();
                    // GameObject sparks = Instantiate(bloodSparksVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                    //  sparks.transform.parent = enemyScript.transform;
                    playerController.mainGameManager.DoHaptics(.35f, 1f, 1f);
                    weapinFirstImpactFeedback?.PlayFeedbacks();

                    DOTween.Kill(playerController.transform);
                    

                }
            }
        }     

        if (isEnemy)
        {

            foreach (GameObject obj in targetsInRange)
            {
                PlayerState targetScript = obj.GetComponent<PlayerState>();
                targetScript.TakeDamage(enemyController.currentAttackDamage, enemyController.enemyAttackType); //Damage the player
            }

            sprintAttackTargets.Clear();
        }

    }

    public void KillEnemy(BasicEnemyScript enemyScript)
    {
        if (playerController.attackState == playerAttackType.HeavyAxeHit.ToString()) //Axe kill
        {

            //Collect and detach limbs
            // limbCheckerScript.LimbCheck(); 

            foreach (Rigidbody limb in limbCheckerScript.hitLimbs)
            {

                if (enemyScript.ragdollRbs.Contains(limb))
                {
                    if (limb.GetComponent<CharacterJoint>() != null)
                    {
                        //Add blood 
                        CharacterJoint joint = limb.GetComponent<CharacterJoint>();
                        GameObject blood2 = Instantiate(limbBloodVFX, joint.transform.position, joint.transform.rotation);
                        GameObject blood3 = Instantiate(bloodDrip, joint.transform.position, joint.transform.rotation);
                        GameObject blood4 = Instantiate(limbBloodVFX, joint.transform.position, joint.transform.rotation);
                        GameObject axeDust = Instantiate(axeDustVFX, joint.transform.position, joint.transform.rotation);
                        blood4.transform.parent = enemyScript.transform;
                        blood2.transform.parent = blood3.transform.parent = axeDust.transform.parent = limb.transform;
                        blood2.AddComponent<CleanUpScript>();
                        axeDust.AddComponent<CleanUpScript>();
                        

                        //Detach and launch limb rb's 
                        Destroy(joint);
                        limb.transform.parent = null;
                        limb.velocity = new Vector3(0, 0, 0);
                        limb.AddExplosionForce(10f, limb.position, 2f, 4f, ForceMode.Impulse);
                        limb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
                        if (enemyScript.canBeTargeted) limb.mass *= 3f;
                    }
                }
            }


            foreach (GameObject enemyInside in limbCheckerScript.hitInsides) //Enable enemy insides
            {
                if (enemyInside.GetComponent<MeshRenderer>() != null) enemyInside.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        else if (playerController.attackState == playerAttackType.LightPunchHit.ToString() && enemyScript.canBeTargeted) //Kill someone with punch
        {
            enemyScript.transform.LookAt(playerController.transform);
            playerController.mainGameManager.DoHaptics(.15f, 1f, 1f);
            punchKillList.Add(enemyScript);
            punchKillFeedback?.PlayFeedbacks();


            //Spawn punch force effect 2
            GameObject dashAttackEffect = Instantiate(playerController.airPunchEffect, enemyScript.transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
            dashAttackEffect.transform.LookAt(playerController.transform.forward + new Vector3(0, .5f, 0));
            dashAttackEffect.transform.eulerAngles = new Vector3(dashAttackEffect.transform.eulerAngles.x - 180, dashAttackEffect.transform.eulerAngles.y, dashAttackEffect.transform.eulerAngles.z);
            dashAttackEffect.transform.localScale *= .45f;

            //Punch explosion force effect
            GameObject punchForceEffect = Instantiate(punchForceVFX2, enemyScript.transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
            punchForceEffect.transform.LookAt(playerController.transform.forward + new Vector3(0, .5f, 0));
            punchForceEffect.transform.localScale *= .55f;

            //Set effect postion and rotation 
            dashAttackEffect.transform.eulerAngles = punchForceEffect.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);


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
                //swordrb.velocity = new Vector3(0, 0, 0);
                swordrb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
            }

            enemyScript.canBeTargeted = false;

            if (playerController.attackState == playerAttackType.HeavyAxeHit.ToString())
            {
                playerController.mainGameManager.DoHaptics(.35f, 1f, 1f);
                weaponKillFeedback?.PlayFeedbacks();
            }
            else if (playerController.attackState == playerAttackType.LightPunchHit.ToString())
            {
                //  playerController.mainGameManager.DoHaptics(.2f, .55f, .75f);
                // punchKillFeedback?.PlayFeedbacks();
            }
        }
    }

    private void AddhitFeedback(BasicEnemyScript enemyScript, Vector3 backDirection)
    {

        if (playerController.attackState == playerAttackType.AirLaunchAttack.ToString())
        {
            GameObject dust = Instantiate(punchDustVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
            GameObject sparks = Instantiate(punchSparksVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
            dust.transform.parent = enemyScript.transform;
            sparks.transform.parent = enemyScript.transform;
            dust.AddComponent<CleanUpScript>();
            airLaunchHitFeedback?.PlayFeedbacks(); 
            playerController.mainGameManager.DoHaptics(.2f, .5f, .5f);
            airLaunchTargets.Add(enemyScript);
            enemyScript.canStopFall = true;
            enemyScript.isLaunched = true;
           
            // enemyScript.transform.DOMove(transform.position + transform.forward * .5f, .1f).SetUpdate(UpdateType.Fixed);
            Vector3 airLaunchPoint = enemyScript.transform.position + Vector3.up * (playerController.airLaunchHeight - 1f);
            DOTween.Kill(enemyScript.transform);
            enemyScript.moveSequence.Append(enemyScript.transform.DOMove(airLaunchPoint, .3f).SetUpdate(UpdateType.Fixed));
            enemyScript.transform.LookAt(playerController.transform);


            //   enemyScript.transform.parent = playerController.transform;
        }
        else if (playerController.attackState == playerAttackType.HeavyAxeHit.ToString()) //Axe hit feedback 
        {
            hitpauseScript.hitPauseDuration = playerController.axeHitPauseLength;
            hitpauseScript.doHitPause = true;
            hitpauseScript.DoHitPause();
           // GameObject sparks = Instantiate(bloodSparksVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
          //  sparks.transform.parent = enemyScript.transform;
            playerController.mainGameManager.DoHaptics(.35f, 1f, 1f);
            weapinFirstImpactFeedback?.PlayFeedbacks();        
            enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);

            //Move enemy backwards
            if (playerController.isGrounded && !enemyScript.CompareTag("BigEnemy")) enemyScript.moveSequence.Append(enemyScript.transform.DOMove(backDirection, .3f).SetUpdate(UpdateType.Fixed));  //Move enemy backwards 
            else if(!enemyScript.CompareTag("BigEnemy"))
            {
                enemyScript.transform.DOMove(backDirection + new Vector3(0, 1f, 0), .3f).SetUpdate(UpdateType.Fixed);
                enemyScript.canStopFall = true;
            }    
            else if(enemyScript.CompareTag("BigEnemy")) DOTween.Kill(playerController.transform); 
                            

        }

        else if (playerController.attackState == playerAttackType.LightPunchHit.ToString()) //Punch hit feedback
        {
            hitpauseScript.hitPauseDuration = playerController.punchPauseLength;
            hitpauseScript.doHitPause = true;
            hitpauseScript.DoHitPause();
            punchFirstImpactFeedback?.PlayFeedbacks();
            playerController.mainGameManager.DoHaptics(.25f, .8f, .8f);
            enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);

            //Move enemy backwards
            if (playerController.isGrounded && !enemyScript.CompareTag("BigEnemy")) enemyScript.moveSequence.Append(enemyScript.transform.DOMove(backDirection, .3f).SetUpdate(UpdateType.Fixed));  //Move enemy backwards 
            else if(!enemyScript.CompareTag("BigEnemy"))
            {
                enemyScript.transform.DOMove(backDirection + new Vector3(0, 1f, 0), .3f).SetUpdate(UpdateType.Fixed);
                enemyScript.canStopFall = true; 
            }
            else if(enemyScript.CompareTag("BigEnemy")) DOTween.Kill(playerController.transform); 
            

      
        }

        //Sprintattack hit feedback
        else if (playerController.attackState == playerAttackType.SprintAttack.ToString() && !playerController.sprintHitList.Contains(enemyScript) && enemyScript.canBeStunned)
        {
            sprintTargetCount++;
            playerController.meshR.materials = playerController.defaultSkinMat; 
            playerController.sprintHitList.Add(enemyScript);
            playerController.mainGameManager.DoHaptics(.2f, .7f, .7f);
            playerController.dashAttackTarget = enemyScript.transform.gameObject;
            enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);
            enemyScript.transform.parent = playerController.transform;
            enemyScript.transform.position = playerController.transform.position;
            transform.DOMove(transform.position, .001f);
            DOTween.Kill(playerController.transform);

            if (sprintTargetCount == 1)
            {
                
                sprintAttackHitFeedback?.PlayFeedbacks();
            }

        }
    }

    public void PunchKillFeedback()
    {
        foreach (BasicEnemyScript enemyScript in punchKillList)
        {
            enemyScript.LaunchEnemy(playerController.frontWallChecker.transform.forward, sprintAttackBackForce * .6f, 5f);

            foreach (Rigidbody rb in enemyScript.ragdollRbs)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }

        punchKillList.Clear();
    }
  
    public void SprintAttackImpact()
    {
        //sprintAttackTarget.transform.position = backPointCheck.transform.position; 

        foreach (BasicEnemyScript enemy in sprintAttackTargets)
        {
            enemy.transform.parent = null;
            enemy.transform.position = backPointCheck.transform.position;
            GameObject dashAttackEffect = Instantiate(playerController.airPunchEffect, enemy.transform.position, transform.rotation);
            dashAttackEffect.transform.LookAt(-playerController.aimPoint.forward + new Vector3(0, .5f, 0));
            dashAttackEffect.AddComponent<CleanUpScript>(); 
            //dashAttackEffect.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            
            //Punch explosion force effect
            GameObject punchForceEffect = Instantiate(punchForceVFX2, enemy.transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
            punchForceEffect.transform.LookAt(playerController.transform.forward + new Vector3(0, .5f, 0));
            punchForceEffect.transform.localScale *= .55f;
            punchForceEffect.AddComponent<CleanUpScript>(); 

            //Set effect postion and rotation 
            dashAttackEffect.transform.eulerAngles = punchForceEffect.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            enemy.LaunchEnemy(transform.forward, sprintAttackBackForce, 0f);
        }

        sprintAttackTargets.Clear();
        sprintTargetCount = 0;
        playerController.mainGameManager.DoHaptics(.2f, .4f, .6f);
    }

    private void OnDrawGizmosSelected()
    {
        if(isPlayer){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerController.transform.position,playerController.blockStunRadius);
        }
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
 
}
