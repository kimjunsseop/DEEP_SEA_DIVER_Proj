using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerItemHandler : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerMovement movement;
    
    [Header("UI & Visuals")]
    public Image itemGage;
    public ParticleSystem bubbleEffect;
    public Image arrow;
    public Light2D spotLight;
    
    [Header("Post Processing")]
    public Volume volume;
    private Bloom bloom;

    private float maxItemDuration = 5f;
    private bool isItemActive = false;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        if (volume != null && volume.profile.TryGet(out bloom)) { }
    }

    void Start()
    {
        // 시작할 때 UI들을 비활성화 (이게 없어서 계속 켜져 있던 것입니다)
        if (itemGage != null) itemGage.enabled = false;
        if (arrow != null) arrow.gameObject.SetActive(false);
        if (bubbleEffect != null) bubbleEffect.gameObject.SetActive(false);
        var main = bubbleEffect.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }

    public void UseItem(type itemType)
    {
        if (isItemActive && itemType != type.Oxygen) return;

        switch (itemType)
        {
            case type.Speed: StartCoroutine(SpeedUpRoutine(maxItemDuration)); break;
            case type.Light: StartCoroutine(ExpandLightRoutine(maxItemDuration)); break;
            case type.Compass: StartCoroutine(OnCompassRoutine(maxItemDuration)); break;
            case type.Oxygen: StartCoroutine(OXChargeEffect(3f)); break;
        }
    }

    IEnumerator SpeedUpRoutine(float duration)
    {
        isItemActive = true;

        itemGage.enabled = true;
        bubbleEffect.gameObject.SetActive(true);

        float originalSpeed = movement.speed;
        movement.speed = originalSpeed * 2f;

        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // 🔥 이동 방향 기준으로 파티클 회전
            Vector2 dir = movement.LastInput;

            if (dir != Vector2.zero)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                // 🔥 반대 방향으로 분출
                bubbleEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
            }

            itemGage.fillAmount = timer / duration;

            yield return null;
        }

        movement.speed = originalSpeed;

        bubbleEffect.gameObject.SetActive(false);

        FinishItem();
    }

    IEnumerator ExpandLightRoutine(float duration)
    {
        isItemActive = true;
        itemGage.enabled = true;

        float originalRadius = spotLight.pointLightOuterRadius;
        spotLight.pointLightOuterRadius *= 2;
        
        yield return StartCoroutine(GageUpdateRoutine(duration));

        spotLight.pointLightOuterRadius = originalRadius;
        FinishItem();
    }

    IEnumerator OnCompassRoutine(float duration)
    {
        Transform targetObj = null;
        float minDistance = Mathf.Infinity;

        // 1. 타겟 찾기 (ItemSpawner 검증)
        if (ItemSpawner.Instance == null || ItemSpawner.Instance.location == null)
        {
            Debug.LogWarning("ItemSpawner가 없거나 location이 비어있습니다.");
            yield break;
        }

        for (int i = 0; i < ItemSpawner.Instance.itemSize; i++)
        {
            Transform currentItem = ItemSpawner.Instance.location[i];
            if (currentItem != null)
            {
                Item itemScript = currentItem.GetComponent<Item>();
                // 아이템이 존재하고 아직 안 먹은 상태인지 확인
                if (itemScript != null && !itemScript.isPickUped)
                {
                    float dist = Vector2.Distance(transform.position, currentItem.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        targetObj = currentItem;
                    }
                }
            }
        }

        // 2. 타겟이 있을 때만 화살표 활성화
        if (targetObj != null)
        {
            isItemActive = true;
            itemGage.enabled = true;
            arrow.gameObject.SetActive(true);

            float currentTime = 0f;
            while (currentTime < duration)
            {
                // 추적 중에 아이템을 먹어버리면 타겟을 다시 찾거나 중단
                if (targetObj == null || targetObj.GetComponent<Item>().isPickUped) break; 

                currentTime += Time.deltaTime;
                itemGage.fillAmount = 1f - (currentTime / duration);

                // 화살표 회전 로직
                Vector2 direction = targetObj.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                
                yield return null;
            }
            
            arrow.gameObject.SetActive(false);
            FinishItem();
        }
        else
        {
            Debug.Log("추적할 아이템이 씬에 없습니다.");
        }
    }

    IEnumerator OXChargeEffect(float duration)
    {
        if (bloom == null) yield break;
        int totalCount = 2;
        float timePerCount = duration / totalCount;

        for (int i = 0; i < totalCount; i++)
        {
            float elapsed = 0f;
            float half = timePerCount / 2f;
            while (elapsed < half) { elapsed += Time.deltaTime; bloom.intensity.value = Mathf.Lerp(1f, 10f, elapsed / half); yield return null; }
            elapsed = 0f;
            while (elapsed < half) { elapsed += Time.deltaTime; bloom.intensity.value = Mathf.Lerp(10f, 1f, elapsed / half); yield return null; }
        }
        bloom.intensity.value = 0f;
    }

    IEnumerator GageUpdateRoutine(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            itemGage.fillAmount = timer / duration;
            yield return null;
        }
    }

    private void FinishItem()
    {
        isItemActive = false;
        if (itemGage != null) itemGage.enabled = false;
        if (arrow != null) arrow.gameObject.SetActive(false);
    }
}