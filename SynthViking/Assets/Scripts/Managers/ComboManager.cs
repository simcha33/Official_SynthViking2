using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class ComboManager : MonoBehaviour
{

    [Header("STYLE VALUES")]
   public int[] styleLevels;
  //  public List<int> styleLevels = new List<int>();
//    public List<float> maxStyleAmount = new List<float>(); 
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

    public float lowStyleScoreValueCap;
    public float mediumStyleScoreValueCap;
    public float highStyleScoreValueCap;
    public float extremeStyleScoreValueCap;


    [Header("COMBOS")]
    public float currentComboLength;
    public float comboTimer;
    public float maxTimeBetweenCombos; 



    [Header("UI")]
    // public Image styleMeter;
    public Slider styleMeter; 

    public GameObject styleSliderButton; 
    public Text styleText;
    public TextMeshProUGUI comboText; 
    public Text styleLevelText;
    public TextMeshPro currentStyleScoreText; 
    public TextMeshPro lowStyleScoreText; 
    public TextMeshPro highStyleScoreText; 
    public TextMeshPro mediumStyleScoreText; 
    public TextMeshPro extremeStyleScoreText; 

    

    public List<TextMeshPro> styleTextsInScene = new List<TextMeshPro>();

    public Camera cam; 
 //   public TextMesh currentStyleScoreText; 

 

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
        CheckForCombo(); 
        MoveTowardsStyleMeter(); 
    }

    public void AddStyle(string styleType, Transform styleSource)
    {
        // currentStyleAmount += gainedStyle; 

        float oldStyleAmount = currentStyleAmount;
        float styleDifference; 
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


        if (styleType == playerAttackType.HeavyAxeHit.ToString()) //Enemy is killed with axe 
        {
            currentStyleAmount += axeHitKillValue; 
        }

        styleDifference = currentStyleAmount - oldStyleAmount; //Check for style gained

        //Check which style text to select based on style gained
        if (styleDifference > 0 && styleDifference <= lowStyleScoreValueCap) currentStyleScoreText = lowStyleScoreText;
        else if (styleDifference > lowStyleScoreValueCap && styleDifference <= mediumStyleScoreValueCap) currentStyleScoreText = mediumStyleScoreText;
        else if (styleDifference > mediumStyleScoreValueCap && styleDifference <= highStyleScoreValueCap) currentStyleScoreText = highStyleScoreText;
        else if (styleDifference > highStyleScoreValueCap) currentStyleScoreText = extremeStyleScoreText;

        
        TextMeshPro styleScoreText = Instantiate(currentStyleScoreText, styleSource.position + new Vector3(0, 2.3f, 0), playerController.transform.rotation); 
        styleScoreText.text = styleDifference.ToString();
        styleScoreText.GetComponent<ScoreText>().target = styleSource;     
       // }
    }

    void SetUIDepth()
    {

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

    void MoveTowardsStyleMeter()
    {
        if(styleTextsInScene.Count > 0)
        {
            foreach(TextMeshPro text in styleTextsInScene)
            {
                Vector3 screenPoint = styleSliderButton.transform.position; 
                Vector3 sliderWorldpos = cam.ScreenToWorldPoint(screenPoint); 

               //text.transform.position = Vector3.MoveTowards(text.transform.position, sliderWorldpos, .04f); 
             //  text.transform.LookAt(cam.transform.forward);
                if(Vector3.Distance(text.transform.position, styleMeter.transform.position) < .5f)
                {
                    styleTextsInScene.Remove(text); 
                    Destroy(text); 
                }
            }
        }
    }    
    

    void CheckForStyle()
    {
        timeSinceLastStyle += Time.deltaTime; 

        if(timeSinceLastStyle > maxTimeBetweenstyle && currentStylelevel > 0)
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

    public void AddCombo()
    {
        //Combo stuff
        currentComboLength++;
        comboTimer = 0f;
        timeSinceLastStyle = 0f;
        if (comboText.alpha < 1) comboText.alpha = 1; 
    }

    public void ResetCombo()
    {
        currentComboLength = 0f;
        comboTimer = 0f;
    }


    void CheckForCombo()
    {
    
        if(currentComboLength > 0)
        {
            comboTimer += Time.deltaTime;
            
            if(comboTimer >= maxTimeBetweenCombos)
            {
                comboText.alpha -= Time.deltaTime;
                if (comboText.alpha <= 0)
                {
                    ResetCombo();
                }
            }
        }
        
        if(comboText.alpha > 0 && currentComboLength <= 0)
        {
            comboText.alpha -= Time.deltaTime; 
        }

        comboText.text = "X" + currentComboLength.ToString(); 
    }

}
