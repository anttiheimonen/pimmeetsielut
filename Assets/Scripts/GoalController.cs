using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public AudioManager audioManager;
    public int sceneToLoad;

    // Update is called once per frame
    void OnTriggerEnter2D (Collider2D col)
    {
        Debug.Log("TRIGGER");
        PlayerController pc = col.GetComponent<PlayerController>();
        if(pc.CompareTag("Player"))
            SceneManager.LoadScene(sceneToLoad);

    }
}
