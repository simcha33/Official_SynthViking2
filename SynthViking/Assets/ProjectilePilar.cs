using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePilar : MonoBehaviour
{
    public float maxSpawnDelay;
    public float minSpawnDelay;
    private float spawnDelayTimer; 
    public float spawnDelay;
    //public bool canSpawn; 
    public GameObject circleBeam; 


    void Start()
    {
        SetNewAttack();
    }

  
    void Update()
    {
        WaitForSpawnDelay(); 
    }

    void WaitForSpawnDelay()
    {
        spawnDelayTimer += Time.deltaTime; 
        if(spawnDelayTimer >= spawnDelay)
        {
            SpawnAttack(); 
            SetNewAttack(); 
        }
    }

    void SetNewAttack()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        spawnDelayTimer = 0f; 

    }

    void SpawnAttack()
    {
        Instantiate(circleBeam, transform.position + new Vector3(0, -2, 0), transform.rotation); 
    }


}
