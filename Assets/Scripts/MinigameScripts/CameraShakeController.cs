/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Control del shake de la cámara. Se activa luego de tomar las gemas principales]
 */

using UnityEngine;
using Unity.Cinemachine;


public class CameraShakeController : MonoBehaviour
{
    [Header("Configuración inicial")]
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 3f;

    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        // Al principio del minijuego se setean los valores de la cámara en 0 => cámara estable.

        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    // Aplica nueva amplitud y frecuencia al temblor de la cámara.
    public void Shake(float amplitud, float frecuencia)
    {
        noise.AmplitudeGain = amplitud;
        noise.FrequencyGain = frecuencia;
    }

    // Metodo en respuesta al evento de inicio del derrumbe
    private void StartShake()
    {
        Shake(amplitude, frequency);
    }

    // Se notifica cuando comience el derrumbe
    private void OnEnable()
    {
        CollapseController.OnCollapseStarted += StartShake;
    }

    // Deja de recibir notifiaciones del derrumbe 
    private void OnDisable()
    {
        CollapseController.OnCollapseStarted -= StartShake;
    }

    // Estabiliza la cámara reestableciendo los valores de amplitud y frecuencia
    public void StopShake()
    {
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

}
