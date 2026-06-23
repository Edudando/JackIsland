using System;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;


public class CameraShakeController : MonoBehaviour
{
    
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        // Al principio del minijuego se setean los valores de la cámara en 0 => cámara estable.

        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    // Se toman los nuevos valores para el temblor
    public void Shake(float amplitud, float frecuencia)
    {
        noise.AmplitudeGain = amplitud;
        noise.FrequencyGain = frecuencia;
    }

    // Metodo para frenar el movimiento del temblor
    public void StopShake()
    {
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

}
