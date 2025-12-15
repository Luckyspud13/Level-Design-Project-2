using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapController : MonoBehaviour
{
    public string trapReaction;
    public float reactionDuration;
    public bool trapTriggered = false;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private TextMeshProUGUI reactionTextbox;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator waitForReaction()
    {
        yield return new WaitForSeconds(reactionDuration);

        playerInputHandler.movementAction.Enable();
        playerInputHandler.jumpAction.Enable();
        playerInputHandler.interactAction.Enable();
        reactionTextbox.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (!trapTriggered && other.CompareTag("Player"))
        {
            playerInputHandler.movementAction.Disable();
            playerInputHandler.jumpAction.Disable();
            playerInputHandler.interactAction.Disable();

            reactionTextbox.text = trapReaction;

            trapTriggered = true;
            StartCoroutine(waitForReaction());
        } 
    }
}
