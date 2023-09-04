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

    private ChaseLimiter chaseLimiter;

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

    [SerializeField]
    private float alertDuration = 2f;

    [SerializeField]
    private float gloatDuration = 2f;

    private GameObject targetPlayer;

    public float distanceFromPatrol = 0f;

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
        chaseLimiter = patrolZone.GetComponent<ChaseLimiter>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckChaseLimit();
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
            //the skeleton should transition out of dismiss if they arrive at a patrol point
            if(thisMode == EnemyMode.Dismiss)
            {
                thisMode = EnemyMode.Patrol;
            }
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

    //method to trigger chase behavior if the contact alert zone trigger script fires
    public void DetectPlayer(GameObject playerObject, bool canExitDismiss)
    {
        if(thisMode == EnemyMode.Idle || thisMode == EnemyMode.Patrol)
        {
            thisMode = EnemyMode.Alert;
            targetPlayer = playerObject;

            StartCoroutine(AlertTimer());
        }
        if(thisMode == EnemyMode.Dismiss && canExitDismiss)
        {
            thisMode = EnemyMode.Alert;
            targetPlayer = playerObject;

            StartCoroutine(AlertTimer());
        }

    }

    public void CheckChaseLimit()
    {
        if(thisMode == EnemyMode.Chase)
        {
            distanceFromPatrol = Vector2.Distance(this.transform.position, patrolZone.transform.position);
            if (distanceFromPatrol > chaseLimiter.GetChaseDistance())
            {
                thisMode = EnemyMode.Dismiss;
                Debug.Log("Dismissing chase");
            }
        }
    }

    public void ConfirmHitPlayer()
    {
        thisMode = EnemyMode.Gloat;
        StartCoroutine(GloatTimer());
        
    }

    //trigger when the alert animation is complete to start the chase behavior
    public void CompleteAlert()
    {
        if(thisMode == EnemyMode.Alert)
        {
            Debug.Log("Begin Chase");
            thisMode = EnemyMode.Chase;
        }
    }

    public void CompleteGloat()
    {
        if(thisMode == EnemyMode.Gloat)
        {
            Debug.Log("Resume Chase");
            thisMode = EnemyMode.Chase;
        }
    }

    IEnumerator AlertTimer()
    {
        Debug.Log("Alert Started");
        yield return new WaitForSeconds(alertDuration);
        
        CompleteAlert();
    }

    IEnumerator GloatTimer()
    {
        Debug.Log("Gloat Started");
        yield return new WaitForSeconds(gloatDuration);

        CompleteGloat();
    }

}
