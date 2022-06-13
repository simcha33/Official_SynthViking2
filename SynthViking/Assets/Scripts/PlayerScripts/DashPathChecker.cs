using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class DashPathChecker : MonoBehaviour
{
    public ThirdPerson_PlayerControler playerController;
    void Update()
    {
        if(playerController.isDashing) CheckForEnvorinment();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerController.isDashing || playerController.dashDelayDuration > 0 && !playerController.input.dashButtonPressed) 
        {


            //Dash collides with enemy
            if (other.gameObject.layer == LayerMask.NameToLayer("DashPoint") && !playerController.enemyDashObjectReached && !other.gameObject.CompareTag("Dead") && playerController.fullyChargedDash) 
            {
       
                print(" Trigger Dash Attack");

                playerController.playerAnim.SetInteger("DashAttackType",0); 
                playerController.playerAnim.SetTrigger("DashAttackTrigger");

                playerController.enemyDashObjectReached = true;
                playerController.playerRb.velocity = new Vector3(0, 0, 0);
                playerController.DoDash();
                DashPoint dashTargetScript = other.GetComponent<DashPoint>(); 
                dashTargetScript.enemyScript.enemyRb.velocity = new Vector3(0,0,0);
                dashTargetScript.freezeEnemy = false;
                dashTargetScript.enemyScript.transform.position = playerController.transform.position + transform.forward; 


                playerController.dashAttackTarget = other.gameObject;
                playerController.transform.LookAt(other.transform.position, Vector3.up);    
                playerController.DashAttackFeedback?.PlayFeedbacks();
            }
        }

        //Cancel dash
        if((other.gameObject.layer == playerController.EnvorinmentLayer || other.gameObject.layer == LayerMask.NameToLayer("InvisibleWall")) && playerController.isDashing && !playerController.isDashAttacking)
        {
            playerController.solidDashObjectReached = true;
            playerController.ResetDash();
            playerController.playerRb.velocity = new Vector3(0, 0, 0);
        }

    }

    void CheckForEnvorinment()
    {

        //Check for dash path collisions 
        RaycastHit dashHit;
        Ray dashRay = new Ray(transform.position, playerController.dashDirection);
        if (Physics.Raycast(dashRay, out dashHit, 2f))
        {
            //Dash collides with environmental object
            if (dashHit.collider.gameObject.layer == LayerMask.NameToLayer("Environment") || dashHit.collider.gameObject.layer == LayerMask.NameToLayer("InvisibleWall"))
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

}
