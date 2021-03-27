using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// what it means to have an abstract class is that it is incomplete
// and needs to be implemented in the "derived class". 
public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    // the layer where we check for collision. 
    public LayerMask BlockingLayer;


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    private float inverseMoveTime;

    // Use this for initialization. protected virtual functions can be overwritten by their 
    // inheriting classes. useful if we want inheriting classes to have different implementation. 
    // protected - means that it is only visible inside the class and classes derived from it
    // virtual   - means that it can be overridden by derived classes.
    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        // doing it this way means that we can perform multiplications instead of 
        // divisions, which is more efficient computation wise. 
        inverseMoveTime = 1f / moveTime;
    }

    // SmoothMovement coroutine. 
    // end - The end parameter specifies where the object will move to
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // Calculate the remainingdistance by squaring the difference between our
        // current position and the end parameter. sqrMagnitude is computationally cheaper than
        // magnitude. 
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // while the remaining distance is greater than a very small number, almost zero
        while (sqrRemainingDistance > float.Epsilon)
        {
            // Vector3.MoveTowards() will take a point and move it in a straightline towards the 
            // target point. takes parameters: Vector3 current, Vector3 target, float maxDistanceDelta
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            // yield return null means that we will wait for a frame before reevaluating
            // the condition of our loop, i.e. the "sqrRemainingDistance > float.Epsilon"
            // just like in generators in python3
            yield return null;
        }
    }

    // out - the keyword causes the arguments  to be parsed by reference. In this case making it 
    // return more than one value from our move function, we have the bool value and also the 
    // RayvastHit2D called "hit" value.
    protected bool Move(int xdir, int ydir, out RaycastHit2D hit)
    {
        // transform.position is a vector3, but by casting it to a Vector2 we implicitly
        // convert it. 
        Vector2 start = transform.position;
        // make sure to have new in front of the Vector2 method. 
        Vector2 end = start + new Vector2(xdir, ydir);

        // disabling our boxcollider, to make sure that when we cast out our "Ray" we won't
        // hit our own boxcollider. 
        boxCollider.enabled = false;
        // here we cast a line between our start and end position to see if we detect a 
        // hit. 
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            // telling the function that we were able to move. 
            return true;
        }
        // return false to say that move was unsuccessful.
        return false;
    }



    // T - the generic, note generic, component T will specify the component we expect
    // our unit, note unit, to interact with when blocked. in the case of enemies it is going to be
    // the player, and in the case of the player, it is going to be the walls. 
    protected virtual void AttemptMove<T>(int xdir, int ydir)
        // where - we use the where keyword to specify that T is going to be a component. 
        where T : Component
    {
        RaycastHit2D hit;
        // I.e. true if successful or false if faield. 
        // that hit is the outparameter of Move, will allow us to check if hit is null
        bool canMove = Move(xdir, ydir, out hit);
        if(hit.transform == null)
        {
            // i.e. if we hadnt hit anything, we will not execute the following code
            return;
        }

        // if something was hit, we will get a component reference to the 
        // type T component, 
        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null)
        {
            onCantMove(hitComponent);
        }
    }

	// Takes a generic parameter called T . . .
    // here the abstract modified indicates that the thing being modified has a missing or incomplete
    // implementation. 
    // onCantMove is going to be overridden by functions in the inheriting classes. 
    // Because it is an abstract function, it has no opening or closing brackets. 
    // the reason we have it this way is because we want to be able to use AttemptMove in 
    // different objects, such as the player and the enemies, where the interactions will be different
    // and we thus do not know in advance what exact components the interactions will occur with
    // onCantMove will be specified for the respective object later on. Thus we use abstract component
    // declarations.
	protected abstract void onCantMove <T> (T component)
        where T : Component;
    
}
