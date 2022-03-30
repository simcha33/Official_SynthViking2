using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHitScript : MonoBehaviour
{

    public BasicEnemyScript mainScript;
    public bool isOrigin;
    public float chainHitMaxDistance = 20f;
    public float chainHitStunDuration;

    [Header ("AXE CHAIN HIT")]
    public float axeHitForce;
    public float axeHitChainActiveDuration;
    public float axeHitChainDamage = 0;

    [Header("POWER PUNCH CHAIN HIT")]
    //Forces    
    public float powerPunchForce;
    public float powerPunchChainActiveDuration;
    public float powerPunchChainDamage = 30f;

    [Header("DEFAULT CHAIN HIT")]
    public float chainHitForce;
    public float chainHitActiveDuration;
    private float chainHitActiveTimer;
    public float chainHitDamage = 5f;
    [HideInInspector] public string chainHitString;

    //Current values
    private float currentDamage;
    private float currentChainHitBackForce;
    [HideInInspector] public float currentChainHitActiveDuration;
    private string stunType;


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
            }
            else Destroy(this.gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && mainScript.targetDistance < chainHitMaxDistance && (mainScript.isStunned || mainScript.isDead))
        {
            Debug.Log("DetectTheEnemy"); 
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
                else SetChainHitType(chainHitString); 
                
                otherScript.TakeDamage(currentDamage, chainHitString);
                otherScript.enemyRb.freezeRotation = true;
                otherScript.launchDirection = mainScript.launchDirection; //Base launch direction of attackers forward 
                otherScript.enemyRb.AddForce(mainScript.launchDirection * currentChainHitBackForce, ForceMode.Impulse);
                otherScript.enemyMeshr.materials = otherScript.stunnedSkinMat;
            }
        }
    }

    public void SetChainHitType(string stunType)
    {
        isOrigin = true; 

        if(stunType == playerAttackType.PowerPunch.ToString())
        {
            currentChainHitActiveDuration = powerPunchChainActiveDuration;
            currentChainHitBackForce = powerPunchForce;
            currentDamage = powerPunchChainDamage; 
        }

        if(stunType == playerAttackType.LightAxeHit.ToString())
        {
            currentChainHitActiveDuration = axeHitChainActiveDuration; 
            currentChainHitBackForce = axeHitForce;
            currentDamage = axeHitChainDamage; 
        }

        if (stunType == chainHitString)
        {
            currentChainHitActiveDuration = chainHitActiveDuration;
            currentChainHitBackForce = chainHitForce;
            currentDamage = chainHitDamage; 
        }
    }
}
