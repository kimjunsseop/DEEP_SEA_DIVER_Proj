using System.Collections.Generic;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    public static DistanceManager Instance;

    private List<DistanceObject> targets = new List<DistanceObject>();
    private Transform cam;

    [SerializeField] private float checkInterval = 0.3f;
    private float timer;

    void Awake()
    {
        Instance = this;
        cam = Camera.main.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < checkInterval) return;
        timer = 0f;

        Vector3 camPos = cam.position;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].CheckDistance(camPos);
        }
    }

    public void Register(DistanceObject obj)
    {
        if (!targets.Contains(obj))
            targets.Add(obj);
    }

    public void Unregister(DistanceObject obj)
    {
        if (targets.Contains(obj))
            targets.Remove(obj);
    }
}