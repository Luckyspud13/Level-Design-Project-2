using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightInteract : MonoBehaviour, IInteractable
{
    public Interactor interactor;

    public bool lightOn = false;
    public string interactionPrompt;
    [SerializeField] private float defaultLightBrightness;
    [SerializeField] private Material lightOffMaterial;
    [SerializeField] private Material lightOnMaterial;
    [SerializeField] private GameObject lightBulb;
    [SerializeField] private Transform lightSwitch;
    Vector3 originalScale;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip switchSound;


    private void Awake()
    {
        if(lightOn == false)
        {
            gameObject.GetComponent<Light>().intensity = 0;
        }
        
        originalScale = lightSwitch.localScale;
    }

    //Performs the door opening or closing interaction
    public void Interact()
    {
        interactor.showLightPrompt = false;

        if(lightOn == false)
        {
            gameObject.GetComponent<Light>().intensity = defaultLightBrightness;
            lightBulb.GetComponent<MeshRenderer>().sharedMaterial = lightOnMaterial;
            if(gameObject.TryGetComponent<LampHandler>(out LampHandler lampHandler))
            {
                lampHandler.lampShade.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOnMaterial;
                lampHandler.shadeTopRim.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOnMaterial;
                lampHandler.shadeBottomRim.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOnMaterial;
            }
            else
            {
                lightSwitch.localScale = new Vector3(originalScale.x, originalScale.y, -0.0675f);
            }
            lightOn = true;
            audioSource.Play();
            interactionPrompt = "Turn Off Light [E]";
        }
        else
        {
            gameObject.GetComponent<Light>().intensity = 0;
            lightBulb.GetComponent<MeshRenderer>().sharedMaterial = lightOffMaterial;
            if(gameObject.TryGetComponent<LampHandler>(out LampHandler lampHandler))
            {
                lampHandler.lampShade.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOffMaterial;
                lampHandler.shadeTopRim.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOffMaterial;
                lampHandler.shadeBottomRim.GetComponent<MeshRenderer>().sharedMaterial = lampHandler.lampOffMaterial;
            }
            else
            {
                lightSwitch.localScale = new Vector3(originalScale.x, originalScale.y, 0.0675f);
            }
            lightOn = false;
            audioSource.Play();
            interactionPrompt = "Turn On Light [E]";
        }
    }

    //0.5 second delay for interactions
    public IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(0.5f);
        interactor.lightInteractionStarted = false;
        interactor.showLightPrompt = true;
        //interactor.playerInputHandler.interactAction.Enable();
    }
}
