using Logging;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemViewModel : MonoBehaviour
{

    [SerializeField] private EventSystem eventSystem;

    private int numOfInputLocks = 0;
    private int numOfSwipeLocks = 0;

    // This lock disables the input system
    public bool IsInputLocked => numOfInputLocks > 0;

    // This lock is manual and blocks unique operations (swipe for example)
    public bool IsSwipeLocked => numOfSwipeLocks > 0;
    public void RequestLock()
    {
        numOfInputLocks++;
        if (eventSystem == null)
        {
            CuteLogger.LogError("Event system is null. Did you fetch a scene initialized component?");
            return;
        }
        eventSystem.enabled = false;
    }

    public void RequestRelease()
    {
        numOfInputLocks--;
        if (numOfInputLocks == 0 && eventSystem != null)
        {
            eventSystem.enabled = true;
        }
    }

    public void RequestSwipeLock()
    {
        numOfSwipeLocks++;
    }

    public void RequestSwipeRelease()
    {
        numOfSwipeLocks--;
    }
}
