// using System.Collections;
// using UnityEngine;

// public class DoorOnClick : MonoBehaviour
// {
//     [Header("Setup")]
//     [Tooltip("What actually rotates. Leave empty to use this object.")]
//     [SerializeField] private Transform pivot;

//     [Header("Motion")]
//     [Tooltip("Degrees to rotate when opening. Use negative to flip direction.")]
//     [SerializeField] private float openAngle = 90f;
//     [SerializeField] private float duration = 0.5f;
//     [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

//     [Header("State")]
//     [SerializeField] private bool startOpen = false;

//     private Quaternion closedRot;
//     private Quaternion openRot;
//     private bool isOpen;
//     private Coroutine anim;

//     private void Awake()
//     {
//         if (pivot == null) pivot = transform;

//         closedRot = pivot.localRotation;
//         openRot   = closedRot * Quaternion.Euler(0f, openAngle, 0f);

//         isOpen = startOpen;
//         pivot.localRotation = isOpen ? openRot : closedRot;
//     }

//     // Called by Unity when this object (or a child) with a Collider is clicked
//     private void OnMouseDown()
//     {
//         Toggle();
//     }

//     public void Toggle()
//     {
//         if (anim != null) StopCoroutine(anim);
//         anim = StartCoroutine(RotateDoor(isOpen ? openRot : closedRot,
//                                          isOpen ? closedRot : openRot));
//         isOpen = !isOpen;
//     }

//     private IEnumerator RotateDoor(Quaternion from, Quaternion to)
//     {
//         float t = 0f;
//         while (t < 1f)
//         {
//             t += Time.deltaTime / duration;
//             pivot.localRotation = Quaternion.Slerp(from, to, ease.Evaluate(Mathf.Clamp01(t)));
//             yield return null;
//         }
//         pivot.localRotation = to;
//     }
// }

using System.Collections;
using UnityEngine;

public class SingleDoorClick : MonoBehaviour
{
    [Header("Door Settings")]
    [Tooltip("The pivot point (hinge) for this door. Usually an empty GameObject placed at the hinge side.")]
    public Transform doorPivot;

    [Tooltip("Degrees to rotate when opening. Positive for outward left swing.")]
    public float openAngle = 90f;

    [Tooltip("Time in seconds to fully open or close.")]
    public float duration = 0.6f;

    [Tooltip("Smooth animation curve for door motion.")]
    public AnimationCurve ease = null;

    [Tooltip("Max distance player can click to open the door. 0 = unlimited.")]
    public float maxUseDistance = 0f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private Coroutine anim;

    void Start()
    {
        if (ease == null)
            ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

        if (doorPivot == null)
            doorPivot = transform;

        // Save initial local rotation as closed
        closedRotation = doorPivot.localRotation;

        // Positive Y angle swings outward to the left
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            if (!cam) return;

            float maxDist = (maxUseDistance <= 0f) ? Mathf.Infinity : maxUseDistance;

            // Raycast to detect clicking on this door
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, maxDist))
            {
                if (hit.collider != null && (hit.collider.transform == doorPivot || hit.collider.transform == transform))
                {
                    ToggleDoor();
                }
            }
        }
    }

    public void ToggleDoor()
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(RotateDoor(isOpen));
        isOpen = !isOpen;
    }

    private IEnumerator RotateDoor(bool currentlyOpen)
    {
        float t = 0f;
        Quaternion from = currentlyOpen ? openRotation : closedRotation;
        Quaternion to = currentlyOpen ? closedRotation : openRotation;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, duration);
            float easedT = ease.Evaluate(Mathf.Clamp01(t));
            doorPivot.localRotation = Quaternion.Slerp(from, to, easedT);
            yield return null;
        }

        doorPivot.localRotation = to;
        anim = null;
    }
}
