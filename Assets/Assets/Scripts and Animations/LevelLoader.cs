using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1;
    public PlayerInputHandler playerInputHandler;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputHandler.StartNextSceneTriggered)
        {
            Debug.Log("Loading Next Scene");
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerInputHandler.startNextSceneAction.Disable();
        }
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);


    }
}
