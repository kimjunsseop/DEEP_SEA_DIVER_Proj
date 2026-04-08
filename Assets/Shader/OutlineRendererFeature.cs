using UnityEngine;
using UnityEngine.Rendering.Universal;

// RenderPass 생성 및 Renderer에 등록
public class OutlineRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Material outlineMaterial;

    OutlineRenderPass outlinePass;

    public override void Create()
    {
        outlinePass = new OutlineRenderPass(outlineMaterial);
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (outlineMaterial == null)
            return;

        renderer.EnqueuePass(outlinePass);
    }
}
