using UnityEngine;
using UnityEngine.UI; // Importante: nos permite trabajar con imágenes de la interfaz

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Casilleros Visuales")]
    public Image[] slots; // Aquí guardaremos los huecos que creaste

    public SlotInventario[] scriptsDeSlots; // Aquí guardaremos los scripts de cada casillero para asignarles el nombre del item

    void Awake()
    {
        Instance = this;
    }

// Modifica DibujarItem para que reciba también el nombre:
    public void DibujarItem(Sprite imagenObjeto, string nombreDelItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == null)
            {
                slots[i].sprite = imagenObjeto;
                slots[i].color = new Color(1, 1, 1, 1);
                scriptsDeSlots[i].nombreItem = nombreDelItem; // ¡Guardamos el nombre!
                return; 
            }
        }
    }

    public void DeseleccionarTodos()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite != null)
            {
                slots[i].color = new Color(1, 1, 1, 1); // Vuelve al color normal
            }
        }
    }

    public void RemoverItemVisualmente(string nombreDelItem)
    {
        for (int i = 0; i < scriptsDeSlots.Length; i++)
        {
            // Buscamos qué casillero tiene el nombre que le pasamos
            if (scriptsDeSlots[i].nombreItem == nombreDelItem)
            {
                // ¡Lo encontramos! Ejecutamos tu función para limpiarlo
                scriptsDeSlots[i].VaciarSlot(); 
                return; // Cortamos el ciclo porque ya hicimos el trabajo
            }
        }
    }
    public bool HayEspacio()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                // Si encuentra una imagen vacía (null), significa que hay espacio
                if (slots[i].sprite == null)
                {
                    return true; 
                }
            }
            // Si termina de revisar todos y ninguno estaba vacío, está lleno
            return false; 
        }
}