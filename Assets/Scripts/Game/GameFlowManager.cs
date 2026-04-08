using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    private bool result;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnd += EndGame;
    }

    void OnDisable()
    {
        GameEvents.OnGameStart -= StartGame;
        GameEvents.OnGameEnd -= EndGame;
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void EndGame(bool isWin)
    {
        result = isWin;
        SceneManager.LoadScene("EndScene");
    }

    public bool GetResult() => result;

    public void GoLobby()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }
}