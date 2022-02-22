using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class BasicEnemyScript : MonoBehaviour
{
    public bool isStunned;
    public bool canBeStunned;
    public bool canBeLaunched; 
    public bool isLaunched;

    public float currentHealth;
    public float maxHealth; 
    public float moveSpeed;
    public Animator enemyAnim;
    private Rigidbody enemyRb;
    public ThirdPerson_PlayerControler playerController; 

    [Header("FEEDBACKS")]
    #region
    public MMFeedbacks stunnedFeebacks; 
    #endregion


    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; 

    }


    void Update()
    {
        
    }


    public void LaunchEnemy(Vector3 direction, float force)
    {
        transform.position = playerController.transform.position + playerController.transform.forward + transform.up; 
        stunnedFeebacks?.PlayFeedbacks();
        enemyRb.useGravity = true;
        enemyRb.isKinematic = false;
        enemyRb.AddForce(direction * force, ForceMode.VelocityChange);
    }
       
    
    void StunEnemy()
    {

    }
}
