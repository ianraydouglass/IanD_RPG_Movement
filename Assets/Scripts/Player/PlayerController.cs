using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//created for the Ruins of Vha version 2 movement module
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    public void OnMove(InputValue input)
    {
        playerMovement.movementVector = input.Get<Vector2>();
    }

    public void OnSprint(InputValue input)
    {
        float sprintVal = input.Get<float>();
        if (sprintVal > 0.01)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }
}
