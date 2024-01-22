
using Cysharp.Threading.Tasks;
using Logging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationService : TickableSubscriber, IAnimationService
    {
        public Dictionary<GameObject, Action<Transform>> gameObjectsToAnimate = new Dictionary<GameObject, Action<Transform>>();

        public void Subscribe(GameObject obj, Action<Transform> animation)
        {
            if (gameObjectsToAnimate.ContainsKey(obj))
            {
                return;
            }
            gameObjectsToAnimate.Add(obj, animation);
        }

        public void UnSubscribe(GameObject obj)
        {
            gameObjectsToAnimate.Remove(obj);
        }

        protected override void MakeTick()
        {
            foreach (GameObject key in gameObjectsToAnimate.Keys)
            {
                if (key != null && key.activeSelf)
                {
                    gameObjectsToAnimate[key].Invoke(key.transform);
                }
            }
        }

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