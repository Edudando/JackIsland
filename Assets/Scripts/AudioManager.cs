using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private Bus musicaBus;
    private Bus efectosBus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicaBus = RuntimeManager.GetBus("bus:/Musica");
        efectosBus = RuntimeManager.GetBus("bus:/Efectos");

        // Chequeo: isValid() te dice si el bus realmente existe
        Debug.Log("Bus Musica válido: " + musicaBus.isValid());
        Debug.Log("Bus Efectos válido: " + efectosBus.isValid());
    }
    

    private void Start()
    {
        SetMusicaVolume(PlayerPrefs.GetFloat("MusicaVolume", 1f));
        SetEfectosVolume(PlayerPrefs.GetFloat("EfectosVolume", 1f));
    }

    // El slider manda un valor 0-1 directo, FMOD lo toma como ganancia lineal
    public void SetMusicaVolume(float value)
    {
        musicaBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicaVolume", value);
    }

    public void SetEfectosVolume(float value)
    {
        efectosBus.setVolume(value);
        PlayerPrefs.SetFloat("EfectosVolume", value);
    }
}