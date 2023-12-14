using Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TickableSubscriber : ITickable
{
    private TickableManager tickManager;

    [Inject]
    public void Construct(TickableManager tickManager)
    {
        this.tickManager = tickManager;
        tickManager.Subscribe(this);
    }

    public void Tick()
    {
        MakeTick();
    }

    protected virtual void MakeTick() { }

    ~TickableSubscriber()
    {
        tickManager.UnSubscribe(this);
    }
}
