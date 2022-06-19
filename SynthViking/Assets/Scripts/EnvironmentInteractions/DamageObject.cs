using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class DamageObject : MonoBehaviour
{
    public ThirdPerson_PlayerControler player; 

    public float damageAmount;

    public bool isLava;
    public bool isHammer;
    public bool isLaser;
    public bool isMine; 
    public bool canHurtEnemies;
    public bool isSawBalde; 

    [Header ("Components")]
    public GameObject mineExplosionEffect; 
    public GameObject lavaDeathVisual;

    [Header("Components")]
    public MMFeedbacks explosionFeedback;

    public List<GameObject> sawBlades = new List<GameObject>(); 


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

        if (isSawBalde)
        {
            ExplodeTarget(target); 
        }
    }

    void ExplodeTarget(BasicEnemyScript enemyScript)
    {
        foreach(Rigidbody limb in enemyScript.ragdollRbs)
        {


            //Add blood 
            if (limb.GetComponent<CharacterJoint>() != null)
            {
                CharacterJoint joint = limb.GetComponent<CharacterJoint>();
                GameObject blood2 = Instantiate(player.attackTargetScript.limbBloodVFX, joint.transform.position, joint.transform.rotation);
                GameObject blood3 = Instantiate(player.attackTargetScript.bloodDrip, joint.transform.position, joint.transform.rotation);
                GameObject blood4 = Instantiate(player.attackTargetScript.limbBloodVFX, joint.transform.position, joint.transform.rotation);
                GameObject axeDust = Instantiate(player.attackTargetScript.axeDustVFX, joint.transform.position, joint.transform.rotation);
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


}
