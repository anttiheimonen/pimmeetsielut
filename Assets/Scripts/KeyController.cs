using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KeyController : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D col)
    {
        if(!col.CompareTag("Player"))
            return;

        col.GetComponent<PlayerController>().FoundKey();
        Destroy(gameObject);
    }
}
