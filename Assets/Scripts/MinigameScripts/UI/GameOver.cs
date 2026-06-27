/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que habilita la pantalla de GameOver]
 */

using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void GetGameOverPanel()
    {
        // Se llama al método set del gameManager para indicar que finalizó el juego
        MinigameManager.Instance.SetGameFinished();

        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
}
