/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 21-06-2026 21:06:57
 * @modify date 21-06-2026 21:06:57
 * @desc [description]
 */
using UnityEngine;
using FMODUnity;

public class ItemRecogible : MonoBehaviour
{
    [Header("Configuración del Item")]
    public string nombreRecurso;
    public int cantidad = 1;
    public bool esHerramienta = false;
    public Sprite miSprite;

    [Header("Eventos FMOD")]
    [SerializeField] private EventReference sfxItemRecogido;

    private void Start()
    {
        miSprite = GetComponent<SpriteRenderer>().sprite; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Le preguntamos a la UI si le queda al menos un casillero libre
            if (InventoryUI.Instance.HayEspacio())
            {
                // Hay espacio: lo guardamos en la lógica
                if (esHerramienta)
                {
                    InventoryManager.Instance.RecogerHerramienta(nombreRecurso);
                }
                else
                {
                    InventoryManager.Instance.AñadirRecurso(nombreRecurso, cantidad);
                }
                
                // Lo dibujamos en la pantalla
                InventoryUI.Instance.DibujarItem(miSprite, nombreRecurso); 

                // Sonido de pickup
                RuntimeManager.PlayOneShot(sfxItemRecogido, transform.position);
                
                // Lo destruimos del mapa
                Destroy(gameObject);
            }
        }
    }
}