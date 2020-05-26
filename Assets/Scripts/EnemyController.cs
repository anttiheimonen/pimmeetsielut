using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int maxHealth = 2;
    public EnemyState enemyState;
    int currentHealth;
    public Transform attackArea;
    public float attackRange;     // Attacks damaging area
    public float detectionArea;   // When player is in detection area enemy attacks
    public LayerMask playerLayer;
    public Animator animator;
    float attackDuration = 1;
    public int maxAmmo;
    int ammo;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ammo = maxAmmo;
        enemyState = EnemyState.Idle;
    }

    void attack()
    {
        if (enemyState == EnemyState.Idle)
        {
            enemyState = EnemyState.Attacking;
            ammo--;
            Invoke("AttackEnd", attackDuration);
        }
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackArea.position, attackRange, playerLayer);
        foreach(Collider2D player in playersHit)
        {
            player.GetComponent<PlayerController>().HandleIncomingHit();
        }
    }

    public void AttackEnd ()
    {
        enemyState = EnemyState.Idle;
        animator.SetBool("IsAttacking", false);
    }


    public void takeHit()
    {
        currentHealth--;

        if(currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        if (enemyState != EnemyState.Dead)
        {
            enemyState = EnemyState.Dead;
            Debug.Log("Vihollinen kuoli");
            transform.Rotate(0, 0, -90);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackArea.position, detectionArea, playerLayer);
        // Debug.Log("Pelaajia kantamalla: " + playersHit.Length);
        if (playersHit.Length > 0)
        {
            animator.SetBool("IsAttacking", true);
            attack();
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }


    public enum EnemyState
    {
        Idle,
        Aware,
        Attacking,
        Cooldown,
        Dead
    }


        void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;
        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }
}
