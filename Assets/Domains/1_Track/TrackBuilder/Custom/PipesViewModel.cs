using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesViewModel : MonoBehaviour
{
    [SerializeField] private Transform safeZoneUp;
    [SerializeField] private Transform safeZoneDown;
    [SerializeField] private BoxCollider pipesCollider;

    public bool IsInSafeZone(Collider collider)
    {
        if (collider == null)
        {
            Debug.LogError("Collider is null.");
            return false;
        }

        // Get the bounds of the collider
        Bounds colliderBounds = collider.bounds;

        // Check if the collider's boundaries are within the safe zone positions
        bool isInSafeZoneUp = safeZoneUp.position.y > colliderBounds.max.y;
        bool isInSafeZoneDown = safeZoneDown.position.y < colliderBounds.min.y;

        return (isInSafeZoneUp && isInSafeZoneDown) || colliderBounds.min.z > pipesCollider.bounds.max.z || colliderBounds.max.z < pipesCollider.bounds.min.z;
    }
}
