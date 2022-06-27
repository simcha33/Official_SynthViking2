using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHitScript : MonoBehaviour
{

    public BasicEnemyScript mainScript;
    public ThirdPerson_PlayerControler playerController; 
    public bool isOrigin;
    private bool canCheck = false;
    public float chainHitMaxDistance = 20f;
    public float currentStunDuration; 



    public Vector3 launchDirection; 

    [Header ("AXE CHAIN HIT")]
    public float axeHitForce;
    public float axeHitChainActiveDuration;
    public float axeHitChainDamage = 0;
    public float axeHitStunDuration;
    
    [Header ("PUNCH CHAIN HIT")]
    public float punchHitForce;
    public float punchHitChainActiveDuration;
    public float punchHitChainDamage = 0;
    public float punchHitStunDuration;

    [Header("BLOCK CHAIN HIT")]
    public float blockHitForce; 
    public float blockActiveDuration;
    public float blockChainDamage = 0;
    public float blockStunDuration;


    [Header("POWER PUNCH CHAIN HIT")]
    //Forces    
    public float powerPunchForce;
    public float powerPunchChainActiveDuration;
    public float powerPunchChainDamage = 30f;
    public float powerPunchStunDuration;  


    [Header("DEFAULT CHAIN HIT")]
    public float chainHitForce;
    public float chainHitActiveDuration;
    private float chainHitActiveTimer;
    public float chainHitDamage = 5f;
    public float chainHitStunDuration; 
    [HideInInspector] public string chainHitString;

    //Current values
    private float currentDamage;
    private float currentChainHitBackForce;
    [HideInInspector] public float currentChainHitActiveDuration;
    private string chainType; 


    private void Start()
    {
        chainHitString = "ChainStunHIt";
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        chainHitActiveTimer = 0f; 
    }

    private void Update()
    {

        chainHitActiveTimer += Time.deltaTime; 

        if(chainHitActiveTimer >= chainHitActiveDuration)
        {
            if (!mainScript.isDead)
            {
                GetComponent<ChainHitScript>().enabled = false;
                chainHitActiveTimer = 0f;
                isOrigin = false;
                canCheck = false; 
            }
            else if(mainScript.isDead) Destroy(this.gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && mainScript.targetDistance < chainHitMaxDistance && !mainScript.isBlockStunned && (mainScript.isStunned || mainScript.isDead) && canCheck)
        {
            //            Debug.Log("DetectTheEnemy"); 
            if (other.gameObject.GetComponent<BasicEnemyScript>() != null)
            {
                BasicEnemyScript otherScript = other.gameObject.GetComponent<BasicEnemyScript>();

                if (otherScript.canBeChainHit && !otherScript.isDead)
                {
                    otherScript.enemyRb.velocity = new Vector3(0f, 0f, 0f);
                    otherScript.enemyAnim.speed = Random.Range(1, .8f);
                    otherScript.enemyAnim.SetFloat("DamageReaction", Random.Range(1f, 3f));
                    otherScript.enemyAnim.SetTrigger("DamageTrigger");


                    //Set type values equel to orgins type
                    if (isOrigin)
                    {
                        otherScript.chainHitScript.currentChainHitActiveDuration = currentChainHitActiveDuration;
                        otherScript.chainHitScript.currentDamage = currentDamage;
                        otherScript.chainHitScript.currentChainHitBackForce = currentChainHitBackForce;


                    }
                    else
                    {
                        otherScript.chainHitScript.SetChainHitType(chainType);
                    }



                    otherScript.TakeDamage(currentDamage, chainType);
                    if (chainType == playerAttackType.PowerPunch.ToString()) //Power punch 
                    {
                        otherScript.enemyRb.mass = otherScript.originalRbMass;
                        otherScript.LaunchEnemy(launchDirection, currentChainHitBackForce, 10f);


                    }
                    else
                    {
                        otherScript.enemyRb.freezeRotation = true;

                        //Set launch direction 
                        otherScript.enemyRb.velocity = new Vector3(0f, 0f, 0f);
                        otherScript.launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
                        otherScript.enemyRb.AddForce(mainScript.launchDirection * currentChainHitBackForce, ForceMode.Impulse);

                        //Spawn punch force effect 2
                        GameObject dashAttackEffect = Instantiate(playerController.airPunchEffect, otherScript.transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
                        dashAttackEffect.transform.LookAt(otherScript.transform.forward + new Vector3(0, .5f, 0));
                        dashAttackEffect.transform.eulerAngles = new Vector3(dashAttackEffect.transform.eulerAngles.x - 180, dashAttackEffect.transform.eulerAngles.y, dashAttackEffect.transform.eulerAngles.z);
                        dashAttackEffect.transform.localScale *= .45f;
                        dashAttackEffect.AddComponent<CleanUpScript>(); 

                    }


                    //   otherScript.enemyRb.AddForce(launchDirection * currentChainHitBackForce, ForceMode.Impulse);
                    otherScript.enemyMeshr.materials = otherScript.stunnedSkinMat;
                    otherScript.stunDuration = currentStunDuration;
                    otherScript.mainCollider.isTrigger = true;
                    if(otherScript.stunnedEffect != null) otherScript.stunnedEffect.SetActive(true); 
                }
            }
        }
    }

    public void SetChainHitType(string stunType)
    {
        //isOrigin = true;     
        
        if(stunType == playerAttackType.PowerPunch.ToString()) //Power punch 
        {
            currentChainHitActiveDuration = powerPunchChainActiveDuration;
            currentChainHitBackForce = powerPunchForce;
            currentDamage = powerPunchChainDamage;
            stunType = playerAttackType.PowerPunch.ToString();
        }

          if(stunType == playerAttackType.SprintAttack.ToString()) //Power punch 
        {
            currentChainHitActiveDuration = powerPunchChainActiveDuration / 2f;
            currentChainHitBackForce = powerPunchForce / 2;
            currentDamage = powerPunchChainDamage / 2;
            stunType = playerAttackType.PowerPunch.ToString();
        }

        if(stunType == playerAttackType.HeavyAxeHit.ToString()) //Light axe hit 
        {

            currentChainHitActiveDuration = axeHitChainActiveDuration; 
            currentChainHitBackForce = axeHitForce;
            currentDamage = axeHitChainDamage; 
            stunType = playerAttackType.HeavyAxeHit.ToString();
            currentStunDuration = axeHitStunDuration; 
            
        }

        
        if(stunType == playerAttackType.LightPunchHit.ToString()) //Light axe hit 
        {
            
            currentChainHitActiveDuration = punchHitChainActiveDuration; 
            currentChainHitBackForce = punchHitForce;
            currentDamage = punchHitChainDamage; 
            stunType = playerAttackType.LightPunchHit.ToString();
            currentStunDuration = punchHitStunDuration; 
            
        }

        if (stunType == playerAttackType.BlockStun.ToString()) //Block stun 
        {
            currentChainHitActiveDuration = blockActiveDuration;
            currentChainHitBackForce = blockHitForce;
            currentDamage = blockChainDamage;
            currentStunDuration = blockStunDuration; 
            stunType = playerAttackType.BlockStun.ToString();
        }

       
        if (stunType == chainHitString) //Normale chain stun 
        {     
            currentChainHitActiveDuration = chainHitActiveDuration;
            currentChainHitBackForce = chainHitForce;
            currentDamage = chainHitDamage;
            currentStunDuration = chainHitStunDuration; 
            stunType = chainHitString;
            
        }
        

        chainType = stunType;
        canCheck = true; 

       
    }

    public void CheckForLaunchDirection(Transform other, BasicEnemyScript otherScript)
    {


        if(chainType == playerAttackType.PowerPunch.ToString())
        {
            float leftSide = Vector3.Distance(transform.position - transform.right * .5f, other.position); //Look for closest side
            float rightSide = Vector3.Distance(transform.position - transform.right * .5f, other.position); 

            if(leftSide > rightSide) launchDirection = transform.right; //Launch enemy to the left
            else launchDirection = -transform.right; //Launch enemy to the right 
        }

        if(chainType == playerAttackType.HeavyAxeHit.ToString())
        {
            launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
        }

          if(chainType == playerAttackType.LightPunchHit.ToString())
        {
            launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
        }

        if (chainType == chainHitString)
        {
            launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
        }

        
    }
}
