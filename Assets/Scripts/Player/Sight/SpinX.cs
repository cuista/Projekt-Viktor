using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinX : MonoBehaviour
{
	
	public float speed = 10f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0, speed, 0); //-> affect even the position of the object
        transform.Rotate(0,speed * Time.deltaTime,0); //-> Con il Time.deltaTime lo rendo FRAME RATE INDIPENDENT
    }
}
