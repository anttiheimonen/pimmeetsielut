using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public float acceleration;
    // Vector2 move;
    // public GameObject player;
    public Transform attackArea;
    public float attackRange;
    public LayerMask enemyLayer;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public Animator animator;
    public PlayerState playerState;

    void FixedUpdate ()
    {
        InputToMovement ();
    }

    void InputToMovement ()
    {
        if (playerState < PlayerState.Attacking) {
            float MoveHorizontal = Input.GetAxisRaw("Horizontal");
            // float MoveVertical = Input.GetAxisRaw("Vertical");
            if (MoveHorizontal > 0)
            {
                rb.AddForce(Vector3.right * acceleration);
                spriteRenderer.flipX = false;
            }
            if (MoveHorizontal < 0)
            {
                rb.AddForce(Vector3.left * acceleration);
                spriteRenderer.flipX = true;
            }

            // if (MoveHorizontal == 0)
            // {
            //     rb.velocity = Vector2.zero;
            //     // rb.velocity = Vector2.Lerp(gameObject.transform.position, )
            // }

            // Reduce movement speed if too high
            if(rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            if (rb.velocity.magnitude == 0)
            {
                playerState = PlayerState.Idle;
            }
            else
            {
                playerState = PlayerState.Running;
            }
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);
        // Debug.Log (rb.velocity.x);
    }


    private void Attack()
    {
        Debug.Log("Attacking!!!");
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
        animator.SetBool("IsAttacking", false);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackArea.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log (enemy);
            enemy.GetComponent<Enemy>().takeHit();
        }
    }


    public void PrintEvent()
    {
        Debug.Log("PrintEvent");
        playerState = PlayerState.Idle;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerState = PlayerState.Idle;
        // attackArea = transform.Find("AttackArea");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
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


}
