
using Cysharp.Threading.Tasks;
using Logging;
using UnityEngine;

namespace Animations
{
    public class AnimationService : IAnimationService
    {
        public async UniTask PlayClip(IAnimated playedObject, AnimationClip clip)
        {
            if (playedObject == null)
            {
                CuteLogger.LogError("Trying to animate destroyed gameobject");
                return;
            }

            if (playedObject.Animation == null)
            {
                playedObject.Animation = playedObject.GameObject.GetComponent<Animation>();
                if (playedObject.Animation == null)
                {
                    playedObject.Animation = playedObject.GameObject.AddComponent<Animation>();
                    playedObject.Animation.playAutomatically = false;
                }
            }

            clip.legacy = true;
            playedObject.Animation.AddClip(clip, clip.name);

            playedObject.Animation.Play(clip.name);
            await UniTask.WaitUntil(() =>
            {
                if (playedObject == null || playedObject.Animation)
                {
                    return true;
                }
                return !playedObject.Animation.isPlaying;
            });
        }
    }
}