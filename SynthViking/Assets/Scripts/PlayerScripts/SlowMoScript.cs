using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using Cinemachine; 

public class SlowMoScript : MonoBehaviour
{

    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input;
    public Slider slowMoSlider;
    public Image slowMoBar;
    public CinemachineFreeLook aimCam;
    public float defaultAimY;
    public float defaultAimX; 

    public float maxSlowMoTime;
    public float currentSlowmoTime;

    public float slowmoRechargeCooldownDuration;
    private float slowmoRechargeCooldownTimer; 
    public float rechargeSpeed;
    public bool canSlowmo;
    public bool isInSlowmo; 
    public bool isRecharging; 
    public float currentTimeScale;
    public float dashChargeTimeScale = .45f; 

    public MMTimeManager timeManager; 

    void Start()
    {
        currentSlowmoTime  = maxSlowMoTime;
        slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;
        timeManager.SetTimescaleTo(1f);

        defaultAimX = aimCam.m_XAxis.m_MaxSpeed;
        defaultAimY = aimCam.m_YAxis.m_MaxSpeed; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.mainGameManager.gameIsPaused) CheckForSlowmo();
     
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

                //  aimCam.m_YAxis.m_MaxSpeed /= (dashChargeTimeScale * 1.2f);
                //  aimCam.m_XAxis.m_MaxSpeed /= (dashChargeTimeScale * 1.2f); 

                aimCam.m_XAxis.m_MaxSpeed = defaultAimX / dashChargeTimeScale;
                aimCam.m_YAxis.m_MaxSpeed = defaultAimY / dashChargeTimeScale;
            }

            isInSlowmo = true;            
            isRecharging = false; 
            
            DoSlowmo(true, dashChargeTimeScale); 

        }

        //UI Elements
        slowMoSlider.value = currentSlowmoTime / maxSlowMoTime;
        if (currentSlowmoTime < maxSlowMoTime / 4) slowMoBar.color = Color.red;
        else slowMoBar.color = Color.blue *.85f; 



        //Player is not in slowmo 
        if (currentSlowmoTime < maxSlowMoTime || !input.dashButtonPressed)
        {                          
            if(!isRecharging)
            { 
                canSlowmo = false; 
                isInSlowmo = false; 
                isRecharging = true;
                aimCam.m_YAxis.m_MaxSpeed *= dashChargeTimeScale;
                aimCam.m_XAxis.m_MaxSpeed *= dashChargeTimeScale;
          
                timeManager.SetTimescaleTo(1f);  
                
                slowmoRechargeCooldownTimer = slowmoRechargeCooldownDuration;             
            }      

            DoSlowmoRecharge();           
        }   
    }

    public void DoSlowmo(bool hasCost, float timeScaleModifier)
    {  
        if(hasCost) currentSlowmoTime -= Time.unscaledDeltaTime;
        timeManager.SetTimescaleTo(timeScaleModifier);
    }

    void DoSlowmoRecharge()
    {
        slowmoRechargeCooldownTimer -= Time.deltaTime; //Wait for recharge to begin 
        if (slowmoRechargeCooldownTimer <= 0) currentSlowmoTime += Time.deltaTime * rechargeSpeed;
        isInSlowmo = false; 
    }

}
