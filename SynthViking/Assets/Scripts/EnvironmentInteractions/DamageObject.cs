using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class DamageObject : MonoBehaviour
{

    public float damageAmount;

    public bool isLava;
    public bool isHammer;
    public bool isLaser;
    public bool isMine; 
    public bool canHurtEnemies;

    [Header ("Components")]
    public GameObject mineExplosionEffect; 
    public GameObject lavaDeathVisual;

    [Header("Components")]
    public MMFeedbacks explosionFeedback;


    public enum trapType
    {
        lava,
        spike,
    }

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy") && canHurtEnemies)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                PlayerState playerStateScript = other.gameObject.GetComponent<PlayerState>(); 
                ThirdPerson_PlayerControler playerController = playerStateScript.playerController;  
               // playerController.ResetStates(); 
                if(playerController.isAirSmashing)playerController.EndAirSmash();
                if (isLava) playerStateScript.TakeDamage(damageAmount, "LavaDamage");
                else if (isLaser) playerStateScript.TakeDamage(damageAmount, "LaserDamage");
                else if (isMine)
                {
                    playerStateScript.TakeDamage(damageAmount, "LaserDamage");
                    GameObject effect = Instantiate(mineExplosionEffect, transform.position, transform.rotation);
                    effect.AddComponent<CleanUpScript>();
                    explosionFeedback?.PlayFeedbacks(); 
                    Destroy(this.gameObject);  
                }
            }

            else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                BasicEnemyScript enemyScript = other.gameObject.GetComponent<BasicEnemyScript>(); 
                enemyScript.TakeDamage(enemyScript.maxHealth, "EnvironmentDamage");
                // DoType(other.transform); 
                DoType(enemyScript); 
            }
        }
    }

    void DamageTarget(Transform target)
    {
        
    }

    void DoType(BasicEnemyScript target)
    {
        if (isLava)
        {
         //   GameObject deathEffect = Instantiate(lavaDeathVisual, target.transform.position + new Vector3(0,3,0), transform.rotation);
          //  deathEffect.AddComponent<CleanUpScript>().SetCleanUp(4f); 
        }

        if (isHammer)
        {
            target.LaunchEnemy(transform.forward, 25f, 6f); 
        }
    }


}
