using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveSpawnData : ScriptableObject
{
 //   public int maxEnemiesInWave;
    public int maxEnemiesInScene;
    public int minGroupSpawnSize;
    public int maxGroupSpawnSize;
    public string WaveTitle; 
    public int maxGruntsToSpawn; 
    public int maxBigGuyToSpawn; 
    public int maxTortiToSpawn;
    public float groupSpawnCooldownDuration;
    public float waveCountdownDuration; 
    public int eventToTrigger; 
    public bool canSpawnPilars;
    public bool canTriggerEvent;
    public bool canSpawnEyeballs;
    public bool canSpawnLasers; 
}
