using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{

    public float damageAmount;
    public GameObject lavaDeathVisual;
    public bool isLava;
    public bool isHammer;
    public bool isLaser; 
    public bool canHurtEnemies; 
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
                if(isLava) playerStateScript.TakeDamage(damageAmount, "LavaDamage");
                else if(isLaser) playerStateScript.TakeDamage(damageAmount, "LaserDamage");
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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
