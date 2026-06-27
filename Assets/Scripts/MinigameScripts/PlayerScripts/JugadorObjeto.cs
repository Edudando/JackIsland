/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que maneja la interacción entre el jugador y los objetos interactuables (gemas)]
 */

using UnityEngine;
using UnityEngine.InputSystem;

//Script para que el jugador interactue con las gemas

public class JugadorObjeto : MonoBehaviour
{
    //Variables a usar para identificar y tomar las gemas.
    [SerializeField] private Transform controladorInteractuar;
    [SerializeField] private Vector2 dimensionesCaja;
    [SerializeField] private LayerMask capasInteractuables;

    private void Update()
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
