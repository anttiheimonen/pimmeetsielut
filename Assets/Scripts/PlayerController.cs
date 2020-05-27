using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public int maxHealth;
    public float acceleration;
    // Vector2 move;
    // public GameObject player;
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


    void InputToMovement ()
    {
        if (playerState == PlayerState.Dead || playerState == PlayerState.Uncontrollable)
            return;     // Player cannot be controlled

        if (playerState < PlayerState.Attacking)
        {
            float MoveHorizontal = Input.GetAxisRaw("Horizontal");
            // float MoveVertical = Input.GetAxisRaw("Vertical");
            if (MoveHorizontal > 0)
            {
                rbody.AddForce(Vector3.right * acceleration);
                spriteRenderer.flipX = false;
            }
            if (MoveHorizontal < 0)
            {
                rbody.AddForce(Vector3.left * acceleration);
                spriteRenderer.flipX = true;
            }

            // if (MoveHorizontal == 0)
            // {
            //     rb.velocity = Vector2.zero;
            //     // rb.velocity = Vector2.Lerp(gameObject.transform.position, )
            // }

            // Reduce movement speed if too high
            if(rbody.velocity.magnitude > maxSpeed)
                rbody.velocity = rbody.velocity.normalized * maxSpeed;

            if (rbody.velocity.magnitude == 0)
                playerState = PlayerState.Idle;
            else
                playerState = PlayerState.Running;
        }

        animator.SetFloat("Speed", rbody.velocity.magnitude);
        // Debug.Log (rb.velocity.x);
    }


    private void Attack()
    {
        if (playerState == PlayerState.Attacking)
            return;     // Player is already attacking
        Debug.Log("Attacking!!!");
        Invoke("DealDamage", AttackTimeToDamage);
        Invoke("AttackEnd", AttackDuration);
        animator.SetBool("IsAttacking", true);
        playerState = PlayerState.Attacking;
    }


    public void AttackEnd ()
    {
        // Debug.Log("Isku loppui");
        playerState = PlayerState.Idle;
        animator.SetBool("IsAttacking", false);
    }


    public void DealDamage ()
    {
        // animator.SetBool("IsAttacking", false);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackArea.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log (enemy);
            enemy.GetComponent<EnemyController>().takeHit();
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
        currentHealth--;
        Debug.Log("Pelaaja sai osuman");
        IsInvulnerable = true;
        animator.SetBool("IsRecoveringFromHit", true);
        Invoke("RemoveInvulnerable", InvulnerableDuration);
    }


    private void TakeKnockback ()
    {
        playerState = PlayerState.Uncontrollable;
        Invoke("KnockbackEnd", knockbackDuration);
        rbody.AddForce(new Vector2(-1, 1) * 5, ForceMode2D.Impulse);
        // rbody.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
    }


    private void KnockbackEnd ()
    {
        animator.SetBool("IsRecoveringFromHit", false);
        playerState = PlayerState.Idle;
    }


    private void Die ()
    {
        playerState = PlayerState.Dead;
        animator.SetBool("IsDead", true);
        Debug.Log("Kuoli");
    }


    private void RemoveInvulnerable ()
    {
        IsInvulnerable = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerState = PlayerState.Idle;
        currentHealth = maxHealth;
        IsInvulnerable = false;
        InvokeRepeating("DebugPlayerState", 1, 1);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }


    void FixedUpdate ()
    {
        InputToMovement ();
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
