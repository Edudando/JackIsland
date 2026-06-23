/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 21-06-2026 23:06:41
 * @modify date 21-06-2026 23:06:41
 * @desc [description]
 */
using UnityEngine;
using TMPro;                   
using System.Collections;

public class TituloEscena : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoTitulo;  
    public float duracionVisible = 3f;
    public float velocidadFade = 1.5f;

    void Start()
    {
        if (textoTitulo != null)
            StartCoroutine(MostrarTitulo());
    }

    IEnumerator MostrarTitulo()
    {
        SetAlpha(0f);
        textoTitulo.gameObject.SetActive(true);

        yield return StartCoroutine(FadeTexto(0f, 1f));
        yield return new WaitForSeconds(duracionVisible);
        yield return StartCoroutine(FadeTexto(1f, 0f));

        textoTitulo.gameObject.SetActive(false);
    }

    IEnumerator FadeTexto(float desde, float hasta)
    {
        float tiempo = 0f;
        float duracion = 1f / velocidadFade;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            SetAlpha(Mathf.Lerp(desde, hasta, tiempo / duracion));
            yield return null;
        }

        SetAlpha(hasta);
    }

    void SetAlpha(float alpha)
    {
        Color c = textoTitulo.color;
        c.a = alpha;
        textoTitulo.color = c;
    }
}