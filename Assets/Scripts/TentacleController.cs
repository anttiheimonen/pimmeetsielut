using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{

    public Animator animator;
    public Transform attackArea;
    public LayerMask playerLayer;
    public float attackRange;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", 2);
        Invoke("DealDamage", 2);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void Remove ()
    {
        animator.StopPlayback();
        Destroy(gameObject);
    }


    private void DealDamage ()
    {
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackArea.position, attackRange, playerLayer);
        foreach(Collider2D player in playersHit)
        {
            player.GetComponent<PlayerController>().HandleIncomingHit();
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;
        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }
}
