using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior enemyBehavior;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player entered alert area of enemy");
            enemyBehavior.DetectPlayer(other.gameObject);
        }
    }
}
