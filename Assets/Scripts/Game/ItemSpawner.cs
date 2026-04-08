using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    public Transform[] spawnPoints;
    public Transform[] playerItemSpawnPoints;

    public GameObject[] items;
    public GameObject[] playerItems;

    public List<Transform> location;

    public int playerItemSize = 5;
    public int itemSize = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnItems();
        SpawnPlayerItems();
    }

    void SpawnItems()
    {
        location = new List<Transform>();

        List<int> spawnIdx = Shuffle(spawnPoints.Length);
        List<int> itemIdx = Shuffle(items.Length);

        for (int i = 0; i < itemSize; i++)
        {
            GameObject go = Instantiate(
                items[itemIdx[i]],
                spawnPoints[spawnIdx[i]].position,
                Quaternion.identity
            );

            Item item = go.GetComponent<Item>();
            item.index = i;

            location.Add(go.transform);
        }

        GameEvents.OnItemsInitialized?.Invoke(items, itemIdx, itemSize);
    }
    void SpawnPlayerItems()
    {
        List<int> spawnIdx = Shuffle(playerItemSpawnPoints.Length);

        for (int i = 0; i < playerItemSize; i++)
        {
            int rand = Random.Range(0, playerItems.Length);

            Instantiate(
                playerItems[rand],
                playerItemSpawnPoints[spawnIdx[i]].position,
                Quaternion.identity
            );
        }
    }

    List<int> Shuffle(int count)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < count; i++)
            list.Add(i);

        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }

        return list;
    }
}