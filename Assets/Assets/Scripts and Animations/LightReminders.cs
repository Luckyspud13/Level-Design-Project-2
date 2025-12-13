using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightReminders : MonoBehaviour
{
    public string lightReaction;
    public bool lightTurnedOnYet = false;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private TextMeshProUGUI reactionTextbox;
    [SerializeField] private LightInteract lightInteract;
    [SerializeField] private GameObject invisWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lightInteract.lightOn)
        {
            lightTurnedOnYet = true;
            invisWall.SetActive(false);
        }
    }

    public IEnumerator waitForReaction()
    {
        Debug.Log("Wait Period Started");
        yield return new WaitForSeconds(5);

        playerInputHandler.movementAction.Enable();
        playerInputHandler.jumpAction.Enable();
        playerInputHandler.interactAction.Enable();
        reactionTextbox.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Light Reminder Collided With");
        if (!lightTurnedOnYet && other.CompareTag("Player"))
        {
            playerInputHandler.movementAction.Disable();
            playerInputHandler.jumpAction.Disable();
            playerInputHandler.interactAction.Disable();

            reactionTextbox.text = lightReaction;

            StartCoroutine(waitForReaction());
        } 
    }
}
