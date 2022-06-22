using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulObject : MonoBehaviour
{

    //public GameObject soulSphere;
    [HideInInspector] public SoulPilar attachedSoulPilar; 
    public float soulValue;
    public float stayDuration;
    private float stayTimer;
    public bool isBeingSucked;

    public bool soulIsFree; 
    //public bool hasBeenSucked;
    [HideInInspector] public Vector3 suckPoint;
  
    [HideInInspector] public  float suckSpeed; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //stayTimer += Time.deltaTime;
        //if (stayTimer >= stayDuration && !isBeingSucked) RemoveObject();
        if (isBeingSucked) MoveTowardsSuckPoint(); 
        if(soulIsFree && !isBeingSucked) stayTimer += Time.deltaTime;
        if(stayTimer >= stayDuration) RemoveObject(); 
        
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
   
    public void MoveTowardsSuckPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, suckPoint, suckSpeed); 
        if(Vector3.Distance(transform.position, suckPoint) < .3f)
        {
          //  hasBeenSucked = true;
            isBeingSucked = false;
            attachedSoulPilar.AddSoul(soulValue); 
            RemoveObject();
        }
     
    }

}
