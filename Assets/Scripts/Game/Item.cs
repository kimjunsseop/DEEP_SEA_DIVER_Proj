using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    public int itemType;
    public bool isPickUped;
    public int index;

    void Start()
    {
        isPickUped = false;
    }

    public void Pickuped()
    {
        if (isPickUped) return;

        isPickUped = true;

        // 🔥 index 기준으로 이벤트
        GameEvents.OnItemCollected?.Invoke(index);

        Destroy(gameObject);
}
}