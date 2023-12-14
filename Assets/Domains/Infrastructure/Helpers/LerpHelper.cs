using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LerpHelper 
{
    public static IEnumerator LerpBetweenValue(float startValue, float endValue, float duration, AnimationCurve animationCurve, Action<float> onLerp)
    {
        var progress = 0f;

        while (progress < duration)
        {
            try
            {
                progress += Time.deltaTime;
                var progressPercent = Mathf.Clamp01(progress / duration);
                var valueOnCurve = animationCurve.Evaluate(progressPercent);
                var currentValue = Mathf.Lerp(startValue, endValue, valueOnCurve);
              //    Debug.Log($"{startValue}, {endValue}, {valueOnCurve}");

                onLerp?.Invoke(currentValue);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                break;
            }

            yield return null;
        }
    }

}
