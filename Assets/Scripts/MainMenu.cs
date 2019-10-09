using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
