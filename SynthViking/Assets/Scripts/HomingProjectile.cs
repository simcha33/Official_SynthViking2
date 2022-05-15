using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class HomingProjectile : MonoBehaviour
{
    [Header("STATS")]
    public float projectileDamage;
    public float aliveDuration;
    public float moveSpeed;
    public float rotateSpeed; 

    private float aliveTimer;
    [HideInInspector] public bool hasTouchedTarget;
    private Rigidbody rb; 
    private Transform target;

    [Header("VISUALS")]
    public GameObject destructionEffect;  



    public 


    void Start()
    {
        SetTarget(); 
        aliveTimer = 0f; 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
      //  ChaseTarget(); 
    }

     void FixedUpdate() 
     {
        ChaseTarget();    
    }

    void SetTarget()
    {
        target = GameObject.Find("Player").transform; 
        
    }

    void ChaseTarget()
    {
        // transform.position = Vector3.MoveTowards(transform.position, target.position  + new Vector3(0,2f,0), moveSpeed * Time.fixedDeltaTime); 

        //  rb.AddForce((targetPos - transform.position) * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange); //Steering force
        // rb.AddForce((targetPos - transform.position) * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        Vector3 targetPos = target.transform.position + new Vector3(0, 2f, 0);
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();    
        rb.angularVelocity = -Vector3.Cross(dir, transform.forward) * rotateSpeed;
        rb.velocity = transform.forward * moveSpeed; 
   
        /*
        //Look at target
      
        Vector3 relativePos = targetPos - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lookSpeed * Time.fixedDeltaTime);

        //Add force
        rb.AddForce(transform.forward * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);
        */

        /*
        if(rb.velocity.magnitude >= moveSpeed)
        {
            if(!maxSpeedReached)
            { 
                maxvelocity = rb.velocity; 
                maxSpeedReached =  true;
            }

            rb.velocity = maxvelocity; 
        }
        else{
            maxSpeedReached = false; 
        }
        
        aliveTimer += Time.deltaTime;
        if(aliveTimer >= aliveDuration) DestroyProjectile(); 

        */


    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerState>().TakeDamage(projectileDamage, "Projectile");
            hasTouchedTarget = true; 
            DestroyProjectile();  
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            DestroyProjectile();  
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().TakeDamage(projectileDamage, "Projectile"); 
        }
    }

    void Damagetarget()
    {

    }

    void DestroyProjectile()
    {   
        //Play some kind of an effect

        Destroy(gameObject); 
    }

    // Update is called once per frame
   
}
