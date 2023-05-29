using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_Movement : MonoBehaviour
{
    public Transform respawnLocation;
    private CheckPointManager_Movement checkPointManager;

    void Start()
    {
        if (!respawnLocation)
        {
            respawnLocation = this.gameObject.transform;
        }
    }
    //heals player on enter
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            checkPointManager.currentCheckPoint = this;
            other.gameObject.SendMessage("RecoverAllLife", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void RegisterWithManager(CheckPointManager_Movement thisManager)
    {
        checkPointManager = thisManager;
    }
}
