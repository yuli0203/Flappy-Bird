using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CollectibleSpawnerSO", order = 1)]
public class CollectibleSpawnerSO : ScriptableObject
{
    public CollectibleSpawnerData spawnerData;
}

[Serializable]
public class CollectibleSpawnerData
{
    public CollectibleData[] supportedTypes;

    public float width = 1.5f;
    public float height = 5f;
    public float range = 20f;
    public float minSpawnDistance = 1;
    public float center = 0.2f;

    public float percentToFailSpawn = 25f;
    public float spawnAttempts = 10;

    public float distanceBetweenSpawns = 1;
    public float moveSpeed = 1f;
    public int initialPoolSize = 40;
    public AudioClip savedAudioClip;
}

[Serializable]
public class CollectibleData
{
    public string collectibleId;
    public GameObject prefab;
    public AudioClip sound;
    public float distanceBetweenLastSpawn;
}