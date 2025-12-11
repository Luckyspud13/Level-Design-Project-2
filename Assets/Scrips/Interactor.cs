using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform InteractorSource;
    public float InteractRange;
    public PlayerInputHandler playerInputHandler;
    public CharacterController characterController;
    public bool interactionStarted = false;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player presses the Interact Key (E) and an interaction isnt already happening
        if (playerInputHandler.InteractTriggered && !interactionStarted)
        {
            //Makes a new raycast in front of the player from the InteractorSource (camera)
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

            //Checks if the raycast detects an object within the InteractRange
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {

                //Checks the hit object if it is an interactable object and if an interaction hasn't already started
                //True: starts and interaction, disables the ability to interact during it, and performs the interaction
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj) && !interactionStarted)
                {
                    interactionStarted = true;
                    playerInputHandler.interactAction.Disable();
                    interactObj.Interact();
                    StartCoroutine (ActionDelay());
                    playerInputHandler.interactAction.Enable();
                }
                else
                {
                   interactionStarted = false;
                }
            }
            else
            {
                interactionStarted = false;
            }
        }
    }

    //1.25 second delay for interactions
    private IEnumerator ActionDelay()
    {
        yield return new WaitForSeconds(1.25f);

        interactionStarted = false;
    }
}
