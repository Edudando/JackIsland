/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script de rocas para generar prefab. También genera las gemas dentro del prefab]
 */

using UnityEngine;

public class Rock : MonoBehaviour
{
    private GemDrop gemDrop;

    private void Awake()
    {
        // Obtiene el componente encargado de generar la gema.
        gemDrop = GetComponent<GemDrop>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // La roca solo reacciona cuando toca el suelo.
        if (!collision.gameObject.CompareTag("Suelo"))
            return;

        // Genera la gema.
        gemDrop?.Drop();

        // Destruye la roca.
        Destroy(gameObject);
    }
}