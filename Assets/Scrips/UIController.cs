using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private Interactor interactor;
    public TextMeshProUGUI textMeshProComponent;
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
            //If the hit object has the DoorInteract script (its a door), then change the prompt to the appropriate door promp then display it on the UI
            if (hitInfo.collider.gameObject.TryGetComponent(out DoorInteract doorInteract))
            {
                prompt = doorInteract.interactionPrompt;
                textMeshProComponent.text = prompt;
            }
        }
        else
        {
            textMeshProComponent.text = ""; //Resets the UI when not looking at an interactable object
        }
    }
}
