using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteract : MonoBehaviour, IInteractable
{
    public Interactor interactor;
    
    private bool doorOpen = false;
    private Animator doorAnim;
    public string interactionPrompt;
    public GameObject animatedBottle;
    public GameObject dummyBottle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorCloseSound;
    [SerializeField] private AudioClip doorKickInSound;


    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();

        if(!gameObject.CompareTag("Bedroom Door"))
        {
            animatedBottle = null;
            dummyBottle = null;
        }
        else
        {
            dummyBottle.SetActive(false);
        }
    }

    //Performs the door opening or closing interaction
    public void Interact()
    {
        if(!interactor.houseBrokenIntoYet && gameObject.CompareTag("Front Door"))
        {
            if(doorOpen == false)
            {
                doorAnim.Play("Door Break In", 0, 0.0f);
                doorOpen = true;

                audioSource.clip = doorKickInSound;
                audioSource.Play();

                interactionPrompt = "Close Door [E]";
            }
            else
            {
                doorAnim.Play("Door Close After Break In", 0, 0.0f);
                doorOpen = false;
                
                audioSource.clip = doorCloseSound;
                audioSource.Play();

                interactionPrompt = "Open Door [E]";
                interactor.houseBrokenIntoYet = true;
            }
        }
        else if(!interactor.bedroomOpened && gameObject.CompareTag("Bedroom Door") && doorOpen == false)
        {
            doorAnim.Play("Bedroom Door Partial Open");
            doorOpen = true;
            
            audioSource.clip = doorOpenSound;
            audioSource.Play();

            interactor.bedroomOpened = true;
            animatedBottle.SetActive(false);
            dummyBottle.SetActive(true);
            interactionPrompt = "Close Door [E]";
        }
        else
        {
            if (doorOpen == false)
            {
                doorAnim.Play("Door Open", 0, 0.0f);
                doorOpen = true;
                
                audioSource.clip = doorOpenSound;
                audioSource.Play();

                interactionPrompt = "Close Door [E]";
            }
            else
            {   
                doorAnim.Play("Door Close", 0, 0.0f);
                doorOpen = false;

                audioSource.clip = doorCloseSound;
                audioSource.Play();

                interactionPrompt = "Open Door [E]";
            }
        }
        
    }

    //1.25 second delay for interactions
    public IEnumerator InteractDelay()
    {
        
        yield return new WaitForSeconds(1.25f);
        interactor.doorInteractionStarted = false;
        interactor.showDoorPrompt = true;
        //interactor.playerInputHandler.interactAction.Enable();
    }
}
