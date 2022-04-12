using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerState : MonoBehaviour
{
   
    [Header("Health VALUES")]
    #region
    public float currentHealth;
    public float maxHealth;
    public Slider helathSlider;
    public Image healthBackground;
    #endregion

    [Header("STUNNED STATE")]  
    #region
    public float stunDuration;
    public float stunTimer;
    public bool isStunned;
    public bool canBeHit;
    public bool wasHitByAttack;
    private float recoveryTimer;
    public float recoveryDuration; 
    #endregion

    [Header("UI")]  
    #region
    public Image dashChargeIndicator;
    private Vector3 originalDashIndicatorSize;
    public Color dashChargeColor;
    public Image dashChargeBackground;
    #endregion

    [Header("SCRIPTS")]  
    #region
    public ThirdPerson_PlayerControler playerController;
    public ComboManager comboManagerScript; 
    #endregion

    [Header("COMPONENTS")]  
    #region
    public Animator playerAnim;
    public GameObject attacker;
    #endregion

    [Header("STATES")] 
    #region
    private int playerState;
    public enum currentState
    {
        NOTHING,
        MOVING,
        WALLRUNNING,
        ATTACKING,
        DASHING,
        AIRSMASHING,
        STUNNED,
    }
    #endregion



    private void Start()
    {
        currentHealth = maxHealth;
        canBeHit = true; 

        originalDashIndicatorSize = dashChargeIndicator.transform.localScale;
        dashChargeColor = dashChargeIndicator.color;
    }

    private void Update()
    {
        switch (playerState)
        {
            case (int)currentState.NOTHING:
                PlayerRecovery();
                break;
            case (int)currentState.STUNNED:
                StunPlayer();
                PlayerRecovery();
                break;
        }

        HandleUI();

    }

    void CanBeHit()
    {

    }

    void HandleUI()
    {
        //Dash Charge indicator
        float dashIndicatorSizeModifier = 1.3f;
        dashChargeIndicator.fillAmount = (playerController.currentDashDistance - playerController.minDashDistance) / (playerController.maxDashDistance - playerController.minDashDistance);

        //Switch between charged and uncharged dash visuals
        if (dashChargeIndicator.transform.localScale == originalDashIndicatorSize && playerController.currentDashDistance >= playerController.maxDashDistance)
        {
            dashChargeIndicator.transform.localScale *= dashIndicatorSizeModifier;
            dashChargeBackground.transform.localScale *= dashIndicatorSizeModifier;
        }
        else if (playerController.isDashing)
        {
            dashChargeIndicator.transform.localScale = originalDashIndicatorSize;
            dashChargeBackground.transform.localScale = originalDashIndicatorSize;
        }
        else if(playerController.isRechargingDash)
        {
            dashChargeBackground.color = Color.grey;
            dashChargeIndicator.color = Color.white;
            dashChargeIndicator.fillAmount = playerController.dashCooldownTimer / playerController.dashCooldownDuration;
        }
        else
        {
            dashChargeIndicator.color = dashChargeColor;
            dashChargeBackground.color = Color.white;
        }
    }
            

    public void TakeDamage(float damageAmount, string DamageType)
    {
        if (canBeHit || DamageType == "EnvironmentDamage")
        {
            currentHealth -= damageAmount;
            helathSlider.value = currentHealth / maxHealth;
            comboManagerScript.ResetCombo();
            playerController.PlayerDamagedFeedback?.PlayFeedbacks(); 

            if (currentHealth > 0)
            {
                
                if(DamageType == "BasicMeleeAttackDamage")
                {
                    canBeHit = false;
                    wasHitByAttack = true;                   

                }

                if(DamageType == "EnvironmentDamage")
                {
                    playerController.DoJump(550); 
                }

              
                playerController.ResetStates();
                playerController.ResetAnimator();
                playerController.isStunned = isStunned = true;
                playerController.playerAnim.SetTrigger(DamageType + "StunTrigger");

                playerState = (int)currentState.STUNNED;
                playerController.controllerState = (int)currentState.STUNNED;
                playerController.fixedControllerState = (int)currentState.STUNNED;
                  
                
           
                stunDuration = 10f;
                                               
            }
        }
    }

    void StunPlayer()
    {
  
        stunTimer += Time.deltaTime;

        if (stunTimer >= stunDuration)
        {
            //canBeHit = true;
            stunTimer = 0f;
            playerController.isStunned = isStunned = false;
            playerState = (int)currentState.NOTHING;
            playerController.controllerState = (int)currentState.MOVING;
            playerController.fixedControllerState = (int)currentState.MOVING;
        }

        stunDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;


    }

    void PlayerRecovery()
    {
        if(wasHitByAttack && !canBeHit)
        {
            recoveryTimer += Time.deltaTime; 

            if(recoveryTimer > stunDuration + recoveryDuration)
            {
                canBeHit = true;
                wasHitByAttack = false;
                recoveryTimer = 0f; 
            }
        }
    }
 

}
