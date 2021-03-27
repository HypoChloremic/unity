using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// instead of having the Player class inheriting from MonoBehaviour, 
// we will be inheriting from MovingObject. 
public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    // we use this to store a reference to our animator component!
    private Animator animator;

    // stores our food points in the current level before parsing it 
    // back to the GameManager. 
    private int food; 


    public void LoseFood(int loss)
    {
        animator.setTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }



    private void CheckIfGameOver()
    {
        if(food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }



    // we do a protected override here because we have a different implementation of Start
    // in the Player class compared to the MovingObject class. 
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;

        // i.e. calling the Start function of our base class which is the MovingObject class.
        base.Start();
    }




    // Update is called once per frame
    void Update () {
        if (!GameManager.instance.playersTurn) return;

        // store the direction we are moving as either 1, or -1 along the axis
        int horizontal = 0;
        int vertical = 0;

        // we are casting the Input from float to int by the '(int)' parameter
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");


        // do this to prevent the player from moving diagonally.
        if(horizontal != 0)
        {
            vertical = 0;
        }


        if(vertical != 0 || horizontal == 0)
        {
            // note that we are parsing the parameter Wall, which is an object the player can interact with. 
            AttemptMove(horizontal, vertical);
        }
	}


    // Now we are going to specifiy the AttemptMove method which returns void. 
    // note that it also is going to take a generic parameter T, specifiying the type of 
    // component the mover will encounter. 
    protected override void AttemptMove<T>(int xdir, int ydir)
    {
        // everytime the player moves, one foodpoint is lost. 
        food--;

        base.AttemptMove<T>(xdir, ydir);

        // such that we can reference the line cast in Move. s
        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }


    // note that a lower-case c for component is used here. 
    //protected override void onCantMove<T>(T component)
    //{

    //    // variable of type Wall
    //    // equals the component which was parsed in as a parameter, while casting that component to object Wall. 
    //    Wall hitWall = component as Wall;

    //    hitWall.DamageWall(wallDamage);

    //    // this is maybe the most important thing to note, haha. 
    //    animator.SetTrigger("playerMine");
    //    throw new System.NotImplementedException();
    //}






    //  ************************* API STUFF *************************

    // OnDisable is part of the Unity application API, 
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // this function is also part of the unity API
    // it will activate this will activate when we interact with an 
    // object trigger associated with a collider. so it can either be an exit sign object
    // or it can be a food item
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            // Invokes the string methodname after a given time, in this case
            // restartLevelDelay. 
            Invoke("Restart", restartLevelDelay);
        }else if (other.tag == "Food")
        {
            food += pointsPerFood;
        }else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
        }
    }

    private void Restart()
    {
        // because we are generating levels procedurally via script
        // we merely reload the same scene for a new level. 
        SceneManager.LoadScene(0);
    }


}
