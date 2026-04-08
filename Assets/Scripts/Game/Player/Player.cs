using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerMovement movement;

    public Light2D globalLight;
    public Light2D spotLight;

    public bool isPlaying = false;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        GameEvents.OnPlayerDeath += OnDeath;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= OnDeath;
    }

    void Start()
    {
        UIManager.Instance.StartMessage();
    }

    void Update()
    {
        if (!isPlaying) return;

        movement.HandleMove(!stats.IsDead);

        if (!stats.IsBreathing)
            globalLight.intensity = 0.1f + (transform.position.y / 200);
        else
            globalLight.intensity = 1f;

        spotLight.enabled = !stats.IsBreathing;

        UIManager.Instance.SetDepth(transform);
    }

    void OnDeath()
    {
        StartCoroutine(LoseEnding());
    }

    IEnumerator LoseEnding()
    {
        isPlaying = false;

        yield return new WaitForSeconds(1f);

        while (spotLight.intensity > 0)
        {
            spotLight.intensity -= Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.ShowDeath();

        GameEvents.OnGameEnd?.Invoke(false);
    }

    public void StartWinEnding()
    {
        StartCoroutine(WinEnding());
    }

    IEnumerator WinEnding()
    {
        isPlaying = false;

        yield return new WaitForSeconds(0.2f);

        UIManager.Instance.ShowSuccess();

        yield return new WaitForSeconds(1.5f);

        GameEvents.OnGameEnd?.Invoke(true);
    }
}