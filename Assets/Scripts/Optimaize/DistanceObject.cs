using UnityEngine;

public class DistanceObject : MonoBehaviour
{
    [SerializeField] private float checkDistance = 15f;

    private bool isActive = true;

    private MonoBehaviour[] behaviours;
    private Renderer render;

    void Awake()
    {
        behaviours = GetComponents<MonoBehaviour>();
        render = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        DistanceManager.Instance.Register(this);
    }

    void OnDestroy()
    {
        if (DistanceManager.Instance != null)
            DistanceManager.Instance.Unregister(this);
    }


    public void CheckDistance(Vector3 camPos)
    {
        float sqrDist = (transform.position - camPos).sqrMagnitude;
        float sqrCheck = checkDistance * checkDistance;

        if (sqrDist > sqrCheck && isActive)
        {
            SetActiveState(false);
        }
        else if (sqrDist <= sqrCheck && !isActive)
        {
            SetActiveState(true);
        }
    }

    void SetActiveState(bool active)
    {
        isActive = active;

        foreach (var b in behaviours)
        {
            if (b == this) continue;
            b.enabled = active;
        }

        if (render != null)
            render.enabled = active;
    }
}