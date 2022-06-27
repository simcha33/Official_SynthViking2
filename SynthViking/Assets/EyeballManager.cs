using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.UI;

public class EyeballManager : MonoBehaviour
{

    public List<EyeBall> eyeballsInScene1 = new List<EyeBall>();

    [Header("Components")]

    public EnemySpawnManager spawnManager;
    public ObjectSpotManager spotManager;
    public ThirdPerson_PlayerControler player;

    public float minSpawnWaitDuration;
    public float maxSpawnWaitDuration;
    private float eyeSpawnDuration;
    public float eyeSpawnTimer;

    public int maxEyesInScene;
    public int currentEyesInScene; 
   

    public string choosenEyeType;






    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>();
    }

    void Update()
    {
        if (currentEyesInScene < maxEyesInScene && spawnManager.waveHasStarted && spawnManager.canSpawnEyeballs)
        {
            CheckForEyeSpawn();
        }
    }

    void CheckForEyeSpawn()
    {
        eyeSpawnTimer += Time.deltaTime;
        if (eyeSpawnTimer >= eyeSpawnDuration && spotManager.freeSpotList.Count > 0)
        {
            SpawnEye();
        }
    }

    void SpawnEye()
    {
        //Select spot
        int randomSpot = Random.Range(0, spotManager.freeSpotList.Count);
        SpotScript choosenSpotScript = spotManager.freeSpotList[randomSpot].GetComponent<SpotScript>();
        //   choosenSpotScript.rewardPilar.gameObject.SetActive(true);
        choosenSpotScript.eyeball.gameObject.SetActive(true); 
        choosenSpotScript.inUse = true;
        choosenSpotScript.spotManager.freeSpotList.Remove(choosenSpotScript);

        //Set eyeball type


        //Activate pilar   
        EyeBall choosenEyeball = choosenSpotScript.eyeball; 
        choosenEyeball.hasSpawned = true;

        currentEyesInScene++;
        eyeballsInScene1.Add(choosenEyeball); 
        eyeSpawnTimer = 0; 
        eyeSpawnDuration = Random.Range(minSpawnWaitDuration, maxSpawnWaitDuration);

    }
}


