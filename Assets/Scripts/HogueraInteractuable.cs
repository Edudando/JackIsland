/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 21-06-2026 21:50:12
 * @modify date 21-06-2026 21:50:12
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class HogueraInteractuable : MonoBehaviour, IPointerClickHandler
{
    [Header("Evolución Visual")]
    public Sprite hogueraArmada;     // Acá va Hoguera_1 (Troncos acomodados)
    public Sprite hogueraEncendida;  // Acá va Hoguera_2 (Fuego chico)
    public Sprite hogueraAvivada;    // Acá va Hoguera_3 (Fuego completo)

    [Header("Diálogo del Loro")]
    public CompanionDialogue loroDialogo; 

    [Header("Eventos FMOD")]
    [SerializeField] private EventReference sfxItemCorrecto;
    [SerializeField] private EventReference sfxItemFail;
    [SerializeField] private EventReference sfxFinalEpisodio;

    private SpriteRenderer miSpriteRenderer;

    void Start()
    {
        miSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string item = InventoryManager.Instance.itemSeleccionado;

        // --- 1. VALIDACIÓN: SI NO HAY NADA SELECCIONADO EN LA BARRA ---
        if (string.IsNullOrEmpty(item))
        {
            Debug.Log("¡Primero debes seleccionar un ítem del inventario!");
            if (loroDialogo != null)
            {
                string[] frase = { "¡Oye Jack, primero selecciona un ítem de la barra!" };
                loroDialogo.ShowDialogue(frase);
            }
            return; // Cortamos la ejecución aquí para evitar que salten otros mensajes
        }

        // Identificamos dinámicamente el estado visual actual de la fogata
        bool estaEnTroncosInicial = (miSpriteRenderer.sprite != hogueraArmada && miSpriteRenderer.sprite != hogueraEncendida && miSpriteRenderer.sprite != hogueraAvivada);
        bool estaEnMaderaLista = (miSpriteRenderer.sprite == hogueraArmada);
        bool estaEnFuegoChico = (miSpriteRenderer.sprite == hogueraEncendida);

        // --- 2. CASO: SE USA EL HACHA ---
        if (item == "Hacha" || item == "hacha")
        {
            if (estaEnTroncosInicial)
            {
                miSpriteRenderer.sprite = hogueraArmada;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();
                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                
                if (loroDialogo != null)
                {
                    string[] frase = { "Bien maldito pirata, ya podemos prender el fuego" };
                    loroDialogo.ShowDialogue(frase);
                }
            }
        }
        // --- 3. CASO: SE USA EL ENCENDEDOR ---
        else if (item == "Encendedor" || item == "encendedor")
        {
            if (estaEnMaderaLista)
            {
                miSpriteRenderer.sprite = hogueraEncendida;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();
                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                
                if (loroDialogo != null)
                {
                    string[] frase = { "¡Fuego a la vista! Aunque todavia no es suficiente para pasar la noche" };
                    loroDialogo.ShowDialogue(frase);
                }
            }
            else if (estaEnTroncosInicial) // Advertencia: quiere prender fuego antes de preparar los leños
            {
                RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
                if (loroDialogo != null)
                {
                    string[] frase = { "¡No podemos prender esto así! Primero hay que preparar los leños con el hacha." };
                    loroDialogo.ShowDialogue(frase);
                }
            }
        }

        // --- 4. CASO: SE USA EL ABANICO ---
        else if (item == "Abanico" || item == "abanico")
        {
            if (estaEnFuegoChico)
            {
                miSpriteRenderer.sprite = hogueraAvivada;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();

                // Último paso: además del sonido de acierto, suena el cierre del episodio
                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                RuntimeManager.PlayOneShot(sfxFinalEpisodio, transform.position);
                
                if (loroDialogo != null)
                {
                    string[] frase = { "¡Eso es! Dale aire para que prenda con todo. ¡Misión cumplida, pirata!" };
                    loroDialogo.ShowDialogue(frase);
                }
            }
            else if (estaEnTroncosInicial || estaEnMaderaLista) // Advertencia: aire sin fuego previo
            {
                RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
                if (loroDialogo != null)
                {
                    string[] frase = { "¡No hay fuego que avivar todavía, Jack!" };
                    loroDialogo.ShowDialogue(frase);
                }
            }
        }
        // --- 5. CASO: CUALQUIER OTRO ÍTEM TOTALMENTE EQUIVOCADO ---
        else
        {
            Debug.Log("Este ítem no interactúa con la hoguera: " + item);
            RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
            if (loroDialogo != null)
            {
                string[] frase = { "¡Eso no nos va a servir para la hoguera, cabeza de alcornoque!" };
                loroDialogo.ShowDialogue(frase);
            }
        }
    }
}