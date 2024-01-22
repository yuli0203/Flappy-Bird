using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Animations
{
    public interface IAnimationService
    {
        UniTask PlayClip(IAnimated playedObject, AnimationClip clip);

        public void Subscribe(GameObject obj, Action<Transform> animation);
        public void UnSubscribe(GameObject obj);
    }
}

