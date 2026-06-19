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