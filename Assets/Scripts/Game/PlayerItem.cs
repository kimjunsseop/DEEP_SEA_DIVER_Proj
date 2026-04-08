using UnityEngine;

public enum type
{
    Speed,
    Light,
    Oxygen,
    Compass
}

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerItem : MonoBehaviour
{
    public type itemType;
    public float plusOx = 30f;

    private SpriteRenderer sr;
    private MaterialPropertyBlock mpb;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        ApplyShaderProperties();
    }

    void ApplyShaderProperties()
    {
        sr.GetPropertyBlock(mpb);

        sr.SetPropertyBlock(mpb);
    }
}