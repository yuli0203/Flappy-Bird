using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Linq;
using Logging;

/// <summary>
/// This class allows us to bypass the heavy Update call in monobehavior and create our own update loop within the controllers,
/// without relying on the monobehavior system. This will make our controller classes pure C# and more testable.
/// for a class to subscribe to TickableManager inherit from TickableSubscriber. It is recomanded to have one manager per scope
/// so that it won't attempt to tick dead objects from subscenes
/// </summary>
public class TickableManager : MonoBehaviour
{
    private List<ITickable> tickables = new List<ITickable>();

    void Update()
    {
        // Manually call Tick on all ITickable instances
        foreach (var tickable in tickables)
        {
            tickable.Tick();
        }
    }

    public void Subscribe(ITickable instance)
    {
        if (!tickables.Contains(instance))
        {
            tickables.Add(instance);
        }
    }

    public void UnSubscribe(ITickable instance)
    {
        if (tickables.Contains(instance))
        {
            tickables.Remove(instance);
        }
    }
}
