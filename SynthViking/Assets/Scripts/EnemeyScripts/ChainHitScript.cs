using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHitScript : MonoBehaviour
{

    public BasicEnemyScript mainScript;
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
                else otherScript.chainHitScript.SetChainHitType(chainType);

                

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
                }
                      
                           
               // otherScript.enemyRb.AddForce(launchDirection * currentChainHitBackForce, ForceMode.Impulse);
               otherScript.enemyMeshr.materials = otherScript.stunnedSkinMat;
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

        if(stunType == playerAttackType.LightAxeHit.ToString()) //Light axe hit 
        {

            currentChainHitActiveDuration = axeHitChainActiveDuration; 
            currentChainHitBackForce = axeHitForce;
            currentDamage = axeHitChainDamage; 
            stunType = playerAttackType.LightAxeHit.ToString();
            
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

        if(chainType == playerAttackType.LightAxeHit.ToString())
        {
            launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
        }

        if (chainType == chainHitString)
        {
            launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
        }

        
    }
}
