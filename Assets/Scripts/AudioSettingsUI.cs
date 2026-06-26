using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicaSlider;
    public Slider efectosSlider;

    private void Start()
    {
        // Carga los valores guardados y los refleja en los sliders al abrir el panel
        musicaSlider.value = PlayerPrefs.GetFloat("MusicaVolume", 1f);
        efectosSlider.value = PlayerPrefs.GetFloat("EfectosVolume", 1f);

        // Conecta el movimiento de cada slider con el AudioManager
        musicaSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicaVolume);
        efectosSlider.onValueChanged.AddListener(AudioManager.Instance.SetEfectosVolume);
    }
}
