using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{

    public float rotationSpeed;
    public Transform planet; 


    private void FixedUpdate()
    {
        RotatePlanet();    
    }

    void RotatePlanet()
    {
        planet.transform.eulerAngles += new Vector3(0, rotationSpeed, 0); 
    }
}

