using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreText : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public float lifeTime = 3f;
    public TextMeshPro text;
    public Camera cam; 

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        text = this.gameObject.GetComponent<TextMeshPro>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            lifeTime -= Time.deltaTime;
            text.transform.LookAt(cam.transform.forward);
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            if (lifeTime <= 0) text.alpha -= Time.deltaTime;
            if (text.alpha <= 0) Destroy(this.gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
