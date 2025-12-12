using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class UIController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private LayerMask buildingMask;
    [SerializeField] private Interactor interactor;
    [SerializeField] private GameObject crosshair;
    public TextMeshProUGUI textMeshProComponent;
    public PlayerInputHandler playerInputHandler;
    private string prompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Makes a new raycast in front of the player from the InteractorSource (camera)
        Ray r = new Ray(interactor.InteractorSource.position, interactor.InteractorSource.forward);

        //Checks if the raycast detects an object on the interactableMask within the InteractRange
        if(Physics.Raycast(r, out RaycastHit hitInfo, interactor.InteractRange, interactableMask))
        {
            if(Physics.Raycast(r, out RaycastHit hitInfo2, interactor.InteractRange, buildingMask))
            {
                if(hitInfo.distance < hitInfo2.distance)
                {
                    //If the hit object has the DoorInteract script (its a door), and the door isn't on cooldown, then change the prompt to the appropriate door promp then display it on the UI
                    if (hitInfo.collider.gameObject.TryGetComponent(out DoorInteract doorInteract) && interactor.showDoorPrompt)
                    {
                        prompt = doorInteract.interactionPrompt;
                    }
                    else if (hitInfo.collider.gameObject.TryGetComponent(out LightInteract lightInteract) && interactor.showLightPrompt)
                    {
                        prompt = lightInteract.interactionPrompt;
                    }
                    else
                    {
                        prompt = "";
                    }
                }
                else
                {
                    prompt = "";
                }
            }
            else
            {
                //If the hit object has the DoorInteract script (its a door), then change the prompt to the appropriate door promp then display it on the UI
                if (hitInfo.collider.gameObject.TryGetComponent(out DoorInteract doorInteract) && interactor.showDoorPrompt)
                {
                    prompt = doorInteract.interactionPrompt;
                }
                else if (hitInfo.collider.gameObject.TryGetComponent(out LightInteract lightInteract) && interactor.showLightPrompt)
                {
                    prompt = lightInteract.interactionPrompt;
                }
                else
                {
                    prompt = "";
                }
            }
        }
        else
        {
            prompt = "";
        }

        textMeshProComponent.text = prompt;

        if (playerInputHandler.ToggleCrosshairTriggered)
        {
            playerInputHandler.toggleCrosshairAction.Disable();
            crosshair.SetActive(!crosshair.activeSelf);
            StartCoroutine(crosshairToggleDelay());
        }
    }

    private IEnumerator crosshairToggleDelay()
    {
        yield return new WaitForSeconds(0.25f);
        playerInputHandler.toggleCrosshairAction.Enable();
    }
}
