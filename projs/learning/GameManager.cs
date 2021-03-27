using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // public such that it can be accessed outside of the class
    // static such that it belongs to the class, and not an instance of the class. 
    // now this means that we can access the public variables and functions of our game 
    // manager from any script in our game. 
    public static GameManager instance = null;

    public BoardManager boardScript;

    public int playerFoodPoints = 100;

    // we use the HideInInspector such that although it is a public variable, it will not be
    // displayed in the editor. 
    [HideInInspector] public bool playersTurn = true;

    // we wanna test level 3
    private int level = 3;



    public void GameOver()
    {
        // we disable the GameManager here. 
        enabled = false;
    }

    void Awake()
    {

        if(instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        // this is used when loading into a new scene, where the old one 
        // will be destroyed, upon the load of the new scene. in order to keep track
        // of shit, we do not want the destruction to occur immediately, and thus we do this. 
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
