using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBeam : MonoBehaviour
{

    public float expandTimer;
    public float expandDuration;
    public float expandSpeed;
    public Vector3 circleScale; 


    public float maxSize; 

    void Start()
    {
        
    }

    void Update()
    {
        ExpandCircle(); 
    }

    void ExpandCircle()
    {
        expandTimer += Time.deltaTime;
    
        circleScale += new Vector3(Time.deltaTime * expandSpeed, 0, Time.deltaTime * expandSpeed);
        transform.localScale += circleScale;
    
        if(expandTimer >= expandDuration)
        {
            DestroyAttack(); 
        }

        //  if (expandTimer < expandDuration) transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, transform.localScale.y, maxSize), (expandTimer / expandDuration) * Time.deltaTime);          
        //  else if(expandTimer >= expandDuration) DestroyAttack();

    }

    private void DestroyAttack()
    {
        Destroy(this.gameObject);
    }
}
