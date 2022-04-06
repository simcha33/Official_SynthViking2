using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ComboManager : MonoBehaviour
{

    [Header("STYLE VALUES")]
    public int[] styleLevels;
    public float[] maxStyleAmount;
    public int currentStylelevel;


    public float currentStyleAmount;
    public float levelMaxStyleAmount;

    private float timeSinceLastStyle;
    public float maxTimeBetweenstyle;
    public float styleDecreaseValue;
    public float styleMultiplier;

    [Header("SCOREVALUES")]
    public float axeHitKillValue = 5f;
    public float powerPunchKillValue = 15f;
    public float groundSlamKillValue = 1f;
    public float environmentalKillValue = 20f;
    public float parryValue = 1f;
    public float impactDamageKillValue =20f;


    [Header("UI")]
    // public Image styleMeter;
    public Slider styleMeter; 
    public Text styleText;
    public Text styleLevelText;

 

    public enum styleTypes
    {
        axeAttack,
        Parry,
        Stunts,
        CC,
    }

    [Header("SCRIPTS")]
    public ThirdPerson_PlayerControler playerController;
    public AttackTargetScript playerAttackScript;
    public PlayerState playerStateScript; 


    void Start()
    {
        SetPlayerLevel(0); 
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForStyle(); 
    }

    public void AddStyle(string styleType)
    {
        // currentStyleAmount += gainedStyle; 
        Debug.Log("StyleSet"); 

        float oldStyleAmount = currentStyleAmount;
        float minStyleResetValue = 5f; 

        if (styleType == playerAttackType.PowerPunch.ToString()) //Enemy is killed power punch
        {
            currentStyleAmount += powerPunchKillValue; 
        }

        

        if (styleType == playerAttackType.GroundSlam.ToString()) //Enemy is killed by ground slam 
        {
            currentStyleAmount += groundSlamKillValue; 
        }

        if (styleType == playerAttackType.BlockStun.ToString()) //Enemy is parried
        {
            currentStyleAmount += parryValue; 
        }

        if(styleType == "ImpactDamage" )
        {
            currentStyleAmount += impactDamageKillValue;
        }

       // if(styleType == "ChainHit")


        if (styleType == playerAttackType.LightAxeHit.ToString()) //Enemy is killed with axe 
        {
            currentStyleAmount += axeHitKillValue; 
        }

       // if(currentStyleAmount - oldStyleAmount > minStyleResetValue) //If enough style points where gathered 
       // {
            timeSinceLastStyle = 0f; 
       // }


    }

    void SetPlayerLevel(int levelDirection)
    {
        if(levelDirection > 0) currentStyleAmount -= levelMaxStyleAmount;
        //else if (levelDirection < 0) currentStylelevel += levelDirection;
        currentStylelevel += levelDirection; 

        levelMaxStyleAmount = maxStyleAmount[currentStylelevel];
        styleLevelText.text = currentStylelevel.ToString();
        timeSinceLastStyle = 0f;
    } 

    
     
    

    void CheckForStyle()
    {
        timeSinceLastStyle += Time.deltaTime; 

        if(timeSinceLastStyle > maxTimeBetweenstyle)
        {
            DecreaseStyle(); 
        }

        if(currentStyleAmount > levelMaxStyleAmount)
        {
            SetPlayerLevel(1); 
        }

        styleMeter.value = currentStyleAmount / levelMaxStyleAmount;
    }

    void DecreaseStyle()
    {
        currentStyleAmount -= styleDecreaseValue * Time.deltaTime;

        if (currentStyleAmount <= 0 && currentStylelevel > 0)
        {
            SetPlayerLevel(-1); 
        }

     
    }

}
