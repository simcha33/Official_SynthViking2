using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class SlowMoScript : MonoBehaviour
{

    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input; 

  //  public float aimSlowmoDuration;
   // public float aimSlowmoTimer;

    public float maxSlowMoTime;
  //  public float minSlowMoTime; 
    public float currentSlowmoTime;

    public float slowmoRechargeCooldownDuration;
    public float slowmoRechargeCooldownTimer; 
    public float rechargeSpeed;
    public bool canSlowmo;
    public bool isInSlowmo;
    public bool startSlowmo; 
   // public bool isRecharing;
   // public bool canRecharge; 


    private MMFeedbackTimescaleModifier mmSlowmoTime;
    public MMFeedbacks slowmoFeedback;

    //State
    private int slowmoState; 
    private int fixedControllerState;
    [HideInInspector]
    public enum currentState
    {
        InSlowmo,
        Recharge,
        Wait,
    }

    void Start()
    {
        mmSlowmoTime = slowmoFeedback.GetComponent<MMFeedbackTimescaleModifier>();
        currentSlowmoTime  = mmSlowmoTime.TimeScaleDuration = maxSlowMoTime;
        slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSlowmo();

        switch (slowmoState)
        {
            case (int)currentState.Wait:
                break;

            case (int)currentState.InSlowmo:

                DoSlowmo();
                break;

            case (int)currentState.Recharge:
                DoSlowmoRecharge();
                break;
        }

        CheckForSlowmo();
    }

    public void CheckForSlowmo()
    {
        if (currentSlowmoTime <= 0) canSlowmo = false;

        if (input.slowMoButtonPressed && canSlowmo && !isInSlowmo)
        {

            slowmoState = (int)currentState.InSlowmo; 
        }
        else if (input.slowMoButtonPressed && isInSlowmo)
        {
            slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration; 
            slowmoState = (int)currentState.Recharge; 
        }


        else if (!input.slowMoButtonPressed && currentSlowmoTime < maxSlowMoTime)
        {
            DoSlowmoRecharge();
        }
    }

    public void HandleSlowmo()
    {
        mmSlowmoTime.TimeScaleDuration = currentSlowmoTime; 
       // mmSlowmoTime.stop
    
        if (currentSlowmoTime > 0)
        {
            canSlowmo = true; 
        }       
        else if (!isInSlowmo && currentSlowmoTime < maxSlowMoTime)
        {
            DoSlowmoRecharge();
            if (currentSlowmoTime > maxSlowMoTime) currentSlowmoTime = maxSlowMoTime;
        }

        isInSlowmo = false;
    }

   

    public void DoSlowmo()
    {

        Time.timeScale = .3f; 
        isInSlowmo = true;
        slowmoFeedback?.PlayFeedbacks();
        currentSlowmoTime -= Time.unscaledDeltaTime;
        slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;

    }



    void DoSlowmoRecharge()
    {
        //  Time.timeScale = .f;
        Debug.Log("InRecharge");
        Time.timeScale = 1f;
        slowmoRechargeCooldownTimer -= Time.deltaTime;


        if (slowmoRechargeCooldownTimer <= 0)
        {        
            currentSlowmoTime += Time.deltaTime * rechargeSpeed;
        }
   
    }
}
