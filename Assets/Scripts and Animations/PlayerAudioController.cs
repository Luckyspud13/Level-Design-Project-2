using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip walkOnSoft;
    [SerializeField] private AudioClip walkOnHard;
    private bool audioPlaying = true;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(loadBuffer());
    }

    // Update is called once per frame
    void Update()
    {

        if(firstPersonController.currentMovement.x != 0 && firstPersonController.currentMovement.z != 0 && firstPersonController.characterController.isGrounded)
        {
            playerAudio.volume = 0.3f * firstPersonController.CurrentSpeed;
        }
        else
        {
            playerAudio.volume = 0;
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Entering Hard Floor"))
        {
            Debug.Log("Now Walking on Hard Floor");
            playerAudio.clip = walkOnHard;
            playerAudio.Play();
        }
        else if(other.CompareTag("Entering Soft Floor"))
        {
            Debug.Log("Now Walking on Soft Floor");
            playerAudio.clip = walkOnSoft;
            playerAudio.Play();
        }
    }

    private IEnumerator loadBuffer()
    {
        yield return new WaitForSeconds(1);
        playerAudio.loop = true;
        playerAudio.Play();
    }
}
