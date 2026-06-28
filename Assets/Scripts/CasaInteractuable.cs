/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:58:54
 * @modify date 27-06-2026 23:58:54
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class CasaInteractuable : MonoBehaviour, IPointerClickHandler
{
    [Header("Evolución Visual")]
    public Sprite chozaSprite;       // Carpita (inicial) -> Choza
    public Sprite casaSprite;        // Choza -> Casa
    public Sprite casaFinalSprite;   // Casa -> Casa_final

    [Header("Final (al construir Casa_final)")]
    public GameObject efectoFinal;          // opcional: humo, brillo, etc. (dejar vacío si no usás)
    public bool cargarEscenaAlFinal = false;
    public string nombreSiguienteEscena = "";
    public float tiempoAntesDeEscena = 3f;
    public bool freezearJack = false;

    [Header("Referencias")]
    public CompanionDialogue loroDialogo;   // OPCIONAL: si lo dejás vacío, no habla (solo audio)
    public PlayerMovement playerMovement;

    [Header("Eventos FMOD")]
    [SerializeField] private EventReference sfxItemCorrecto;
    [SerializeField] private EventReference sfxItemFail;
    [SerializeField] private EventReference sfxFinal;   // opcional

    private SpriteRenderer miSpriteRenderer;
    private bool finalActivado = false;

    void Start()
    {
        miSpriteRenderer = GetComponent<SpriteRenderer>();
        if (efectoFinal != null)
            efectoFinal.SetActive(false);
        // El sprite inicial (Carpita) ya lo dejás puesto en el SpriteRenderer desde el editor
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (finalActivado) return;

        string item = InventoryManager.Instance.itemSeleccionado;

        if (string.IsNullOrEmpty(item))
        {
            Hablar("¡Oye Jack, primero selecciona un ítem de la barra!");
            return;
        }

        bool estaEnCarpita = (miSpriteRenderer.sprite != chozaSprite && miSpriteRenderer.sprite != casaSprite && miSpriteRenderer.sprite != casaFinalSprite);
        bool estaEnChoza = (miSpriteRenderer.sprite == chozaSprite);
        bool estaEnCasa = (miSpriteRenderer.sprite == casaSprite);

        // --- MADERAS (Carpita -> Choza) ---
        if (item == "Maderas" || item == "maderas")
        {
            if (estaEnCarpita)
            {
                miSpriteRenderer.sprite = chozaSprite;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();
                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                Hablar("¡Bien! Con estas maderas ya tenemos una choza decente.");
            }
            else
            {
                Fail("Las maderas ya no nos sirven en esta etapa, Jack.");
            }
        }
        // --- PALA (Choza -> Casa) ---
        else if (item == "Pala" || item == "pala")
        {
            if (estaEnChoza)
            {
                miSpriteRenderer.sprite = casaSprite;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();
                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                Hablar("¡Cavando los cimientos! Ahora sí parece una casa de verdad.");
            }
            else if (estaEnCarpita)
            {
                Fail("¡Todavía no, Jack! Primero hay que levantar la choza con maderas.");
            }
            else
            {
                Fail("La pala ya no nos sirve acá.");
            }
        }
        // --- MEZCLADORA (Casa -> Casa_final) ---
        else if (item == "Mezcladora" || item == "mezcladora")
        {
            if (estaEnCasa)
            {
                finalActivado = true;

                miSpriteRenderer.sprite = casaFinalSprite;
                InventoryUI.Instance.RemoverItemVisualmente(item);
                InventoryManager.Instance.ConsumirItemSeleccionado();

                if (efectoFinal != null)
                    efectoFinal.SetActive(true);

                if (freezearJack && playerMovement != null)
                    playerMovement.estaHablando = true;

                RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);
                if (!sfxFinal.IsNull)
                    RuntimeManager.PlayOneShot(sfxFinal, transform.position);

                Hablar("¡La casa quedó terminada! Buen trabajo, pirata.");

                if (cargarEscenaAlFinal && !string.IsNullOrEmpty(nombreSiguienteEscena))
                    StartCoroutine(CargarSiguienteEscena());
            }
            else if (estaEnCarpita || estaEnChoza)
            {
                Fail("¡Todavía no hay paredes para revocar, Jack!");
            }
        }
        // --- ÍTEM INCORRECTO ---
        else
        {
            Fail("¡Eso no nos sirve para construir la casa, cabeza de alcornoque!");
        }
    }

    private void Hablar(string frase)
    {
        if (loroDialogo != null)
            loroDialogo.ShowDialogue(new string[] { frase });
    }

    private void Fail(string frase)
    {
        RuntimeManager.PlayOneShot(sfxItemFail, transform.position);
        Hablar(frase);
    }

    IEnumerator CargarSiguienteEscena()
    {
        yield return new WaitForSeconds(tiempoAntesDeEscena);
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}                   