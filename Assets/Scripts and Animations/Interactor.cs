using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public IEnumerator InteractDelay();
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform InteractorSource;
    public float InteractRange;
    public PlayerInputHandler playerInputHandler;
    public CharacterController characterController;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private LayerMask buildingMask;
    public bool doorInteractionStarted = false;
    public bool lightInteractionStarted = false;
    public bool houseBrokenIntoYet = false;
    public bool bedroomOpened = false;
    public bool showDoorPrompt = true;
    public bool showLightPrompt = true;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player presses the Interact Key (E) and an interaction isnt already happening
        if (playerInputHandler.InteractTriggered)
        {   //Makes a new raycast in front of the player from the InteractorSource (camera)
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

            //Checks if the raycast detects an object within the InteractRange
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, interactableMask))
            {

                //Checks the hit object if it is an interactable object and if an interaction hasn't already started
                //True: starts and interaction, disables the ability to interact during it, and performs the interaction
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    if(Physics.Raycast(r, out RaycastHit hitInfo2, InteractRange, buildingMask))
                    {
                        if(hitInfo.distance < hitInfo2.distance)
                        {
                            if(hitInfo.collider.gameObject.TryGetComponent(out DoorInteract doorInteract) && !doorInteractionStarted)
                            {
                                playerInputHandler.interactAction.Disable();
                                doorInteractionStarted = true;
                                showDoorPrompt = false;
                                interactObj.Interact();
                                StartCoroutine(doorInteract.InteractDelay());
                                StartCoroutine(buttonSpamDelay());
                            }
                            else if(hitInfo.collider.gameObject.TryGetComponent(out LightInteract lightInteract) && !lightInteractionStarted)
                            {
                                playerInputHandler.interactAction.Disable();
                                lightInteractionStarted = true;
                                showLightPrompt = false;
                                interactObj.Interact();
                                StartCoroutine(lightInteract.InteractDelay());
                                StartCoroutine(buttonSpamDelay());
                            }
                        }
                    }
                    else
                    {
                        if(hitInfo.collider.gameObject.TryGetComponent(out DoorInteract doorInteract) && !doorInteractionStarted)
                        {
                            playerInputHandler.interactAction.Disable();
                            doorInteractionStarted = true;
                            showDoorPrompt = false;
                            interactObj.Interact();
                            StartCoroutine(doorInteract.InteractDelay());
                            StartCoroutine(buttonSpamDelay());
                        }
                        else if(hitInfo.collider.gameObject.TryGetComponent(out LightInteract lightInteract) && !lightInteractionStarted)
                        {
                            playerInputHandler.interactAction.Disable();
                            lightInteractionStarted = true;
                            showLightPrompt = false;
                            interactObj.Interact();
                            StartCoroutine(lightInteract.InteractDelay());
                            StartCoroutine(buttonSpamDelay());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator buttonSpamDelay()
    {
        yield return new WaitForSeconds(0.5f);
        playerInputHandler.interactAction.Enable();
    }
}
