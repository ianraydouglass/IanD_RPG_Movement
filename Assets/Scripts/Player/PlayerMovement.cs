using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7.5f;
    [SerializeField]
    private float sprintSpeed = 10f;

    [SerializeField]
    private Rigidbody2D playerBody;

    [SerializeField]
    private Animator animator;

    //set by controller object
    public Vector2 movementVector;
    //recorded value from moving for animator idle
    private Vector2 stopVector;


    //if we need to store the player's position briefly
    public Vector2 storedPosition;
    //if the player's movement needs to be frozen, this is set to true until un-frozen
    public bool isStopped = false;

    public bool isSprinting = false;
    

    void Start()
    {
        
    }

    

    void Update()
    {
        if (isStopped)
        {
            return;
        }
        AnimatePlayer();
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            return;
        }
        MovePlayer();
        StoreDirection();
    }

    void MovePlayer()
    {
        float newSpeed = moveSpeed;
        if (isSprinting)
        {
            newSpeed = sprintSpeed;
        }
        playerBody.MovePosition(playerBody.position + movementVector * newSpeed * Time.fixedDeltaTime);
    }

    //store the last direction moved-in
    void StoreDirection()
    {
        float xVal = 0f;
        float yVal = 0f;
        if (movementVector.x > 0.01f || movementVector.x < -0.01f)
        {
            xVal = movementVector.x;
            //stopVector.x = movementVector.x;
        }
        if (movementVector.y > 0.01f || movementVector.y < -0.01f)
        {
            yVal = movementVector.y;
            //stopVector.y = movementVector.y;
        }
        //make them both positive
        if (xVal < 0)
        {
            xVal = -xVal;
        }
        if (yVal < 0)
        {
            yVal = -yVal;
        }
        //compare them
        if (xVal > yVal)
        {
            stopVector.x = movementVector.x;
            stopVector.y = 0f;
        }
        if (xVal <= yVal && yVal != 0f)
        {
            stopVector.x = 0f;
            stopVector.y = movementVector.y;
        }
    }

    //sends the direction moved, whether or not we are moving, and instructions for what direction we were facing last
    void AnimatePlayer()
    {
        animator.SetFloat("horizontal", movementVector.x);
        animator.SetFloat("vertical", movementVector.y);
        animator.SetFloat("speed", movementVector.sqrMagnitude);
        animator.SetFloat("stop horizontal", stopVector.x);
        animator.SetFloat("stop vertical", stopVector.y);
    }

    void FreezeAnimatePlayer()
    {
        animator.SetFloat("speed", 0f);
    }

    public void Freeze()
    {
        Debug.Log("Player Movement Frozen");
        isStopped = true;
    }

    public void UnFreeze()
    {
        Debug.Log("Player Movement UnFrozen");
        isStopped = false;
    }
}
