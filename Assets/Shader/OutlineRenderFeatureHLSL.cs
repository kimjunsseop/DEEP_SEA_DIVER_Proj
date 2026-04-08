using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRendererFeatureHLSL : ScriptableRendererFeature
{
    // 이제 인스펙터에서 Material을 받지 않고, 필요하다면 설정값만 받습니다.
    private Material _outlineMaterial;
    private const string ShaderName = "Custom/URP/OutlineHLSLShader";
    OutlineRenderPassHLSL outlinePass;

    public override void Create()
    {
        // 셰이더를 이름으로 찾습니다.
        Shader shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            Debug.LogError($"[{nameof(OutlineRendererFeatureHLSL)}] 셰이더를 찾을 수 없습니다: {ShaderName}");
            return;
        }

        // 런타임에 임시 머티리얼 생성
        if (_outlineMaterial == null || _outlineMaterial.shader != shader)
        {
            _outlineMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        outlinePass = new OutlineRenderPassHLSL(_outlineMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // 머티리얼이 정상적으로 생성되었을 때만 패스 추가
        if (_outlineMaterial != null)
        {
            renderer.EnqueuePass(outlinePass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        // 메모리 누수 방지를 위해 생성한 머티리얼 제거
        CoreUtils.Destroy(_outlineMaterial);
        _outlineMaterial = null;
    }
}