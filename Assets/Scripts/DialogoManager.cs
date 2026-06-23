using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogoData
{
    public string id;
    public string[] frases;
}

[System.Serializable]
public class DialogoContainer
{
    public List<DialogoData> dialogos;
}

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;

    private Dictionary<string, string[]> _cache = new();

    void OnEnable()
    {
        Instance = this;
        Debug.Log("DialogoManager registrado: " + gameObject.name);
    }

    void OnDisable()
    {
        if (Instance == this) Instance = null;
    }

    public string[] GetFrases(string archivoJson, string id)
    {
        string cacheKey = $"{archivoJson}/{id}";

        if (_cache.TryGetValue(cacheKey, out var cached))
            return cached;

        TextAsset json = Resources.Load<TextAsset>($"Dialogos/{archivoJson}");
        if (json == null)
        {
            Debug.LogError($"JSON no encontrado: Dialogos/{archivoJson}");
            return null;
        }

        var container = JsonUtility.FromJson<DialogoContainer>(json.text);

        if (container == null || container.dialogos == null)
        {
            Debug.LogError("El JSON no se parseó correctamente");
            return null;
        }

        foreach (var d in container.dialogos)
            _cache[$"{archivoJson}/{d.id}"] = d.frases;

        return _cache.TryGetValue(cacheKey, out var result) ? result : null;
    }
}