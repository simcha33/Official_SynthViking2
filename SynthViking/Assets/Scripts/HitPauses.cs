using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPauses : MonoBehaviour
{
    public float hitPauseTimer; 
    public float hitPauseDuration;
    public float pullOutReduction; 
    public float sprintHitPauseLength; 
    public float axeHitPauseLength;
    
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
                else anim.speed = .25f; 
            }

            hitPauseTimer += Time.deltaTime;
            playerController.canRotate = false;
        }

        //Turn anims back on
        else if (hitPauseTimer >= hitPauseDuration)
        {
            foreach (Animator anim in objectsToPause)
            {
                 
                if (anim == playerAnim) anim.speed = attackTargetScript.playerController.animAttackSpeed * pullOutReduction;
                else
                {
                    BasicEnemyScript enemyScript = anim.transform.GetComponent<BasicEnemyScript>();
                    GameObject blood = Instantiate(attackTargetScript.axeHitBloodVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                    blood.AddComponent<CleanUpScript>();
                    attackTargetScript.weaponHitFeedback?.PlayFeedbacks();
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
