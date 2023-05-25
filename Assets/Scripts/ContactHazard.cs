using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module

public class ContactHazard : MonoBehaviour
{
    enum PushDirection
    {
        North,
        South,
        East,
        West
    };
    [SerializeField]
    private PushDirection defaultPushDirection = PushDirection.West;

    private Vector2 GetDefaultVector()
    {
        Vector2 push = new Vector2();
        if (defaultPushDirection == PushDirection.North)
        {
            push.y = 1;
        }
        if (defaultPushDirection == PushDirection.East)
        {
            push.x = 1;
        }
        if (defaultPushDirection == PushDirection.South)
        {
            push.y = -1;
        }
        if (defaultPushDirection == PushDirection.West)
        {
            push.x = -1;
        }
        return push;
    }

    private Vector2 SnapDirection(Vector2 otherPosition)
    {
        //get a direction from the two objects
        Vector2 hazardPosition = this.gameObject.transform.position;
        Vector2 push = (otherPosition - hazardPosition);
        //sets the distance to 1 basically
        push.Normalize();
        //can return push here if we want to knock back in any direction, but this is for cardinal directions

        //round the direction to a cardinal direction
        float xVal = 0f;
        float yVal = 0f;
        if (push.x > 0.01f || push.x < -0.01f)
        {
            xVal = push.x;
        }
        if (push.y > 0.01f || push.y < -0.01f)
        {
            yVal = push.y;
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
            
            push.y = 0f;
        }
        if (xVal <= yVal && yVal != 0f)
        {
            push.x = 0f;
            
        }
        push.Normalize();
        return push;
    }

    //re-work to snap direction by default instead based on relative position
    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            other.gameObject.SendMessage("HazardDamage", SnapDirection(other.transform.position), SendMessageOptions.DontRequireReceiver);
        }
    }
}
