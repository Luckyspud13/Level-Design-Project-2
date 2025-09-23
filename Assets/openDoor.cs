// using System.Collections;
// using UnityEngine;

// public class openDoor : MonoBehaviour
// {
//     [Header("Door Settings")]
//     [Tooltip("The pivot point (hinge) for this door. Usually an empty GameObject placed at the hinge side.")]
//     public Transform doorPivot;

//     [Tooltip("Degrees to rotate when opening. Positive for outward left swing.")]
//     public float openAngle = 90f;

//     [Tooltip("Time in seconds to fully open or close.")]
//     public float duration = 0.6f;

//     [Tooltip("Smooth animation curve for door motion.")]
//     public AnimationCurve ease = null;

//     [Tooltip("Max distance player can click to open the door. 0 = unlimited.")]
//     public float maxUseDistance = 0f;

//     private Quaternion closedRotation;
//     private Quaternion openRotation;
//     private bool isOpen = false;
//     private Coroutine anim;

//     void Start()
//     {
//         if (ease == null)
//             ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

//         if (doorPivot == null)
//             doorPivot = transform;

//         // Save initial local rotation as closed
//         closedRotation = doorPivot.localRotation;

//         // Positive Y angle swings outward to the left
//         openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             Camera cam = Camera.main;
//             if (!cam) return;

//             float maxDist = (maxUseDistance <= 0f) ? Mathf.Infinity : maxUseDistance;

//             // Raycast to detect clicking on this door
//             if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, maxDist))
//             {
//                 if (hit.collider != null && (hit.collider.transform == doorPivot || hit.collider.transform == transform))
//                 {
//                     ToggleDoor();
//                 }
//             }
//         }
//     }

//     public void ToggleDoor()
//     {
//         if (anim != null) StopCoroutine(anim);
//         anim = StartCoroutine(RotateDoor(isOpen));
//         isOpen = !isOpen;
//     }

//     private IEnumerator RotateDoor(bool currentlyOpen)
//     {
//         float t = 0f;
//         Quaternion from = currentlyOpen ? openRotation : closedRotation;
//         Quaternion to = currentlyOpen ? closedRotation : openRotation;

//         while (t < 1f)
//         {
//             t += Time.deltaTime / Mathf.Max(0.0001f, duration);
//             float easedT = ease.Evaluate(Mathf.Clamp01(t));
//             doorPivot.localRotation = Quaternion.Slerp(from, to, easedT);
//             yield return null;
//         }

//         doorPivot.localRotation = to;
//         anim = null;
//     }
// }

using System.Collections;
using UnityEngine;

public class openDoor : MonoBehaviour
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
        // Ensure there's always a valid ease curve
        if (ease == null)
            ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

        // Fallback if no pivot is assigned
        if (doorPivot == null)
            doorPivot = transform;

        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        // If door or pivot got destroyed, stop running Update
        if (this == null || doorPivot == null)
            return;

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
        // Ensure door still exists before toggling
        if (doorPivot == null) return;

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
            if (doorPivot == null) yield break; // Stop animation if destroyed

            t += Time.deltaTime / Mathf.Max(0.0001f, duration);
            float easedT = ease.Evaluate(Mathf.Clamp01(t));
            doorPivot.localRotation = Quaternion.Slerp(from, to, easedT);
            yield return null;
        }

        if (doorPivot != null)
            doorPivot.localRotation = to;

        anim = null;
    }
}
