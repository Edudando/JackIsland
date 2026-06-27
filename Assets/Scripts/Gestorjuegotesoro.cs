using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GESTOR del minijuego "El Tesoro" — VERSIÓN INTEGRADA AL INVENTARIO existente.
///
/// En vez de instanciar gemas en el mundo, usa los casilleros (Slot_X) del inventario
/// (InventoryUI / InventoryManager / SlotInventario) como "barra de colectables".
/// No modifica ninguno de esos 4 scripts: solo los usa.
///
/// FLUJO:
///   1. Se llena el inventario con gemas al azar (las 4 del objetivo + distractores).
///   2. Se muestra el objetivo unos segundos para memorizar; después se oculta.
///   3. El jugador clickea un casillero (queda en itemSeleccionado, tu SlotInventario lo pinta rojo)
///      y después clickea un agujero de la piedra para colocar esa gema.
///   4. Al colocar la 4ta, se revela el objetivo y se comparan.
///   5. Si no coincide -> una calavera pasa a la de fuego. A los 3 errores -> pantalla negra.
///
/// REQUISITOS:
///   - InventoryUI en la escena, con AL MENOS 4 casilleros libres
///     (sus Image en 'slots' y sus SlotInventario en 'scriptsDeSlots').
///   - Cada SlotInventario ya dispara AlHacerClick() al clickearlo (como en tu juego).
///   - Los 4 agujeros (AgujeroTesoro) son objetos del mundo con Collider2D.
/// </summary>
public class GestorJuegoTesoro : MonoBehaviour
{
    [Header("═══ GEMAS ═══")]
    [Tooltip("Sprites de gemas. Solo se usan las primeras 'cantidadGemasPosibles'.")]
    public List<Sprite> spritesGemas = new List<Sprite>();
    [Tooltip("Tipos de gema en juego (objetivo + inventario).")]
    [Min(1)] public int cantidadGemasPosibles = 7;
    [Tooltip("Prefijo del nombre interno con que se guardan las gemas en el inventario (ej. Gema_0).")]
    public string prefijoNombreGema = "Gema_";

    [Header("═══ INVENTARIO (barra de colectables) ═══")]
    [Tooltip("Cuántas gemas se cargan por ronda. Se limita a la cantidad de casilleros.")]
    [Min(4)] public int cantidadColectables = 7;
    [Tooltip("Si está activo, el inventario SIEMPRE incluye las 4 gemas del objetivo (juego ganable).")]
    public bool garantizarObjetivoEnInventario = true;

    [Header("═══ PIEDRA PRINCIPAL (4 agujeros) ═══")]
    public AgujeroTesoro[] agujeros = new AgujeroTesoro[4];

    [Header("═══ GEMAS OBJETIVO ═══")]
    public Image[] ranurasObjetivo = new Image[4];
    [Tooltip("Sprite del '?' que se ve mientras el objetivo está oculto.")]
    public Sprite spriteObjetivoOculto;
    [Tooltip("Segundos que el objetivo queda visible al inicio para memorizarlo.")]
    public float segundosObjetivoVisible = 5f;
    [Tooltip("Vuelve a mostrar el objetivo al resolver la ronda (para comparar).")]
    public bool revelarObjetivoAlResolver = true;

    [Header("═══ VIDAS / CALAVERAS ═══")]
    public Image[] calaveras = new Image[3];
    public Sprite calaveraViva;
    public Sprite calaveraFuego;
    [Min(1)] public int maxErrores = 3;
    [Tooltip("ON = 1 error por cada gema mal puesta.  OFF = 1 error por ronda fallada (recomendado).")]
    public bool errorPorGema = false;

    [Header("═══ UI / TEXTOS ═══")]
    public TMP_Text textoRecolectadas;
    public TMP_Text textoErrores;
    [Tooltip("Opcional: muestra '¡MEMORIZÁ! 5', '¡ELEGÍ LAS GEMAS!' y el resultado.")]
    public TMP_Text textoMensaje;
    [Tooltip("Panel negro full-screen para el Game Over. Si tiene CanvasGroup hace fundido.")]
    public GameObject pantallaNegra;
    public float duracionFundido = 1f;

    [Header("═══ TIEMPOS ═══")]
    public float esperaAntesDeRevelar = 0.4f;
    public float esperaResultado = 1.4f;

    [Header("═══ EVENTOS (FMOD, animaciones) ═══")]
    public UnityEvent alColocarGema;
    public UnityEvent alAcertarRonda;
    public UnityEvent alFallarRonda;
    public UnityEvent alPerder;

    // ───────── Estado interno ─────────
    private readonly List<int> objetivo = new List<int>();
    private int gemasColocadas;
    private int errores;
    private int rondasGanadas;
    private bool rondaBloqueada;

    void Start() => IniciarJuego();

    // ═════════════ CICLO DE JUEGO ═════════════

    public void IniciarJuego()
    {
        errores = 0;
        rondasGanadas = 0;
        Time.timeScale = 1f;
        if (pantallaNegra) pantallaNegra.SetActive(false);
        ReiniciarCalaveras();
        ActualizarTextoErrores();
        IniciarRonda();
    }

    /// <summary>Reinicia toda la partida (enchufar a un botón "Reintentar").</summary>
    public void ReiniciarJuego() => IniciarJuego();

    public void IniciarRonda()
    {
        gemasColocadas = 0;

        for (int i = 0; i < agujeros.Length; i++)
        {
            if (agujeros[i] == null) continue;
            agujeros[i].indice = i;
            agujeros[i].Configurar(this);
            agujeros[i].Limpiar();
        }

        GenerarObjetivo();
        CargarInventarioConGemas();
        ActualizarTextoRecolectadas();

        StartCoroutine(MemorizarObjetivo());
    }

    IEnumerator MemorizarObjetivo()
    {
        rondaBloqueada = true;          // no se puede colocar mientras se memoriza
        RevelarObjetivo();

        float restante = segundosObjetivoVisible;
        while (restante > 0f)
        {
            if (textoMensaje != null)
                textoMensaje.text = $"¡MEMORIZÁ! {Mathf.CeilToInt(restante)}";
            restante -= Time.deltaTime;
            yield return null;
        }

        OcultarObjetivo();
        if (textoMensaje != null) textoMensaje.text = "¡ELEGÍ LAS GEMAS!";
        rondaBloqueada = false;
    }

    // ═════════════ GEMAS / INVENTARIO ═════════════

    int GemasEnJuego() => Mathf.Clamp(cantidadGemasPosibles, 1, spritesGemas.Count);

    void GenerarObjetivo()
    {
        objetivo.Clear();
        int n = GemasEnJuego();
        for (int i = 0; i < 4; i++)
            objetivo.Add(Random.Range(0, n));
    }

    void CargarInventarioConGemas()
    {
        var ui = InventoryUI.Instance;
        if (ui == null) { Debug.LogError("[Tesoro] Falta InventoryUI en la escena."); return; }

        // Limpiar SOLO los casilleros que tengan gemas (no toca Hacha/Sierra/recursos).
        if (ui.scriptsDeSlots != null)
            foreach (var s in ui.scriptsDeSlots)
                if (s != null && EsNombreDeGema(s.nombreItem)) s.VaciarSlot();

        if (InventoryManager.Instance != null && EsNombreDeGema(InventoryManager.Instance.itemSeleccionado))
            InventoryManager.Instance.itemSeleccionado = "";

        // Armar lista de ids a cargar
        List<int> ids = new List<int>();
        if (garantizarObjetivoEnInventario) ids.AddRange(objetivo);   // asegura que el objetivo es alcanzable

        int n = GemasEnJuego();
        int maxSlots = (ui.slots != null) ? ui.slots.Length : 4;
        int cantidad = Mathf.Min(cantidadColectables, maxSlots);
        while (ids.Count < cantidad) ids.Add(Random.Range(0, n));     // distractores al azar
        Barajar(ids);

        // Dibujar en los casilleros libres (DibujarItem ignora si no hay espacio)
        foreach (int id in ids)
            ui.DibujarItem(spritesGemas[id], NombreDesdeId(id));
    }

    // ═════════════ INTERACCIÓN ═════════════
    // La selección de la gema la hace TU SlotInventario.AlHacerClick() -> itemSeleccionado.
    // Acá solo reaccionamos al click sobre un agujero.

    public void IntentarColocarEnAgujero(AgujeroTesoro agujero)
    {
        if (rondaBloqueada) return;
        if (agujero == null || agujero.ocupado) return;
        if (InventoryManager.Instance == null) return;

        string sel = InventoryManager.Instance.itemSeleccionado;
        if (string.IsNullOrEmpty(sel)) return;     // no hay nada elegido
        if (!EsNombreDeGema(sel)) return;          // hay algo elegido que no es una gema (ej. Hacha)

        int id = IdDesdeNombre(sel);
        if (id < 0 || id >= spritesGemas.Count) return;

        agujero.ColocarGema(id, spritesGemas[id]);

        // Sacar la gema del inventario y limpiar la selección
        InventoryUI.Instance.RemoverItemVisualmente(sel);
        InventoryUI.Instance.DeseleccionarTodos();
        InventoryManager.Instance.itemSeleccionado = "";

        gemasColocadas++;
        ActualizarTextoRecolectadas();
        alColocarGema?.Invoke();

        if (gemasColocadas >= 4) StartCoroutine(ResolverRonda());
    }

    // ═════════════ RESOLUCIÓN ═════════════

    IEnumerator ResolverRonda()
    {
        rondaBloqueada = true;
        yield return new WaitForSeconds(esperaAntesDeRevelar);

        if (revelarObjetivoAlResolver) RevelarObjetivo();

        int aciertos = ContarAciertos();
        bool rondaPerfecta = aciertos == 4;

        if (textoMensaje != null)
            textoMensaje.text = rondaPerfecta ? "¡CORRECTO!" : "¡FALLASTE!";

        int nuevosErrores = errorPorGema ? (4 - aciertos) : (rondaPerfecta ? 0 : 1);
        for (int i = 0; i < nuevosErrores; i++) RegistrarError();

        if (rondaPerfecta) { rondasGanadas++; alAcertarRonda?.Invoke(); }
        else alFallarRonda?.Invoke();

        yield return new WaitForSeconds(esperaResultado);

        if (errores >= maxErrores) FinDelJuego();
        else IniciarRonda();
    }

    /// <summary>Cuenta cuántas gemas colocadas coinciden con el objetivo (sin importar el orden).</summary>
    int ContarAciertos()
    {
        List<int> objRestante = new List<int>(objetivo);
        int aciertos = 0;
        foreach (var a in agujeros)
        {
            if (a == null) continue;
            int idx = objRestante.IndexOf(a.idGemaColocada);
            if (idx >= 0) { objRestante.RemoveAt(idx); aciertos++; }
        }
        return aciertos;
    }

    // ═════════════ CALAVERAS ═════════════

    void RegistrarError()
    {
        if (errores >= maxErrores) return;
        if (calaveras != null && errores < calaveras.Length
            && calaveras[errores] != null && calaveraFuego != null)
            calaveras[errores].sprite = calaveraFuego;
        errores++;
        ActualizarTextoErrores();
    }

    void ReiniciarCalaveras()
    {
        if (calaveras == null) return;
        foreach (var c in calaveras)
            if (c != null && calaveraViva != null) c.sprite = calaveraViva;
    }

    // ═════════════ OBJETIVO ═════════════

    void RevelarObjetivo()
    {
        for (int i = 0; i < ranurasObjetivo.Length && i < objetivo.Count; i++)
            if (ranurasObjetivo[i] != null)
            {
                ranurasObjetivo[i].sprite = spritesGemas[objetivo[i]];
                ranurasObjetivo[i].enabled = true;
            }
    }

    void OcultarObjetivo()
    {
        foreach (var r in ranurasObjetivo)
        {
            if (r == null) continue;
            if (spriteObjetivoOculto != null)
                r.sprite = spriteObjetivoOculto;   // muestra el "?"
            else
                r.enabled = false;                 // sin "?": simplemente esconde la gema
        }
    }

    // ═════════════ UI ═════════════

    void ActualizarTextoRecolectadas()
    {
        if (textoRecolectadas != null)
            textoRecolectadas.text = $"RECOLECTADAS: {gemasColocadas} / 4";
    }

    void ActualizarTextoErrores()
    {
        if (textoErrores != null)
            textoErrores.text = $"ERRORES: {errores} / {maxErrores}";
    }

    // ═════════════ FIN DEL JUEGO ═════════════

    void FinDelJuego()
    {
        alPerder?.Invoke();
        StartCoroutine(FundidoANegro());
    }

    IEnumerator FundidoANegro()
    {
        if (pantallaNegra == null) yield break;
        pantallaNegra.SetActive(true);

        CanvasGroup cg = pantallaNegra.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duracionFundido);
                cg.alpha = Mathf.Clamp01(t);
                yield return null;
            }
            cg.alpha = 1f;
        }
        // Si querés congelar todo al perder, descomentá:
        // Time.timeScale = 0f;
    }

    // ═════════════ HELPERS DE NOMBRES ═════════════

    string NombreDesdeId(int id) => prefijoNombreGema + id;

    bool EsNombreDeGema(string nombre) =>
        !string.IsNullOrEmpty(nombre) && nombre.StartsWith(prefijoNombreGema);

    int IdDesdeNombre(string nombre)
    {
        if (!EsNombreDeGema(nombre)) return -1;
        return int.TryParse(nombre.Substring(prefijoNombreGema.Length), out int id) ? id : -1;
    }

    // ═════════════ UTILIDADES ═════════════

    void Barajar(List<int> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }
}