using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance;
    public Monster[] monsters;
    public int MaxSize;
    public Transform[] monSpawns;
    Queue<int> arr = new Queue<int>();
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        InitializeMonster();
    }
    void Update()
    {
        
    }

    void InitializeMonster()
    {
        List<int> rand = new List<int>();
        for(int i = 0; i < monSpawns.Length; i++)
        {
            arr.Enqueue(i);
        }
        rand = arr.ToList();
        for(int i = 0; i < rand.Count; i++)
        {
            int temp = rand[i];
            int randomIndex = Random.Range(i,rand.Count);
            rand[i] = rand[randomIndex];
            rand[randomIndex] = temp;
        }
        arr.Clear();
        for(int i = 0; i < rand.Count; i++)
        {
            arr.Enqueue(rand[i]);
        }
        for(int i = 0; i < MaxSize; i++)
        {
            int mr = Random.Range(0, monsters.Length);
            int a = arr.Dequeue();
            Instantiate(monsters[mr], monSpawns[a].position, Quaternion.identity);
            arr.Enqueue(a);
            
        }
    }

    public void ReSpawn()
    {
        int mr = Random.Range(0, monsters.Length);
        int a = arr.Dequeue();
        Instantiate(monsters[mr], monSpawns[a].position, Quaternion.identity);
        arr.Enqueue(a);
    }
}
