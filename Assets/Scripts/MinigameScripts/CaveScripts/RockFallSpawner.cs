/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Spawner de las rocas que caen en la cueva a partir de spawnpoints]
 */

using System.Collections;
using UnityEngine;

public class RockFallSpawner : MonoBehaviour
{
    [Header("Configuración de rocas")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0.5f;

    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        CollapseController.OnCollapseStarted += StartSpawner;
    }

    private void OnDisable()
    {
        CollapseController.OnCollapseStarted -= StartSpawner;
    }

    // Se ejecuta cuando comienza el derrumbe
    private void StartSpawner()
    {
        if (spawnRoutine != null)
            return;

        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnRock();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRock()
    {
        if (spawnPoints.Length == 0)
            return;

        int randomIndex = Random.Range(
            0,
            spawnPoints.Length);

        Transform spawnPoint =
            spawnPoints[randomIndex];

        Instantiate(
            rockPrefab,
            spawnPoint.position,
            Quaternion.identity);
    }
}