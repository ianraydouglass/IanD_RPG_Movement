using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module
//for the movement module only, damage would be applied by a different script from where attributes live
public class PlayerHealth_Movement : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator lifeAnimator;

    [SerializeField]
    private CheckPointManager_Movement checkPointManager;

    [SerializeField]
    private float invulnerabilityTime = 2;

    [SerializeField]
    private float showLifeTime = 3;

    private PlayerMovement playerMovement;

    //life value. Setting it to private since the asset is made for a life max of 5.
    private int maxLife = 5;

    private int currentLife = 5;
    public int GetPlayerLife()
    {
        return currentLife;
    }

    //this setting is for debug mode, and will prevent the player's health from falling below 1.
    public bool isImmortal = false;

    public bool isInvulnerable = false;

    public bool isLifeRevealed = true;

    //reference to the show life timer
    private IEnumerator lifeRoutine;
    private bool isLifeRoutineRunning = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lifeRoutine = HideLifeClock();
        //show life value and hide shortly after start
        ShowLife();
    }

    void Update()
    {
        playerAnimator.SetBool("invulnerable", isInvulnerable);
        lifeAnimator.SetInteger("player life", currentLife);
        lifeAnimator.SetBool("sleep", !isLifeRevealed);
    }

    public void HazardDamage(Vector2 knockbackVector)
    {
        if (isInvulnerable)
        {
            return;
        }
        
        TakeDamage(knockbackVector);
    }

    public void EnemyDamage(GameObject enemyObject)
    {
        if (isInvulnerable)
        {
            return;
        }
        enemyObject.SendMessage("ConfirmHitPlayer", SendMessageOptions.DontRequireReceiver);
        //get a direction from the two objects
        Vector2 enemyPosition = enemyObject.transform.position;
        Vector2 myPosition = this.gameObject.transform.position;
        Vector2 push = (myPosition - enemyPosition);
        //sets the distance to 1 basically
        push.Normalize();
        TakeDamage(push);
    }

    void TakeDamage(Vector2 knockbackVector)
    {
        if(currentLife <= 1 && isImmortal)
        {
            //skip subtracting and talking to life animator
        }
        else
        {
            currentLife -= 1;
            ShowLife();
        }

        if(currentLife <= 0)
        {
            Debug.Log("player killed");
            checkPointManager.RespawnPlayer(this.gameObject);
            return;
        }

        SendMessage("KnockbackPlayer", knockbackVector, SendMessageOptions.DontRequireReceiver);
        isInvulnerable = true;

        StartCoroutine(InvulnerableClock());
        //play invulnerable animation
    }

    public void RecoverAllLife()
    {
        currentLife = maxLife;
        ShowLife();
    }

    public void ShowLife()
    {
        if(isLifeRevealed && isLifeRoutineRunning)
        {
            isLifeRoutineRunning = false;
            StopCoroutine(lifeRoutine);
        }
        isLifeRevealed = true;
        //we only want to start the life routine if we plan to hide the life value
        //we want to keep the life value displayed if it's at 1
        if(currentLife > 1)
        {
            lifeRoutine = HideLifeClock();
            StartCoroutine(lifeRoutine);
        }
    }

    IEnumerator InvulnerableClock()
    {
        Debug.Log("invulnerable timer start");
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        Debug.Log("invulnerable timer ended");
    }

    IEnumerator HideLifeClock()
    {
        isLifeRoutineRunning = true;
        Debug.Log("show life timer start");
        yield return new WaitForSeconds(invulnerabilityTime + showLifeTime);
        isLifeRevealed = false;
        Debug.Log("show life timer ended");
        isLifeRoutineRunning = false;
    }
}
