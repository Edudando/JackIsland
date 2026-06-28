/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:58:17
 * @modify date 27-06-2026 23:58:17
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class BoteInteractuable : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public class EtapaBote
    {
        public string nombre;                  // solo para identificar en el Inspector (ej: "Casco", "Mástil")
        public GameObject objetoEtapa;         // el bote_X que se muestra en ESTA etapa
        public string itemRequerido;           // ítem que hace avanzar a la SIGUIENTE etapa
        [TextArea] public string fraseExito;   // qué dice el loro al colocar bien el ítem
        [TextArea] public string fraseFail;    // (opcional) mensaje si trae el ítem equivocado en esta etapa
    }

    [Header("Etapas del bote (en orden: bote_0, bote_1, bote_2, bote_3...)")]
    public EtapaBote[] etapas;

    [Header("Final (al completar la última etapa)")]
    public GameObject efectoFinal;            // opcional: humo, brillo, etc. (dejar vacío si no usás)
    public bool cargarEscenaAlFinal = false;
    public string nombreSiguienteEscena = "";
    public float tiempoAntesDeEscena = 3f;
    public bool freezearJack = false;

    [Header("Referencias")]
    public CompanionDialogue loroDialogo;     // OPCIONAL: si lo dejás vacío, no habla (solo audio)
    public PlayerMovement playerMovement;

    [Header("Eventos FMOD")]
    [SerializeField] private EventReference sfxItemCorrecto;
    [SerializeField] private EventReference sfxItemFail;
    [SerializeField] private EventReference sfxFinal;   // opcional

    [Header("Mensajes genéricos")]
    [TextArea] public string fraseSinItem = "¡Oye Jack, primero selecciona un ítem de la barra!";
    [TextArea] public string fraseItemIncorrecto = "¡Eso no nos sirve para arreglar el bote, cabeza de alcornoque!";

    private int etapaActual = 0;
    private bool finalActivado = false;

    void Start()
    {
        if (efectoFinal != null)
            efectoFinal.SetActive(false);

        MostrarSoloEtapa(0); // arranca mostrando bote_0
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (finalActivado) return;

        // Si ya no quedan etapas por delante, no hay nada que construir
        if (etapas == null || etapaActual >= etapas.Length - 1)
            return;

        string item = InventoryManager.Instance.itemSeleccionado;

        if (string.IsNullOrEmpty(item))
        {
            Hablar(fraseSinItem);
            return;
        }

        EtapaBote actual = etapas[etapaActual];

        if (CoincideItem(item, actual.itemRequerido))
        {
            // --- Avanzar a la siguiente etapa ---
            etapaActual++;
            MostrarSoloEtapa(etapaActual);

            InventoryUI.Instance.RemoverItemVisualmente(item);
            InventoryManager.Instance.ConsumirItemSeleccionado();
            RuntimeManager.PlayOneShot(sfxItemCorrecto, transform.position);

            Hablar(string.IsNullOrEmpty(actual.fraseExito)
                ? "¡Bien hecho, Jack!"
                : actual.fraseExito);

            // ¿Llegamos a la última etapa?
            if (etapaActual >= etapas.Length - 1)
                ActivarFinal();
        }
        else
        {
            // Ítem equivocado para esta etapa
            Fail(string.IsNullOrEmpty(actual.fraseFail)
                ? fraseItemIncorrecto
                : actual.fraseFail);
        }
    }

    private void ActivarFinal()
    {
        finalActivado = true;

        if (efectoFinal != null)
            efectoFinal.SetActive(true);

        if (freezearJack && playerMovement != null)
            playerMovement.estaHablando = true;

        if (!sfxFinal.IsNull)
            RuntimeManager.PlayOneShot(sfxFinal, transform.position);

        if (cargarEscenaAlFinal && !string.IsNullOrEmpty(nombreSiguienteEscena))
            StartCoroutine(CargarSiguienteEscena());
    }

    // Activa solo el bote_X de la etapa indicada y apaga los demás
    private void MostrarSoloEtapa(int indice)
    {
        for (int i = 0; i < etapas.Length; i++)
        {
            if (etapas[i].objetoEtapa != null)
                etapas[i].objetoEtapa.SetActive(i == indice);
        }
    }

    // Comparación tolerante (ignora mayúsculas/minúsculas y espacios)
    private bool CoincideItem(string seleccionado, string requerido)
    {
        if (string.IsNullOrEmpty(requerido)) return false;
        return seleccionado.Trim().ToLower() == requerido.Trim().ToLower();
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