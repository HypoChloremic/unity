using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMovement : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // remember that OnTriggerEnter2D is inherited from
    // Monobehavior, and what we are doing now is overriding
    // the method, with some tweaks. 
    void OnTriggerEnter2D()
    {
        Debug.Log("Hello world");
    }
}
