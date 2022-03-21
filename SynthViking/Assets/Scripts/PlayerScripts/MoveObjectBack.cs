using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectBack : MonoBehaviour
{
   
    public Transform originalPos; 
    public AttackTargetScript playerAttackScript; 
    public Transform player; 
    public Vector3 playerMovePoint;
    public Vector3 enemyMovePoint;
    public float moveAmount;  
    public float moveSpeed; 
    public bool posIsSet; 
    public bool enemyPosReached;
    public bool playerPosReached;


    void Update()
    {
     
        foreach(GameObject enemy in playerAttackScript.targetsInRange){
            enemy.transform.position = (enemyMovePoint - enemy.transform.position) * moveSpeed; 
        }

        player.position =  (playerMovePoint - player.transform.position) * moveSpeed; 

        if(Vector3.Distance(playerMovePoint, player.transform.position) < .4f && !playerPosReached)
        {
            playerPosReached = true; 
        }

        
        if(Vector3.Distance(playerMovePoint, player.transform.position) < .4f && !enemyPosReached)
        {
            enemyPosReached = true;
        }

        


    }

    void SetMovePos()
    {
        if(!posIsSet)
        {
            //Set player move back point 
            playerMovePoint = player.forward + player.position * moveAmount; 
            enemyMovePoint = playerMovePoint + player.forward * moveAmount; 
            posIsSet = true; 
        }
    }
}
