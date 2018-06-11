using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// note that the colon in c# refers to inheritance
public class GameControl : MonoBehaviour {
    // Regarding transform, it seems that all objects in unity
    // have a transform. 
    public static GameControl instance = null;

    public static bool gameStopped = false;

    [SerializeField]
    // in effect, we are serializing this
    // such that we will be passing the obstacle
    // objects from the unity editor, instead of manually
    // doing it here. 
    public GameObject[] obstacles;

    [SerializeField]
    // In essence what this means, is that we are
    // creating an object of type transform (there may be
    // objects that themselves contain the transform object
    // inheriting it in effect, but in this case we are 
    // solely interested in the transform object, and thats
    // it), which we can use in turn. 
    Transform spawnPoint;
    
	// Use this for initialization
	void Start () {
        Debug.Log(obstacles.ToString());
        spawnObstacle();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawnObstacle()
    {
        // I guess that this will generate an integer, and not a
        // floating point number. We do this to randomize an index
        // such that we select an obstacle object randomly. 
        int someNumber = Random.Range(0, obstacles.Length);
        // in effect Instantiate is a method, which I assume
        // uses new to generate a new object, and passes the 
        // transform object containing the position of the new
        // gameobject, whilst also establishing whether this is 
        // in relation to the parent or not?
        //
        // remember that the transform is an object, in order to access
        // its position, which is a Vector3, we need to use the .position object
        // inside transform
        Instantiate(obstacles[someNumber], spawnPoint.position, Quaternion.identity);
    }
}
