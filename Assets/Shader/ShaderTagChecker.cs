using UnityEngine;
using UnityEngine.Rendering;

public class ShaderTagChecker : MonoBehaviour
{
    [Header("조회할 셰이더를 할당하세요")]
    public Shader targetShader;

    [Header("조회할 태그 이름 (예: LightMode, RenderType)")]
    public string tagName = "UniversalForward";

    void Start()
    {
        if (targetShader == null)
        {
            Debug.LogError("인스펙터에서 Shader를 할당해주세요!");
            return;
        }

        GetShaderTagInfo();
    }

    void GetShaderTagInfo()
    {
        // 1. 찾고 싶은 태그 자체의 ID (예: 'LightMode'라는 이름의 ID)
        ShaderTagId tagIdToFind = new ShaderTagId(tagName);

        // 2. 0번 패스에서 해당 태그에 설정된 '값'의 ID를 가져옴
        // 만약 Pass가 여러 개라면 반복문을 돌려야 합니다.
        ShaderTagId resultTagValueId = targetShader.FindPassTagValue(0, tagIdToFind);

        // 3. 결과 출력
        if (resultTagValueId != ShaderTagId.none)
        {
            Debug.Log($"[태그 확인] 셰이더: {targetShader.name}");
            Debug.Log($"조회한 태그 이름: {tagName}");
            Debug.Log($"설정된 값의 ID 이름: {resultTagValueId.name}");
            
            // 비교 예시
            if (resultTagValueId == new ShaderTagId("UniversalForward"))
            {
                Debug.Log("이 셰이더는 URP 기본 포워드 패스입니다.");
            }
        }
        else
        {
            Debug.LogWarning($"{targetShader.name}의 0번 패스에는 '{tagName}' 태그가 정의되어 있지 않습니다.");
        }
    }
}