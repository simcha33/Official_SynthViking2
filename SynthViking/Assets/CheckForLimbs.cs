using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForLimbs : MonoBehaviour
{
    public AttackTargetScript attackScript; 
    public ThirdPerson_PlayerControler playerController; 

    public List <Rigidbody> hitLimbs = new List<Rigidbody>(); 

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!playerController.isAttacking){
            foreach (Rigidbody limbRb in hitLimbs)
            {
                hitLimbs.Remove(limbRb); 
            }
        }
        */

        if(!playerController.isAttacking){
            hitLimbs.Clear(); 
        }
     
        
    }

    public void Limbs()
    {
    foreach(Rigidbody limbRb in attackScript.selectedTarget.GetComponent<BasicEnemyScript>().ragdollRbs)
    {
        //if(limbRb.name ==)
    }

    }

    void OnTriggerEnter(Collider other)
    {
        if(!hitLimbs.Contains(other.attachedRigidbody) && other.gameObject.CompareTag("Limb"))
        {
            hitLimbs.Add(other.attachedRigidbody);   
            Debug.Log("We have hit a limb"); 
        }

        /*
        if(other.gameObject == attackScript.selectedTarget.GetComponent<BasicEnemyScript>().ragdollRbs)
        hitLimbs.Add(other.attachedRigidbody);
        */       
    }

    /*

    void OnTriggerExit(Collider other)
    {        
        
        if(hitLimbs.Contains(other.attachedRigidbody)) hitLimbs.Remove(other.attachedRigidbody); 
    }
    */



 
/*
      if(enemyTargetsInRange.Count > 0)
        {
            foreach(GameObject obj in enemyTargetsInRange)
            {
                BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>(); 
                enemyScript.TakeDamage(playerController.currentAttackDamage); 
                //enemyScript.enemyRb.AddForce(-transform.forward * f, ForceMode.VelocityChange); 
                enemyScript.transform.position += transform.forward * 1.3f; 
            }
        }
        */
}
