using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class EyeBall : MonoBehaviour
{

    [Header("EYEBALLTYPE")]
    public bool isLaserEye;
    public bool isProjectileEye;


    [Header("SHOOTING")]
    public bool isFiring;
    public int projectilesFiredCount;
    public int maxProjectiles;
    public float fireDelay;
    public float cooldownDuration;
    private float cooldownTimer;
    public AudioClip shootSound;
    public AudioClip shotIndicationSound;
    public AudioClip destroyedSound;




    [Header("COMPONENTS")]
    public Transform target;
    public Transform eyeBall;
    public Transform barrel;
    private Transform hiddenPos;
    private Transform outPos;
    public GameObject projectileObject;
    public MeshRenderer pupilMeshr;
    [HideInInspector] public EyeballManager eyeBallManager;
    private EyeBall thisScript;
    public SpotScript attachedSpot;
    public List<HomingProjectile> projectilesFired = new List<HomingProjectile>();
    public ThirdPerson_PlayerControler playerController;
    public ComboManager _styleManager; 
    private AudioSource source;
    public AudioSource spotSource; 
    public MMFeedbacks shootFeedback; 

    [Header ("STATE")]
    public float maxHealth = 300f;
    public float currentHealth;
    public bool hasAppeared;
    public bool canFire;
    public bool canFollow;
    public bool hasSpawned;
    public bool isStunned;
    public float stunDuration;
    public float stunTimer;

    [Header("VISUALS")]
    private Material defaultMat;
    public Material greenMat;
    public Material redMat;
    public GameObject explodeEffect1;
    public GameObject explodeEffect2;
    public GameObject explodeEffect3;
    public GameObject shotIndicationEffect; 



    private void Start()
    {
        defaultMat = pupilMeshr.material;
        currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;
        playerController = target.gameObject.GetComponent<ThirdPerson_PlayerControler>(); 
       
        thisScript = gameObject.GetComponent<EyeBall>();
        eyeBallManager = GameObject.Find("EyeBallManager").GetComponent<EyeballManager>();
       source = GetComponent<AudioSource>();

        if (GetComponentInParent <SpotScript>() != null ) 
        {
            attachedSpot = GetComponentInParent<SpotScript>();
            spotSource = attachedSpot.GetComponent<AudioSource>();
        }
        ResetEyeball();
        
    
    }

    void Update()
    {
        if (isProjectileEye) CheckForProjectile();
        if (isStunned) CheckForStun();
        if(!isStunned) CheckForPlayer(); 
     
    }

    public void TakeDamage(float damageAmount, string DamageType)
    {
        currentHealth -= damageAmount;
        isStunned = true; 

        if(currentHealth <= 0)
        {
            KillEye(); 
        }
    }

    public void CheckForStun()
    {
        stunTimer += Time.deltaTime; 
        if(stunTimer >= stunDuration)
        {
            stunTimer = 0f; 
            isStunned = false; 
        }
    }

    void KillEye()
    {
        GameObject explosion1 = Instantiate(explodeEffect1, eyeBall.position, eyeBall.rotation);
        GameObject explosion2 = Instantiate(explodeEffect2, eyeBall.position, eyeBall.rotation);

        explosion1.AddComponent<CleanUpScript>();
        explosion2.AddComponent<CleanUpScript>();
        spotSource.PlayOneShot(destroyedSound);

        if (attachedSpot != null)
        {
            attachedSpot.inUse = false;
            attachedSpot.spotManager.freeSpotList.Add(attachedSpot);
            if(eyeBallManager.eyeballsInScene1.Contains(thisScript)) eyeBallManager.eyeballsInScene1.Remove(thisScript);
            eyeBallManager.currentEyesInScene--;
        }
        
        gameObject.SetActive(false);
        hasSpawned = false; 

        foreach(HomingProjectile projectile in projectilesFired)
        {
            projectile.DestroyProjectile(); 
        }

        projectilesFired.Clear();

        if (playerController.attackTargetScript.targetsInRange.Contains(this.gameObject))
        {
            playerController.attackTargetScript.targetsInRange.Remove(this.gameObject); 
        }
    }


    void CheckForPlayer()
    {
        if(Vector3.Distance(transform.position, target.position) < 500f) canFollow = true; 
        else canFollow = false;
        if(canFollow) eyeBall.LookAt(target.position);

    }


    void CheckForProjectile()
    {
        cooldownTimer += Time.deltaTime; 

        if(cooldownTimer >= cooldownDuration - 1)
        {
            shotIndicationEffect.SetActive(true);
            if (pupilMeshr.material == defaultMat) source.PlayOneShot(shotIndicationSound); 
            pupilMeshr.material = redMat; 
        }

        if(cooldownTimer >= cooldownDuration && projectilesFiredCount < maxProjectiles && !isStunned)
        {
            FireProjectile();
            projectilesFiredCount++;
            cooldownTimer -= fireDelay; 
        }
        else if(projectilesFiredCount >= maxProjectiles)
        {
            ResetEyeball(); 
        }
    }

    void ResetEyeball()
    {
        cooldownTimer = 0f;
        projectilesFiredCount = 0;
        pupilMeshr.material = defaultMat;
        shotIndicationEffect.SetActive(false);
    }

    void FireProjectile()
    {
       GameObject projectile = Instantiate(projectileObject, barrel.transform.position, barrel.transform.rotation);
       HomingProjectile projectileScript =  projectile.GetComponent<HomingProjectile>();
       projectileScript.origin = this.transform;
       projectilesFired.Add(projectileScript);
        //   shootFeedback?.PlayFeedbacks();
        source.PlayOneShot(shootSound); 
     

    }

    void Appear()
    {
   
    }

}
