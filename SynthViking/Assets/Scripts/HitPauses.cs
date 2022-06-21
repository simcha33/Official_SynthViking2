using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPauses : MonoBehaviour
{
    public float hitPauseTimer; 
    public float hitPauseDuration;
    public float pullOutReduction; 
    public float sprintHitPauseLength; 

    
    public bool doHitPause;

  //  public List<GameObject> theHitPaused = new List<GameObject>();
    public List<Animator> objectsToPause = new List<Animator>();

    public ThirdPerson_PlayerControler playerController;
    public AttackTargetScript attackTargetScript;
    public Animator playerAnim;

    void Start()
    {
        objectsToPause.Add(playerAnim); 
    }

    void Update()
    {
        if(doHitPause) DoHitPause();        
    }

    public void DoHitPause()
    {

        //Turn anims off 
        if (hitPauseTimer < hitPauseDuration) 
        {          
            foreach (Animator anim in objectsToPause)
            {
                if (anim == playerAnim) anim.speed = .02f;
                else if(!anim.gameObject.CompareTag("BigEnemy")) anim.speed = .25f; 

            }

            hitPauseTimer += Time.deltaTime;
            playerController.canRotate = false;
        }

        //Turn anims back on
        else if (hitPauseTimer >= hitPauseDuration)
        {
            foreach (Animator anim in objectsToPause)
            {
                 
                if (anim == playerAnim) 
                {
                    if(playerController.attackState == playerAttackType.HeavyAxeHit.ToString())anim.speed = attackTargetScript.playerController.axeAttackSpeed * pullOutReduction;
                    else if(playerController.attackState == playerAttackType.LightPunchHit.ToString())anim.speed = attackTargetScript.playerController.punchAttackSpeed;
                    playerController.canRotate = true;
                }
                else if(anim != playerAnim)
                {
                 
                    BasicEnemyScript enemyScript = anim.transform.GetComponent<BasicEnemyScript>();
                    if (playerController.attackState == playerAttackType.HeavyAxeHit.ToString())
                    {
                        GameObject blood = Instantiate(attackTargetScript.axeHitBloodVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                        GameObject blood2 = Instantiate(attackTargetScript.axeHitBloodVFX2, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                        blood.AddComponent<CleanUpScript>();                 
                        attackTargetScript.weaponHitFeedback?.PlayFeedbacks();
                      
                    }
                    else if (playerController.attackState == playerAttackType.LightPunchHit.ToString())
                    {
                        GameObject dust = Instantiate(attackTargetScript.punchDustVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                        GameObject sparks = Instantiate(attackTargetScript.punchSparksVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                        dust.transform.parent = enemyScript.transform;
                        sparks.transform.parent = enemyScript.transform; 
                        dust.AddComponent<CleanUpScript>();
                        attackTargetScript.punchHitFeedback?.PlayFeedbacks();
                    }

                        anim.speed = 1f;
                    // objectsToPause.Remove(anim); 
                }          
            }
            objectsToPause.Clear();
            objectsToPause.Add(playerAnim);
            doHitPause = false;
            hitPauseTimer = 0;            
        }

      


    }
   
}
