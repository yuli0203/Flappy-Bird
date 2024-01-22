using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectibleEffectFactory
{
    public UniTask CreateEffect(Collider collider);

}
