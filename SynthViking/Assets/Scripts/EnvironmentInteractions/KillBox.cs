using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{

    public Transform playerRespawnPoint;
    public ThirdPerson_PlayerControler player; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.gameObject.GetComponent<BasicEnemyScript>() != null)
            {
                BasicEnemyScript enemyScript = other.gameObject.GetComponent<BasicEnemyScript>();
                if (!enemyScript.canRecover)
                {
                    enemyScript.TakeDamage(enemyScript.currentHealth, "Killbox");
                    enemyScript.DestroySelf();
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            print("player respawn");
            player.transform.position = playerRespawnPoint.position; 
        }
    }
}
