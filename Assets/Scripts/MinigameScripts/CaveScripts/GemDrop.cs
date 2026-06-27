/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Genera la gema luego de que la roca colisiona con el suelo]
 */

using UnityEngine;

public class GemDrop : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;

    // Instancia una gema en la posición de la roca.
    public void Drop()
    {
        Instantiate(
            gemPrefab,
            transform.position,
            Quaternion.identity);
    }
}