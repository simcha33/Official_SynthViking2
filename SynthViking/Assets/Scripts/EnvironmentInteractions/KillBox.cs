using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemyScript = other.gameObject.GetComponent<BasicEnemyScript>();
            if (!enemyScript.canRecover)
            {
                enemyScript.TakeDamage(enemyScript.currentHealth, "Killbox");
                enemyScript.DestroySelf();
                print("KILLBOX KILL"); 
            }         
        }
    }
}
