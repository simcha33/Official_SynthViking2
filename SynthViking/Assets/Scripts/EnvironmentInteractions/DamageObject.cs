using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{

    public float damageAmount;
    public GameObject lavaDeathVisual;
    public bool isLava; 
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
        if(other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(other.gameObject.CompareTag("Player"))
            {
                PlayerState playerStateScript = other.gameObject.GetComponent<PlayerState>(); 
                ThirdPerson_PlayerControler playerController = other.gameObject.GetComponent<ThirdPerson_PlayerControler>(); 
                playerStateScript.TakeDamage(damageAmount, "EnvironmentDamage"); 
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                BasicEnemyScript enemyScript = other.gameObject.GetComponent<BasicEnemyScript>(); 
                enemyScript.TakeDamage(enemyScript.maxHealth, "EnvironmentDamage");
               // DoType(other.transform); 
            }
        }
    }

    void DamageTarget(Transform target)
    {
        
    }

    void DoType(Transform target)
    {
        if (isLava)
        {
         //   GameObject deathEffect = Instantiate(lavaDeathVisual, target.transform.position + new Vector3(0,3,0), transform.rotation);
          //  deathEffect.AddComponent<CleanUpScript>().SetCleanUp(4f); 
        }
    }
}
