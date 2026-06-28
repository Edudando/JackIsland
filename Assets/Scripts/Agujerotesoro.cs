/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:59:48
 * @modify date 27-06-2026 23:59:48
 * @desc [description]
 */
using UnityEngine;

/// <summary>
/// Uno de los 4 agujeros de la piedra principal (Tesoro).
/// El jugador primero elige una gema de la barra y después clickea un agujero vacío.
///
/// REQUISITOS DEL OBJETO:
///   - Un Collider2D que cubra el agujero -> necesario para OnMouseDown
///   - Este script
///   - "renderGema": un SpriteRenderer (puede ser un hijo, ej. "GemaColocada")
///       donde se dibuja la gema una vez colocada. Empieza desactivado.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class AgujeroTesoro : MonoBehaviour
{
    [Tooltip("SpriteRenderer (puede ser un hijo) donde se muestra la gema colocada.")]
    public SpriteRenderer renderGema;

    [HideInInspector] public int indice;
    [HideInInspector] public int idGemaColocada = -1;
    [HideInInspector] public bool ocupado = false;

    private GestorJuegoTesoro gestor;

    public void Configurar(GestorJuegoTesoro g) => gestor = g;

    void OnMouseDown()
    {
        if (gestor != null) gestor.IntentarColocarEnAgujero(this);
    }

    public void ColocarGema(int id, Sprite sprite)
    {
        idGemaColocada = id;
        ocupado = true;
        if (renderGema != null)
        {
            renderGema.sprite = sprite;
            renderGema.enabled = true;
            renderGema.gameObject.SetActive(true);   // por si el hijo "GemaColocada" arranca desactivado
        }
    }

    public void Limpiar()
    {
        idGemaColocada = -1;
        ocupado = false;
        if (renderGema != null)
            renderGema.gameObject.SetActive(false);  // oculta la gema hasta que se coloque una nueva
    }
}