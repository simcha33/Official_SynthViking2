using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class RewardPilarManager : MonoBehaviour
{

 

    public bool activePilarInScene;

    public RewardPilar activePilar;
    public Transform orbPoint;
    public int soulsNeeded;
    public int currentSoulsFed;
    public bool isFullyFed;

    public float stayTimer;
    public float stayDuration;
    public EnemyManager enemyManager;
    public PlayerState playerState;
    public ThirdPerson_PlayerControler playerController;
    public GameManager gameManager;
    public TextMeshPro pilarText;
    public string currentPilarType;
    [HideInInspector] public List<SoulObject> suckedSouls = new List<SoulObject>();
    [HideInInspector] public List<RewardPilar> pilarLocations = new List<RewardPilar>();

    public enum pilarType
    {
        healthPilar,
        trapPilar,
    }



    // Start is called before the first frame update
    void Start()
    {
        activePilarInScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTime();
        if (activePilarInScene) CheckForSouls();
    }

    void CheckForNewSpawn()
    {

    }

    void CheckForSouls()
    {
        //  foreach(GameObject enemy in enemyManager)
        if (currentSoulsFed >= soulsNeeded)
        {
            isFullyFed = true;
            DoPilarReward();
            RemovePilar();
        }
    }

    void DoPilarReward()
    {
        print("Very nice here is your rewawarddd");

        if (currentPilarType == pilarType.healthPilar.ToString())
        {
            pilarText.text = "MAX HEALTH INCREASED";
        }

        if (currentPilarType == pilarType.trapPilar.ToString())
        {
            pilarText.text = "TRAP ACTIVATED";
        }


    }

    void CheckForTime()
    {
        stayTimer += Time.deltaTime;
        if (stayTimer >= stayDuration)
        {
            RemovePilar();
        }
    }

    void RemovePilar()
    {
        activePilarInScene = false;
        foreach (SoulObject souls in suckedSouls)
        {
            souls.RemoveObject();
        }

        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SoulObject") && !isFullyFed)
        {
            SoulObject soulObjectScript = other.gameObject.GetComponent<SoulObject>();
            soulObjectScript.isBeingSucked = true;
            soulObjectScript.suckPoint = orbPoint;
            suckedSouls.Add(soulObjectScript);
        }
    }
}
