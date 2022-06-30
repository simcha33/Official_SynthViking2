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
    //   public float[] maxStyleAmount;
    //    public int currentStylelevel;
    public float currentStyleAmount;
    public float levelMaxStyleAmount;
    private float timeSinceLastStyle;
    public float maxTimeBetweenstyle;
    public float styleDecreaseValue;
    public bool styleMeterFUll;
    //   public float styleMultiplier;

    [Header("SCOREVALUES")]
    public float axeHitKillValue = 5f;
    public float puncHitKillValue = 5f;
    public float projectileParryKillValue; 
    public float powerPunchKillValue = 15f;
    public float groundSlamKillValue = 1f;
    public float environmentalKillValue = 20f;
    public float parryValue = 1f;
    public float impactDamageKillValue = 20f;

    //   public float lowStyleScoreValueCap;
    //  public float mediumStyleScoreValueCap;
    //  public float highStyleScoreValueCap;
    //  public float extremeStyleScoreValueCap;

    [Header("VISUALS")]

    [Header("COMPONENTS")]
    public EnemySpawnManager _spawnManager;
    public ThirdPerson_PlayerControler player;
    public GameObject rageButtonImage;

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
    //  public Text styleLevelText;
    //  public TextMeshPro currentStyleScoreText; 
    //  public TextMeshPro lowStyleScoreText; 
    //  public TextMeshPro highStyleScoreText; 
    //  public TextMeshPro mediumStyleScoreText; 
    // public TextMeshPro extremeStyleScoreText; 



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
        // SetPlayerLevel(0);   
        rageButtonImage.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        CheckForStyle();
        CheckForCombo();
        MoveTowardsStyleMeter();
    }

    public void AddStyle(string styleType)
    {
        // currentStyleAmount += gainedStyle; 

        float oldStyleAmount = currentStyleAmount;
        float styleDifference;
        float minStyleResetValue = 5f;
        float styleToAdd = 0;
        timeSinceLastStyle = 0f;

        if (!styleMeterFUll)
        {

            if (styleType == playerAttackType.PowerPunch.ToString()) //Enemy is killed power punch
            {
                styleToAdd = powerPunchKillValue;
            }

            if (styleType == playerAttackType.GroundSlam.ToString()) //Enemy is killed by ground slam 
            {
                styleToAdd = groundSlamKillValue;
            }

            if (styleType == playerAttackType.BlockStun.ToString()) //Enemy is parried
            {
                styleToAdd = parryValue;
            }

            if (styleType == "ImpactDamage")
            {
                styleToAdd = impactDamageKillValue;
            }

            if(styleType == "EnvironmentDamage")
            {
                styleToAdd = environmentalKillValue; 
            }

            // if(styleType == "ChainHit")


            if (styleType == playerAttackType.HeavyAxeHit.ToString()) //Enemy is killed with axe 
            {
                styleToAdd = axeHitKillValue;
            }

            if (styleType == playerAttackType.LightPunchHit.ToString())
            {
                styleToAdd = puncHitKillValue;
            }

            if(styleType == "ProjectileParry")
            {
                styleToAdd = projectileParryKillValue; 
            }

            currentStyleAmount += styleToAdd;

            if (currentStyleAmount >= levelMaxStyleAmount && !styleMeterFUll)
            {
                styleMeterFUll = true;
                playerController.canRage = true;
                currentStyleAmount = levelMaxStyleAmount;
                rageButtonImage.SetActive(true);
            }
            styleDifference = currentStyleAmount - oldStyleAmount; //Check for style gained
        }
    }



    void MoveTowardsStyleMeter()
    {
        if (styleTextsInScene.Count > 0)
        {
            foreach (TextMeshPro text in styleTextsInScene)
            {
                Vector3 screenPoint = styleSliderButton.transform.position;
                Vector3 sliderWorldpos = cam.ScreenToWorldPoint(screenPoint);

                //text.transform.position = Vector3.MoveTowards(text.transform.position, sliderWorldpos, .04f); 
                //  text.transform.LookAt(cam.transform.forward);
                if (Vector3.Distance(text.transform.position, styleMeter.transform.position) < .5f)
                {
                    styleTextsInScene.Remove(text);
                    Destroy(text);
                }
            }
        }
    }


    void CheckForStyle()
    {

        if (_spawnManager.waveHasStarted) timeSinceLastStyle += Time.deltaTime;
        else timeSinceLastStyle = 0f;

        if (timeSinceLastStyle > maxTimeBetweenstyle && currentStyleAmount > 0 && (!styleMeterFUll || playerController.isRaging))
        {
            DecreaseStyle();
        }

        styleMeter.value = currentStyleAmount / levelMaxStyleAmount;
    }

    void DecreaseStyle()
    {
        currentStyleAmount -= styleDecreaseValue * Time.deltaTime;
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

        if (currentComboLength > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= maxTimeBetweenCombos)
            {
                comboText.alpha -= Time.deltaTime;
                if (comboText.alpha <= 0)
                {
                    ResetCombo();
                }
            }
        }

        if (comboText.alpha > 0 && currentComboLength <= 0)
        {
            comboText.alpha -= Time.deltaTime;
        }

        comboText.text = "X" + currentComboLength.ToString();
    }

}
