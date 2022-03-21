using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
        public List<BasicEnemyScript> engagedEnemies = new List<BasicEnemyScript>();
        public List<BasicEnemyScript> attackingEnemies = new List<BasicEnemyScript>();

          public int currentAttackers;
          public int maxAttackers = 5; 

          //public float normaleAttackDistance;  


          public float minTimeNewAttackerToWait; 
          public float maxTimeNewAttackerToWait; 


            public float setNewAttackerCooldownTimer;

          private float setNewAttackerCooldownDuration;

          public int minAttackers = 1; 

        void Start()
        {
          
        }

        void Update()
        {
            
            if(attackingEnemies.Count > 0)
            { 
                foreach(BasicEnemyScript attacker in engagedEnemies)
                {
                    if(attacker.targetDistance < attacker.circleDistance)  CheckForAttackers();             
                }

                HandleAttackers();          
            }

   
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
            BasicEnemyScript choosenEnemy; 

            choosenEnemy = engagedEnemies[Random.Range(engagedEnemies.Count, 0)]; 

            if(attackingEnemies.Contains(choosenEnemy) || choosenEnemy.targetDistance >= choosenEnemy.circleDistance + 1 || !choosenEnemy.canBeManaged) return; 
    

            attackingEnemies.Add(choosenEnemy); 
            choosenEnemy.currentRequiredDistance = choosenEnemy.attackDistance; 
            choosenEnemy.canAttack = true;
       

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
            */
        }

        public void SetNewEngager(BasicEnemyScript engager)
        {
            if(engagedEnemies.Contains(engager)) return; 
            
            engagedEnemies.Add(engager); 
      
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
}
