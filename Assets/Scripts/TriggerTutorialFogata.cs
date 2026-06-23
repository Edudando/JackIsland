/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 22-06-2026 00:18:08
 * @modify date 22-06-2026 00:18:08
 * @desc [description]
 */
using UnityEngine;
using System.Collections.Generic;

public class TriggerTutorialFogata : MonoBehaviour
{
    [Header("Referencias")]
    public CompanionDialogue loroDialogo; 
    public SpriteRenderer spriteHoguera; 
    public Sprite hogueraInicial; 

    // Esta es la memoria del loro para no repetir la queja del frío
    private bool yaDijoFrio = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Si la hoguera ya fue cortada, apagamos este tutorial por completo
            if (spriteHoguera != null && spriteHoguera.sprite != hogueraInicial)
            {
                return; 
            }

            List<string> frases = new List<string>();
            
            // 1. ¿Ya nos quejamos del frío? Si no, lo decimos y lo marcamos como dicho.
            if (!yaDijoFrio)
            {
                frases.Add("Maldita sea Jack, hace mucho frío. Si no prendemos una fogata mañana estaremos congelados.");
                yaDijoFrio = true; 
            }

            // 2. Instrucciones dinámicas (Esto sí se actualiza según lo que hagas)
            if (!InventoryManager.Instance.tieneHacha)
            {
                frases.Add("Busca por la playa, necesitamos encontrar un hacha tirada para cortar esa madera.");
            }
            else 
            {
                string item = InventoryManager.Instance.itemSeleccionado;
                
                if (item != "Hacha" && item != "hacha")
                {
                    frases.Add("¡Ya tienes el hacha! Toca su casillero en la barra inferior para equiparla en tu mano.");
                }
                else
                {
                    frases.Add("¡Bien equipado! Ahora haz clic directamente sobre la madera para empezar a cortarla.");
                }
            }

            if (loroDialogo != null && frases.Count > 0)
            {
                loroDialogo.ShowDialogue(frases.ToArray());
            }
        }
    }
}