/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que controla la vida del jugador: Establece valor de la vida, daño recibido, barra de vida y pantalla de derrota]
 */


using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Action comunicará el cambio de la vida actual del player
    public Action<float> PlayerTakeDamage;

    [SerializeField] private float healthMax = 100;
    [SerializeField] private float health;

    private void Awake()
    {
        health = healthMax;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took damage. Current health: " + health);

        PlayerTakeDamage?.Invoke(health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        // Se busca y llama al método en GameOver para mostrar el panel UI en pantalla
        FindAnyObjectByType<GameOver>().GetGameOverPanel();
    }

    public float GetHealthMax()
    {
        return healthMax;
    }

    public float GetHealth()
    {
        return health;
    }
}
