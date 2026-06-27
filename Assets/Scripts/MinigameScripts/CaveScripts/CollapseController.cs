/**
 * @author Claudia Chauque
 * @email claudiachauque9@gmail.com
 * @desc [Script que maneja el comportamiento del derrumbe de la cueva]
 */

using System;
using UnityEngine;

public class CollapseController : MonoBehaviour
{
    public static event Action OnCollapseStarted;

    private bool collapseStarted;

    private void OnEnable()
    {
        Gems.OnTreasureCollected += StartCollapse;
    }

    private void OnDisable()
    {
        Gems.OnTreasureCollected -= StartCollapse;
    }

    private void StartCollapse()
    {
        if (collapseStarted)
            return;

        collapseStarted = true;

        Debug.Log("Comenzó el derrumbe");

        OnCollapseStarted?.Invoke();
    }
}
