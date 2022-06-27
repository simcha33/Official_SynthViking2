using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScript : MonoBehaviour
{
    public bool inUse;
    public string useType; 
    [HideInInspector] public ObjectSpotManager spotManager;
    [HideInInspector] public SoulPilar rewardPilar;
    public GameObject bladeTrap; 
    public GameObject hamerTrap;
    [HideInInspector] public EyeBall eyeball;

    public List<GameObject> trapList = new List<GameObject>(); 


    void Start()
    {
        spotManager = GameObject.Find("ObjectSpotManager").GetComponent<ObjectSpotManager>(); 
        rewardPilar = GetComponentInChildren<SoulPilar>();
        eyeball = GetComponentInChildren<EyeBall>();
        inUse = false; 
        rewardPilar.gameObject.SetActive(false);
        eyeball.gameObject.SetActive(false);

        foreach(GameObject trap in trapList)
        {
            trap.gameObject.SetActive(false); 
        }

     
    }

}
