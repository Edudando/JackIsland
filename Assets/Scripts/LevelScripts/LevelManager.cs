using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public void LoadLevel(string levelName = "TitleScreen")
    {
        Time.timeScale = 1f; //Reset del nivel
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel( int levelIndex)
    {
        Time.timeScale = 1f; //Reset del nivel
        SceneManager.LoadScene(levelIndex);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; //Reset del nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}