using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{

    public int LevelToLoad;
    public Animator animator;
    private float animationDuration = 1.5f;

    void OnTriggerEnter2D (Collider2D col)
    {
        if(!col.CompareTag("Player"))
            return;

        if (col.GetComponent<PlayerController>().HasKey())
        {
            animator.SetBool("HasKey", true);
            Invoke("Unlock", animationDuration);
        }
    }

    void OnTriggerStay2D (Collider2D col)
    {

    }

    void OnTriggerExit2D (Collider2D col)
    {

    }


    private void Unlock ()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
