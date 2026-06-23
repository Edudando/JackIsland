/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 23-06-2026 16:05:01
 * @modify date 23-06-2026 16:05:01
 * @desc [description]
 */
using UnityEngine;

public class TriggerInstruccion : MonoBehaviour
{
    public string archivoJson;
    public string dialogoId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var loro = collision.GetComponentInChildren<CompanionDialogue>();
        if (loro == null) return;

        // Busca el manager aunque no esté registrado aún
        var manager = DialogoManager.Instance ?? FindObjectOfType<DialogoManager>();
        if (manager == null)
        {
            Debug.LogError("DialogoManager no existe en la escena");
            return;
        }

        var frases = manager.GetFrases(archivoJson, dialogoId);
        if (frases != null)
        {
            loro.ShowDialogue(frases);
            Destroy(gameObject);
        }
    }
}