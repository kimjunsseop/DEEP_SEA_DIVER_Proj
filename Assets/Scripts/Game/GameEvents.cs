using System;
using UnityEngine;
using System.Collections.Generic;

public static class GameEvents
{
    // 게임 흐름
    public static Action OnGameStart;
    public static Action<bool> OnGameEnd;

    // 플레이어
    public static Action<float> OnO2Changed;
    public static Action OnPlayerDeath;

    // 아이템
    public static Action<int> OnItemCollected;
    public static System.Action<bool> OnItemNearby; // UI 버튼 표시
    public static System.Action<int> OnItemPickup;  // 아이템 획득
    public static System.Action OnPlayerHit;        // 피격
    public static System.Action<GameObject[], List<int>, int> OnItemsInitialized;
}