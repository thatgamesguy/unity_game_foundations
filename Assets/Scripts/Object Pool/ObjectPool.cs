using UnityEngine;
using System;
using System.Collections.Generic;


/// <summary>
/// An item to be pooled. Also includes number of objects to pool.
/// </summary>
[System.Serializable]
public class ObjectPoolItem
{
    public GameObject prefab;
    public int numToPool = 1;
}

public class ObjectPool : Singleton<ObjectPool>
{
    /// <summary>
    /// The items to pool.
    /// </summary>
    public ObjectPoolItem[] objectPoolItems;

    /// <summary>
    /// The parent that holds all pooled objects. Once spawned the 
    /// objects are removed from container.
    /// </summary>
    public GameObject container;

    /// <summary>
    /// Returned in place of item index when no index found.
    /// </summary>
    private static readonly int PREFAB_NOT_FOUND = -1;

    /// <summary>
    /// The pooled objects currently available.
    /// </summary>
    private List<GameObject>[] pooledObjects;

    void Start()
    {
        if (container == null)
        {
            container = new GameObject("ObjectPool");
        }

        pooledObjects = new List<GameObject>[objectPoolItems.Length];

        // Loop through all objects to pool and instantiate the required number of prefabs.
        // Store these within the object list for later retrieval.
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            pooledObjects[i] = new List<GameObject>();

            var item = objectPoolItems[i];

            for (int n = 0; n < item.numToPool; n++)
            {
                PoolObject(InstantiatePrefab(item.prefab));
            }
        }

    }

    /// <summary>
    /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
    /// </summary>
    /// <param name="obj"> Object to be pooled.</param>
    public void PoolObject(GameObject obj)
    {
        for (int i = 0; i < objectPoolItems.Length; i++)
        {
            if (objectPoolItems[i].prefab.name == obj.name)
            {
                obj.SetActive(false);
                obj.transform.SetParent(container.transform);
                pooledObjects[i].Add(obj);
                return;
            }
        }

        throw new PrefabNotFoundException(obj.name + ": not setup to use object pool");
    }

    /// <summary>
    /// Returns an instantiated object of type if pooled. 
    /// If resource pooled but not available then a new object is instantiated. 
    /// Throws PrefabNotFoundException if prefab not pooled.
    /// </summary>
    /// <param name="objectType">The name of the prefab to retrieve/instantiate.</param>
    /// <returns>The pooled gameobject.</returns>
    public GameObject GetObject(string objectType)
    {
        int itemIndex = GetPrefabIndex(objectType);

        if (itemIndex != PREFAB_NOT_FOUND)
        {
            GameObject gameObject = null;

            try
            {
                gameObject = GetPooledItem(itemIndex);
            }
            catch (PrefabNotFoundException)
            {
                Debug.Log("Instantiating new item: " + objectPoolItems[itemIndex].prefab.name);
                gameObject = Instantiate(objectPoolItems[itemIndex].prefab) as GameObject;
                gameObject.name = objectPoolItems[itemIndex].prefab.name;
            }

            return gameObject;
        }

        throw new PrefabNotFoundException("Item not found in object pool, cannot instantiate or retrieve.");

    }

    /// <summary>
    /// Returns an instantiated object of type if pooled. 
    /// If resource pooled but not available then exception thrown.
    /// Throws PrefabNotFoundException if prefab not pooled or not available.
    /// </summary>
    /// <param name="objectType">The name of the prefab to retrieve/instantiate.</param>
    /// <returns>The pooled gameobject.</returns>
    public GameObject GetAvailableObject(string objectType)
    {
        int itemIndex = GetPrefabIndex(objectType);

        try
        {
            return GetPooledItem(itemIndex);
        }
        catch (PrefabNotFoundException)
        {
            throw new PrefabNotFoundException("Resource not available in pool.");
        }
    }

    /// <summary>
    /// Returns an objects index in objectPoolItems array.
    /// </summary>
    /// <param name="objectType">The type of objects index to retrieve.</param>
    /// <returns>Returns index of object of type.</returns>
    private int GetPrefabIndex(string objectType)
    {
        for (int i = 0; i < objectPoolItems.Length; i++)
        {
            var prefab = objectPoolItems[i].prefab;
            if (prefab.name == objectType)
            {
                return i;
            }
        }

        return PREFAB_NOT_FOUND;
    }

    /// <summary>
    /// Returns an item from the object pool and removes reference to item. 
    /// Throws PrefabNotFoundException if no pooled items.
    /// </summary>
    /// <param name="itemIndex">Index of item to retrieve.</param>
    /// <returns>Object from pool.</returns>
    private GameObject GetPooledItem(int itemIndex)
    {
        if (itemIndex != PREFAB_NOT_FOUND && pooledObjects[itemIndex].Count > 0)
        {
            GameObject pooledObject = pooledObjects[itemIndex][0];

            if (pooledObject)
            {
                pooledObjects[itemIndex].RemoveAt(0);
                pooledObject.transform.SetParent(null, false);
                return pooledObject;
            }

        }

        throw new PrefabNotFoundException("Cannot retrieve item");
    }

    /// <summary>
    /// Instantiates and returns gameobject. Expensive method used 
    /// during initialisation.
    /// </summary>
    /// <param name="prefab">Object to create.</param>
    /// <returns>Instantiated Object.</returns>
    private GameObject InstantiatePrefab(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab) as GameObject;
        newObj.name = prefab.name;
        newObj.SetActive(false);
        return newObj;
    }
}

/// <summary>
/// Exception thrown when prefab not found in ObjectPool.
/// </summary>
public class PrefabNotFoundException : Exception
{

    public PrefabNotFoundException()
    {
    }

    public PrefabNotFoundException(string message)
    : base(message)
    {
    }

    public PrefabNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

