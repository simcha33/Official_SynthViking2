using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.UI; 

  public enum pilarTypes
   {
        HealthPilar,
        TrapPilar,
   }

public class RewardPilarManager : MonoBehaviour
{
   
   public int maxPilarsInScene; 
    public int currentPilarsInScene;


   //public bool canSpawnPilar; 
   //public bool hasSpawnedPilar; 

    [Header ("Components")]
    public TextMeshProUGUI pilarStateText;   
    public TextMeshProUGUI pilarRewardText; 
    public EnemySpawnManager spawnManager; 
    public ObjectSpotManager spotManager; 

    public ThirdPerson_PlayerControler player;

   public bool textOnScreen;
   public float textDespawnTimer;
   public float textDespawnDuration; 

    public float minSpawnWaitDuration;
    public float maxSpawnWaitDuration; 
   private float pilarSpawnDuration;
   public  float pilarSpawnTimer; 

  // public float spawnDivergent; 

   public List<SoulPilar> pilarsInScene1; 

   public string choosenPilarType;


   



    void Start()
    {
        pilarSpawnDuration = Random.Range(minSpawnWaitDuration, maxSpawnWaitDuration);
        pilarSpawnTimer = 0f; 
        pilarStateText.alpha = pilarRewardText.alpha = 0; 
        player = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
    }

    void Update()
    {
        if(currentPilarsInScene < maxPilarsInScene && spawnManager.waveHasStarted)
        {
            CheckForPillarSpawn(); 
        }

        if(textOnScreen)
        {
            TextTimer(); 
        }
    }

    void CheckForPillarSpawn()
    {
        pilarSpawnTimer += Time.deltaTime; 
        if(pilarSpawnTimer >= pilarSpawnDuration && spotManager.freeSpotList.Count > 0)
        {
            //canSpawnPilar = true;
            SpawnPilar(); 
        }
    }

    void SpawnPilar()
    {    
        //Select spot
        int randomSpot = Random.Range(0, spotManager.freeSpotList.Count); 
        SpotScript choosenSpotScript = spotManager.freeSpotList[randomSpot].GetComponent<SpotScript>();
        choosenSpotScript.rewardPilar.gameObject.SetActive(true); 
        choosenSpotScript.inUse = true;
        choosenSpotScript.spotManager.freeSpotList.Remove(choosenSpotScript); 

        //Set pilar type
        SoulPilar choosenPilar = choosenSpotScript.rewardPilar.GetComponent<SoulPilar>(); 
        int randomPilarType = Random.Range(0, 1); 
        if(player.playerState.currentHealth < player.playerState.maxHealth / 4f) randomPilarType = 0;
        else
        {
            if(randomPilarType == 0) choosenPilar.pilarType = pilarTypes.HealthPilar.ToString(); 
            else if(randomPilarType == 1) choosenPilar.pilarType = pilarTypes.TrapPilar.ToString(); 
        }


        //Activate pilar   
        choosenPilar.hasSpawned = true; 

        currentPilarsInScene++;       
        pilarSpawnTimer = 0f; 
        pilarSpawnDuration = Random.Range(minSpawnWaitDuration, maxSpawnWaitDuration); 
        SetText("A SHRINE HAS APEARED", null); 
    }

    void TextTimer()
    {
        textDespawnTimer += Time.deltaTime; 
        if(textDespawnTimer > textDespawnDuration)
        {
            pilarStateText.alpha -= Time.deltaTime * .65f;
            pilarRewardText.alpha -= Time.deltaTime * .65f; 
            if(pilarStateText.alpha  <= 0 && pilarRewardText.alpha <= 0)
            { 
                textOnScreen = false;
                textDespawnTimer = 0; 
            }
        }
    }

    public void SetText(string stateText, string rewardText)
    {
        textDespawnTimer = 0; 
        textOnScreen = true;

        //State text 
        if(pilarStateText != null)
        { 
            pilarStateText.alpha = 1; 
            pilarStateText.text = stateText; 
        }

        //Reward text
        if(pilarRewardText != null)
        {
            pilarRewardText.text = rewardText;
            pilarRewardText.alpha = 1f; 
        }
      
   
    }

    

    

 

}
