,/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 22-06-2026 00:10:52
 * @modify date 22-06-2026 00:10:52
 * @desc [description]
 */
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // Esto permite que otros scripts accedan al inventario fácilmente
    public static InventoryManager Instance;

    [Header("Recursos Acumulables")]
    // Usamos un diccionario para guardar el nombre del recurso y su cantidad
    public Dictionary<string, int> recursos = new Dictionary<string, int>();

    [Header("Herramientas")]
    public bool tieneHacha = false;
    public bool tieneSierra = false;
    
    [Header("Interacción")]
    public string itemSeleccionado = ""; // Guardará el nombre de lo que seleccionemos


    void Awake()
    {
        // Configuramos el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para añadir recursos
    public void AñadirRecurso(string nombreRecurso, int cantidad)
    {
        if (recursos.ContainsKey(nombreRecurso))
        {
            recursos[nombreRecurso] += cantidad;
        }
        else
        {
            recursos.Add(nombreRecurso, cantidad);
        }
        
        Debug.Log($"Recogiste {cantidad} de {nombreRecurso}. Total: {recursos[nombreRecurso]}");
    }

    // Método para comprobar si tienes suficiente de un recurso
    public bool TieneRecurso(string nombreRecurso, int cantidadNecesaria)
    {
        if (recursos.ContainsKey(nombreRecurso))
        {
            return recursos[nombreRecurso] >= cantidadNecesaria;
        }
        return false;
    }

    // Método para gastar recursos (al construir)
    public void GastarRecurso(string nombreRecurso, int cantidad)
    {
        if (recursos.ContainsKey(nombreRecurso))
        {
            recursos[nombreRecurso] -= cantidad;
        }
    }

    // Método para recoger herramientas únicas
    public void RecogerHerramienta(string nombreHerramienta)
    {
        if (nombreHerramienta == "Hacha") tieneHacha = true;
        if (nombreHerramienta == "Sierra") tieneSierra = true;
        Debug.Log("¡Has recogido: " + nombreHerramienta + "!");
        
    }

// Método para consumir el ítem seleccionado (usado en la hoguera)
    public void ConsumirItemSeleccionado()
    {
        // Si no hay nada seleccionado, no hacemos nada
        if (string.IsNullOrEmpty(itemSeleccionado)) 
        {
            Debug.Log("No hay ningún ítem seleccionado para consumir.");
            return;
        }

        // Verificamos qué ítem está seleccionado y lo quitamos del inventario
        if (itemSeleccionado == "Hacha")
        {
            tieneHacha = false;
            Debug.Log("El Hacha ha sido consumida y desapareció del inventario.");

        }
        else if (itemSeleccionado == "Sierra")
        {
            tieneSierra = false;
            Debug.Log("La Sierra ha sido consumida y desapareció del inventario.");
        }

        // Vaciamos la selección porque el ítem ya fue usado
        itemSeleccionado = "";
   }

}