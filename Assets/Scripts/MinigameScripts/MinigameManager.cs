/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Manager del minijuego]
 */


using TMPro;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    //Instancia estática única del MinigameManager
    public static MinigameManager Instance {get; private set; }

    // // Puntuación de gemas
    // [Header("Estadisticas del juego")]
    // [SerializeField] private int score = 0;
    // [SerializeField] private TMP_Text scoreText;

    // // Instancia de pantalla de victoria
    // [SerializeField] private GameObject victoryPanel;

    // El juego termino?
    private bool gameFinished = false;


    private void Awake()
    {
        Instance = this;
    }

    // // Método para obtener la puntuación actual.
    // public int GetCurrentScore()
    // {
    //     return score;
    // }

    // // Metodo de victoria
    // void Victory()
    // {
    //     gameFinished = true;

    //     Time.timeScale = 0f;

    //     if (victoryPanel != null)
    //         victoryPanel.SetActive(true);
    // }
    
    public bool GetGameFinished()
    {
        return gameFinished;
    }
    public void SetGameFinished()
    {
        gameFinished = true;
    }
    
}
