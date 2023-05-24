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
        
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            return;
        }
        MovePlayer();
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
