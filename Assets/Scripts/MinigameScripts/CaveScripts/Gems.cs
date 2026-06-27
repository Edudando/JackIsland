/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que maneja la barra de vida del jugador]
 */

using System;
using UnityEngine;

public class Gems : MonoBehaviour
{
    // Las gemas sólo van a detectar si fueron recogidas.
    
    public static event Action OnTreasureCollected;

    public void RecogerObjeto()
    {
        OnTreasureCollected?.Invoke();

        Destroy(gameObject);
    }

}
