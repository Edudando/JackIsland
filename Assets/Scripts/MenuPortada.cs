/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:58:42
 * @modify date 27-06-2026 23:58:42
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPortada : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelSettings;
    public GameObject panelCreditos;

    [Header("Escena a cargar")]
    public string nombreEscenaJuego = "ElFuego";

    void Start()
    {
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelCreditos != null) panelCreditos.SetActive(false);
    }

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void AbrirSettings()
    {
        panelSettings.SetActive(true);
    }

    public void CerrarSettings()
    {
        panelSettings.SetActive(false);
    }

    public void AbrirCreditos()
    {
        panelCreditos.SetActive(true);
    }

    public void CerrarCreditos()
    {
        panelCreditos.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
    }
}