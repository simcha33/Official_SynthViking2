using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DashPoint : MonoBehaviour
{
    public bool isEnemy;
    public BasicEnemyScript enemyScript;
    public bool isTargeted;
    public bool readyTarget; 
    public GameObject buttonPrompt;
    public Transform playerCamera;
    public bool freezeEnemy;
    public Vector3 stayPos; 
    public ThirdPerson_PlayerControler playerControlle; 


    private void Start()
    {

        if (isEnemy)
        {
            playerControlle = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
            enemyScript = transform.parent.parent.GetComponent<BasicEnemyScript>();
            playerCamera = playerControlle.playerCamera.transform;
        }

        buttonPrompt.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isTargeted)
        {
            if (!readyTarget)
            {
                buttonPrompt.SetActive(true);
                readyTarget = true;
              //  enemyScript.enemyRb.isKinematic = true;
                enemyScript.isDashTargeted = true; 
            }

            if (freezeEnemy)
            {
                enemyScript.enemyRb.isKinematic = true;
                enemyScript.enemyRb.useGravity = false;
                enemyScript.enemyRb.velocity = new Vector3(0, 0, 0);
                Debug.Log("Freeze");
                enemyScript.transform.position = stayPos; 

                foreach(Rigidbody rb in enemyScript.ragdollRbs)
                {
                    rb.isKinematic = true;
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.useGravity = false; 
                }
            }


            // buttonPrompt.transform.LookAt(playerCamera);
            // buttonPrompt.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward); 

        }
        else if(!isTargeted && !playerControlle.isDashing)
        {
            buttonPrompt.SetActive(false);           
           
            readyTarget = false;

            if (freezeEnemy)
            {
                foreach (Rigidbody rb in enemyScript.ragdollRbs)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                freezeEnemy = false; 
                enemyScript.isDashTargeted = false;
                enemyScript.enemyRb.isKinematic = false;
                enemyScript.enemyRb.useGravity = true;
            }
            
        }


    }

    



   



}