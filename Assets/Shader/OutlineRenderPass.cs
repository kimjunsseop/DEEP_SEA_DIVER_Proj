using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


// Shader Graph로 작성된 Shader를 사용할때와
// HLSL 코드로 작성된 Shader를 사용할 때, Shader를 추가하는 방식의 차이가 존재함.
public class OutlineRenderPass : ScriptableRenderPass
{
    FilteringSettings filteringSettings;
    Material outlineMaterial;

    public OutlineRenderPass(Material material)
    {
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        filteringSettings = new FilteringSettings(
            RenderQueueRange.all,
            LayerMask.GetMask("Outline")
        );

        outlineMaterial = material;
    }

    public override void Execute(
        ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        if (outlineMaterial == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get("Outline Pass");

        DrawingSettings drawingSettings =
            CreateDrawingSettings(
                new ShaderTagId("UniversalForward"),
                ref renderingData,
                SortingCriteria.CommonTransparent
            );

        drawingSettings.overrideMaterial = outlineMaterial;

        context.DrawRenderers(
            renderingData.cullResults,
            ref drawingSettings,
            ref filteringSettings
        );

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
