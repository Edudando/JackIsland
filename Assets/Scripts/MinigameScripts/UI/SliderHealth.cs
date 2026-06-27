/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que maneja la barra de vida del jugador]
 */

using UnityEngine;
using UnityEngine.UI;

public class SliderHealth : MonoBehaviour
{
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private PlayerHealth playerHealth;


    private void Start()
    {
        // Se inicia el juego tomando la vida máxima del jugador sin tener que asignar manualmente en Unity
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        InitializeSlideHealth(playerHealth.GetHealthMax(), playerHealth.GetHealth());
    
        // se toma la referencia del evento para modificar la vida en la barra UI
        playerHealth.PlayerTakeDamage += ModifySlideHealth;
    }

    //  Para dejar de llamar al evento, se utliza el método OnDisable(). Este método se ejecuta cuando el objeto (en este caso player) se desactiva.
    void Osable()
    {
        playerHealth.PlayerTakeDamage -= ModifySlideHealth;
    }

    private void InitializeSlideHealth(float maxHealth, float healthActual)
    {
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = healthActual;
    }

    private void ModifySlideHealth(float healthActual)
    {
        sliderHealth.value = healthActual;
    }
}
