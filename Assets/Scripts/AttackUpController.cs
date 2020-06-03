using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpController : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D col)
    {
        if(!col.CompareTag("Player"))
            return;

        col.GetComponent<PlayerController>().FoundPowerUp();
        Destroy(gameObject);
    }
}
