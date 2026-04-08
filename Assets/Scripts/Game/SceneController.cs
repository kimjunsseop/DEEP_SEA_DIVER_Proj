using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool result;
    private static SceneController _instance;
    public static SceneController instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new SceneController();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        // 결과씬으로 이동.
        SceneManager.LoadScene(2);
    }
}
