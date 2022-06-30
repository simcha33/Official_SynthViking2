using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class HomingProjectile : MonoBehaviour
{
    [Header("STATS")]
    public float projectileDamage;
    public float parriedProjectileDamage; 
    public float aliveDuration;
    public float moveSpeed;
    public float rotateSpeed;
    public bool isParried;
    public float waitForReverseTimer;
    public float waitForReverseDuration;
    public Transform origin;

    private float aliveTimer;
    [HideInInspector] public bool hasTouchedTarget;
    private Rigidbody rb; 
    public Transform target;

    [Header("VISUALS")]
    public GameObject destructionEffect;
    public MeshRenderer projectMeshr;
    public Material enemyProjectileMat;
    public Material playerProjectileMat; 

    [Header("COMPONENTS")]
    private ThirdPerson_PlayerControler playerController;
    public MMFeedbacks projectileHitFeedback;
    public MMFeedbacks parryReleaseFeedback;
    public MMFeedbacks parryHitFeedback; 
    public Collider projectileAvoid;
    public GameObject parryEffect;
    private HomingProjectile thisScript;
    public ComboManager _styleManager; 

    void Start()
    {
        SetTarget(); 
        aliveTimer = 0f; 
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>();
        thisScript = gameObject.GetComponent<HomingProjectile>();
        _styleManager = playerController._styleManager; 
        projectMeshr.material = enemyProjectileMat; 
    }

    void Update()
    {
        //  ChaseTarget(); 
       
   
   
      
    }

     void FixedUpdate() 
     {
        if(!isParried) ChaseTarget();

        else if (isParried)
        {
            waitForReverseTimer += Time.fixedDeltaTime;
            Transform axeBladePoint = playerController.attackTargetScript.limbCheckerScript.axeBlade;
            transform.position = axeBladePoint.position;

            if (waitForReverseTimer >= waitForReverseDuration)
            {
                ReverseProjectile();

            }

        }
    }

    public void ReverseProjectile()
    {
        target = origin;
        moveSpeed *= 6f;
        isParried = false;
        rb.velocity = new Vector3(0, 0, 0);
       // transform.parent = null;
        rb.transform.LookAt(target);
        playerController.playerAnim.SetBool("IsProjectileParry", false);
        playerController.playerAnim.SetInteger("BlockType", 0);
        waitForReverseTimer = 0;
        playerController.transform.LookAt(origin.position);
        gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        parryReleaseFeedback?.PlayFeedbacks(); 
    }

    void SetTarget()
    {
        target = GameObject.Find("Player").transform; 
        
    }

    void ChaseTarget()
    {
        Vector3 targetPos;
        if(target != origin) targetPos = target.transform.position + new Vector3(0, 1.5f, 0);
        else
        {
            targetPos = target.transform.position + new Vector3(0, 2f, 0);
        }
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();    
        rb.angularVelocity = -Vector3.Cross(dir, transform.forward) * rotateSpeed;
        rb.velocity = transform.forward * moveSpeed;

    }



    void OnTriggerEnter(Collider other)
    {
        //
        if (target == origin)
        {
            print("here");
            if (other.transform == target)
            {

                print("DamgeThatObject");
                target.GetComponent<EyeBall>().TakeDamage(parriedProjectileDamage, "Projectile"); 
                //DestroyProjectile();
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
            {
                other.gameObject.GetComponent<HomingProjectile>().DestroyProjectile();
                _styleManager.AddStyle("ProjectileParry"); 
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(other.gameObject.GetComponent<BasicEnemyScript>() != null)
                {
                    other.gameObject.GetComponent<BasicEnemyScript>().TakeDamage(100f, "Projectile"); 
                }
            }
        }
   
           

        else if (target != origin)
        {
            //Projectile hits player
            if (other.gameObject.CompareTag("Player") && !isParried)
            {
                //Player is blocking
                if (playerController.isBlocking)
                {
                    isParried = true;
                    Transform axeBladePoint = playerController.attackTargetScript.limbCheckerScript.axeBlade;
                    rb.velocity = new Vector3(0, 0, 0);
                    transform.position = axeBladePoint.position; //Set projectile to axe blade pos
                    //transform.parent = axeBladePoint.parent;
                    playerController.playerAnim.SetInteger("BlockType", 2);
                    playerController.playerAnim.SetBool("IsProjectileParry", true);                    
                    playerController.playerAnim.SetTrigger("HasParriedTrigger");
                    playerController.attackTargetScript.playerBlockFeedback?.PlayFeedbacks();
                    playerController.hasParriedAttack = true;
                    projectileAvoid.gameObject.SetActive(false);
                    projectMeshr.material = playerProjectileMat;
                    GameObject effect = Instantiate(parryEffect, transform.position - new Vector3(0, 0, 0), transform.rotation);
                    effect.transform.parent = this.transform;
                    effect.AddComponent<CleanUpScript>();
                    parryHitFeedback?.PlayFeedbacks();

                    //Stun enemies
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
                                //    AttackTargetScript attackScript = 
                                if (!enemyScript.isDead)
                                {
                                    playerController.canBlockStun = false;

                                    enemyScript.TakeDamage(enemyScript.chainHitScript.blockChainDamage, playerAttackType.BlockStun.ToString());
                                    enemyScript.isLaunched = true;
                                    enemyScript.isBlockStunned = true;

                                    //Feedback
                                    GameObject lightningVFX = Instantiate(enemyScript.attackTargetScript.parryLightningVFX, enemy.transform.position - new Vector3(0, 0, 0), transform.rotation);
                                    //  lightningVFX.transform.eulerAngles = new Vector3(-90, lightningVFX.transform.eulerAngles.y, lightningVFX.transform.eulerAngles.z);  
                                    lightningVFX.AddComponent<CleanUpScript>();
                                    enemyScript.attackTargetScript.parriedFeedback?.PlayFeedbacks();
                                    playerController.mainGameManager.DoHaptics(.2f, .4f, .7f);
                                    enemy.transform.LookAt(playerController.transform);
                                }
                            }

                        }

                        playerController.hasParriedAttack = true;
                        playerController.canBlockStun = true; 


                    }
                }

                //player take damage
                else if (playerController.playerState.canBeHit)
                {
                    playerController.playerState.TakeDamage(projectileDamage, "Projectile");
                    hasTouchedTarget = true;
                    projectileHitFeedback?.PlayFeedbacks();
                    DestroyProjectile();
                }
            }

            //Projectile hits environment
            if (other.gameObject.layer == LayerMask.NameToLayer("Environment") && !isParried)
            {
                DestroyProjectile();
            }

            /*
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.gameObject.GetComponent<BasicEnemyScript>().TakeDamage(projectileDamage, "Projectile");
            }
            */
        }
       
    }

    void Damagetarget()
    {

    }

    

    public void DestroyProjectile()
    {
        //Play some kind of an effect
        GameObject destroyEffect = Instantiate(destructionEffect, transform.position, transform.rotation);
        projectileHitFeedback?.PlayFeedbacks();
        destroyEffect.AddComponent<CleanUpScript>();

        //Destroy projectile
        if (origin != null)
        {
            EyeBall originScript = origin.GetComponent<EyeBall>();
            if (originScript.hasSpawned)
            {
                originScript.projectilesFired.Remove(thisScript);
               
            }

            Destroy(gameObject);
        }
    }

   
}
