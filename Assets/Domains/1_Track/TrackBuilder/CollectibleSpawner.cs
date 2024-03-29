using Animations;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class CollectibleSpawner : TickableSubscriber
{
    private CollectibleSpawnerData spawnerData;
    private IAnimationService animationService;
    private List<GameObject> spawns = new List<GameObject>();
    private List<GameObject> toRemove = new List<GameObject>();
    private List<SimplePool> collectiblePools = new List<SimplePool>();
    private Dictionary<GameObject, Collider> colliderCache = new Dictionary<GameObject, Collider>();
    private Dictionary<CollectibleData, Transform> lastSpawnPositions = new Dictionary<CollectibleData, Transform>();

    private int initialPoolSize = 10;

    // Distance to despawn objects
    private readonly float lineOfSightDistance = -4;

    private GameObject lastSpawn;
    private bool spawn;

    // Animate
    public float rotationSpeed = 50f; // Adjust the speed of rotation in the Inspector
    public Vector3 rotationDirection = new Vector3(0, -1, 0);

    [Inject]
    public void Construct(CollectibleSpawnerData spawnerData, PoolViewModel poolParent, IAnimationService animationService)
    {
        this.spawnerData = spawnerData;
        this.animationService = animationService;

        // Initialize collectible pools
        foreach (CollectibleData collectibleType in spawnerData.supportedTypes)
        {
            var pool = new SimplePool(poolParent.Transform, collectibleType.prefab, initialPoolSize);
            collectiblePools.Add(pool);
        }

        SpawnCollectibles();
    }

    protected override void MakeTick()
    {
        if (!spawn)
        {
            return;
        }

        toRemove.Clear();
        foreach (var obj in spawns)
        {
            if (obj == null)
            {
                return;
            }
            obj.transform.Translate(Vector3.back * spawnerData.moveSpeed * Time.deltaTime);

            if (obj.transform.position.z < lineOfSightDistance)
            {
                toRemove.Add(obj);
                // Return the collectible to the pool when it's no longer needed
                DespawnCollectible(obj);
            }
        }

        foreach(var obj in toRemove)
        {
            spawns.Remove(obj);
        }

        SpawnCollectibles();
    }

    private void SpawnCollectibles()
    {
        float currentZ = spawnerData.minSpawnDistance + (lastSpawn != null ? lastSpawn.transform.position.z : 0);

        for (int i = 0; i < spawnerData.spawnAttempts - spawns.Count; i++)
        {
            if (Random.Range(0, 100) <= spawnerData.percentToFailSpawn)
            {
                continue;
            }

            // Randomly select a collectible type from supported types
            CollectibleData randomCollectible = null;

            while (randomCollectible == null)
            {
                randomCollectible = spawnerData.supportedTypes[Random.Range(0, spawnerData.supportedTypes.Length)];
                // Check if enough distance has passed since the last spawn of this collectible type
                if (randomCollectible.distanceBetweenLastSpawn != 0 && lastSpawnPositions.TryGetValue(randomCollectible, out Transform lastSpawnPosition) &&
                    currentZ - lastSpawnPosition.position.z < randomCollectible.distanceBetweenLastSpawn)
                {
                    // Attempt to spawn something else if too soon
                    randomCollectible = null;
                }

            }

            // Use LeanPool to spawn the collectible prefab
            GameObject collectibleObject = SpawnCollectible(randomCollectible.prefab);
            spawns.Add(collectibleObject);

            if (randomCollectible.rotate)
            {
                // Should not rotate gameobject with a collider. Expecting to rotate child object
                animationService.Subscribe(collectibleObject.transform.GetChild(0).gameObject, RotationAnimation);
            }

            if (collectibleObject != null)
            {
                // Set the position based on currentX
                float randomX = Random.Range(-spawnerData.width, spawnerData.width);

                // Get or cache the collider component
                Collider collider = GetOrCacheCollider(collectibleObject);

                if (collider != null)
                {
                    // Calculate the clamped X position based on the collider bounds
                    float clampedX = Mathf.Clamp(randomX, -spawnerData.width + collider.bounds.extents.x, spawnerData.width - collider.bounds.extents.x);

                    float randomY = Random.Range(-spawnerData.height, spawnerData.height);

                    // Set the position with the clamped X value
                    collectibleObject.transform.position = new Vector3(clampedX, spawnerData.center + randomY, currentZ);
                }
                else
                {
                    Debug.LogError("Collider component not found on the spawned collectible object.");
                }

                // Adjust currentX for the next spawn
                currentZ += spawnerData.distanceBetweenSpawns;

                lastSpawn = collectibleObject;

                // Update the last spawn position for this collectible type
                lastSpawnPositions[randomCollectible] = collectibleObject.transform;
            }
        }
    }

    private Collider GetOrCacheCollider(GameObject collectibleObject)
    {
        // Check if the collider is already cached
        if (colliderCache.TryGetValue(collectibleObject, out Collider cachedCollider))
        {
            return cachedCollider;
        }

        // If not, get the collider and cache it
        Collider collider = collectibleObject.GetComponent<Collider>();

        if (collider != null)
        {
            colliderCache.Add(collectibleObject, collider);
        }
        else
        {
            Debug.LogError("Collider component not found on the spawned collectible object.");
        }

        return collider;
    }

    private GameObject SpawnCollectible(GameObject prefab)
    {
        foreach (var pool in collectiblePools)
        {
            if (pool.prefab == prefab)
            {
                return pool.Spawn(Vector3.zero);
            }
        }


        Debug.LogError("No pool found for the specified prefab.");
        return null;
    }
    public void Despawn(GameObject collectibleObject)
    {
        if (spawns.Contains(collectibleObject))
        {
            spawns.Remove(collectibleObject);
        }
        DespawnCollectible(collectibleObject);
    }

    private void DespawnCollectible(GameObject collectibleObject)
    {
        foreach (var pool in collectiblePools)
        {
            if (pool.prefab.name + "(Clone)" == collectibleObject.name)
            {
                pool.Despawn(collectibleObject);
                return;
            }
        }

        Debug.LogError("No pool found for the specified collectible object.");
    }

    internal void Spawn(bool spawn)
    {
        this.spawn = spawn;
    }

    public void RotationAnimation(Transform trans)
    {
        trans.Rotate(rotationDirection, rotationSpeed * Time.deltaTime);
    }
}
