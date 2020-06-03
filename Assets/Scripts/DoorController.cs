using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{

    public bool locked;
    public int LevelToLoad;
    public Animator animator;
    private float animationDuration = 1.5f;
    public AudioManager audioManager;


    void OnTriggerEnter2D (Collider2D col)
    {

    }

    void OnTriggerStay2D (Collider2D col)
    {
        PlayerController pc = col.GetComponent<PlayerController>();

        if(!pc.CompareTag("Player"))
            return;

        if(!pc.UseDoor())
            return;

        if(locked && !pc.HasKey())
            return;

        if(locked)
            Unlock();

        Invoke("Travel", animationDuration);

    }

    void OnTriggerExit2D (Collider2D col)
    {

    }


    private void Unlock()
    {
        Debug.Log("unlockataan");
        locked = false;
        animator.SetBool("HasKey", true);
        audioManager.Play("DoorOpen");
    }

    private void Travel ()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
