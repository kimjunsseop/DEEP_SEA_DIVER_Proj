using UnityEngine;

public class DarkMaskController : MonoBehaviour
{
    [Header("Reference")]
    public Transform player;
    public Material maskMaterial;

    [Header("Settings")]
    public float radius = 4f;
    public float fade = 1.5f;
    public float darkness = 0.8f;
    public float depthStrength = 0.02f;

    void Start()
    {
        ApplySettings();
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 playerPos = player.position;
            maskMaterial = GetComponent<Renderer>().material;
            maskMaterial.SetVector("_PlayerPos", playerPos);
        }
    }

    void ApplySettings()
    {
        maskMaterial.SetFloat("_Radius", radius);
        maskMaterial.SetFloat("_Fade", fade);
        maskMaterial.SetFloat("_Darkness", darkness);
        maskMaterial.SetFloat("_DepthStrength", depthStrength);
    }
}