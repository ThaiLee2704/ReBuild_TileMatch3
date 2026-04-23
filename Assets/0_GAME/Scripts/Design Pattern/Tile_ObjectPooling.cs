using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_ObjectPooling : Tile_Singleton<Tile_ObjectPooling>
{
    Dictionary<GameObject, List<GameObject>> poolingObjects = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetObject(GameObject prefab, Transform parentTrans)
    {
        if (!poolingObjects.TryGetValue(prefab, out List<GameObject> prefabsPool))
        {
            prefabsPool = new List<GameObject>();
            poolingObjects.Add(prefab, prefabsPool);
        }

        foreach (GameObject go in prefabsPool)
        {
            if (go.activeSelf)
                continue;

            go.transform.SetParent(parentTrans);
            return go;
        }

        GameObject newObject = Instantiate(prefab, parentTrans);
        newObject.SetActive(false);

        prefabsPool.Add(newObject);

        return newObject;
    }
}
