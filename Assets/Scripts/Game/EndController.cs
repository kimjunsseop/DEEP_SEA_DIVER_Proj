using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndController : MonoBehaviour
{

    public TextMeshProUGUI win;
    public TextMeshProUGUI die;
    void Start()
    {
        if(SceneController.instance.result == true)
        {
            pritWin();
        }
        else
        {
            printDie();
        }
    }
    public bool reward = false;
    public void Lobby()
    {
         SceneManager.LoadScene(0);
    }
    public void RE()
    {
        SceneManager.LoadScene(1);
    }
    public void pritWin()
    {
        win.gameObject.SetActive(true);
        die.gameObject.SetActive(false);
    }
    public void printDie()
    {
        die.gameObject.SetActive(true);
        win.gameObject.SetActive(false);
    }
}
