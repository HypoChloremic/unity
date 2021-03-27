using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnMovement : MonoBehaviour {
    [SerializeField]
    // so as to select the player gameobject,
    // for us to access its different parameters
    public GameObject player;

    public Vector3 offset;
	// Use this for initialization
	void Start () {

        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + offset;
	}
}
