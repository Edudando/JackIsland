/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script de autodestrucción principalmente para las gemas creadas tras la caída de rocas]
 */

using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}