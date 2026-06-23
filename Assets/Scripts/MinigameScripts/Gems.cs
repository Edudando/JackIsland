using UnityEngine;

public class Gems : MonoBehaviour
{
    [SerializeField] private CameraShakeController shakeController;


    public void RecogerObjeto()
    {
        // tras recojer el objeto, la cámara comienza a sacudirse.
        shakeController.Shake(2f, 3f);

        Destroy(gameObject);
    }
}
