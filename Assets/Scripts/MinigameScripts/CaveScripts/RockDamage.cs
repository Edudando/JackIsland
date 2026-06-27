/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Daño causado por la caída de las rocas]
 */


using UnityEngine;

public class RockDamage : MonoBehaviour
{
    [SerializeField] private int damage = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Jugador recibió {damage} de daño");
        }
        PlayerHealth playerHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        playerHealth?.TakeDamage(damage);
    }
}