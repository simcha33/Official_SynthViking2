using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPathChecker : MonoBehaviour
{
    public ThirdPerson_PlayerControler playerController;
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
        if (playerController.isDashing) {

            //Dash collides with enemy
            if (other.gameObject.CompareTag("Enemy"))
            {
                playerController.enemyDashObjectReached = true;
                playerController.playerRb.velocity = new Vector3(0, 0, 0);
                // playerRb.transform.LookAt(new Vector3(m_Hit.transform.position.x, m_Hit.transform.position.y, transform.forward.z));  
                playerController.playerAnim.SetTrigger("DashAttackTrigger");
                playerController.attackTarget = other.gameObject; 
                playerController.DashAttackFeeback?.PlayFeedbacks();
                Debug.Log("TriggerAMt"); 
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
