using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialFogata : MonoBehaviour
{
    [Header("Referencias")]
    public CompanionDialogue loroDialogo;
    public SpriteRenderer spriteHoguera;
    public Sprite hogueraInicial;

    private bool yaDijoFrio = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (spriteHoguera != null && spriteHoguera.sprite != hogueraInicial) return;

        if (DialogoManager.Instance == null)
        {
            Debug.LogError("DialogoManager no existe en la escena");
            return;
        }

        List<string> frases = new();

        if (!yaDijoFrio)
        {
            var frase = DialogoManager.Instance.GetFrases("tutorial_fogata", "frio_inicial");
            if (frase != null) frases.AddRange(frase);
            yaDijoFrio = true;
        }

        string estadoHacha = !InventoryManager.Instance.tieneHacha ? "sin_hacha"
            : (InventoryManager.Instance.itemSeleccionado?.ToLower() != "hacha") ? "hacha_sin_equipar"
            : "hacha_equipada";

        var instruccion = DialogoManager.Instance.GetFrases("tutorial_fogata", estadoHacha);
        if (instruccion != null) frases.AddRange(instruccion);

        if (frases.Count > 0)
            loroDialogo.ShowDialogue(frases.ToArray());
    }
}