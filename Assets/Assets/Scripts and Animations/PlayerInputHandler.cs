using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header ("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    [Header ("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header ("Action Map Reference")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string roation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string toggleCrosshair = "ToggleCrosshair";
    [SerializeField] private string startNextScene = "StartNextScene";

    public InputAction movementAction;
    public InputAction rotationAction;
    public InputAction jumpAction;
    public InputAction sprintAction;
    public InputAction interactAction;
    public InputAction toggleCrosshairAction;
    public InputAction startNextSceneAction;

    public Vector2 MovementInput { get; private set; }

    public Vector2 RotationInput { get; private set; }

    public bool JumpTriggered { get; private set; }

    public bool SprintTriggered { get; private set; }

    public bool InteractTriggered { get; private set; }

    public bool ToggleCrosshairTriggered { get; private set; }

    public bool StartNextSceneTriggered { get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(roation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        interactAction = mapReference.FindAction(interact);
        toggleCrosshairAction = mapReference.FindAction(toggleCrosshair);
        startNextSceneAction = mapReference.FindAction(startNextScene);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        interactAction.performed += inputInfo => InteractTriggered = true;
        interactAction.canceled += inputInfo => InteractTriggered = false;
        
        toggleCrosshairAction.performed += inputInfo => ToggleCrosshairTriggered = true;
        toggleCrosshairAction.canceled += inputInfo => ToggleCrosshairTriggered = false;

        startNextSceneAction.performed += inputInfo => StartNextSceneTriggered = true;
        startNextSceneAction.canceled += inputInfo => StartNextSceneTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}
