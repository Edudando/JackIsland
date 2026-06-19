using UnityEngine;
using UnityEngine.UI;

public class SlotInventario : MonoBehaviour
{
    public string nombreItem = ""; // Qué item tiene guardado este hueco
    private Image miImagen;

    void Start()
    {
        miImagen = GetComponent<Image>();
    }

    // Esta función se activará al hacer clic en el casillero
    public void AlHacerClick()
    {
        if (nombreItem != "") // Si el casillero no está vacío
        {
            // 1. Limpiamos los colores de todos los demás slots
            InventoryUI.Instance.DeseleccionarTodos();
            
            // 2. "Iluminamos" este slot tiñendo el dibujo de amarillo (o el color que prefieras)
            miImagen.color = Color.red; // Cambia a rojo para destacar el slot seleccionado
            
            // 3. Le avisamos al Manager qué tenemos en la mano
            InventoryManager.Instance.itemSeleccionado = nombreItem;
            Debug.Log("Equipaste: " + nombreItem);
        }
    }

    public void VaciarSlot() 
    {
        nombreItem = ""; // Borramos el nombre del ítem

        // Quitamos el dibujo del hacha para que se vea vacío
        miImagen.sprite = null; 
        
        // Le devolvemos el color normal (asumiendo que blanco es tu color por defecto)
        // Lo volvemos 100% transparente (Alpha en 0)
        miImagen.color = new Color(1f, 1f, 1f, 0f);
    }


}