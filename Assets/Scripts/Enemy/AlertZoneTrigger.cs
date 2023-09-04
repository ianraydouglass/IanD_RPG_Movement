using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior enemyBehavior;
    [SerializeField]
    private bool isProximityTrigger;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player entered alert area of enemy");
            //if it's a proximity trigger, it can alert the enemy during it's dismiss state
            if(isProximityTrigger)
            {
                enemyBehavior.DetectPlayer(other.gameObject, true);
            }
            else
            {
                enemyBehavior.DetectPlayer(other.gameObject, false);
            }
        }
    }
}
