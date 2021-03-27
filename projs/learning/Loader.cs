using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    // this will allow us to drag in the GameManager prefab to the this
    // GameObject gameManager, as a parameter. 
    public GameObject gameManager;
    
    // Use this for initialization
	void Awake() {
		if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
	}

}
