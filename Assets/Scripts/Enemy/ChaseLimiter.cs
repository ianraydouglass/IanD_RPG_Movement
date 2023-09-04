using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseLimiter : MonoBehaviour
{
    [SerializeField]
    private float chaseDistance = 20f;

    public float GetChaseDistance()
    {
        return chaseDistance;
    }
}
