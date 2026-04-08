using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerItemHandler itemHandler;
    private AudioSource source;

    public AudioClip pickUpClip;
    public AudioClip hitClip;

    private Item nearBy;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        itemHandler = GetComponent<PlayerItemHandler>();
        source = GetComponent<AudioSource>();
    }

    // =========================
    // UI 버튼 → 호출
    // =========================
    public void PickupItem()
    {
        if (nearBy == null) return;

        if (pickUpClip != null)
            source.PlayOneShot(pickUpClip);

        // 🔥 선택 이벤트 (선택사항)
        GameEvents.OnItemPickup?.Invoke(nearBy.index);

        // 🔥 실제 획득 처리 (여기서 OnItemCollected 발생함)
        nearBy.Pickuped();

        nearBy = null;

        GameEvents.OnItemNearby?.Invoke(false);
    }

    // =========================
    // TRIGGER ENTER
    // =========================
    void OnTriggerEnter2D(Collider2D col)
    {
        // 1. 몬스터 충돌
        if (col.CompareTag("Monster"))
        {
            if (hitClip != null)
                source.PlayOneShot(hitClip);

            stats.Recharge(-30f);

            CameraImpulse.instance.Shake();

            // 🔥 이벤트 (확장용)
            GameEvents.OnPlayerHit?.Invoke();
        }

        // 2. 버프 아이템
        else if (col.CompareTag("PlayerItem"))
        {
            PlayerItem item = col.GetComponent<PlayerItem>();

            if (item.itemType == type.Oxygen)
                stats.Recharge(item.plusOx);

            itemHandler.UseItem(item.itemType);

            Destroy(col.gameObject);
        }

        // 3. 수집 아이템
        else if (col.CompareTag("Item"))
        {
            nearBy = col.GetComponent<Item>();

            // 🔥 UI 표시 이벤트
            GameEvents.OnItemNearby?.Invoke(true);
        }

        // 4. 안전지대
        else if (col.CompareTag("SaftyZone"))
        {
            stats.IsBreathing = true;
        }

        // 5. 도착 지점
        else if (col.CompareTag("Finish"))
        {
            if (GameManager.Instance.Check())
            {
                GetComponent<Player>().StartWinEnding();
            }
        }
    }

    // =========================
    // TRIGGER EXIT
    // =========================
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("SaftyZone"))
        {
            stats.IsBreathing = false;
        }

        else if (col.CompareTag("Item"))
        {
            nearBy = null;

            // 🔥 UI 숨김 이벤트
            GameEvents.OnItemNearby?.Invoke(false);
        }
    }
}