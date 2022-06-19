using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{

    public Vector3 rotationSpeed; 
    public Transform planet;
 


    private void FixedUpdate()
    {
        RotatePlanet();    
    }

    void RotatePlanet()
    {
        planet.transform.eulerAngles += new Vector3(rotationSpeed.x, rotationSpeed.y , rotationSpeed.z);
    
    }
}

