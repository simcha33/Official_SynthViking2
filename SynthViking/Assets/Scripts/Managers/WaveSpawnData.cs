using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveSpawnData : ScriptableObject
{
    public int maxEnemiesInWave;
    public int maxEnemiesInScene;
    public int minGroupSpawnSize;
    public int maxGroupSpawnSize;
    public string WaveTitle; 
}
