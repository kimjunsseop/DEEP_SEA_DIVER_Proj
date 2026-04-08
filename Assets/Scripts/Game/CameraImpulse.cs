using UnityEngine;
using Unity.Cinemachine;

public class CameraImpulse : MonoBehaviour
{
    public static CameraImpulse instance;
    [SerializeField] CinemachineImpulseSource impulse;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void Shake()
    {
        impulse.GenerateImpulse();
    }
}
