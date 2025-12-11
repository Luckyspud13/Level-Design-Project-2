using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteract : MonoBehaviour, IInteractable
{
    private bool doorOpen = false;
    private Animator doorAnim;
    public string interactionPrompt;


    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    //Performs the door opening or closing interaction
    public void Interact()
    {
        if(doorOpen == false)
        {
            doorAnim.Play("Door Open", 0, 0.0f);
            doorOpen = true;
            interactionPrompt = "Close Door [E]";
        }
        else
        {
            doorAnim.Play("Door Close", 0, 0.0f);
            doorOpen = false;
            interactionPrompt = "Open Door [E]";
        }
    }
}
