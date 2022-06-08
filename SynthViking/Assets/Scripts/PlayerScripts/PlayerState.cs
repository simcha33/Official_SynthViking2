using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

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

    public string stunString; 
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
    public TextMeshProUGUI healthText; 
    #endregion

    [Header("SCRIPTS")]  
    #region

    public GameManager gameManager; 
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

    public bool playerIsDead; 



    private void Start()
    {
        currentHealth = maxHealth;
        canBeHit = true; 

        originalDashIndicatorSize = dashChargeIndicator.transform.localScale;
        dashChargeColor = dashChargeIndicator.color;

         helathSlider.value = currentHealth / maxHealth;
        healthText.text = currentHealth.ToString(); 
    }

    private void Update()
    {
        switch (playerState)
        {
            case (int)currentState.NOTHING:
                PlayerRecovery();
                break;
            case (int)currentState.STUNNED:
                StunPlayer(stunDuration, stunString);
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
        if (canBeHit || DamageType == "LavaDamage")
        {
            currentHealth -= damageAmount;
            healthText.text = currentHealth.ToString(); 
            helathSlider.value = currentHealth / maxHealth;
           
            comboManagerScript.ResetCombo();
            playerController.PlayerDamagedFeedback?.PlayFeedbacks(); 

            if (currentHealth > 0)
            {
                
                if(DamageType == "BasicMeleeAttackDamage")
                {
                    canBeHit = false;
                    wasHitByAttack = true; 
                    stunString = "AttackHit";    
                    stunDuration = 10f;                    

                }

                if(DamageType == "ChargeAttack")
                {
                    canBeHit = false;
                    wasHitByAttack = true; 
                    stunString = "AttackHit";
                   // stunString = "Grappled"; 
                   // stunDuration = 100f; 
                }

                if(DamageType == "LavaDamage")
                {
                    playerController.DoJump(550); 
                    stunString = "AttackHit";
                    DamageType = "EnvironmentDamage"; 
                    stunDuration = 10f;      
                }

                if(DamageType == "LaserDamage")
                {
                    DamageType = "EnvironmentDamage";
                    stunString = "AttackHit";
                    stunDuration = 10f;
                }

              
                playerController.ResetStates();
                playerController.ResetAnimator();
                playerController.isStunned = isStunned = true;
                playerController.playerAnim.SetTrigger(DamageType + "StunTrigger");

                playerState = (int)currentState.STUNNED;
                playerController.controllerState = (int)currentState.STUNNED;
                playerController.fixedControllerState = (int)currentState.STUNNED;                                          
                                                        
            }
        
        }

        if(currentHealth  <= 0)
        {
            currentHealth = 0f;
            KillPlayer(); 
        }
    }

    public void AddHealth(float addAmount)
    {
        currentHealth += addAmount;
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
        healthText.text = currentHealth.ToString(); 
        helathSlider.value = currentHealth / maxHealth;
    }

    public void StunPlayer(float stunTime, string stunType)
    {
  
        stunTimer += Time.deltaTime;

        if (stunTimer >= stunTime)
        {
            //canBeHit = true;
            stunTimer = 0f;
            playerController.isStunned = isStunned = false;
            playerState = (int)currentState.NOTHING;
            playerController.controllerState = (int)currentState.MOVING;
            playerController.fixedControllerState = (int)currentState.MOVING;
        }

        if(stunType == "AttackHit")
        {
            stunDuration = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length / playerAnim.GetCurrentAnimatorStateInfo(0).speed;
        }


    }

    void KillPlayer()
    {
        //gameManager.ResetScene(); 
        playerIsDead = true;
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
