using System.Collections.Generic;
using UnityEngine;

public class SimplePool
{
    private Queue<GameObject> objectQueue = new Queue<GameObject>();
    private Transform parent;
    public GameObject prefab;

    public SimplePool(Transform parent, GameObject prefab, int initialSize)
    {
        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject();
            objectQueue.Enqueue(obj);
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.transform.SetParent(parent);

        obj.SetActive(false);
        return obj;
    }

    public GameObject Spawn(Vector3 position)
    {
        GameObject obj;

        if (objectQueue.Count > 0)
        {
            obj = objectQueue.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
        }

        obj.transform.position = position;
        obj.SetActive(true);

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }
}