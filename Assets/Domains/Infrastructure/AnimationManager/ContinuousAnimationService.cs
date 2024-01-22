using Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ContinuousAnimationService : TickableSubscriber
{

    public static ContinuousAnimationService instance = null;

    [Inject]
    public void Construct()
    {
        CuteLogger.LogMessage("Construct");
        instance = this;
    }

}
