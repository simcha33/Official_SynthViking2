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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerController.isDashing || playerController.dashDelayDuration > 0 && !playerController.input.dashButtonPressed) 
        {


            //Dash collides with enemy
            if (other.gameObject.layer == LayerMask.NameToLayer("DashPoint") && !playerController.enemyDashObjectReached && !other.gameObject.CompareTag("Dead") && playerController.fullyChargedDash) 
            {
       
                print(" Trigger Dash Attack"); 
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
    }

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

}
