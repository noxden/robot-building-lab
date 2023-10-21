using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Collider))]
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent onTimerReached;
    [SerializeField] private float endOfTimer = 3f;

    bool timerStarted = false;
    bool timeReached = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RobotController>()!=null)
        {
            StartTimer();
            Debug.Log("Collided");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<RobotController>() != null)
        {
            if (timeReached || !timerStarted) return;

            if (Time.time > endOfTimer)
            {
                timeReached = true;
                onTimerReached?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<RobotController>() != null)
        {
            timerStarted = false;
            Debug.Log("out of collision");
        }
    }


    public void StartTimer()
    {
        endOfTimer += Time.time;
        timerStarted = true;
    }

    public void PrintSomething()
    {
        Debug.Log("Plate Activated!");
    }
}
