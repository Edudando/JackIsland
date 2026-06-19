using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    [Header("Configuración del Item")]
    public string nombreRecurso;
    public int cantidad = 1;
    public bool esHerramienta = false;
    public Sprite miSprite;

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
                
                // Lo destruimos del mapa
                Destroy(gameObject);
            }
            else
            {
                // No hay espacio: el objeto ignora al jugador y se queda en el piso
                Debug.Log("El inventario está lleno. No hay lugar para: " + nombreRecurso);
            }
        }
    }
}