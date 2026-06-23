using UnityEngine;
using UnityEngine.InputSystem;

//Script para que el jugador interactue con las gemas

public class JugadorObjeto : MonoBehaviour
{
    //Variables a usar para identificar y tomar las gemas.
    [SerializeField] private Transform controladorInteractuar;
    [SerializeField] private Vector2 dimensionesCaja;
    [SerializeField] private LayerMask capasInteractuables;

    // Manejo de camara luego de tomar las gemas
    public CameraShakeController shakeController;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interactuar();
        }
    }

    private void Interactuar()
    {
        Collider2D[] objetosTocados = Physics2D.OverlapBoxAll(controladorInteractuar.position, dimensionesCaja, 0f, capasInteractuables);

        foreach (Collider2D objeto in objetosTocados)
        {
            Debug.Log(objeto.name);
            if(objeto.TryGetComponent(out Gems gems)){
            gems.RecogerObjeto();   
        }

        }
        Debug.Log("Letra presionada");
    }


}
