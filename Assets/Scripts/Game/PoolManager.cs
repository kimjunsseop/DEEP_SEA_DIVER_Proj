using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("PoolManager");
                go.AddComponent<PoolManager>();
            }
            return _instance;
        }
    }
    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    public void CreatePool(GameObject obj, int initialize)
    {
        string name = obj.name;
        if(!pools.ContainsKey(name))
        {
            pools.Add(obj.name, new ObjectPool(obj, initialize, transform));
        }
    }
    public GameObject Get(GameObject obj)
    {
        string name = obj.name;
        if(!pools.ContainsKey(name))
        {
            CreatePool(obj,10);
        }
        return pools[name].Get();
    }
    public void Return(GameObject obj)
    {
        string name = obj.name.Replace("(Clone)", "");
        if(pools.ContainsKey(name))
        {
            pools[name].Return(obj);
        }
    }
}
