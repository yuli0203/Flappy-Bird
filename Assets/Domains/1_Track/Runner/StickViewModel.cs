using Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StickViewModel : MonoBehaviour
{
    [SerializeField] GameObject stick;
    [SerializeField] float maxSize = 3f;
    [SerializeField] float stickSizeDelta = 0.2f;

    public float StickSize => stick.transform.localScale.z;
    public float StickSizeDelta => stickSizeDelta;

    public void ChangeSize(float newScale)
    {
        if (newScale > maxSize || newScale <= 0)
        {
            return;
        }
        var oldScale = stick.transform.localScale;
        stick.transform.localScale = new Vector3(oldScale.x, oldScale.y, newScale);
    }

    private void OnValidate()
    {
        if (stick == null)
        {
            CuteLogger.LogError("Stick gameobject is null!");
        }
    }
}
