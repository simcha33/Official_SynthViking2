using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetScript : MonoBehaviour
{

    //public List<Transform> nearbyTargets = new List<Transform>(); 
   
    public float targetCheckRadius; 
    public float targetCheckRange; 

    private LayerMask enemyLayer = 7;
    public Transform targetCube; 
    public Transform selectedTarget;
    public ThirdPerson_PlayerControler playerController;

    public List<GameObject> enemyTargetsInRange = new List<GameObject>();  


    void Start()
    {

    }

    void Update(){
        
        //Check for enemies within range 
        RaycastHit hit; 
        if(Physics.SphereCast(playerController.transform.position, targetCheckRadius, playerController.inputDir, out hit, targetCheckRange))
        {
            //Check for enemy layer
            if(hit.transform.gameObject.layer == enemyLayer && !hit.transform.CompareTag("Dead"))
            {
                BasicEnemyScript script = hit.transform.GetComponent<BasicEnemyScript>();             
                if(script.canBeTargeted){
                    selectedTarget = hit.transform; 
                    targetCube.transform.position = selectedTarget.position + new Vector3(0,2,0); 
                }
            }
        }
        else 
        {
            targetCube.transform.position = new Vector3(5000,5000,500); 
            selectedTarget = null;
        }
    }

     void OnTriggerEnter(Collider other)
     {

        if(playerController.isAttacking && other.gameObject.layer == enemyLayer)
        {
            enemyTargetsInRange.Add(other.transform.gameObject); 
        }
    }

     void OnTriggerExit(Collider other)
     {
         foreach(GameObject obj in enemyTargetsInRange)
         {
            if(other.transform.name == obj.name) enemyTargetsInRange.Remove(obj); 
         }

     }


    public void TargetDamageCheck()
    {
        if(enemyTargetsInRange.Count > 0)
        {
            foreach(GameObject obj in enemyTargetsInRange)
            {
                BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>(); 
                enemyScript.TakeDamage(playerController.currentAttackDamage); 
            }
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
    /*

    // Update is called once per frame
    void Update()
    {
        CheckForAttackTarget(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackTargetCheck") && !dontRemoveTarget)
        {
            if (!nearbyTargets.Contains(other.transform))
            {
                nearbyTargets.Add(other.transform);
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AttackTargetCheck") && !dontRemoveTarget)
        {
            if (nearbyTargets.Contains(other.transform))
            {
                nearbyTargets.Remove(other.transform);
            } 
        }
    }

    void CheckForAttackTarget()
    {
        if (nearbyTargets.Count > 0)
        {

            //Find the closest target to attack
            foreach (Transform target in nearbyTargets)
            {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (targetDistance < closedDistance || nearbyTargets.Count >= 1 && closedDistance == 0)
                {
                    closedDistance = targetDistance;
                    selectedTarget = target;
                }
            }
        }

        
        {
            closedDistance = 0; 
            selectedTarget = null;
        }
    }
    */
}
