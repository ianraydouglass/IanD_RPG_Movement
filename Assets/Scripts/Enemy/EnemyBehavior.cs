using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    enum EnemyMode
    {
        Idle, //a safety option for transitions
        Patrol, //the enemy is moving from point to point
        Alert, //the enemy notices the player and plays the emote animation
        Chase, //the enemy is pursuing the player
        Gloat, //the enemy has hit the player, and is taking a moment to gloat
        Dismiss //the player is outside of the greater pursuit range and the enemy returns to patrol
    };

    private EnemyMode thisMode = EnemyMode.Idle;

    private int patrolIndex = 0;

    [SerializeField]
    private Rigidbody2D enemyBody;

    [SerializeField]
    private List<GameObject> patrolPoints = new List<GameObject>();

    [SerializeField]
    private GameObject patrolZone;

    [SerializeField]
    private GameObject alertZone;

    [SerializeField]
    private GameObject alertAnchor;

    [SerializeField]
    private float patrolSpeed = 7f;
    [SerializeField]
    private float chaseSpeed = 9.5f;
    [SerializeField]
    private float dismissSpeed = 8.25f;
    [SerializeField]
    private float gloatTime = 1f;
    [SerializeField]
    private float alertTime = 1f;
    [SerializeField]
    private float closeRange = 0.1f;

    private GameObject targetPlayer;

    private Vector2 MoveDirection(Vector2 otherPosition)
    {
        //get a direction from the two objects
        Vector2 enemyPosition = this.gameObject.transform.position;
        Vector2 direction = (otherPosition - enemyPosition);
        //sets the distance to 1 basically
        direction.Normalize();
        return direction;
    }

    private Vector2 GetTarget()
    {
        Vector2 targetVector = new Vector2(0, 0);
        if (thisMode == EnemyMode.Patrol || thisMode == EnemyMode.Dismiss)
        {
            if (patrolPoints.Count == 0)
            {
                return targetVector;
            }
            targetVector = patrolPoints[patrolIndex].transform.position;
        }
        if (thisMode == EnemyMode.Chase)
        {
            if (!targetPlayer)
            {
                return targetVector;
            }
            targetVector = targetPlayer.transform.position;
        }
        return targetVector;
    }

    private bool CanMove()
    {
        if (thisMode == EnemyMode.Idle || thisMode == EnemyMode.Alert || thisMode == EnemyMode.Gloat)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool HasArrived()
    {
        if(patrolPoints.Count == 0)
        {
            return false;
        }
        float distance = Vector2.Distance(enemyBody.position, patrolPoints[patrolIndex].transform.position);
        if (distance <= closeRange)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    public float CurrentSpeed()
    {
        float resultSpeed = 0f;
        if (thisMode == EnemyMode.Chase)
        {
            resultSpeed = chaseSpeed;
        }
        if (thisMode == EnemyMode.Patrol)
        {
            resultSpeed = patrolSpeed;
        }
        if (thisMode == EnemyMode.Dismiss)
        {
            resultSpeed = dismissSpeed;
        }
        return resultSpeed;
    }

    void Start()
    {
        thisMode = EnemyMode.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(CanMove())
        {
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {
        enemyBody.MovePosition(enemyBody.position + MoveDirection(GetTarget()) * CurrentSpeed() * Time.fixedDeltaTime);
        if(HasArrived())
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        int nextPoint = patrolIndex + 1;
        if (nextPoint >= patrolPoints.Count)
        {
            nextPoint = 0;
        }
        patrolIndex = nextPoint;
    }
    
}
