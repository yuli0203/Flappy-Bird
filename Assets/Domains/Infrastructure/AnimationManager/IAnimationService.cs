using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Animations
{
    public interface IAnimationService
    {
        UniTask PlayClip(IAnimated playedObject, AnimationClip clip);
    }
}

