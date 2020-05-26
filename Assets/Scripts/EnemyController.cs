﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int maxHealth = 2;
    public EnemyState enemyState;
    public Transform attackArea;
    public LayerMask playerLayer;
    public Animator animator;
    public BoxCollider2D boxCollider2D;
    public float attackRange;     // Attacks damaging area
    public float attackDuration;
    public float cooldownDuration;
    int currentHealth;
    public float detectionArea;   // When player is in detection area enemy attacks
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
        if (enemyState == EnemyState.Dead)
            return;

        if (enemyState == EnemyState.Idle)
        {
            animator.SetBool("IsAttacking", true);
            enemyState = EnemyState.Attacking;
            ammo--;
            Invoke("AttackEnd", attackDuration);
            Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackArea.position, attackRange, playerLayer);
            foreach(Collider2D player in playersHit)
            {
                player.GetComponent<PlayerController>().HandleIncomingHit();
            }
        }
    }

    public void AttackEnd ()
    {
        if (enemyState != EnemyState.Dead)
        {
            enemyState = EnemyState.Cooldown;
            Invoke("CooldownEnd", attackDuration);

            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsCooldown", true);
        }
    }


    public void CooldownEnd ()
    {
        animator.SetBool("IsCooldown", false);
        enemyState = EnemyState.Idle;
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
            animator.SetBool("IsDead", true);
            enemyState = EnemyState.Dead;
            Debug.Log("Vihollinen kuoli");
            boxCollider2D.enabled = false;
        }
    }


    private void Idle()
    {
        animator.SetBool("IsAttacking", false);
    }


    private bool PlayersIsOnAttackArea()
    {
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackArea.position, detectionArea, playerLayer);
        return (playersHit.Length > 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyState == EnemyState.Dead)
            return;

        if (PlayersIsOnAttackArea())
        {
            attack();
        }
        else
        {
            Idle();
        }
    }


    public enum EnemyState
    {
        Idle,
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
