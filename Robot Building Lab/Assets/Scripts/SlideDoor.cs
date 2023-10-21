using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SlideDoor : MonoBehaviour
{
    [SerializeField] private DoorPart[] doorParts;

    [SerializeField] private float doorSpeed = 1f;
    [SerializeField] private bool autoCloseOnStart = true;
    [SerializeField] private bool isLocked;

    [SerializeField] private UnityEvent onDoorOpen;
    [SerializeField] private UnityEvent onDoorClose;

    private float currentT = 0f;
    private float targetT = 0f;


    private void Start()
    {
        currentT = autoCloseOnStart ? 0f : 1f;
        targetT = currentT;
        SetDoorPositions(currentT);
    }
    private void Update()
    {
        if (currentT == targetT) return;

        // calculate current doorvalue between current and target value
        currentT = Mathf.MoveTowards(currentT, targetT, doorSpeed * Time.deltaTime);
        SetDoorPositions(currentT);

        // invoke open and closed events when door is open or closed
        if (currentT == targetT)
        {
            if (targetT == 1f) onDoorOpen.Invoke();
            else if (targetT == 0f) onDoorClose.Invoke();
        }
    }

    // lerp doorpositions between closed and open vector
    private void SetDoorPositions(float t)
    {
        for (int i = 0; i < doorParts.Length; i++)
        {
            doorParts[i].doors.localPosition = Vector3.Lerp(doorParts[i].localClosedPosition, doorParts[i].localOpenPosition, t);
        }
    }

    // unlock & lock door
    public void LockDoor()
    {
        isLocked = true;
        targetT = 0f;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    // open and close door
    public void OpenDoor()
    {
        if (isLocked) return;
        targetT = 1f;
    }

    public void CloseDoor()
    {
        if (isLocked) return;
        targetT = 0f;
    }


    // Create struct, visualize transform and door vectors
    [System.Serializable]

    public struct DoorPart
    {
        public Transform doors;
        public Vector3 localOpenPosition;
        public Vector3 localClosedPosition;
    }
}
