using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module
//for the movement module only, damage would be applied by a different script from where attributes live
public class PlayerHealth_Movement : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float invulnerabilityTime = 2;

    private PlayerMovement playerMovement;

    public bool isInvulnerable = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        animator.SetBool("invulnerable", isInvulnerable);
    }

    public void HazardDamage(Vector2 knockbackVector)
    {
        if (isInvulnerable)
        {
            return;
        }
        SendMessage("KnockbackPlayer", knockbackVector, SendMessageOptions.DontRequireReceiver);
        TakeDamage();
        //apply directional knockback
    }

    void TakeDamage()
    {
        
        isInvulnerable = true;

        StartCoroutine(InvulnerableClock());
        //play invulnerable animation
    }

    IEnumerator InvulnerableClock()
    {
        Debug.Log("invulnerable timer start");
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        Debug.Log("invulnerable timer ended");
    }
}
