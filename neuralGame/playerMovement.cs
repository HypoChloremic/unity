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
    Vector2 jumpForce = new Vector2(0,1f);
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

            // note that all unity gameObjects possess a transform class, which
            // then contains the orientation of the object in the game space. 
            // Thus, by accessng transform, which is short in this case for 
            // Component, i.e. this gameObject, we can change its position, 
            // by employing the translate method. 
            transform.Translate(speed * Time.deltaTime / 5f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("keycode Space pressed");
                // Aight, so we have from before gotten the rigidbody2d
                // by GetComponent<RigidBody2D>() method. I assume that this
                // method is generic and waiting for a (object)type to be 
                // passed, such that it will return the actual object
                // which we then can edit. 
                // the Rigidbody2d object in turn has an addforce method
                // where we define a vector, and a ForceMode which tells the addforce
                // method how it will interact with the provided vector, whether
                // it is an acceleration or a force impulse etc.
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
            }

        }
	}
}
