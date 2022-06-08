using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBall : MonoBehaviour
{

    public Transform target;
    public Transform eyeBall;
    public Transform barrel;
    private Transform hiddenPos;
    private Transform outPos; 
    public GameObject projectileObject;
    public bool canFollow;
    private Material defaultMat;
    public Material greenMat;
    public Material redMat;
    public MeshRenderer pupilMeshr; 

    public bool isLaserEye;
    public bool isProjectileEye;
    public bool canFire; 
    public bool isFiring;
    public bool hasAppeared; 

    public int projectilesFired;
    public int maxProjectiles;


    public float activeDuration;
    public float fireDelay; 

    public float cooldownDuration;
    private float cooldownTimer;


    private void Start()
    {
        defaultMat = pupilMeshr.material;
        ResetEyeball(); 
    }

    void Update()
    {
        if(canFollow) eyeBall.LookAt(target.position);
        if(isProjectileEye) CheckForProjectile(); 
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canFollow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canFollow = false; 
        }
    }

    void CheckForProjectile()
    {
        cooldownTimer += Time.deltaTime; 

        if(cooldownTimer >= cooldownDuration - 2)
        {
            pupilMeshr.material = greenMat; 
        }

        if(cooldownTimer >= cooldownDuration && projectilesFired < maxProjectiles)
        {
            FireProjectile();
            projectilesFired++;
            cooldownTimer -= fireDelay; 
        }
        else if(projectilesFired >= maxProjectiles)
        {
            ResetEyeball(); 
        }
    }

    void ResetEyeball()
    {
        cooldownTimer = 0f;
        projectilesFired = 0;
        pupilMeshr.material = defaultMat; 
    }

    void FireProjectile()
    {
        Instantiate(projectileObject, barrel.transform.position, barrel.transform.rotation);
    }

    void Appear()
    {
   
    }

}
