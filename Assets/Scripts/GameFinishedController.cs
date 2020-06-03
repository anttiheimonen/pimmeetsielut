using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFinishedController : MonoBehaviour
{
    public AudioManager audioManager;
    public int sceneToLoad;
    private bool inputsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        // audioManager.Play("GameOverLaugh");
        Invoke("EnableInputs", 2);
    }


    // Update is called once per frame
    void Update()
    {
        if(inputsEnabled)
            if (Input.anyKey)
                SceneManager.LoadScene(sceneToLoad);
    }


    private void EnableInputs()
    {
        inputsEnabled = true;
    }
}
