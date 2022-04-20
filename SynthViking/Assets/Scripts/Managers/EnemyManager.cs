using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyTypes
{
    Grunt,
    Ranger,
}
public class EnemyManager : MonoBehaviour
{
        public List<BasicEnemyScript> engagedEnemies = new List<BasicEnemyScript>();
        public List<BasicEnemyScript> attackingEnemies = new List<BasicEnemyScript>();
        public List<BasicEnemyScript> currentenemiesInScene = new List<BasicEnemyScript>();
        public List<BasicEnemyScript> allDeadEnemiesInScene = new List<BasicEnemyScript>();

        public int maxDeadEnemiesAllowed = 30;
        public Transform deadEnemyParent;
        public Transform aliveEnemyParent; 

    /*
       public int currentAttackers;
       public int maxAttackers = 5; 

          //public float normaleAttackDistance;  


          public float minTimeNewAttackerToWait; 
          public float maxTimeNewAttackerToWait; 


            public float setNewAttackerCooldownTimer;

          private float setNewAttackerCooldownDuration;

          public int minAttackers = 1; 
    */
 

    private void Update()
    {
        CheckForCleanUp(); 

        foreach(BasicEnemyScript enemy in currentenemiesInScene)
        {
            enemy.transform.parent = deadEnemyParent; 
        }

        foreach (BasicEnemyScript enemy in engagedEnemies)
        {
            enemy.transform.parent = aliveEnemyParent; 
        } 
        
    }

    public void CheckForCleanUp()
    {
        if(allDeadEnemiesInScene.Count > maxDeadEnemiesAllowed)
        {
            allDeadEnemiesInScene[0].DestroySelf(); 
        }
    }


    /*

        void Start()
        {
          
        }

        void Update()
        {
            
            if(attackingEnemies.Count > 0)
            { 
               
                HandleAttackers();          
            }

            if(engagedEnemies.Count > 0)
            {
                CheckForAttackers(); 
            }
        }

        void CheckForEnemySpawning()
        {
            if(currentEnemiesInScene < maxEnemiesAllowedInScene)
            {
                SpawnNewEnemy(); 
            }
        }

        void SpawnNewEnemy()
        {
            
        }


         void CheckForAttackers()
        {
            currentAttackers = attackingEnemies.Count; 

            if(currentAttackers < maxAttackers && engagedEnemies.Count > 0)
            {
                setNewAttackerCooldownTimer += Time.deltaTime; 

                if(setNewAttackerCooldownTimer >= setNewAttackerCooldownDuration || currentAttackers < minAttackers)
                {
                    setNewAttackerCooldownTimer = 0F;
                    setNewAttackerCooldownDuration = Random.Range(minTimeNewAttackerToWait, maxTimeNewAttackerToWait); 
                    SetNewAttackers(); 
                }
            }
        }


        void SetNewAttackers()
        {
            //BasicEnemyScript choosenEnemy; 

            BasicEnemyScript choosenEnemy = engagedEnemies[Random.Range(engagedEnemies.Count, 0)]; 

            if(choosenEnemy.targetDistance <= choosenEnemy.circleDistance + 1 && choosenEnemy.canBeManaged)
            {
                attackingEnemies.Add(choosenEnemy); 
                choosenEnemy.currentRequiredDistance = choosenEnemy.attackDistance; 
                choosenEnemy.canAttack = true;
            }
            else return; 
             
       

            /*
            foreach(BasicEnemyScript enemyScript in engagedEnemies)
            {
                if(currentAttackers < maxAttackers)
                {
                    currentAttackers++; 
                    enemyScript.canAttack = true; 
                    enemyScript.attackDistance = 1.5f;
                    attackingEnemies.Add(enemyScript); 
                }
                else
                {
                    enemyScript.attackDistance += 10f; 
                    enemyScript.canAttack = false; 
                }
            }
            
        }

        public void SetNewEngager(BasicEnemyScript engager)
        {
            if(!engagedEnemies.Contains(engager)) engagedEnemies.Add(engager); 
      
            if(engager.canBeManaged)
            {
                engager.currentRequiredDistance = engager.circleDistance; 
                engager.canAttack = false;
            }
            else
            {
                engager.currentRequiredDistance = engager.attackDistance; 
                engager.canAttack = true; 
            }
        }

        public void RemoveAttacker(BasicEnemyScript attacker)
        {
            attackingEnemies.Remove(attacker); 
            attacker.canAttack = false;
            attacker.currentRequiredDistance = attacker.circleDistance; 
        }

        public void RemoveEngager(BasicEnemyScript engager)
        {
            engagedEnemies.Remove(engager); 
        }

        void HandleAttackers()
        {
            foreach(BasicEnemyScript attackerScript in attackingEnemies)
            {
                if(attackerScript.targetDistance > attackerScript.circleDistance -3f || attackerScript.isDead)
                {
                    RemoveAttacker(attackerScript);           
                }
            }
        }
 */
}
