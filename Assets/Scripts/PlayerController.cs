﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public int maxHealth;
    public Transform attackArea;
    public float attackRange;
    public LayerMask enemyLayer;
    Rigidbody2D rbody;
    SpriteRenderer spriteRenderer;

    public Animator animator;
    public PlayerState playerState;
    float AttackDuration = 0.6f;
    float AttackTimeToDamage = 0.4f;
    int currentHealth;
    // How long player is invulnerable to damage after taking a hit
    public float InvulnerableDuration;
    // While player is invulnerable, hits won't affect to character
    bool IsInvulnerable;
    // How long knocback lasts
    public float knockbackDuration;
    public HealthBar healthBar;
    private int attackDamage;
    bool hasKey;
    public AudioManager audioManager;
    private bool wantsToUseDoor;


    void InputToMovement ()
    {
        if (playerState == PlayerState.Dead || playerState == PlayerState.Uncontrollable)
        {
            return;     // Player cannot be controlled
        }

        // Up press means player wants to use door
        if (Input.GetAxisRaw("Vertical") > 0.1f){
            wantsToUseDoor = true;
        }
        else
            wantsToUseDoor = false;

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if (playerState < PlayerState.Attacking)
        {
            // Read movement axis input
            float MoveHorizontal = Input.GetAxisRaw("Horizontal");
            if (MoveHorizontal > 0)
            {
                rbody.velocity = Vector2.right * maxSpeed;
                spriteRenderer.flipX = false;
            }
            else if (MoveHorizontal < 0)
            {
                rbody.velocity = Vector2.left * maxSpeed;
                spriteRenderer.flipX = true;
            }
            else
            {
                StopMovement();
            }

            // Reduce movement speed if too high
            // TODO: Is this still needed?
            if(rbody.velocity.magnitude > maxSpeed)
                rbody.velocity = rbody.velocity.normalized * maxSpeed;

            // Attack
            if (rbody.velocity.magnitude == 0)
                playerState = PlayerState.Idle;
            else
                playerState = PlayerState.Running;
        }

        animator.SetFloat("Speed", rbody.velocity.magnitude);
    }


    private void Attack()
    {
        if (playerState == PlayerState.Attacking)
            return;     // Player is already attacking

        StopMovement();
        Invoke("DealDamage", AttackTimeToDamage);
        Invoke("AttackEnd", AttackDuration);
        animator.SetBool("IsAttacking", true);
        playerState = PlayerState.Attacking;
    }


    public void AttackEnd ()
    {
        playerState = PlayerState.Idle;
        animator.SetBool("IsAttacking", false);
    }


    public void DealDamage ()
    {
        // TODO: This is not good solution for damage dealding if there will
        // be more enemy types.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackArea.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            audioManager.Play("PlayerAttack");
            if (enemy.tag == "Boss")
                enemy.GetComponent<BossController>().TakeHit(attackDamage);
            else
                enemy.GetComponent<EnemyController>().TakeHit(attackDamage);
        }
    }


    public void HandleIncomingHit()
    {
        if (playerState == PlayerState.Dead)
            return;   // Don't hurt the dead player

        if (!IsInvulnerable)
        {
            TakeDamage();
            TakeKnockback ();
        }

        if (currentHealth <= 0) {
            Die();
        }
    }


    // Reduces player's life and sets invulnerability
    private void TakeDamage ()
    {
        // Cancel possible attack invokes
        CancelInvoke("DealDamage");
        CancelInvoke("AttackEnd");
        animator.SetBool("IsAttacking", false);
        currentHealth--;
        IsInvulnerable = true;
        animator.SetBool("IsOnKnockback", true);
        Invoke("RemoveInvulnerable", InvulnerableDuration);
        UpdateHealthBar();
        PlaySound("PlayerDamaged");

    }


    private void TakeKnockback ()
    {
        playerState = PlayerState.Uncontrollable;
        StopMovement();
        Invoke("KnockbackEnd", knockbackDuration);
        rbody.AddForce(new Vector2(-1, 1) * 5, ForceMode2D.Impulse);
        // rbody.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
    }


    private void KnockbackEnd ()
    {
        animator.SetBool("IsOnKnockback", false);
        playerState = PlayerState.Idle;
    }


    private void Die ()
    {
        playerState = PlayerState.Dead;
        animator.SetBool("IsDead", true);
        Debug.Log("Kuoli");
        PlaySound("PlayerDeath");
        Invoke("MoveToGameOverScene", 3);
    }


    private void MoveToGameOverScene()
    {
            SceneManager.LoadScene(3);
    }


    private void PlaySound(string sound)
    {
        audioManager.Play(sound);
    }


    public bool UseDoor() => wantsToUseDoor;


    private void RemoveInvulnerable() => IsInvulnerable = false;


    private bool PlayerIsAlive() => currentHealth > 0;


    private void StopMovement () => rbody.velocity = Vector2.zero;


    private void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth);
    }


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerState = PlayerState.Idle;
        IsInvulnerable = false;
        currentHealth = GlobalObjectController.Instance.health;
        maxHealth = GlobalObjectController.Instance.maxHealth;
        attackDamage = GlobalObjectController.Instance.attackDamage;
        healthBar.SetMaxHealth(maxHealth);
        UpdateHealthBar();
        hasKey = false;
        // InvokeRepeating("DebugPlayerState", 1, 1);
    }

    void OnDestroy()
    {
        SavePlayer();
    }


  public void FoundKey() => hasKey = true;


  public void FoundPowerUp() => attackDamage++;


  public bool HasKey() => hasKey;


    // Update is called once per frame
    void Update()
    {
        if (!PlayerIsAlive())
            return;

        InputToMovement ();
    }


    public void SavePlayer()
    {
        GlobalObjectController.Instance.SetHealth(currentHealth);
        GlobalObjectController.Instance.SetAttackDamage(currentHealth);
    }


    void FixedUpdate ()
    {

    }


    /// Draw an attack area for a weapon in editor
    void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;
        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }


    public enum PlayerState
    {
        Idle,
        Running,
        Attacking,
        Uncontrollable,
        Dead
    }


    // Debug methods

    private void DebugPlayerState ()
    {
        Debug.Log("Health " + currentHealth);
    }

}
