using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager_Movement : MonoBehaviour
{
    public List<CheckPoint_Movement> checkPoints = new List<CheckPoint_Movement>();

    [SerializeField]
    private CheckPoint_Movement defaultCheckPoint;

    public CheckPoint_Movement currentCheckPoint;

    void Start()
    {
        currentCheckPoint = defaultCheckPoint;
        RegisterCheckPoints();
    }

    void RegisterCheckPoints()
    {
        foreach(CheckPoint_Movement checkPoint in checkPoints)
        {
            checkPoint.RegisterWithManager(this);
        }
    }

    public void RespawnPlayer(GameObject playerObject)
    {
        //set important metrics and bools for player
        playerObject.transform.position = currentCheckPoint.respawnLocation.position;
    }
}
