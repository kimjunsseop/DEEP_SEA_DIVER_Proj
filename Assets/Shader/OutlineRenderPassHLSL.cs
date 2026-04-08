using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPassHLSL : ScriptableRenderPass
{
    private FilteringSettings filteringSettings;
    private Material outlineMaterial;
    private readonly ShaderTagId shaderTagId = new ShaderTagId("Universal2D");

    public OutlineRenderPassHLSL(Material material)
    {
        // 렌더링 순서 설정 (투명도 렌더링 이후)
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        // "Outline" 레이어만 렌더링하도록 필터링
        filteringSettings = new FilteringSettings(
            RenderQueueRange.all,
            LayerMask.GetMask("Outline")
        );

        outlineMaterial = material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (outlineMaterial == null) return;

        // CommandBuffer 가져오기
        CommandBuffer cmd = CommandBufferPool.Get("Outline Pass");

        // 렌더링 설정
        DrawingSettings drawingSettings = CreateDrawingSettings(
            shaderTagId, 
            ref renderingData, 
            SortingCriteria.CommonTransparent
        );

        // 작성하신 셰이더를 모든 대상에 덮어씌워(Override) 그립니다.
        drawingSettings.overrideMaterial = outlineMaterial;
        drawingSettings.overrideMaterialPassIndex = 0;

        context.DrawRenderers(
            renderingData.cullResults, 
            ref drawingSettings, 
            ref filteringSettings
        );

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}