using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxO2 = 100f;
    public float currentO2;

    public float o2DrainRate = 5f;
    public float o2RegenRate = 15f;

    public bool IsBreathing { get; set; }
    public bool IsDead { get; private set; }

    void Start()
    {
        currentO2 = maxO2;
    }

    void Update()
    {
        if (IsDead) return;

        if (IsBreathing)
            currentO2 = Mathf.Min(currentO2 + o2RegenRate * Time.deltaTime, maxO2);
        else
            currentO2 -= o2DrainRate * Time.deltaTime;

        GameEvents.OnO2Changed?.Invoke(currentO2);

        if (currentO2 <= 0)
        {
            IsDead = true;
            GameEvents.OnPlayerDeath?.Invoke();
        }
    }

    public void Recharge(float amount)
    {
        currentO2 = Mathf.Clamp(currentO2 + amount, 0, maxO2);
        GameEvents.OnO2Changed?.Invoke(currentO2);
    }
}