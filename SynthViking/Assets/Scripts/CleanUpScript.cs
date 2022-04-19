using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpScript : MonoBehaviour
{

    private float destructionDuration = 5f;
    public float destructionTimer;
    // [HideInInspector] public List<GameObject> objectsToClean = new List<GameObject>();
    GameObject objectToDestroy;

    private void Start()
    {
       // objectToDestroy = GetComponent<GameObject>(); 
    }

    private void Update()
    {
       
        destructionTimer += Time.deltaTime; 

        if(destructionTimer >= destructionDuration)
        {
            Destroy(GetComponent<GameObject>()); 
            Destroy(this.gameObject); 
        }
        

    }

    public void SetCleanUp(float duration)
    {
        destructionDuration = duration; 
    }

}
