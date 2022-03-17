using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks; 

public class AttackTargetScript : MonoBehaviour
{

    //public List<Transform> nearbyTargets = new List<Transform>(); 
    [Header("COMPONENTS")] //GROUND MOVEMENT
    #region
    public MMFeedbacks weaponImpactFeedback;
    public MMFeedbacks weaponKillFeedback;
    public ThirdPerson_PlayerControler playerController;

    public HitPauses hitpauseScript; 
    public CheckForLimbs limbCheckerScript;

    public GameObject bloodFx1;
    public GameObject bloodFx2;
    public Transform swordPoint;
    public Transform targetCube;
    public Transform selectedTarget;
    #endregion

    [Header("VALUES")] //GROUND MOVEMENT
    #region
    public float targetCheckRadius; 
    public float targetCheckRange;
    public bool canTarget; 
    public List<GameObject> enemyTargetsInRange = new List<GameObject>();
    #endregion




    void Start()
    {

    }

    void Update(){
        
        //Check for enemies within range 
        RaycastHit hit; 
        if(Physics.SphereCast(transform.position, targetCheckRadius, playerController.inputDir, out hit, targetCheckRange) && canTarget)
        {
            //Check for enemy layer
            if(hit.transform.gameObject.layer == playerController.enemyLayer && !hit.transform.CompareTag("Dead"))
            {
                BasicEnemyScript script = hit.transform.GetComponent<BasicEnemyScript>();             
                if(script.canBeTargeted)
                {
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

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == playerController.enemyLayer && !enemyTargetsInRange.Contains(other.gameObject))
        {
            enemyTargetsInRange.Add(other.transform.gameObject);
        }
    }



    
    void OnTriggerExit(Collider other)
    {
        if (enemyTargetsInRange.Count > 0)
        {
            enemyTargetsInRange.Remove(other.gameObject); 
            /*
            foreach (GameObject obj in enemyTargetsInRange)
            {
                if (other.transform.name == obj.name && enemyTargetsInRange.Contains(other.gameObject)) enemyTargetsInRange.Remove(obj);
            }
            */
        }

    }
    


    public void TargetDamageCheck()
    {
        if(enemyTargetsInRange.Count <= 0) return; 
        
        foreach(GameObject obj in enemyTargetsInRange)
        {
            //Deal damage to hit target
            BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>(); 
            enemyScript.TakeDamage(playerController.currentAttackDamage);
            Instantiate(bloodFx1, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
            weaponImpactFeedback?.PlayFeedbacks();
     

            //Is the enemy dead after our hit?            
            if (enemyScript.isDead)
            {
         
                //Collect and detach limbs
                foreach (Rigidbody limb in limbCheckerScript.hitLimbs)
                {
                    if(enemyScript.ragdollRbs.Contains(limb))
                    {         
                        CharacterJoint joint = limb.GetComponent<CharacterJoint>();
                        Instantiate(bloodFx1, limb.position, limb.rotation);
                        Destroy(joint);
                        limb.transform.parent = null;             
                        limb.AddForce((swordPoint.position - limb.transform.position) * 1f, ForceMode.Impulse);
                        limb.AddForce(limb.transform.up * .4f, ForceMode.Impulse);

                        if (enemyScript.canBeTargeted)limb.mass *= 3f; 
                    }
                }

                if (enemyScript.canBeTargeted)
                {
                    enemyScript.canBeTargeted = false; 
                    weaponKillFeedback?.PlayFeedbacks();
                }

            }
            else
            {           
               hitpauseScript.objectsToPause.Add(enemyScript.enemyAnim); 
               hitpauseScript.doHitPause = true; 
            }

           // enemyTargetsInRange.Remove(obj); 
            enemyScript.transform.DOMove(playerController.transform.position + transform.forward * playerController.currentAttackForwardForce * 1.5f, .3f).SetUpdate(UpdateType.Fixed); ; //Move enemy backwards
          // enemyTargetsInRange.Clear();

        }

        limbCheckerScript.hitLimbs.Clear();
        //enemyTargetsInRange.Clear();


    }

   

    public void TargetDamageEffects()
    {
     
    
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
