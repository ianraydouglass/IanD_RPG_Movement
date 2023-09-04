using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHazard : MonoBehaviour
{
    [SerializeField]
    GameObject enemyContainer;

    private Vector2 PushDirection(Vector2 otherPosition)
    {
        //get a direction from the two objects
        Vector2 enemyPosition = this.gameObject.transform.position;
        Vector2 push = (otherPosition - enemyPosition);
        //sets the distance to 1 basically
        push.Normalize();
        return push;
    }

    //this is what actually damages the player
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {

            other.gameObject.SendMessage("EnemyDamage", enemyContainer, SendMessageOptions.DontRequireReceiver);
        }
    }
}
