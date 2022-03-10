using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class SlowMoScript : MonoBehaviour
{

    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input; 

    public float maxSlowMoTime;
    public float currentSlowmoTime;

    public float slowmoRechargeCooldownDuration;
    private float slowmoRechargeCooldownTimer; 
    public float rechargeSpeed;
    public bool canSlowmo;
    public bool isInSlowmo; 
    public bool isRecharging; 
    public float currentTimeScale; 

    public MMTimeManager timeManager; 

    void Start()
    {
        currentSlowmoTime  = maxSlowMoTime;
        slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeScale = Time.timeScale; 
        Debug.Log(currentTimeScale); 
        CheckForSlowmo();
    }

    

    public void CheckForSlowmo()
    {
        //Check if we have enough slowmo
        if (currentSlowmoTime <= 0) canSlowmo = false;
        else if(!input.dashButtonPressed) canSlowmo = true; 

        //Player is in slowmo 
        if (canSlowmo && input.dashButtonPressed)
        {
            if(!isInSlowmo)
            { 
                timeManager.SetTimescaleTo(.45f);
                isInSlowmo = true; 
            }
            isRecharging = false; 
            
            DoSlowmo(true, .51f); 
        }

     

        //Player is not in slowmo 
        if(currentSlowmoTime < maxSlowMoTime && !input.dashButtonPressed)
        {                          
            if(!isRecharging)
            { 
                canSlowmo = false; 
                isInSlowmo = false; 
                isRecharging = true; 
                timeManager.SetTimescaleTo(1f);   
                slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;             
            }      

            DoSlowmoRecharge();           
        }   
    }

    public void DoSlowmo(bool hasCost, float timeScaleModifier)
    {  
        if(hasCost) currentSlowmoTime -= Time.unscaledDeltaTime;
    }

    void DoSlowmoRecharge()
    {
        slowmoRechargeCooldownTimer -= Time.deltaTime; //Wait for recharge to begin 
        if (slowmoRechargeCooldownTimer <= 0) currentSlowmoTime += Time.deltaTime * rechargeSpeed;
        isInSlowmo = false; 
    }

}
