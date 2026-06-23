/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 22-06-2026 00:17:44
 * @modify date 22-06-2026 00:17:44
 * @desc [description]
 */
using UnityEngine;

public class TriggerInstruccion : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] frasesDelLoro; // Escribe aquí lo que dirá en esta zona específica

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el que chocó es Jack
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script de diálogo en Jack
            CompanionDialogue loro = collision.GetComponent<CompanionDialogue>();
            
            if (loro != null)
            {
                // Hacemos que hable y le pasamos las frases
                loro.ShowDialogue(frasesDelLoro);
                
                // Destruimos esta zona invisible para que no repita el texto mil veces
                Destroy(gameObject);
            }
        }
    }
}