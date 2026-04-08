using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("O2")]
    public Image O2Gage;

    [Header("Item UI")]
    public Image[] items;

    // ❌ 기존 itemss 제거 (버그 원인)
    // public Dictionary<int, GameObject> itemss

    [Header("Player Ref")]
    public Player player;

    [Header("Depth UI")]
    public RectTransform depthImage;

    [Header("Interaction UI")]
    public Animator buttonAnim;

    [Header("Texts")]
    public TextMeshProUGUI startText;
    public string startMessage;
    public float startTextInterval;

    public TextMeshProUGUI deathText;
    public TextMeshProUGUI succesText;

    // =========================
    // UNITY LIFECYCLE
    // =========================

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        GameEvents.OnO2Changed += SetPercent;
        GameEvents.OnItemCollected += OnItemCollected;
        GameEvents.OnItemNearby += ToggleItemButton;
        GameEvents.OnItemsInitialized += InitItems;
    }

    void OnDisable()
    {
        GameEvents.OnO2Changed -= SetPercent;
        GameEvents.OnItemCollected -= OnItemCollected;
        GameEvents.OnItemNearby -= ToggleItemButton;
        GameEvents.OnItemsInitialized -= InitItems;
    }

    // =========================
    // O2 UI
    // =========================

    public void SetPercent(float value)
    {
        O2Gage.fillAmount = value / 100f;
    }

    // =========================
    // ITEM UI (핵심 수정 영역)
    // =========================

    void InitItems(GameObject[] itemsArr, List<int> indexList, int size)
    {
        // 🔥 UI 전체 초기화 (중요)
        for (int i = 0; i < items.Length; i++)
        {
            items[i].sprite = null;
            items[i].color = new Color(1, 1, 1, 0f);
        }

        // 🔥 index 기반으로만 세팅
        for (int i = 0; i < size; i++)
        {
            if (i >= items.Length)
            {
                Debug.LogWarning("UI 슬롯 부족");
                break;
            }

            Item itemData = itemsArr[indexList[i]].GetComponent<Item>();

            items[i].sprite = itemData.sprite;
            items[i].color = new Color(1, 1, 1, 0.5f);
        }
    }

    // 🔥 핵심: index 기반 UI 갱신
    private void OnItemCollected(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            Debug.LogWarning($"잘못된 index 접근: {index}");
            return;
        }

        items[index].color = new Color(1, 1, 1, 1f);
    }

    // ❌ 기존 ChangeAlpha 완전 제거 (버그 원인)
    // public void ChangeAlpha(int type) → 삭제

    // =========================
    // DEPTH UI
    // =========================

    public void SetDepth(Transform depth)
    {
        Vector2 pos = new Vector2(48f + 1.58f * depth.position.y, 0);
        depthImage.anchoredPosition = pos;
    }

    // =========================
    // INTERACTION BUTTON
    // =========================

    public void ButtonAnimT()
    {
        buttonAnim.SetBool("isItem", true);
    }

    public void ButtonAnimF()
    {
        buttonAnim.SetBool("isItem", false);
    }

    void ToggleItemButton(bool isShow)
    {
        buttonAnim.SetBool("isItem", isShow);
    }

    // =========================
    // TEXT UI
    // =========================

    public void StartMessage()
    {
        StartCoroutine(StartText());
    }

    IEnumerator StartText()
    {
        startText.gameObject.SetActive(true);
        startText.text = "";

        foreach (char c in startMessage)
        {
            startText.text += c;
            yield return new WaitForSeconds(startTextInterval);
        }

        yield return new WaitForSeconds(1.5f);

        startText.gameObject.SetActive(false);

        player.isPlaying = true;
    }

    public void ShowDeath()
    {
        deathText.gameObject.SetActive(true);
    }

    public void ShowSuccess()
    {
        succesText.gameObject.SetActive(true);
    }
}