using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class DashPathChecker : MonoBehaviour
{
    public ThirdPerson_PlayerControler playerController;

   // public Transform target; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.isDashing) CheckForEnvorinment();
        CheckForDashTargets(); 
       // if(playerController.isDashing) CheckForEnvorinment(); 
    }

    //playerController.dashAirAttack && Vector3.Distance(transform.position, playerController.dashDirection) < 5f
    private void OnTriggerEnter(Collider other)
    {
        if (playerController.isDashing) {


            //Dash collides with enemy
            if (other.gameObject.layer == LayerMask.NameToLayer("DashPoint") && !playerController.enemyDashObjectReached && !other.gameObject.CompareTag("Dead") && playerController.fullyChargedDash) 
            {
       
                playerController.playerAnim.SetTrigger("DashAttackTrigger");
                playerController.enemyDashObjectReached = true;
                playerController.playerRb.velocity = new Vector3(0, 0, 0);
                playerController.DoDash();
                DashPoint dashTargetScript = other.GetComponent<DashPoint>(); 
                dashTargetScript.enemyScript.enemyRb.velocity = new Vector3(0,0,0);
                dashTargetScript.freezeEnemy = false;
                dashTargetScript.enemyScript.transform.position = playerController.transform.position + transform.forward; 


                playerController.dashAttackTarget = other.gameObject;
           //     playerController.transform.position -= Vector3.up * .5f;
                playerController.transform.LookAt(other.transform.position, Vector3.up);    
                playerController.DashAttackFeedback?.PlayFeedbacks();
            }
        }
    }

    /*
    private void DashAttack()
    {
        playerController.playerAnim.SetTrigger("DashAttackTrigger");
        playerController.enemyDashObjectReached = true;
        playerController.playerRb.velocity = new Vector3(0, 0, 0);
        playerController.DoDash();
        other.GetComponent<BasicEnemyScript>().enemyRb.velocity = new Vector3(0, 0, 0);

        playerController.dashAttackTarget = other.gameObject;
        //     playerController.transform.position -= Vector3.up * .5f;
        playerController.transform.LookAt(other.transform.position, Vector3.up);
        playerController.DashAttackFeedback?.PlayFeedbacks();
    }
    */

    void CheckForEnvorinment()
    {

        //Check for dash path collisions 
        RaycastHit dashHit;
        Ray dashRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(dashRay, out dashHit, 2f))
        {
            //Dash collides with environmental object
            if (dashHit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                if (!dashHit.collider.gameObject.CompareTag("Breakable")) //Ignore destruction objects
                {
                    playerController.solidDashObjectReached = true;
                    playerController.ResetDash();
                    playerController.playerRb.velocity = new Vector3(0, 0, 0);


                }
            }
        }
    }

    void CheckForDashTargets()
    {
        /*
        var dist = Mathf.Abs(transform.position.z - playerController.playerCamera.transform.position.z);
        //var worldPos = playerController.playerCamera.ScreenToWorldPoint(playerController.playerState.dashChargeBackground.transform.position);
        var v3Pos = playerController.playerCamera.ScreenToWorldPoint(playerController.playerState.dashChargeBackground.transform.position);
        var distanceBetween = Vector3.Distance(v3Pos, target.position);
        Debug.Log(distanceBetween); 
    

        // Vector3 point = playerController.playerCamera.WorldToScreenPoint(playerController.playerState.dashChargeBackground.transform.position);

        //     float targetDis = Vector2.Distance(playerController.playerState.dashChargeBackground.transform.position, target.position);
        //   Debug.Log(targetDis); 

        float checkDistance = 20f;
        float checkRadius = 5f; 

        Collider[] targets;
        targets = Physics.OverlapCapsule(playerController.playerCamera.transform.position, playerController.playerCamera.transform.forward * checkDistance, checkRadius);
        
        

       // Debug.Log(target.name);
        */
    }

    void DisableGravity()
    {

    }



}
