using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    public int activeCount;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        activeCount = 0;

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int idx)
    {
        GameObject go = null;

        foreach(GameObject obj in pools[idx])
        {
            if (obj.activeSelf == false)
            {
                go = obj;
                go.SetActive(true);
                break;
            }
        }

        if (go == null)
        {
            go = Instantiate(prefabs[idx], transform);
            pools[idx].Add(go);
        }

        return go;
    }
}
