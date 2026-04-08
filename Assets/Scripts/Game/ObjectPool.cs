using UnityEngine;
using System.Collections.Generic;
public class ObjectPool
{
    GameObject prefab;
    private Queue<GameObject> pool;
    Transform parent;

    public ObjectPool(GameObject prefab, int initialize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        pool = new Queue<GameObject>();
        for(int i = 0; i < initialize; i++)
        {
            CreatNewObject();
        }
    }

    public void CreatNewObject()
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        go.SetActive(false);
        pool.Enqueue(go);
    }
    public GameObject Get()
    {
        if(pool.Count == 0)
        {
            CreatNewObject();
        }
        GameObject go = pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
