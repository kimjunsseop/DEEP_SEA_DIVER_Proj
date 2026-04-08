using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<bool> found = new List<bool>();

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameEvents.OnItemsInitialized += InitGame;
        GameEvents.OnItemCollected += Change;
    }

    void OnDisable()
    {
        GameEvents.OnItemsInitialized -= InitGame;
        GameEvents.OnItemCollected -= Change;
    }

    public void Change(int index)
    {
        if (index < 0 || index >= found.Count) return;

        found[index] = true;
    }
    public bool Check()
    {
        foreach (var f in found)
            if (!f) return false;

        return true;
    }

    void InitGame(GameObject[] items, List<int> indexList, int size)
    {
        found.Clear();

        for (int i = 0; i < size; i++)
            found.Add(false);
    }
}