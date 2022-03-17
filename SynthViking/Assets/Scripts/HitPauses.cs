using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPauses : MonoBehaviour
{
    public float hitPauseTimer; 
    public float hitPauseDuration; 

    private float waitForTimer; 
    private float waitForDuration = .05f; 
    public bool doHitPause; 

    public List<Animator> objectsToPause = new List<Animator>(); 
    public AttackTargetScript attackTargetScript; 
    public Animator playerAnim;
  //  public Animator hitEnemyAnim; 

    // Start is called before the first frame update
    void Start()
    {
        objectsToPause.Add(playerAnim); 
    }

    // Update is called once per frame
    void Update()
    {
        

        if(doHitPause) 
        {
                //waitForTimer+= Time.deltaTime; 

                //if(waitForTimer >= waitForDuration){
                hitPauseTimer += Time.deltaTime; 
                
                foreach(Animator anim in objectsToPause)
                {

                    //Turn anim off 
                    if(hitPauseTimer < hitPauseDuration) 
                    {
                        if(anim == playerAnim){ anim.speed = .02f; print("PlayerHit");}
                                
                    }

                    //Turn anim back on 
                    else if (hitPauseTimer >= hitPauseDuration)
                    { 
                      
                        anim.speed = 1f;  
                      //  if(anim != playerAnim) objectsToPause.Remove(anim); 
                    }        
                }

                if(hitPauseTimer >= hitPauseDuration)
                {
                    hitPauseTimer = 0f; 
                    doHitPause = false; 
                   // attackTargetScript.TargetDamageEffects();
                } 
        }
        else
        {
            //hitPauseTimer = 0f; 
        }
    }

   
}
