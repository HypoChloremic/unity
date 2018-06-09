using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// I assume that this is class (module?) is necessary
// is necessary for running it on
//using UnityStandardAssets.CrossPlatformInput;


public class playerMovement : MonoBehaviour {

    
    // such that we can interact with the animations?
    Rigidbody2D rb;
    Animator anim;
    [SerializeField]
    float jumpForce = 500f;
    float upDown;
    [SerializeField]
    Vector2 speed = new Vector2(5f, 0f);

    // Use this for initialization
    void Start () {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameControl.gameStopped != true)
        {
            // transform allows us to interact with the player object
            // if we then translate by using a specific vecot, we are allowed to produce
            // changes in the position of the dinosaur
            // the reason that we have this inherently, at the start of the method, is that
            // the player will constantly be moving. 
            transform.Translate(speed * Time.deltaTime / 5f);
        }
	}
}
