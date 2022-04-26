using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulObject : MonoBehaviour
{

    public GameObject soulSphere;
    public float stayDuration;
    public float stayTimer;
    public bool isBeingSucked;
    public bool hasBeenSucked;
    public Transform suckPoint;
    public float suckSpeed; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stayTimer += Time.deltaTime;
        if (stayTimer >= stayDuration && !isBeingSucked) RemoveObject();

        if (isBeingSucked) MoveTowardsSuckPoint(); 
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
   
    void MoveTowardsSuckPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, suckPoint.position, suckSpeed); 
        if(Vector3.Distance(transform.position, suckPoint.position) < .2f)
        {
            hasBeenSucked = true;
            isBeingSucked = false;
            RemoveObject();
        }
     
    }

}
