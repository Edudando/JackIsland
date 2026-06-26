using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class HogueraInteractuable : MonoBehaviour, IPointerClickHandler
{
    [Header("Evolución Visual")]
    public Sprite hogueraArmada;
    public Sprite hogueraEncendida;
    public Sprite hogueraAvivada;

    [Header("Animación Fuego Final")]
    public GameObject fuegoAnimado; // GameObject con Animator de los 4 frames
    public float tiempoAntesDeEscena = 5f;

    [Header("Referencias")]
    public CompanionDialogue loroDialogo;
    public PlayerMovement playerMovement;

    [Header("Siguiente Escena")]
    public string nombreSiguienteEscena = "ElRefugio"; // cambiá cuando la tengas

    [Header("Eventos FMOD")]
    [SerializeField] private EventReference sfxItemCorrecto;
    [SerializeField] private EventReference sfxItemFail;
    [SerializeField] private EventReference sfxFinalEpisodio;

    private SpriteRenderer miSpriteRenderer;
    private bool finalActivado = false;

    void Start()
    {
        miSpriteRenderer = GetComponent<SpriteRenderer>();
        if (fuegoAnimado != null)
            fuegoAnimado.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (finalActivado) return;

        string item = InventoryManager.Instance.itemSeleccionado;

        if (string.IsNullOrEmpty(item))
        {
            if (loroDialogo != null)
            {
                string[] frase = { "¡Oye Jack, primero selecciona un ítem de la barra!" };
                loroDialogo.ShowDialogue(frase);
            }
            return;
        }

        bool estaEnTroncosInicial = (miSpriteRenderer.sprite != hogueraArmada && miSpriteRenderer.sprite != hogueraEncendida && miSpriteRenderer.sprite != hogueraAvivada);
        bool estaEnMaderaLista = (miSpriteRenderer.sprite == hogueraArmada);
        bool estaEnFuegoChico = (miSpriteRenderer.sprite == hogueraEncendida);

        // --- HACHA ---
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
        // --- ENCENDEDOR ---
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
            else if (estaEnTroncosInicial)
            {
                RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
                if (loroDialogo != null)
                {
                    string[] frase = { "¡No podemos prender esto así! Primero hay que preparar los leños con el hacha." };
                    loroDialogo.ShowDialogue(frase);
                }
            }
        }
        // --- ABANICO ---
        else if (item == "Abanico" || item == "abanico")
        {
            if (estaEnFuegoChico)
            {
                finalActivado = true;

                miSpriteRenderer.sprite = hogueraAvivada;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();

                // Activar animación de fuego
                if (fuegoAnimado != null)
                    fuegoAnimado.SetActive(true);

                // Freezear a Jack
                if (playerMovement != null)
                    playerMovement.estaHablando = true;

                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                RuntimeManager.PlayOneShot(sfxFinalEpisodio, transform.position);

                if (loroDialogo != null)
                {
                    string[] frase = { "¡Eso es! Dale aire para que prenda con todo. ¡Misión cumplida, pirata!" };
                    loroDialogo.ShowDialogue(frase);
                }

                StartCoroutine(CargarSiguienteEscena());
            }
            else if (estaEnTroncosInicial || estaEnMaderaLista)
            {
                RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
                if (loroDialogo != null)
                {
                    string[] frase = { "¡No hay fuego que avivar todavía, Jack!" };
                    loroDialogo.ShowDialogue(frase);
                }
            }
        }
        // --- ÍTEM INCORRECTO ---
        else
        {
            RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
            if (loroDialogo != null)
            {
                string[] frase = { "¡Eso no nos va a servir para la hoguera, cabeza de alcornoque!" };
                loroDialogo.ShowDialogue(frase);
            }
        }
    }

    IEnumerator CargarSiguienteEscena()
    {
        yield return new WaitForSeconds(tiempoAntesDeEscena);
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}