using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPauses : MonoBehaviour
{
    public float hitPauseTimer; 
    public float hitPauseDuration;
    public float pullOutReduction; 

    private float waitForTimer; 
    private float waitForDuration = .05f; 
    public bool doHitPause;
    public ThirdPerson_PlayerControler playerController; 

    public List<Animator> objectsToPause = new List<Animator>(); 
    public AttackTargetScript attackTargetScript; 
    public Animator playerAnim;


    public List<GameObject> theHitPaused = new List<GameObject>();
    //  public Animator hitEnemyAnim; 

    // Start is called before the first frame update
    void Start()
    {
        objectsToPause.Add(playerAnim); 
    }

    // Update is called once per frame
    void Update()
    {

        if (doHitPause)
        {
            playerController.canRotate = false;
            hitPauseTimer += Time.deltaTime; 

            foreach (Animator anim in objectsToPause)
            {

                //Turn anim off 
                if (hitPauseTimer < hitPauseDuration)
                {
                    if (anim == playerAnim)
                    {
                        anim.speed = .02f;
                    }
                    //Turn anim back on 
                    
                }
                else if (hitPauseTimer >= hitPauseDuration)
                {

                    foreach (GameObject obj in theHitPaused)
                    {
                        //Deal damage to hit target
                        BasicEnemyScript enemyScript = obj.GetComponent<BasicEnemyScript>();
                        // enemyScript.TakeDamage(attackTargetScript.playerController.currentAttackDamage);
                        GameObject blood = Instantiate(attackTargetScript.axeHitBloodVFX, enemyScript.bloodSpawnPoint.position, enemyScript.bloodSpawnPoint.rotation);
                        blood.AddComponent<CleanUpScript>();
                    }

                    attackTargetScript.weaponHitFeedback?.PlayFeedbacks();
                    theHitPaused.Clear();
                    if (anim == playerAnim) anim.speed = attackTargetScript.playerController.animAttackSpeed * pullOutReduction; 
                    else anim.speed = 1f;
                    //  if(anim != playerAnim) objectsToPause.Remove(anim); 
                }
                 

            }

            if (hitPauseTimer >= hitPauseDuration)
            {
                playerController.canRotate = true;
                hitPauseTimer = 0f;
                doHitPause = false;
                
                // attackTargetScript.TargetDamageEffects();
            }
        }
    }
   
}
