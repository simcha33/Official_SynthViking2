using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.UI; 

public class RewardPilarManager : MonoBehaviour
{
   
   public int maxPilarsInScene; 
    public int currentPilarsInScene;

   //public bool canSpawnPilar; 
   //public bool hasSpawnedPilar; 


   public TextMeshProUGUI pilarText; 
   public bool textOnScreen;
   public float textDespawnTimer;
   public float textDespawnDuration; 

    public float minSpawnWaitDuration;
    public float maxSpawnWaitDuration; 
   private float pilarSpawnDuration;
   public  float pilarSpawnTimer; 

  // public float spawnDivergent; 

   public List<SoulPilar> pilarsInScene1; 



    void Start()
    {
        pilarSpawnDuration = Random.Range(minSpawnWaitDuration, maxSpawnWaitDuration);
        pilarSpawnTimer = 0f; 
        pilarText.alpha = 0; 
    }

    void Update()
    {
        if(currentPilarsInScene < maxPilarsInScene)
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
        if(pilarSpawnTimer >= pilarSpawnDuration)
        {
            //canSpawnPilar = true;
            SpawnPilar(); 
        }
    }

    void SpawnPilar()
    {
        int randomPilar = Random.Range(0, pilarsInScene1.Count);
        pilarsInScene1[randomPilar].hasSpawned = true;  
        pilarsInScene1[randomPilar].gameObject.SetActive(true); 
        currentPilarsInScene++;       
        pilarSpawnTimer = 0f; 
        pilarSpawnDuration = Random.Range(minSpawnWaitDuration, maxSpawnWaitDuration); 
        SetText("A PILAR HAS APEARED"); 
    }

    void TextTimer()
    {
        textDespawnTimer += Time.deltaTime; 
        if(textDespawnTimer > textDespawnDuration)
        {
            pilarText.alpha -= Time.deltaTime * .65f;
            if(pilarText.alpha <= 0)
            { 
                textOnScreen = false;
                textDespawnTimer = 0; 
            }
        }
    }

    public void SetText(string text)
    {
        textDespawnTimer = 0; 
        textOnScreen = true;
        pilarText.alpha = 1; 
        pilarText.text = text; 
    }

    

    

 

}
