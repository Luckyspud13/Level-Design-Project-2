using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Mouse Look")]
    public Transform playerCamera;      
    public float mouseSensitivity = 200f;

    private float xRotation = 0f;      

    void Start()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
      
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);   

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical = Input.GetAxisRaw("Vertical");   // W/S

        
        Vector3 moveDir = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}
