/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:58:06
 * @modify date 27-06-2026 23:58:06
 * @desc [description]
 */
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Rutas EXACTAS de los buses (copialas de FMOD Studio > Mixer > Copy Path)")]
    [SerializeField] private string rutaMusica = "bus:/Musica";
    [SerializeField] private string rutaEfectos = "bus:/Efectos";

    private Bus musicaBus;
    private Bus efectosBus;
    private bool musicaOk;
    private bool efectosOk;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Aseguramos que el Master Bank esté cargado antes de pedir los buses
        RuntimeManager.LoadBank("Master", true);
        RuntimeManager.LoadBank("Master.strings", true);

        musicaBus = ObtenerBus(rutaMusica, out musicaOk);
        efectosBus = ObtenerBus(rutaEfectos, out efectosOk);
    }

    private Bus ObtenerBus(string ruta, out bool ok)
    {
        try
        {
            Bus b = RuntimeManager.GetBus(ruta);
            ok = b.isValid();
            if (!ok) Debug.LogWarning($"[AudioManager] El bus '{ruta}' no es válido. Revisá la ruta en FMOD Studio > Mixer.");
            return b;
        }
        catch (BusNotFoundException)
        {
            ok = false;
            Debug.LogWarning($"[AudioManager] No existe el bus '{ruta}'. Copiá la ruta exacta desde FMOD Studio > Mixer > Copy Path.");
            return default;
        }
    }

    private void Start()
    {
        SetMusicaVolume(PlayerPrefs.GetFloat("MusicaVolume", 1f));
        SetEfectosVolume(PlayerPrefs.GetFloat("EfectosVolume", 1f));
    }

    public void SetMusicaVolume(float value)
    {
        if (musicaOk) musicaBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicaVolume", value);
    }

    public void SetEfectosVolume(float value)
    {
        if (efectosOk) efectosBus.setVolume(value);
        PlayerPrefs.SetFloat("EfectosVolume", value);
    }
}